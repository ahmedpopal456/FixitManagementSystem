using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using AutoMapper;
using Fix.Management.ServerlessApi.Helpers;
using Fix.Management.ServerlessApi.Managers;
using Fixit.Core.DataContracts.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.Security.Authorization.AzureFunctions.Access;
using Fixit.Core.Security.Authorization.AzureFunctions.Attributes;
using Fixit.Core.Security.Authorization.Roles.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fix.Management.ServerlessApi
{
  public class UpdateFixPlanStructure
  {
    private readonly IFixPlanMediator _fixPlanMediator;
    private readonly IMapper _mapper;

    public UpdateFixPlanStructure(IFixPlanMediator fixPlanMediator, IMapper mapper) : base()
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanStructure)} expects a value for {nameof(mapper)}... null argument was provided");
      _fixPlanMediator = fixPlanMediator ?? throw new ArgumentNullException($"{nameof(UpdateFixPlanStructure)} expects a value for {nameof(fixPlanMediator)}... null argument was provided");
    }

    [FunctionName("UpdateFixPlanStructure")]
    [OpenApiOperation("put", "FixPlans")]
    [OpenApiRequestBody("application/json", typeof(FixPlanUpdateRequestDto), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(FixPlanDto))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fixPlans/{id:Guid}/structure")]
                                          HttpRequestMessage httpRequestMessage,
                                          CancellationToken cancellationToken,
                                          Guid id)
    {
      return await UpdateFixPlanStructureAsync(httpRequestMessage, cancellationToken, id);
    }

    public async Task<IActionResult> UpdateFixPlanStructureAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken, Guid fixPlanId)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (!FixPlansDtoValidators.IsValidUpdateFixPlanRequest(httpRequestMessage.Content, out FixPlanUpdateRequestDto fixPlanDto))
      {
        return new BadRequestObjectResult($"Either {nameof(FixPlanUpdateRequestDto)} is null or has one or more invalid fields...");
      }

      if (fixPlanId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixPlanId)} is not valid.");
      }

      var result = await _fixPlanMediator.UpdateFixPlanStructureAsync(fixPlanId, fixPlanDto, cancellationToken);

      if (result == null)
      {
        return new NotFoundObjectResult($"Unable to update fix plan with Id {nameof(fixPlanId)}..");
      }

      return new OkObjectResult(result);
    }
  }
}
