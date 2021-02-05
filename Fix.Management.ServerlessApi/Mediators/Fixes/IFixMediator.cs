using Fixit.Core.Database.DataContracts;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fix.Management.ServerlessApi.Mediators.Fixes
{
  public interface IFixMediator
  {
    /// <summary>
    /// Create Fix
    /// </summary>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixResponseDto> CreateFixAsync(FixCreateRequestDto fixCreateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Delete Fix
    /// </summary>
    /// <param name="fixId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteFixAsync(Guid fixId, CancellationToken cancellationToken);
    /// <summary>
    /// Get Fix
    /// </summary>
    /// <param name="fixId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixResponseDto> GetFixAsync(Guid fixId, CancellationToken cancellationToken);
    /// <summary>
    /// Get Fix Cost
    /// </summary>
    /// <param name="fixId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixCostResponseDto> GetFixCostAsync(Guid fixId, CancellationToken cancellationToken);
    /// <summary>
    /// Update Fix
    /// </summary>
    /// <param name="fixId"></param>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixResponseDto> UpdateFixAsync(Guid fixId, FixUpdateRequestDto fixUpdateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Update Fix AssignedCraftsman
    /// </summary>
    /// <param name="fixId"></param>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixResponseDto> UpdateFixAssignAsync(Guid fixId, FixUpdateAssignRequestDto fixUpdateAssignRequestDto, CancellationToken cancellationToken);
  }
}
