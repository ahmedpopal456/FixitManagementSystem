using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Mediators.FixLocations;
using Fixit.Core.DataContracts.Fixes.Locations;
using Fixit.Core.Security.Authorization.AzureFunctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Fix.Management.ServerlessApi.Functions
{
  public class GetFixLocation : AzureFunctionRoute
  {
    private readonly IFixLocationMediator _fixLocationMediator;
    public GetFixLocation(IFixLocationMediator fixLocationMediator, IMapper mapper) : base()
    {
      _fixLocationMediator = fixLocationMediator ?? throw new ArgumentNullException($"{nameof(GetFixLocation)} expects a value for {nameof(fixLocationMediator)}... null argument was provided");
    }

    [FunctionName("GetFixLocation")]
    [OpenApiOperation("get", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixLocationDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId:Guid}/location")]
                                                CancellationToken cancellationToken,
                                                Guid userId)
    {
      return await GetFixLocationAsync(userId,cancellationToken);
    }

    public async Task<IActionResult> GetFixLocationAsync(Guid userId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (userId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(userId)} is not valid..");
      }

      var result = await _fixLocationMediator.GetFixLocationAsync(userId, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}
