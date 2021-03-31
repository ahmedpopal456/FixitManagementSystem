using Fix.Management.Lib.Models.Document;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.DataContracts;

namespace Fix.Management.ServerlessApi.Helpers.Fixes
{
  public static class FixDocumentValidators
  {
    public static bool IsNotNullAndOperationSuccessful(DocumentCollectionDto<FixDocument> fixDocumentCollection)
    {
      return fixDocumentCollection != null && fixDocumentCollection.Results != null && fixDocumentCollection.IsOperationSuccessful;
    }

    public static bool IsNotNullAndOperationSuccessful(CreateDocumentDto<FixDocument> creationResponse)
    {
      return creationResponse != null && creationResponse.IsOperationSuccessful && creationResponse.Document != null;
    }

    public static bool IsNotNullAndOperationSuccessful(OperationStatus queueResponse)
    {
      return queueResponse != null && queueResponse.IsOperationSuccessful;
    }
  }
}
