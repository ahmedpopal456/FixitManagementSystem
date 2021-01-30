using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Managers;
using Fixit.Core.DataContracts.FixPlans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fix.Management.ServerlessApi
{
  public class UpdateFixPlanProposalState
  {
    private readonly IFixPlanMediator _fixPlanMediator;
    private readonly IMapper _mapper;

    public UpdateFixPlanProposalState(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanProposalState)} expects a value for {nameof(mapper)}... null argument was provided");
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanProposalState)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixPlanProposalStateClient")]
    [OpenApiOperation("put", "FixPlans")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPlanDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixPlans/{id:Guid}/approve")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {
      return await UpdateClientFixPlanProposalStatusAsync(cancellationToken, id);
    }

    public async Task<IActionResult> UpdateClientFixPlanProposalStatusAsync(CancellationToken cancellationToken, Guid fixPlanId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixPlanId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixPlanId)} is not valid.");
      }

      var result = await _fixPlanMediator.UpdateClientFixPlanProposalStatusAsync(fixPlanId, cancellationToken);
      if(result == null)
      {
        return new NotFoundObjectResult($"Fix plan with Id {nameof(fixPlanId)} was not found");
      }

      return new OkObjectResult(result);
    }
  }
}
