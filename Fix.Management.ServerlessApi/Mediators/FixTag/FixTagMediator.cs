using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Tags;
using Microsoft.Extensions.Configuration;

namespace Fix.Management.ServerlessApi.Mediators.FixTag
{
  public class FixTagMediator : IFixTagMediator
  {
    private readonly IMapper _mapper;
    private readonly IDatabaseTableEntityMediator _databaseFixTagTable;

    public FixTagMediator(IMapper mapper,
                          IConfiguration configurationProvider,
                          IDatabaseMediator databaseMediator)
    {
      var databaseName = configurationProvider["FIXIT-FMS-DB-NAME"];
      var databaseFixTableName = configurationProvider["FIXIT-FMS-DB-FIXTAGTABLENAME"];

      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Database as {{FIXIT-FMS-DB-NAME}} ");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTableName))
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects the {nameof(configurationProvider)} to have defined the Fix Management Table as {{FIXIT-FMS-DB-FIXTABLE}} ");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixLocationMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTagTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTableName);
    }

    public FixTagMediator(IMapper mapper,
                          IDatabaseMediator databaseMediator,
                          string databaseName,
                          string databaseFixTagTableName)
    {
      if (string.IsNullOrWhiteSpace(databaseName))
      {
        throw new ArgumentNullException($"{nameof(FixTagMediator)} expects a value for {nameof(databaseName)}... null argument was provided");
      }

      if (string.IsNullOrWhiteSpace(databaseFixTagTableName))
      {
        throw new ArgumentNullException($"{nameof(FixTagMediator)} expects a value for {nameof(databaseFixTagTableName)}... null argument was provided");
      }

      if (databaseMediator == null)
      {
        throw new ArgumentNullException($"{nameof(FixTagMediator)} expects a value for {nameof(databaseMediator)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FixTagMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _databaseFixTagTable = databaseMediator.GetDatabase(databaseName).GetContainer(databaseFixTagTableName);
    }

    public async Task<IEnumerable<TagResponseDto>> GetFixTagAsync(int topTags, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = new List<TagResponseDto>();

      var (fixTagDocumentCollection, continuationToken) = await _databaseFixTagTable.GetItemQueryableAsync<FixTagDocument>(null, cancellationToken, fixTagDocument => fixTagDocument.Statistics.TotalFixesCount > 0);
      if (fixTagDocumentCollection.Results.Count != default(int) && fixTagDocumentCollection.IsOperationSuccessful)
      {
        result = fixTagDocumentCollection.Results.Select(document => _mapper.Map<FixTagDocument, TagResponseDto>(document)).OrderByDescending(tagResponse => tagResponse.Statistics.TotalFixesCount).Take(topTags).ToList();
      }
      return result;
    }

    public async Task<OperationStatus> OnFixCreateAndUpdateTags(FixDocument fixDocument, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      var result = default(OperationStatus);
      var tagsCollection = fixDocument.Tags;

      if (tagsCollection != null)
      {
        foreach (var fixTagItem in tagsCollection)
        {
          var (fixTagDocumentCollection, tagContinuationToken) = await _databaseFixTagTable.GetItemQueryableAsync<FixTagDocument>(null, cancellationToken, fixTagDocument => fixTagDocument.Name.ToLower().Equals(fixTagItem.Name.ToLower()));

          if (fixTagDocumentCollection.Results.Count != default(int))
          {
            var fixTagResults = fixTagDocumentCollection.Results.SingleOrDefault();

            if(fixDocument.CreatedTimestampUtc == fixDocument.UpdatedTimestampUtc)
            {
              fixTagResults.Statistics.TotalFixesCount++;
            }
            result = await _databaseFixTagTable.UpsertItemAsync(fixTagResults, fixDocument.EntityId, cancellationToken);
          }
          else
          {
            var fixTagDocument = _mapper.Map<TagDto, FixTagDocument>(fixTagItem);
            fixTagDocument.Statistics = new TagStatisticsDto { TotalFixesCount = 1 };
            result = await _databaseFixTagTable.CreateItemAsync(fixTagDocument, fixDocument.EntityId, cancellationToken);
          }
        }
      }

      return result;
    }
  }
}
