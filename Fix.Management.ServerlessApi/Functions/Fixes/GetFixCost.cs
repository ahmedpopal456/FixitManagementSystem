using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;
using Fix.Management.ServerlessApi.Mediators.Fixes;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;
using Fixit.Core.Security.Authorization.AzureFunctions;
using AutoMapper;

namespace Fix.Management.ServerlessApi.Functions.Fixes
{
  public class GetFixCost : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public GetFixCost(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(GetFixCost)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("GetFixCost")]
    [OpenApiOperation("get", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixCostResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixes/{id:Guid}/cost")]
                                                HttpRequestMessage httpRequest,
                                                CancellationToken cancellationToken,
                                                Guid id)
    {
      return await GetFixAsync(id, cancellationToken);
    }

    public async Task<IActionResult> GetFixAsync(Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixId)} is not valid..");
      }

      var result = await _fixMediator.GetFixCostAsync(fixId, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix with id {fixId} could not be found..");
      }

      return new OkObjectResult(result);

    }
  }

}
