using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.Fixes.Locations;

namespace Fix.Management.ServerlessApi.Mediators.FixLocations
{
  public interface IFixLocationMediator
  {
    /// <summary>
    /// Get Fix location by Id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<FixLocationResponseDto>> GetFixLocationAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Trigger for Fix Location
    /// </summary>
    /// <param name="fixLocationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> OnFixCreateAndUpdateFixLocation(FixDocument fixDocument, CancellationToken cancellationToken);

  }
}
