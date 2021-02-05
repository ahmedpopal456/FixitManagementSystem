using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Helpers.FixPlans;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fix.Management.ServerlessApi.Functions.FixPlans.Craftsman
{
  public class UpdateFixPlanPhaseStatus
  {
    private readonly IFixPlanMediator _fixPlanMediator;

    public UpdateFixPlanPhaseStatus(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanPhaseStatus)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixPlanPhaseStatus")]
    [OpenApiOperation("put", "FixPlans")]
    [OpenApiRequestBody("application/json", typeof(FixPhaseDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPhaseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixPlans/{id:Guid}/phases/{phaseId:Guid}/status")]
                                          HttpRequestMessage httpRequestMessage,
                                          CancellationToken cancellationToken,
                                          Guid id,
                                          Guid phaseId)
    {
      return await UpdateFixPlanPhaseStatusAsync(httpRequestMessage, cancellationToken, id, phaseId);
    }

    public async Task<IActionResult> UpdateFixPlanPhaseStatusAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken, Guid fixPlanId, Guid phaseId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixPlansDtoValidators.IsValidFixPlanPhaseRequest(httpRequestMessage.Content, out FixPhaseStatusUpdateRequestDto fixRequestDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixPhaseStatusUpdateRequestDto)} is null or has one or more invalid fields...");
      }

      if (fixPlanId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixPlanId)} is not valid.");
      }
      if (phaseId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(phaseId)} is not valid.");
      }

      var result = await _fixPlanMediator.UpdateFixPlanPhaseStatusAsync(fixPlanId, phaseId, fixRequestDto, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix Phase with Id {nameof(phaseId)} was not found..");
      }

      return new OkObjectResult(result);
    }
  }
}
