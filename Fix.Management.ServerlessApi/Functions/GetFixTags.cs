using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Mediators.FixTag;
using Fixit.Core.DataContracts.Fixes.Tags;
using Fixit.Core.Security.Authorization.AzureFunctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace Fix.Management.ServerlessApi.Functions
{
  public class GetFixTags : AzureFunctionRoute
  {
    private readonly IFixTagMediator _fixTagMediator;
    public GetFixTags(IFixTagMediator fixTagMediator, IMapper mapper) : base()
    {
      _fixTagMediator = fixTagMediator ?? throw new ArgumentNullException($"{nameof(GetFixTags)} expects a value for {nameof(fixTagMediator)}... null argument was provided");
    }

    [FunctionName("GetFixTags")]
    [OpenApiOperation("get", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(TagDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tags/{topTags:int}")]
                                                CancellationToken cancellationToken,
                                                int topTags)
    {
      return await GetFixTagsAsync(topTags, cancellationToken);
    }

    //get list of tags by pages of popularity 
    public async Task<IActionResult> GetFixTagsAsync(int topTags, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      var result = await _fixTagMediator.GetFixTagAsync(topTags, cancellationToken);

      return new OkObjectResult(result);

    }
  }
}
