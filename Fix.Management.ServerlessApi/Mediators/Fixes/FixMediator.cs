using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fix.Management.ServerlessApi.Helpers.Fixes;
using Fix.Management.ServerlessApi.Models.Document;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Enums;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;
using Fixit.Core.Storage.Queue.Mediators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Fix.Management.ServerlessApi.Mediators.Fixes
{
  public class FixMediator : IFixMediator
  {
    private readonly IMapper _mapper;
    private readonly IDatabaseTableEntityMediator _databaseFixTable;
    private readonly IQueueClientMediator _queueStorage;

    public FixMediator(IMapper mapper,
                       IConfiguration configurationProvider,
                       IQueueServiceClientMediator queueStorageMediator,
                       IDatabaseMediator databaseMediator)
    {
      var databaseName = configurationProvider["FIXIT-FMS-DB-NAME"];
      var databaseFixTableName = configurationProvider["FIXIT-FMS-DB-FIXTABLE"];
      var queueName = configurationProvider["FIXIT-FMS-QUEUE-NAME"];

      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Database as {{FIXIT-FMS-DB-NAME}} ");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTableName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Table as {{FIXIT-FMS-DB-FIXTABLE}} ");
      }

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Queue Storage as {{FIXIT-FMS-QUEUE-NAME}} ");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      if (queueStorageMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(queueStorageMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTableName);
      _queueStorage = queueStorageMediator.GetQueueClient(queueName);
    }

    public FixMediator(IMapper mapper,
                       IQueueServiceClientMediator queueStorageMediator,
                       IDatabaseMediator databaseMediator,
                       string databaseName,
                       string databaseFixTableName,
                       string queueName)
    {

      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(databaseName)} to have defined the Fix Management Database as {{FIXIT-FMS-DB-NAME}} ");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTableName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(databaseFixTableName)} to have defined the Fix Management Table as {{FIXIT-FMS-DB-FIXTABLE}} ");
      }

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects the {nameof(queueName)} to have defined the Fix Queue Storage as {{FIXIT-FMS-QUEUE-NAME}} ");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      if (queueStorageMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(queueStorageMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTableName);
      _queueStorage = queueStorageMediator.GetQueueClient(queueName);
    }

    #region Create Fixes
    public async Task<FixResponseDto> CreateFixAsync(FixCreateRequestDto fixCreateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = new FixResponseDto();
      var documentToCreate = _mapper.Map<FixCreateRequestDto, FixDocument>(fixCreateRequestDto);
      var currentTimestampUtc = DateTimeOffset.Now.ToUnixTimeSeconds();
      documentToCreate.UpdatedTimestampUtc = currentTimestampUtc;
      documentToCreate.CreatedTimestampUtc = currentTimestampUtc;

      var serializedDocument = JsonConvert.SerializeObject(documentToCreate);
      var queueResponse = await _queueStorage.SendMessageAsync(serializedDocument);

      if (queueResponse != null)
      {
        result = FixGetResponseStatusHelper.MapResponseStatus(result, queueResponse);
        var creationResponse = FixDocumentValidators.IsNotNullAndOperationSuccessful(queueResponse) ? await _databaseFixTable.CreateItemAsync(documentToCreate, fixCreateRequestDto.CreatedByClient.Id.ToString(), cancellationToken): default;

        if (creationResponse != null)
        {
          result = FixDocumentValidators.IsNotNullAndOperationSuccessful(creationResponse) ? _mapper.Map<FixDocument, FixResponseDto>(creationResponse.Document) : result;
          result = FixGetResponseStatusHelper.MapResponseStatus(result, creationResponse); 
        }
      }

      return result;
    }
    #endregion

    #region Delete Fixes
    public async Task<OperationStatus> DeleteFixAsync(Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(null, cancellationToken, fixDocument => fixDocument.id == fixId.ToString());
      result.OperationException = fixDocumentCollection.OperationException;
      if (FixDocumentValidators.IsNotNullAndOperationSuccessful(fixDocumentCollection))
      {
        FixDocument fixDocument = fixDocumentCollection.Results.SingleOrDefault();
        result = fixDocument != null ? await _databaseFixTable.DeleteItemAsync<FixDocument>(fixId.ToString(), fixDocument.EntityId, cancellationToken) : result;
      }

      return result;
    }

    #endregion

    #region Get Fixes


    public async Task<FixResponseDto> GetFixAsync(Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(FixResponseDto);

      var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(null, cancellationToken, fixDocument => fixDocument.id == fixId.ToString());

      FixDocument fixDocument = FixDocumentValidators.IsNotNullAndOperationSuccessful(fixDocumentCollection) ? fixDocumentCollection.Results.SingleOrDefault() : default;
      
      if(fixDocument != null)
      {
        result = _mapper.Map<FixDocument, FixResponseDto>(fixDocument);
        result = FixGetResponseStatusHelper.MapResponseStatus(result, fixDocumentCollection);
      }
      
      return result;
    }
    
    public async Task<IEnumerable<FixResponseDto>> GetFixesByUserAsync(Guid userId, CancellationToken cancellationToken, IEnumerable<FixStatuses> fixStatuses = null)
    {
      cancellationToken.ThrowIfCancellationRequested(); 

      if(userId.Equals(Guid.Empty))
      {
        throw new ArgumentNullException(nameof(userId));
      }

      fixStatuses ??= new List<FixStatuses>();

      string currentContinuationToken = "";
      var fixResponses = new List<FixResponseDto>();

      Expression<Func<FixDocument, bool>> expression = fixDocument => (fixStatuses.Count() <= 0 || (fixStatuses.Contains(fixDocument.Status))) &&
                                                                      ((fixDocument.AssignedToCraftsman != null && userId == fixDocument.AssignedToCraftsman.Id) || fixDocument.EntityId == userId.ToString());

      while (currentContinuationToken != null)
      {
        var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(string.IsNullOrWhiteSpace(currentContinuationToken) ? null : currentContinuationToken, cancellationToken, expression);
        
        currentContinuationToken = continuationToken;
        if (fixDocumentCollection.IsOperationSuccessful && fixDocumentCollection.Results != null && fixDocumentCollection.Results.Any())
        {
          var timeLogResults = fixDocumentCollection.Results.Select(item => _mapper.Map<FixDocument, FixResponseDto>(item)).ToList();
          fixResponses.AddRange(timeLogResults);
        }
      }

      return fixResponses; 
    }

    /// <summary>
    /// Currently, GetFixCostAsync does not operate as intended.
    /// Its purpose was to Get the different FixCost (ClientEstimatedCost, SystemCalculatedCost, CraftsmanEstimatedCost) 
    /// from the AzureFunction EstimateFixCost, but it is not yet implemented.
    /// Connect with EstimateFixCost azure function when it is available
    /// </summary>
    public async Task<FixCostResponseDto> GetFixCostAsync(Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(FixCostResponseDto);

      var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(null, cancellationToken, fixDocument => fixDocument.id == fixId.ToString());

      FixDocument fixDocument = FixDocumentValidators.IsNotNullAndOperationSuccessful(fixDocumentCollection) ? fixDocumentCollection.Results.SingleOrDefault() : default;

      if (fixDocument != null)
      {
        result = _mapper.Map<FixDocument, FixCostResponseDto>(fixDocument);
        result = FixGetResponseStatusHelper.MapResponseStatus(result, fixDocumentCollection);
      }

      return result;
    }

    #endregion

    #region Update Fixes 

    public async Task<FixResponseDto> UpdateFixAsync(Guid fixId, FixUpdateRequestDto fixUpdateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result =  new FixResponseDto();

      var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(null, cancellationToken, fixDocument => fixDocument.id == fixId.ToString());
      if (fixDocumentCollection != null)
      {
        result = !fixDocumentCollection.IsOperationSuccessful ? FixGetResponseStatusHelper.MapResponseStatus(result, fixDocumentCollection) : default;

        FixDocument fixDocument = FixDocumentValidators.IsNotNullAndOperationSuccessful(fixDocumentCollection) ? fixDocumentCollection.Results.SingleOrDefault() : default;
        if (fixDocument != null)
        {
          fixDocument = _mapper.Map<FixUpdateRequestDto, FixDocument>(fixUpdateRequestDto, fixDocument);
          fixDocument.UpdatedTimestampUtc = DateTimeOffset.Now.ToUnixTimeSeconds();

          var operationStatus = await _databaseFixTable.UpdateItemAsync(fixDocument, fixDocument.EntityId, cancellationToken);  
          
          result = operationStatus.IsOperationSuccessful ? _mapper.Map<FixDocument, FixResponseDto>(fixDocument) : default;
          result = result != null ? FixGetResponseStatusHelper.MapResponseStatus(result, operationStatus) : default;
        }
      }

      return result;
    }

    public async Task<FixResponseDto> UpdateFixAssignAsync(Guid fixId, FixUpdateAssignRequestDto fixUpdateAssignRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = new FixResponseDto();
      var (fixDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixDocument>(null, cancellationToken, fixDocument => fixDocument.id == fixId.ToString());
      
      if(fixDocumentCollection != null)
      {
        result = !fixDocumentCollection.IsOperationSuccessful ? FixGetResponseStatusHelper.MapResponseStatus(result, fixDocumentCollection) : default;

        FixDocument fixDocument = fixDocumentCollection.IsOperationSuccessful ? fixDocumentCollection.Results.SingleOrDefault() : default;
        if (fixDocument != null)
        {
          fixDocument = _mapper.Map<FixUpdateAssignRequestDto, FixDocument>(fixUpdateAssignRequestDto, fixDocument);
          fixDocument.UpdatedTimestampUtc = DateTimeOffset.Now.ToUnixTimeSeconds();
          fixDocument.Status = FixStatuses.Pending;

          var operationStatus = await _databaseFixTable.UpdateItemAsync(fixDocument, fixDocument.EntityId, cancellationToken);

          result = operationStatus.IsOperationSuccessful ? _mapper.Map<FixDocument, FixResponseDto>(fixDocument) : result;
          result = result != null ? FixGetResponseStatusHelper.MapResponseStatus(result, operationStatus) : default;
        }
      }

      return result;
    }

    #endregion
  }
}
