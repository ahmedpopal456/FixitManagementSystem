using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Fix.Management.ServerlessApi.Mediators;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fix.Management.ServerlessApi.UnitTests.FixLocationUnitTest
{
  [TestClass]
  public class FixLocationMediatorTest: TestBase
  {
    private FixLocationMediator _fixLocationMediator;

    private IEnumerable<FixLocationDocument> _fakeFixLocation;

    private readonly string _fixDatabasebName = "TestDatabaseName";
    private readonly string _fixDataTableName = "TestTableName";

    [TestInitialize]
    public void TestInitialize()
    {
      _configuration = new Mock<IConfiguration>();
      _databaseMediator = new Mock<IDatabaseMediator>();
      _databaseTableMediator = new Mock<IDatabaseTableMediator>();
      _databaseTableEntityMediator = new Mock<IDatabaseTableEntityMediator>();

      _fakeFixLocation = _fakeDtoSeedFactory.CreateSeederFactory<FixLocationDocument>(new FixLocationDocument());

      // Create fake data objects

      _databaseMediator.Setup(databaseMediator => databaseMediator.GetDatabase(_fixDatabasebName))
                      .Returns(_databaseTableMediator.Object);
      _databaseTableMediator.Setup(databaseTableMediator => databaseTableMediator.GetContainer(_fixDataTableName))
                            .Returns(_databaseTableEntityMediator.Object);

      _fixLocationMediator = new FixLocationMediator(_mapperConfiguration.CreateMapper(), _databaseMediator.Object, _fixDatabasebName, _fixDataTableName);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task GetFixLocationAsync_GetRequestSuccess_ReturnsSuccess(string userId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid userIdGuid = new Guid(userId);
      var documentCollection = new DocumentCollectionDto<FixLocationDocument>()
      {
        Results = { },
        IsOperationSuccessful = true
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixLocationDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixLocationDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixLocationMediator.GetFixLocationAsync(userIdGuid, cancellationToken);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task GetFixLocationAsync_GetRequestFailure_ReturnsFailure(string userId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid userIdGuid = new Guid(userId);
      var documentCollection = new DocumentCollectionDto<FixLocationDocument>()
      {
        Results = { },
        IsOperationSuccessful = false
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixLocationDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixLocationDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixLocationMediator.GetFixLocationAsync(userIdGuid, cancellationToken);

      Assert.IsTrue(result.Count() == 0);

    }

    [TestMethod]
    public async Task CreateUpdateFixLocationAsync_RequestFailure_ReturnsFailure()
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      FixDocument fixLocationDocument = new FixDocument
      {
        Location = new FixLocationDto
        {
          Address = "Hello"
        }
      };

      var documentCollection = new DocumentCollectionDto<FixLocationDocument>()
      {
        Results = { },
        IsOperationSuccessful = false
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixLocationDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixLocationDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixLocationMediator.OnFixCreateAndUpdateFixLocation(fixLocationDocument, cancellationToken);

      Assert.IsNull(result);
    }
  }
}
