using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Helpers.FixPlans;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fix.Management.ServerlessApi.Functions.FixPlans.Craftsman
{
  public class UpdateFixPlanTaskStatus
  {
    private readonly IFixPlanMediator _fixPlanMediator;

    public UpdateFixPlanTaskStatus(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanTaskStatus)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixPlanTaskStatus")]
    [OpenApiOperation("put", "FixPlans")]
    [OpenApiRequestBody("application/json", typeof(FixPhaseTaskDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPhaseTaskDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixPlans/{id:Guid}/phases/{phaseId:Guid}/tasks/{taskId:Guid}/status")]
                                          HttpRequestMessage httpRequestMessage,
                                          CancellationToken cancellationToken,
                                          //[FixitAccessAttribute(Name = "UpdateFixAsync", Role = RoleDefinition.Craftsman)] AccessResult fixitAccessResult,
                                          Guid id,
                                          Guid phaseId, 
                                          Guid taskId)
    {
      return await UpdateFixPlanTaskStatusAsync(httpRequestMessage, cancellationToken, id, phaseId, taskId);
    }

    public async Task<IActionResult> UpdateFixPlanTaskStatusAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken, Guid id, Guid phaseId,Guid taskId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixPlansDtoValidators.IsValidFixPlanTaskRequest(httpRequestMessage.Content, out FixTaskStatusUpdateRequestDto fixRequestDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixTaskStatusUpdateRequestDto)} is null or has one or more invalid fields...");
      }

      if (id.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(id)} is not valid.");
      }
      if (phaseId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(phaseId)} is not valid.");
      }
      if (taskId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(taskId)} is not valid.");
      }

      var result = await _fixPlanMediator.UpdateFixPlanTaskStatusAsync(id, phaseId, taskId, fixRequestDto, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix Task with Id {nameof(taskId)} was not found..");
      }

      return new OkObjectResult(result);
    }
  }
}
