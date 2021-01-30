using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Managers;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fix.Management.ServerlessApi
{
  public class UpdateFixPlanPhaseStatus
  {
    private readonly IFixPlanMediator _fixPlanMediator;
    private readonly IMapper _mapper;

    public UpdateFixPlanPhaseStatus(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanPhaseStatus)} expects a value for {nameof(mapper)}... null argument was provided");
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanPhaseStatus)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixPlanPhaseStatusClient")]
    [OpenApiOperation("put", "FixPlans")]
    [OpenApiRequestBody("application/json", typeof(FixPhaseDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPhaseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixPlans/{id:Guid}/phases/{phaseId:Guid}/approve")]
                                          CancellationToken cancellationToken,
                                          Guid id,
                                          Guid phaseId)
    {
      return await UpdateClientFixPlanPhaseStatusAsync(cancellationToken, id, phaseId);
    }

    public async Task<IActionResult> UpdateClientFixPlanPhaseStatusAsync(CancellationToken cancellationToken, Guid fixPlanId, Guid phaseId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixPlanId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixPlanId)} is not valid.");
      }
      if (phaseId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(phaseId)} is not valid.");
      }

      var result = await _fixPlanMediator.UpdateClientFixPlanPhaseStatusAsync(fixPlanId, phaseId, cancellationToken);
      if (result == null)
      {
        return new NotFoundObjectResult($"Fix phase with Id {nameof(phaseId)} was not found");
      }

      return new OkObjectResult(result);
    }
  }
}
