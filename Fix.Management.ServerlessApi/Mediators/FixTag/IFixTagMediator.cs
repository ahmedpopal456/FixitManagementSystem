using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.Fixes.Tags;

namespace Fix.Management.ServerlessApi.Mediators.FixTag
{
  public interface IFixTagMediator
  {
    /// <summary>
    /// Get Fix tags by Id
    /// </summary>
    /// <param name="fixTagId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<TagResponseDto>> GetFixTagAsync(int topTags, CancellationToken cancellationToken);

    /// <summary>
    /// Trigger for fix tags
    /// </summary>
    /// <param name="fixDocument"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> OnFixCreateAndUpdateTags(FixDocument fixDocument, CancellationToken cancellationToken);
  }
}
