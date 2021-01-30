using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Managers;
using Fixit.Core.Database.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Fix.Management.ServerlessApi.Functions
{
  public class DeleteFixPlan
  {
    private readonly IFixPlanMediator _fixPlanMediator;
    private readonly IMapper _mapper;

    public DeleteFixPlan(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(DeleteFixPlan)} expects a value for {nameof(mapper)}... null argument was provided");
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(DeleteFixPlan)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("DeleteFixPlanAsync")]
    [OpenApiOperation("delete", "FixPlans")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(OperationStatus))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "fixPlans/{id:Guid}")]
                                           CancellationToken cancellationToken,
                                           Guid id)
    {
      return await DeleteFixPlanAsync(cancellationToken, id);
    }

    public async Task<IActionResult> DeleteFixPlanAsync(CancellationToken cancellationToken, Guid fixPlanId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixPlanId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixPlanId)} is not valid.");
      }

      var result = await _fixPlanMediator.DeleteFixPlanAsync(fixPlanId, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix plan with Id {nameof(fixPlanId)} was not found");
      }

      return new OkObjectResult(result);
    }
  }
}
