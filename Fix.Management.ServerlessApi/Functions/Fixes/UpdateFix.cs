using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Fixit.FixManagement.ServerlessApi.Helpers.Fixes;
using Fixit.Core.Security.Authorization.AzureFunctions;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fix.Management.ServerlessApi.Mediators.Fixes;
using AutoMapper;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;

namespace Fix.Management.ServerlessApi.Functions
{
  public class UpdateFix : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public UpdateFix(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(UpdateFix)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFix")]
    [OpenApiOperation("put", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiRequestBody("application/json", typeof(FixUpdateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixes/{id:Guid}")]
                                                HttpRequestMessage httpRequest,
                                                CancellationToken cancellationToken,
                                                Guid id)
    {
      return await UpdateFixAsync(httpRequest, id, cancellationToken);
    }

    public async Task<IActionResult> UpdateFixAsync(HttpRequestMessage httpRequest, Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixId)} is not valid..");
      }

      if (!FixDtoValidators.IsValidFixUpdateRequest(httpRequest.Content, out FixUpdateRequestDto fixDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixUpdateRequestDto)} is null or has one or more invalid fields...");
      }

      var result = await _fixMediator.UpdateFixAsync(fixId, fixDto, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix with id {fixId} could not be found..");
      }

      return new OkObjectResult(result);

    }

  }
}
