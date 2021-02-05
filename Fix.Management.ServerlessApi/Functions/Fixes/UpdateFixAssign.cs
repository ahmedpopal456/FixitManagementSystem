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
using Fixit.FixManagement.ServerlessApi.Helpers.Fixes;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.Security.Authorization.AzureFunctions;
using AutoMapper;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;

namespace Fix.Management.ServerlessApi.Functions
{
  public class UpdateFixAssign : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public UpdateFixAssign(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixAssign)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixAssign")]
    [OpenApiOperation("put", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiRequestBody("application/json", typeof(FixUpdateAssignRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixes/{id:Guid}/users/{userId:Guid}/assign")]
                                                HttpRequestMessage httpRequest,
                                                CancellationToken cancellationToken,
                                                Guid id,
                                                Guid userId)
    {
      return await UpdateFixAssignAsync(httpRequest, id, userId, cancellationToken);
    }

    public async Task<IActionResult> UpdateFixAssignAsync(HttpRequestMessage httpRequest, Guid fixId, Guid userId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixId)} is not valid..");
      }

      if (userId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(userId)} is not valid..");
      }

      if (!FixDtoValidators.IsValidFixAssignUpdateRequest(httpRequest.Content, out FixUpdateAssignRequestDto fixDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixUpdateAssignRequestDto)} is null or has one or more invalid fields...");
      }
      var result = await _fixMediator.UpdateFixAssignAsync(fixId, fixDto, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Fix with id {fixId} could not be found..");
      }

      return new OkObjectResult(result);

    }

  }
}
