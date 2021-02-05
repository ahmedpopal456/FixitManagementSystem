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
using Fixit.Core.Security.Authorization.AzureFunctions;
using Fixit.Core.DataContracts;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Fix.Management.ServerlessApi.Mediators.Fixes;

namespace Fix.Management.ServerlessApi.Functions.Fixes
{
  public class DeleteFix : AzureFunctionRoute
  {
    private readonly IFixMediator _fixMediator;

    public DeleteFix(IFixMediator fixMediator, IMapper mapper) : base()
    {
      _fixMediator = fixMediator ?? throw new ArgumentNullException($"{nameof(DeleteFix)} expects a value for {nameof(fixMediator)}... null argument was provided");
    }

    [FunctionName("DeleteFix")]
    [OpenApiOperation("delete", "Fixes")]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(OperationStatus))]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "fixes/{id:Guid}")]
                                                HttpRequestMessage httpRequest,
                                                CancellationToken cancellationToken,
                                                Guid id)
    {
      return await DeleteFixAsync(id, cancellationToken);
    }

    public async Task<IActionResult> DeleteFixAsync(Guid fixId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (fixId.Equals(Guid.Empty))
      {
        return new BadRequestObjectResult($"{nameof(fixId)} is not valid...");
      }

      var result = await _fixMediator.DeleteFixAsync(fixId, cancellationToken);

      if (!result.IsOperationSuccessful && result.OperationException == null)
      {
        return new NotFoundObjectResult($"A fix with id {fixId} counld not be found..");
      }
      return new OkObjectResult(result);

    }

  }
}
