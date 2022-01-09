using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fix.Management.ServerlessApi.Mediators.FixTag;
using Fix.Management.Lib.Models.Document;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fix.Management.Triggers
{
  public class OnFixCreateAndUpdateTags
  {
    private readonly IFixTagMediator _fixTagMediator;

    //add constructor
    public OnFixCreateAndUpdateTags(IFixTagMediator fixTagMediator)
    {
      _fixTagMediator = fixTagMediator ?? throw new ArgumentNullException($"{nameof(OnFixCreateAndUpdateTags)} expects a value for {nameof(fixTagMediator)}... null argument was provided");
    }

    [FunctionName("OnFixCreateAndUpdateTags")]
    public async Task RunAsync([CosmosDBTrigger(
            databaseName: "fixit",
            collectionName: "Fixes",
            ConnectionStringSetting = "FIXIT-FMS-DB-CS",
            LeaseCollectionPrefix = "updatetags",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log, CancellationToken cancellationToken)
    {
      var documentEnumerator = input.GetEnumerator();
      
      while (documentEnumerator.MoveNext())
      {
        var fixDocumentString = documentEnumerator.Current.ToString();
        var fixDocument = JsonConvert.DeserializeObject<FixDocument>(fixDocumentString);

        //extract Fix Tag
        var tagTriggerResult = await _fixTagMediator.OnFixCreateAndUpdateTags(fixDocument, cancellationToken);

        log.LogInformation($"Operation Status: {tagTriggerResult.IsOperationSuccessful}, OperationException: {tagTriggerResult.OperationException}, Operation Message: {tagTriggerResult.OperationMessage}");
      }
    }
  }

  
}
