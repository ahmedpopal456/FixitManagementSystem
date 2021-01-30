using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fix.Management.ServerlessApi.Managers;
using Fix.Management.ServerlessApi.Models;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Enums;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Enums;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks.Enums;
using Microsoft.Extensions.Configuration;

namespace Fix.Management.ServerlessApi.Mediators
{
  public class FixPlanMediator : IFixPlanMediator
  {
    private readonly IMapper _mapper;
    private readonly IDatabaseTableEntityMediator _databaseFixTable;
    private readonly IConfiguration _configurationProvider;

    public FixPlanMediator(IMapper mapper,
                           IConfiguration configurationProvider,
                           IDatabaseMediator databaseMediator)
    {
      var databaseName = configurationProvider["FIXIT-FMS-DB-NAME"];
      var databaseFixTableName = configurationProvider["FIXIT-FMS-DB-FIXPLANTABLENAME"];
      _configurationProvider = configurationProvider;

      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Database as {{FIXIT-FMS-DB-NAME}} ");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTableName))
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Table as {{FIXIT-FMS-DB-FIXPLANTABLENAME}} ");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTableName);
    }

    public FixPlanMediator(IMapper mapper,
                        IDatabaseMediator databaseMediator,
                        string databaseName,
                        string databaseUserTableName)
    {
      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(databaseName)}... null argument was provided");
      }

