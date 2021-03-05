using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Fix.Management.Lib.Models.Document;
using Fix.Management.ServerlessApi.Mediators.FixTag;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fix.Management.ServerlessApi.UnitTests.FixTagUnitTest
{
  [TestClass]
  public class FixTagMediatorTest : TestBase
  {
    private FixTagMediator _fixTagMediator;

    private IEnumerable<FixTagDocument> _fakeFixTag;

    private readonly string _fixDatabasebName = "TestDatabaseName";
    private readonly string _fixDataTableName = "TestTableName";

    [TestInitialize]
    public void TestInitialize()
    {
      _configuration = new Mock<IConfiguration>();
      _databaseMediator = new Mock<IDatabaseMediator>();
      _databaseTableMediator = new Mock<IDatabaseTableMediator>();
      _databaseTableEntityMediator = new Mock<IDatabaseTableEntityMediator>();

      _fakeFixTag = _fakeDtoSeedFactory.CreateSeederFactory<FixTagDocument>(new FixTagDocument());

      _databaseMediator.Setup(databaseMediator => databaseMediator.GetDatabase(_fixDatabasebName))
                      .Returns(_databaseTableMediator.Object);
      _databaseTableMediator.Setup(databaseTableMediator => databaseTableMediator.GetContainer(_fixDataTableName))
                            .Returns(_databaseTableEntityMediator.Object);

      _fixTagMediator = new FixTagMediator(_mapperConfiguration.CreateMapper(), _databaseMediator.Object, _fixDatabasebName, _fixDataTableName);
    }

    [TestMethod]
    public async Task GetFixTagAsync_GetRequestSuccess_ReturnsSuccess()
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      int topTags = 1;
      var documentCollection = new DocumentCollectionDto<FixTagDocument>()
      {
        Results = { },
        IsOperationSuccessful = true
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixTagDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixTagDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixTagMediator.GetFixTagAsync(topTags, cancellationToken);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetFixTagAsync_GetRequestFailure_ReturnsFailure()
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      int topTags = 1;
      var documentCollection = new DocumentCollectionDto<FixTagDocument>()
      {
        Results = { },
        IsOperationSuccessful = false
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixTagDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixTagDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixTagMediator.GetFixTagAsync(topTags, cancellationToken);

      Assert.IsTrue(result.Count() ==0);
    }

    [TestMethod]
    public async Task OnFixCreateAndUpdateTagAsync_GetRequestFailure_ReturnsFailure()
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;

      FixDocument fixDocument = new FixDocument
      {
        Tags = new List<TagDto>
        {
          new TagDto {Name = "Hello"}
        }
      };

      var documentCollection = new DocumentCollectionDto<FixTagDocument>()
      {
        Results = { },
        IsOperationSuccessful = false
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixTagDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixTagDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixTagMediator.OnFixCreateAndUpdateTags(fixDocument, cancellationToken);

      Assert.IsNull(result);
    }
  }
}
