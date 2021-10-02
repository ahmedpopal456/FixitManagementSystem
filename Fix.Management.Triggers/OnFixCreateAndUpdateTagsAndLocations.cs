using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fix.Management.ServerlessApi.Mediators.FixLocations;
using Fix.Management.ServerlessApi.Mediators.FixTag;
using Fix.Management.Lib.Models.Document;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fix.Management.Triggers
{
  public class OnFixCreateAndUpdateTagsAndLocations
  {
    private readonly IFixLocationMediator _fixLocationMediator;
    private readonly IFixTagMediator _fixTagMediator;

    //add constructor
    public OnFixCreateAndUpdateTagsAndLocations(IFixLocationMediator fixLocationMediator, IFixTagMediator fixTagMediator)
    {
      _fixLocationMediator = fixLocationMediator ?? throw new ArgumentNullException($"{nameof(OnFixCreateAndUpdateTagsAndLocations)} expects a value for {nameof(fixLocationMediator)}... null argument was provided");
      _fixTagMediator = fixTagMediator ?? throw new ArgumentNullException($"{nameof(OnFixCreateAndUpdateTagsAndLocations)} expects a value for {nameof(fixTagMediator)}... null argument was provided");
    }

    [FunctionName("OnFixCreateAndUpdateLocations")]
    public async Task RunAsync([CosmosDBTrigger(
            databaseName: "fixit",
            collectionName: "Fixes",
            ConnectionStringSetting = "FIXIT-FMS-DB-CS",
            LeaseCollectionPrefix = "updatetagsandlocations",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log, CancellationToken cancellationToken)
    {
      var documentEnumerator = input.GetEnumerator();
      
      while (documentEnumerator.MoveNext())
      {
        var fixDocumentString = documentEnumerator.Current.ToString();
        var fixDocument = JsonConvert.DeserializeObject<FixDocument>(fixDocumentString);

        //extract Fix Location
        var locationTriggerResult = await _fixLocationMediator.OnFixCreateAndUpdateFixLocation(fixDocument, cancellationToken);

        //extract Fix Tag
        var tagTriggerResult = await _fixTagMediator.OnFixCreateAndUpdateTags(fixDocument, cancellationToken);

        log.LogInformation($"Operation Status: {locationTriggerResult.IsOperationSuccessful}, OperationException: {locationTriggerResult.OperationException}, Operation Message: {locationTriggerResult.OperationMessage}");
        log.LogInformation($"Operation Status: {tagTriggerResult.IsOperationSuccessful}, OperationException: {tagTriggerResult.OperationException}, Operation Message: {tagTriggerResult.OperationMessage}");
      }
    }
  }

  
}