      if (string.IsNullOrWhiteSpace(databaseUserTableName))
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(databaseUserTableName)}... null argument was provided");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixPlanMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseUserTableName);
    }

    #region Create
    public async Task<FixPlanDto> CreateFixPlanAsync(FixPlanCreateRequestDto fixPlanCreateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixPlanDto);
      var fixPlan = _mapper.Map<FixPlanCreateRequestDto, FixPlanDocument>(fixPlanCreateRequestDto);
      var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
      fixPlan.CreatedTimestampUtc = currentTimeStamp;
      fixPlan.UpdatedTimestampUtc = currentTimeStamp;

      var creationResponse = await _databaseFixTable.CreateItemAsync(fixPlan, fixPlanCreateRequestDto.CreatedByCraftsman.Id.ToString(), cancellationToken);
      if(creationResponse != null)
      {
        result = new FixPlanDto()
        {
          OperationException = creationResponse.OperationException,
          OperationMessage = creationResponse.OperationMessage
        };
        if (creationResponse.IsOperationSuccessful && creationResponse.Document != null)
        {
          result = _mapper.Map<FixPlanDocument, FixPlanDto>(creationResponse.Document);
          result.IsOperationSuccessful = true;
        }
      }
      return result;
    }
    #endregion

    #region Delete
    public async Task<OperationStatus> DeleteFixPlanAsync(Guid fixPlanId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(OperationStatus);
      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());  
      var fixPlanDocument = fixPlanDocumentCollection?.Results.SingleOrDefault();
      if (fixPlanDocument != null)
      {
        result = await _databaseFixTable.DeleteItemAsync<FixPlanDocument>(fixPlanId.ToString(), fixPlanDocument.EntityId, cancellationToken);
      }       
      
      return result;
    }
    #endregion

    #region Get
    public async Task<FixPlanDto> GetFixPlanAsync(Guid fixPlanId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(FixPlanDto);

      var(fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());
      if(fixPlanDocumentCollection != null)
      {
        result = new FixPlanDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };
        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixPlanDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if(fixPlanDocument != null)
          {
            result = _mapper.Map<FixPlanDocument, FixPlanDto>(fixPlanDocument);
            result.IsOperationSuccessful = true;
          }
        }
      }
      return result;
    }

    public async Task<IEnumerable<FixPlanDto>> GetFixPlanHistoryAsync(Guid userId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      List<FixPlanDto> result = new List<FixPlanDto>();

      var (fixPlanDocumentCollection, continuationToken) = (await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.CreatedByCraftsman.Id == userId));

      if (fixPlanDocumentCollection.IsOperationSuccessful && fixPlanDocumentCollection != null)
      {
        result = fixPlanDocumentCollection.Results.Select(document => _mapper.Map<FixPlanDocument, FixPlanDto>(document)).ToList();
      }     
      return result;
    }
    #endregion

    #region Updates by Craftsman
    public async Task<FixPlanDto> UpdateFixPlanStructureAsync(Guid fixPlanId, FixPlanUpdateRequestDto fixPlanUpdateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixPlanDto);

      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());

      if (fixPlanDocumentCollection != null)
      {
        result = new FixPlanDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };

        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixPlanUpdateDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if(fixPlanUpdateDocument != null)
          {
            //Bookmarks
            //TODO: Extract bookmarked as it's own api
            fixPlanUpdateDocument.IsBookmarked = fixPlanUpdateRequestDto.IsBookmarked;

            //Updated Time Stamp
            fixPlanUpdateDocument.UpdatedTimestampUtc = DateTimeOffset.Now.ToUnixTimeSeconds();

            //Phases
            fixPlanUpdateDocument.Phases = fixPlanUpdateRequestDto.Phases;

            //Change state back to Tentative
            fixPlanUpdateDocument.ProposalState = FixPlanProposalStates.Tentative;

            var operationStatus = await _databaseFixTable.UpdateItemAsync(fixPlanUpdateDocument, fixPlanUpdateDocument.EntityId, cancellationToken) ;

            result = operationStatus.IsOperationSuccessful ? _mapper.Map<FixPlanDocument, FixPlanDto>(fixPlanUpdateDocument) : result;
            result.IsOperationSuccessful = operationStatus.IsOperationSuccessful;
            result.OperationException = operationStatus?.OperationException;
            result.OperationMessage = operationStatus?.OperationMessage;
          }
        }
      }
      return result;
    }

    public async Task<FixPhaseDto> UpdateFixPlanPhaseStatusAsync(Guid fixPlanId, Guid phaseId, FixPhaseStatusUpdateRequestDto fixPhaseStatusUpdateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(FixPhaseDto);
      

      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());
      if(fixPlanDocumentCollection != null)
      {
        result = new FixPhaseDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };

        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixUpdateDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if(fixUpdateDocument != null)
          {
            var fixPlanPhaseToUpdate = fixUpdateDocument.Phases.Where(phase => phase.Id == phaseId).SingleOrDefault();
            if (Enum.IsDefined(typeof(PhaseStatuses), fixPhaseStatusUpdateRequestDto.Status) && fixPhaseStatusUpdateRequestDto.Status != PhaseStatuses.Done)
            {
              fixPlanPhaseToUpdate.Status = fixPhaseStatusUpdateRequestDto.Status;         
            }
            var operationStatus = await _databaseFixTable.UpdateItemAsync(fixUpdateDocument, fixUpdateDocument.EntityId, cancellationToken);

            result = operationStatus.IsOperationSuccessful ? fixPlanPhaseToUpdate : result;
            result.IsOperationSuccessful = operationStatus.IsOperationSuccessful;
            result.OperationException = operationStatus?.OperationException;
            result.OperationMessage = operationStatus?.OperationMessage;
          }
        }
      }
      return result;
    }

    public async Task<FixPhaseTaskDto> UpdateFixPlanTaskStatusAsync(Guid fixPlanId, Guid phaseId, Guid taskId, FixTaskStatusUpdateRequestDto fixTaskStatusUpdateRequestDto, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixPhaseTaskDto);
      
      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());
      if(fixPlanDocumentCollection != null)
      {
        result = new FixPhaseTaskDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };

        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixUpdateDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if (fixUpdateDocument != null)
          {            
              var fixPlanTaskToUpdate = fixUpdateDocument.Phases.Where(fixPhase => fixPhase.Id == phaseId).SingleOrDefault()
                                                         .Tasks.Where(fixTask => fixTask.Id == taskId).SingleOrDefault();
              if (fixPlanTaskToUpdate != null)
              {
                if (Enum.IsDefined(typeof(TaskStatuses), fixTaskStatusUpdateRequestDto.Status) && fixTaskStatusUpdateRequestDto.Status != TaskStatuses.Done)
                {
                  fixPlanTaskToUpdate.Status = fixTaskStatusUpdateRequestDto.Status;
                }
                var operationStatus = await _databaseFixTable.UpdateItemAsync(fixUpdateDocument, fixUpdateDocument.EntityId, cancellationToken);

                result = operationStatus.IsOperationSuccessful ? fixPlanTaskToUpdate:result;
                result.IsOperationSuccessful = operationStatus.IsOperationSuccessful;
                result.OperationException = operationStatus?.OperationException;
                result.OperationMessage = operationStatus?.OperationMessage;
            }     
          }
        }
      }
      return result;
    }
    #endregion

    #region Update by Client
    public async Task<FixPlanDto> UpdateClientFixPlanProposalStatusAsync(Guid fixPlanId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixPlanDto);

      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());

      if(fixPlanDocumentCollection != null)
      {
        result = new FixPlanDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };
        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixUpdateDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if(fixUpdateDocument != null)
          {
            if (fixUpdateDocument.ProposalState != FixPlanProposalStates.Approved)
            {
              fixUpdateDocument.ProposalState = FixPlanProposalStates.Approved;
            }
            var operationStatus = await _databaseFixTable.UpdateItemAsync(fixUpdateDocument, fixUpdateDocument.EntityId, cancellationToken);

            result = operationStatus.IsOperationSuccessful ? _mapper.Map<FixPlanDocument, FixPlanDto>(fixUpdateDocument) : result;
            result.IsOperationSuccessful = operationStatus.IsOperationSuccessful;
            result.OperationException = operationStatus?.OperationException;
            result.OperationMessage = operationStatus?.OperationMessage;
          }
        }
      }
      return result;
    }

    public async Task<FixPhaseDto> UpdateClientFixPlanPhaseStatusAsync(Guid fixPlanId, Guid phaseId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = default(FixPhaseDto);

      var (fixPlanDocumentCollection, continuationToken) = await _databaseFixTable.GetItemQueryableAsync<FixPlanDocument>(null, cancellationToken, i => i.id == fixPlanId.ToString());
      if(fixPlanDocumentCollection != null)
      {
        result = new FixPhaseDto()
        {
          OperationException = fixPlanDocumentCollection.OperationException,
          OperationMessage = fixPlanDocumentCollection.OperationMessage
        };
        if (fixPlanDocumentCollection.IsOperationSuccessful)
        {
          FixPlanDocument fixUpdateDocument = fixPlanDocumentCollection.Results.SingleOrDefault();
          if(fixUpdateDocument != null)
          {
            var fixPlanPhaseToUpdate = fixUpdateDocument.Phases.SingleOrDefault(phase => phase.Id == phaseId);
            if (fixPlanPhaseToUpdate != null && fixPlanPhaseToUpdate.Status != PhaseStatuses.Done)
            {
              fixPlanPhaseToUpdate.Status = PhaseStatuses.Done;
            }

            var operationStatus = await _databaseFixTable.UpdateItemAsync(fixUpdateDocument, fixUpdateDocument.EntityId, cancellationToken);

            result = operationStatus.IsOperationSuccessful ? fixPlanPhaseToUpdate : result;
            result.IsOperationSuccessful = operationStatus.IsOperationSuccessful;
            result.OperationException = operationStatus.OperationException;
            result.OperationMessage = operationStatus.OperationMessage;
          }
        }
      }
      return result;
    }
    #endregion
  }
}
