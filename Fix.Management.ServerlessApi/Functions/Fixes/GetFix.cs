using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Fixit.Core.Security.Authorization.AzureFunctions;
using Fix.Management.ServerlessApi.Mediators.Fixes;
using AutoMapper;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;

namespace Fix.Management.ServerlessApi.Functions.Fixes
{
  public class GetFix : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public GetFix(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(GetFix)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("GetFix")]
    [OpenApiOperation("get", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixes/{id:Guid}")]
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

      var result = await _fixMediator.GetFixAsync(fixId, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix with id {fixId} could not be found..");
      }

      return new OkObjectResult(result);

    }
  }

}
