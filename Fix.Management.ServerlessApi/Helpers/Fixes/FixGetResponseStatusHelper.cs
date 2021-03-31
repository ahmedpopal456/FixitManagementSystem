using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;

namespace Fix.Management.ServerlessApi.Helpers.Fixes
{
  public static class FixGetResponseStatusHelper
  {
    public static FixResponseDto MapResponseStatus(FixResponseDto result, CreateDocumentDto<FixDocument> response)
    {
      result.OperationException = response.OperationException;
      result.OperationMessage = response.OperationMessage;
      result.IsOperationSuccessful = response.IsOperationSuccessful;

      return result;
    }
   
    public static FixCostResponseDto MapResponseStatus(FixCostResponseDto result, DocumentCollectionDto<FixDocument> response)
    {
      result.OperationException = response.OperationException;
      result.OperationMessage = response.OperationMessage;
      result.IsOperationSuccessful = response.IsOperationSuccessful;

      return result;
    }

    public static FixResponseDto MapResponseStatus(FixResponseDto result, OperationStatus response)
    {
      result.OperationException = response.OperationException;
      result.OperationMessage = response.OperationMessage;
      result.IsOperationSuccessful = response.IsOperationSuccessful;

      return result;
    }

    public static FixResponseDto MapResponseStatus(FixResponseDto result, DocumentCollectionDto<FixDocument> response)
    {
      result.OperationException = response.OperationException;
      result.OperationMessage = response.OperationMessage;
      result.IsOperationSuccessful = response.IsOperationSuccessful;

      return result;
    }
  }
}
