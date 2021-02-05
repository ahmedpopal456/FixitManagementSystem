using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fixit.Core.DataContracts.FixPlans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Fix.Management.ServerlessApi.Functions.FixPlans.Craftsman
{
  public class GetFixPlanHistory
  {
    private readonly IFixPlanMediator _fixPlanMediator;

    public GetFixPlanHistory(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(GetFixPlanHistory)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("GetFixPlanHistoryAsync")]
    [OpenApiOperation("get", "FixPlans")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<FixPlanDto>))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixPlans/users/{id:Guid}/history")]
                                          CancellationToken cancellationToken,
                                          Guid id)
    {
      return await GetFixPlanHistoryAsync(cancellationToken, id);
    }

    public async Task<IActionResult> GetFixPlanHistoryAsync(CancellationToken cancellationToken, Guid userId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (userId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(userId)} is not valid");
      }

      var result = await _fixPlanMediator.GetFixPlanHistoryAsync(userId, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}
