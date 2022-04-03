using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Fix.Management.ServerlessApi.Mediators.Fixes;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.Storage.Queue.Mediators;
using Fixit.Core.Networking.Local.NMS;

namespace Fix.Management.ServerlessApi.UnitTests.FixesUnitTests
{
  [TestClass]
  public class FixMediatorTests : TestBase, IFixMediatorTests
  {
    private FixMediator _fixMediator;

    // Fake data
    private IEnumerable<FixDocument> _fakeFixDocuments;
    private IEnumerable<FixCreateRequestDto> _fakeFixCreateRequestDtos;
    private IEnumerable<FixUpdateRequestDto> _fakeFixUpdateRequestDtos;
    private IEnumerable<FixUpdateAssignRequestDto> _fakeFixUpdateAssignRequestDtos;

    // DB and table name + Queue Storage

    private readonly string _fixDatabasebName = "TestDatabaseName";
    private readonly string _fixDataTableName = "TestTableName";
    private readonly string _fixQueueStorageName = "TestQueueName";
    private readonly string _chatQueueStorageName = "TestQueueName";

    #region TestInitialize
    [TestInitialize]
    public void TestInitialize()
    {
      // Setup all needed Interfaces to project test controllers
      _configuration = new Mock<IConfiguration>();
      _databaseMediator = new Mock<IDatabaseMediator>();
      _databaseTableMediator = new Mock<IDatabaseTableMediator>();
      _databaseTableEntityMediator = new Mock<IDatabaseTableEntityMediator>();
      _queueStorageMediator = new Mock<IQueueServiceClientMediator>();
      _queueStorageEntityMediator = new Mock<IQueueClientMediator>();
      _chatQueueStorageMediator = new Mock<IQueueServiceClientMediator>();
      _chatQueueStorageEntityMediator = new Mock<IQueueClientMediator>();
      var _fixNmsHttpClient = new Mock<IFixNmsHttpClient>();

      // Create fake data objects
      _fakeFixDocuments = _fakeDtoSeedFactory.CreateSeederFactory<FixDocument>(new FixDocument());
      _fakeFixCreateRequestDtos = _fakeDtoSeedFactory.CreateSeederFactory<FixCreateRequestDto>(new FixCreateRequestDto());
      _fakeFixUpdateRequestDtos = _fakeDtoSeedFactory.CreateSeederFactory<FixUpdateRequestDto>(new FixUpdateRequestDto());
      _fakeFixUpdateAssignRequestDtos = _fakeDtoSeedFactory.CreateSeederFactory<FixUpdateAssignRequestDto>(new FixUpdateAssignRequestDto());


      _databaseMediator.Setup(databaseMediator => databaseMediator.GetDatabase(_fixDatabasebName))
                      .Returns(_databaseTableMediator.Object);
      _databaseTableMediator.Setup(databaseTableMediator => databaseTableMediator.GetContainer(_fixDataTableName))
                            .Returns(_databaseTableEntityMediator.Object);
      _queueStorageMediator.Setup(queueStorageMediator => queueStorageMediator.GetQueueClient(_fixQueueStorageName))
                            .Returns(_queueStorageEntityMediator.Object);
      _chatQueueStorageMediator.Setup(queueStorageMediator => queueStorageMediator.GetQueueClient(_chatQueueStorageName))
                               .Returns(_chatQueueStorageEntityMediator.Object);

      _fixMediator = new FixMediator(_mapperConfiguration.CreateMapper(),
                                     _queueStorageMediator.Object,
                                     _chatQueueStorageMediator.Object,
                                     _databaseMediator.Object,
                                     _fixNmsHttpClient.Object,
                                     _fixDatabasebName,
                                     _fixDataTableName,
                                     _fixQueueStorageName,
                                     _chatQueueStorageName);
    }
    #endregion


    #region CreateFixAsync


