using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;

namespace Fix.Management.ServerlessApi.Mediators.FixPlans
{
  public interface IFixPlanMediator
  {
    /// <summary>
    /// Create fix plan
    /// </summary>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPlanDto> CreateFixPlanAsync(FixPlanCreateRequestDto fixPlanCreateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Get Fix plan by Id
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPlanDto> GetFixPlanAsync(Guid fixPlanId, CancellationToken cancellationToken);
    /// <summary>
    /// Delete Fix plan by Id
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteFixPlanAsync(Guid fixPlanId, CancellationToken cancellationToken);
    /// <summary>
    /// Update fix plan by Id
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPlanDto> UpdateFixPlanStructureAsync(Guid fixPlanId, FixPlanUpdateRequestDto fixPlanUpdateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Update fix plan phase 
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="phaseId"></param>
    /// <param name="fixPhaseDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPhaseDto> UpdateFixPlanPhaseStatusAsync(Guid fixPlanId, Guid phaseId, FixPhaseStatusUpdateRequestDto fixPhaseStatusUpdateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Update fix plan phase task 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phaseId"></param>
    /// <param name="taskId"></param>
    /// <param name="fixTaskDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPhaseTaskDto> UpdateFixPlanTaskStatusAsync(Guid fixPlanId, Guid phaseId, Guid taskId, FixTaskStatusUpdateRequestDto fixTaskStatusUpdateRequestDto, CancellationToken cancellationToken);
    /// <summary>
    /// Update state of fix plan by client
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPlanDto> UpdateClientFixPlanProposalStatusAsync(Guid fixPlanId, CancellationToken cancellationToken);
    /// <summary>
    /// Update fix plan phase by client
    /// </summary>
    /// <param name="fixPlanId"></param>
    /// <param name="phaseId"></param>
    /// <param name="fixDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FixPhaseDto> UpdateClientFixPlanPhaseStatusAsync(Guid fixPlanId, Guid phaseId, CancellationToken cancellationToken);
    /// <summary>
    /// Get fix plan history by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<FixPlanDto>> GetFixPlanHistoryAsync(Guid userId, CancellationToken cancellationToken);

  }
}
