using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fix.Management.ServerlessApi.Helpers.FixPlans;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AutoMapper;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests;
using System.Net;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;

namespace Fix.Management.ServerlessApi.Functions.FixPlans.Craftsman
{
  public class CreateFixPlan
  {
    private readonly IFixPlanMediator _fixPlanMediator;

    public CreateFixPlan(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(CreateFixPlan)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("CreateFixPlanAsync")]
    [OpenApiOperation("post", "FixPlans")]
    [OpenApiRequestBody("application/json", typeof(FixPlanCreateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPlanCreateRequestDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fixPlans")]
                                          HttpRequestMessage httpRequestMessage,
                                          CancellationToken cancellationToken)
    {
      return await CreateFixPlanAsync(httpRequestMessage, cancellationToken);
    }

    public async Task<IActionResult> CreateFixPlanAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixPlansDtoValidators.IsValidFixPlanRequest(httpRequestMessage.Content, out FixPlanCreateRequestDto fixPlanCreateRequestDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixPlanCreateRequestDto)} is null or has one or more invalid fields...");
      }

      var result = await _fixPlanMediator.CreateFixPlanAsync(fixPlanCreateRequestDto, cancellationToken);

      return new OkObjectResult(result);
    }
  }
}