    [TestMethod]
    public async Task CreateFixAsync_CreateRequestSuccess_ReturnsSuccess()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      var documentCollection = new CreateDocumentDto<FixDocument>()
      {
        Document = _fakeFixDocuments.First(),
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var queueResponse = new OperationStatus() { IsOperationSuccessful = true };
      var fixCreateRequestDto = _fakeFixCreateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.CreateItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(documentCollection);
      _queueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(queueResponse);

      // Act
      var actionResult = await _fixMediator.CreateFixAsync(fixCreateRequestDto, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
    }

    #endregion

    #region UpdateFixAsync

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAsync_FixIdNotFound_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        OperationException = new Exception()
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateRequestDto = _fakeFixUpdateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAsync(fixIdGuid, fixUpdateRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
      Assert.IsNotNull(actionResult.OperationException);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAsync_GetRequestException_ReturnsException(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateRequestDto = _fakeFixUpdateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAsync(fixIdGuid, fixUpdateRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
      Assert.IsNotNull(actionResult.OperationException);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAsync_UpdateRequestFailure_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = false };
      var fixUpdateRequestDto = _fakeFixUpdateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAsync(fixIdGuid, fixUpdateRequestDto, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAsync_UpdateRequestException_ReturnsException(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };
      var fixUpdateRequestDto = _fakeFixUpdateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAsync(fixIdGuid, fixUpdateRequestDto, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAsync_GetAndUpdateRequestsSuccess_ReturnsSuccess(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateRequestDto = _fakeFixUpdateRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAsync(fixIdGuid, fixUpdateRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
      Assert.AreEqual(fixUpdateRequestDto.Details, actionResult.Details);
      Assert.IsTrue(fixUpdateRequestDto.Images.SequenceEqual(actionResult.Images));
      Assert.AreEqual(fixUpdateRequestDto.Location, actionResult.Location);
      Assert.IsTrue(fixUpdateRequestDto.Schedule.SequenceEqual(actionResult.Schedule));
      Assert.AreEqual(fixUpdateRequestDto.UpdatedByUser, actionResult.UpdatedByUser);
    }
    #endregion

    #region UpdateFixAssignAsync

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAssignAsync_FixIdNotFound_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        OperationException = new Exception()
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateAssignRequestDto = _fakeFixUpdateAssignRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);
      _chatQueueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAssignAsync(fixIdGuid, fixUpdateAssignRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
      Assert.IsNotNull(actionResult.OperationException);
      _chatQueueStorageEntityMediator.Verify(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAssignAsync_GetRequestException_ReturnsException(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateAssignRequestDto = _fakeFixUpdateAssignRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);
      _chatQueueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAssignAsync(fixIdGuid, fixUpdateAssignRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
      Assert.IsNotNull(actionResult.OperationException);
      _chatQueueStorageEntityMediator.Verify(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAssignAsync_UpdateRequestFailure_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = false };
      var fixUpdateAssignRequestDto = _fakeFixUpdateAssignRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);
      _chatQueueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAssignAsync(fixIdGuid, fixUpdateAssignRequestDto, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
      _chatQueueStorageEntityMediator.Verify(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAssignAsync_UpdateRequestException_ReturnsException(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };
      var fixUpdateAssignRequestDto = _fakeFixUpdateAssignRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);
      _chatQueueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAssignAsync(fixIdGuid, fixUpdateAssignRequestDto, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
      _chatQueueStorageEntityMediator.Verify(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task UpdateFixAssignAsync_GetAndUpdateRequestsSuccess_ReturnsSuccess(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixUpdateAssignRequestDto = _fakeFixUpdateAssignRequestDtos.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);
      _chatQueueStorageEntityMediator.Setup(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      //Act
      var actionResult = await _fixMediator.UpdateFixAssignAsync(fixIdGuid, fixUpdateAssignRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
      Assert.AreEqual(fixUpdateAssignRequestDto.AssignedToCraftsman, actionResult.AssignedToCraftsman);
      Assert.AreEqual(fixUpdateAssignRequestDto.ClientEstimatedCost, actionResult.ClientEstimatedCost);
      Assert.AreEqual(fixUpdateAssignRequestDto.SystemCalculatedCost, actionResult.SystemCalculatedCost);
      Assert.AreEqual(fixUpdateAssignRequestDto.CraftsmanEstimatedCost, actionResult.CraftsmanEstimatedCost);
      Assert.AreEqual(fixUpdateAssignRequestDto.UpdatedByUser, actionResult.UpdatedByUser);
      _chatQueueStorageEntityMediator.Verify(queueStorageEntityMediator => queueStorageEntityMediator.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()), Times.Once());
    }
    #endregion

    #region GetFixAsync

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task GetFixAsync_FixIdNotFound_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        OperationException = new Exception(),
        IsOperationSuccessful = false
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      //Act
      var actionResult = await _fixMediator.GetFixAsync(fixIdGuid, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task GetFixAsync_GetRequestSuccess_ReturnsSuccess(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      //Act
      var actionResult = await _fixMediator.GetFixAsync(fixIdGuid, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsNotNull(actionResult.Tags);
      Assert.IsNotNull(actionResult.Details);
      Assert.IsNotNull(actionResult.Images);
      Assert.IsNotNull(actionResult.Location);
      Assert.IsNotNull(actionResult.Schedule);
      Assert.IsNotNull(actionResult.CreatedByClient);
      Assert.IsNotNull(actionResult.UpdatedByUser);
    }

    #endregion

    #region GetFixCostAsync

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task GetFixCostAsync_FixIdNotFound_ReturnsFailure(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        OperationException = new Exception()
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      //Act
      var actionResult = await _fixMediator.GetFixCostAsync(fixIdGuid, cancellationToken);

      //Assert
      Assert.IsNull(actionResult);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task GetFixCostAsync_GetRequestSuccess_ReturnsSuccess(string fixId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      //Act
      var actionResult = await _fixMediator.GetFixCostAsync(fixIdGuid, cancellationToken);

      //Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
    }

    #endregion

    #region DeleteFixAsync

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task DeleteFixAsync_FixIdNotFound_ReturnsFailure(string fixId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var operationStatus = new OperationStatus
      {
        IsOperationSuccessful = true,
        OperationException = new Exception()
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.DeleteItemAsync<FixDocument>(fixId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await _fixMediator.DeleteFixAsync(fixIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
      Assert.IsNotNull(actionResult.OperationException);
    }

    [TestMethod]
    [DataRow("d3e9d3e8-d33c-4ed0-b3df-822799b7f971", DisplayName = "Any_FixId")]
    public async Task DeleteFixAsync_DeleteFixSuccess_ReturnsSuccess(string fixId)
    {
      // Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixIdGuid = new Guid(fixId);
      var documentCollection = new DocumentCollectionDto<FixDocument>()
      {
        Results = { _fakeFixDocuments.First() },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.DeleteItemAsync<FixDocument>(fixId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);

      // Act
      var actionResult = await _fixMediator.DeleteFixAsync(fixIdGuid, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
      Assert.IsNull(actionResult.OperationException);
    }
    #endregion


    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _configuration.Reset();
      _databaseMediator.Reset();
      _databaseTableMediator.Reset();
      _databaseTableEntityMediator.Reset();
      _queueStorageMediator.Reset();
      _queueStorageEntityMediator.Reset();
      _chatQueueStorageMediator.Reset();
      _chatQueueStorageEntityMediator.Reset();

      // Clean-up data objects
      _fakeFixDocuments = null;
      _fakeFixCreateRequestDtos = null;
      _fakeFixUpdateRequestDtos = null;
      _fakeFixUpdateAssignRequestDtos = null;
    }
    #endregion

  }
}
