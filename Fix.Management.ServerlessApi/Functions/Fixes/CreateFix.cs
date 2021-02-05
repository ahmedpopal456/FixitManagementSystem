using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Fixit.FixManagement.ServerlessApi.Helpers.Fixes;
using Fixit.Core.Security.Authorization.AzureFunctions;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;
using Fix.Management.ServerlessApi.Mediators.Fixes;

namespace Fix.Management.ServerlessApi.Functions.Fixes
{
  public class CreateFix : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public CreateFix(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(CreateFix)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("CreateFix")]
    [OpenApiOperation("post", "Fixes")]
    [OpenApiRequestBody("application/json", typeof(FixCreateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixResponseDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fixes")]
                                                HttpRequestMessage httpRequest,
                                                CancellationToken cancellationToken)
    {
      return await CreateFixAsync(httpRequest, cancellationToken);
    }

    public async Task<IActionResult> CreateFixAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixDtoValidators.IsValidFixCreationRequest(httpRequest.Content, out FixCreateRequestDto fixDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixCreateRequestDto)} is null or has one or more invalid fields...");
      }
      var result = await _fixMediator.CreateFixAsync(fixDto, cancellationToken);
      if (!result.IsOperationSuccessful && result.OperationException == null)
      {
        return new NotFoundObjectResult($"A fix was not able to be created..");
      }
      return new OkObjectResult(result);

    }

  }
}
