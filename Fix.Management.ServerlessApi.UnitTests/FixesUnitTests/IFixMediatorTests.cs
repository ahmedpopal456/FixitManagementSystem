using System.Threading.Tasks;

namespace Fix.Management.ServerlessApi.UnitTests.FixesUnitTests
{
  public interface IFixMediatorTests
  {
    #region Create Fixes 
    Task CreateFixAsync_CreateRequestSuccess_ReturnsSuccess();

    Task CreateFixAsync_DatabaseGetRequestException_ReturnsException();

    Task CreateFixAsync_QueueGetRequestException_ReturnsException();
    #endregion

    #region Delete Fixes 
    Task DeleteFixAsync_DeleteFixSuccess_ReturnsSuccess(string fixId);

    Task DeleteFixAsync_FixIdNotFound_ReturnsFailure(string fixId);
    #endregion

    #region Get Fixes
    Task GetFixAsync_FixIdNotFound_ReturnsFailure(string fixId);

    Task GetFixAsync_GetRequestSuccess_ReturnsSuccess(string fixId);
    #endregion

    #region Get Fix Costs 
    Task GetFixCostAsync_FixIdNotFound_ReturnsFailure(string fixId);

    Task GetFixCostAsync_GetRequestSuccess_ReturnsSuccess(string fixId);
    #endregion

    #region Update Fixes Assign 
    Task UpdateFixAssignAsync_FixIdNotFound_ReturnsFailure(string fixId);

    Task UpdateFixAssignAsync_GetAndUpdateRequestsSuccess_ReturnsSuccess(string fixId);

    Task UpdateFixAssignAsync_GetRequestException_ReturnsException(string fixId);

    Task UpdateFixAssignAsync_UpdateRequestException_ReturnsException(string fixId);

    Task UpdateFixAssignAsync_UpdateRequestFailure_ReturnsFailure(string fixId);
    #endregion

    #region Update Fixes
    Task UpdateFixAsync_FixIdNotFound_ReturnsFailure(string fixId);

    Task UpdateFixAsync_GetAndUpdateRequestsSuccess_ReturnsSuccess(string fixId);

    Task UpdateFixAsync_GetRequestException_ReturnsException(string fixId);

    Task UpdateFixAsync_UpdateRequestException_ReturnsException(string fixId);

    Task UpdateFixAsync_UpdateRequestFailure_ReturnsFailure(string fixId);
    #endregion
  }
}
