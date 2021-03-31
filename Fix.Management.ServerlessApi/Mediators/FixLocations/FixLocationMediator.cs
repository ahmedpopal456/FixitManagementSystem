using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fix.Management.ServerlessApi.Mediators.FixLocations;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Locations;
using Microsoft.Extensions.Configuration;

namespace Fix.Management.ServerlessApi.Mediators
{
  public class FixLocationMediator : IFixLocationMediator
  {
    private readonly IMapper _mapper;
    private readonly IDatabaseTableEntityMediator _databaseFixLocationTable;

    public FixLocationMediator(IMapper mapper,
                       IConfiguration configurationProvider,
                       IDatabaseMediator databaseMediator)
    {
      var databaseName = configurationProvider["FIXIT-FMS-DB-NAME"];
      var databaseFixTableName = configurationProvider["FIXIT-FMS-DB-FIXLOCATIONTABLENAME"];

      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Database as {{FIXIT-FMS-DB-NAME}} ");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTableName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Table as {{FIXIT-FMS-DB-FIXLOCATIONTABLENAME}} ");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixLocationTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTableName);
    }

    public FixLocationMediator(IMapper mapper,
                        IDatabaseMediator databaseMediator,
                        string databaseName,
                        string databaseUserTableName)
    {
      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(databaseName)}... null argument was provided");
      }

      if (string.IsNullOrWhiteSpace(databaseUserTableName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(databaseUserTableName)}... null argument was provided");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixLocationTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseUserTableName);
    }

    public async Task<IEnumerable<FixLocationResponseDto>> GetFixLocationAsync(Guid userId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = new List<FixLocationResponseDto>();

      var (fixLocationDocumentCollection, continuationToken) = await _databaseFixLocationTable.GetItemQueryableAsync<FixLocationDocument>(null, cancellationToken, fixLocationDocument => fixLocationDocument.EntityId == userId.ToString());
      if (fixLocationDocumentCollection != null && fixLocationDocumentCollection.IsOperationSuccessful)
      {
         result = fixLocationDocumentCollection.Results.Select(document => _mapper.Map<FixLocationDocument, FixLocationResponseDto>(document)).ToList();
      }
      return result;
    }

    public async Task<OperationStatus> OnFixCreateAndUpdateFixLocation(FixDocument fixDocument, CancellationToken cancellationToken)
    {
      var fixLocationDocument = _mapper.Map<FixLocationDto, FixLocationDocument>(fixDocument.Location);
      var result = default(OperationStatus);
      //TODO Improve management of location by clients
      var (fixLocationDocumentCollection, locationContinuationToken) = await _databaseFixLocationTable.GetItemQueryableAsync<FixLocationDocument>(null, cancellationToken, fixLocationDocumentCollection => fixLocationDocumentCollection.PostalCode == fixLocationDocument.PostalCode);
      if (fixLocationDocumentCollection.Results.Count != default(int) && fixLocationDocumentCollection.IsOperationSuccessful)
      {
        var fixLocationResults = fixLocationDocumentCollection.Results.SingleOrDefault();
        fixLocationResults.Address = fixLocationDocument.Address;
        fixLocationResults.City = fixLocationDocument.City;
        fixLocationResults.Province = fixLocationDocument.Province;
        fixLocationResults.LastUsedTimeStampUtc = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        result = await _databaseFixLocationTable.UpsertItemAsync(fixLocationResults, fixDocument.EntityId, cancellationToken);
      }
      else
      {
        result = await _databaseFixLocationTable.CreateItemAsync(fixLocationDocument, fixDocument.EntityId, cancellationToken);
      }

      return result;
    }
  }
}
