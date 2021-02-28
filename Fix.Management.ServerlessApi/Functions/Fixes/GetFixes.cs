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
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using Fixit.Core.DataContracts.Fixes.Enums;

namespace Fix.Management.ServerlessApi.Functions.Fixes
{
  public class GetFixes : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public GetFixes(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(GetFix)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName(nameof(GetFixes))]
    [OpenApiOperation("get", "Fixes")]
    [OpenApiParameter("statuses", In = ParameterLocation.Query, Required = false, Type = typeof(IEnumerable<FixStatuses>))]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fixes/users/{id:Guid}")]
                                          HttpRequestMessage httpRequest,
                                          CancellationToken cancellationToken,
                                          Guid id)
    {
      IEnumerable<FixStatuses>? fixStatusesFilter = null;

      var statuses = HttpUtility.ParseQueryString(httpRequest.RequestUri.Query).Get(" ");
      fixStatusesFilter = statuses == null ? fixStatusesFilter : JsonConvert.DeserializeObject<IEnumerable<FixStatuses>>(statuses);

      return await GetFixesAsync(id, cancellationToken, fixStatusesFilter);
    }

    public async Task<IActionResult> GetFixesAsync(Guid userId, CancellationToken cancellationToken, IEnumerable<FixStatuses>? fixStatuses)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (userId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(userId)} is not valid..");
      }

      var fixes = await _fixMediator.GetFixesByUserAsync(userId, cancellationToken, fixStatuses);
      return new OkObjectResult(fixes);
    }
  }
}
