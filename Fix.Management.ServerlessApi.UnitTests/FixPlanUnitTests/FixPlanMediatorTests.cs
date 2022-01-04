using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Fix.Management.Lib.Models.Document;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fixit.Core.DataContracts;
using Fixit.Core.Database.DataContracts.Documents;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fix.Management.ServerlessApi.UnitTests.FixPlanUnitTests
{
  [TestClass]
  public class FixPlanMediatorTests : TestBase
  {
    private FixPlanMediator _fixPlanMediator;

    private IEnumerable<FixPlanDocument> _fakeFixPlan;
    private IEnumerable<FixPlanCreateRequestDto> _fakeFixPlanRequestDtoSeeder;
    private IEnumerable<FixPlanUpdateRequestDto > _fakeFixPlanUpdateRequestDtoSeeder;
    private IEnumerable<FixPhaseStatusUpdateRequestDto> _fakeFixPlanPhaseRequestDtoSeeder;
    private IEnumerable<FixTaskStatusUpdateRequestDto> _fakeFixPlanPhaseTaskRequestDtoSeeder;

    private readonly string _fixPlanDatabasebName= "TestDatabaseName";
    private readonly string _fixPlanDataTableName = "TestTableName";

    [TestInitialize]
    public void TestInitialize()
    {
      _configuration = new Mock<IConfiguration>();
      _databaseMediator = new Mock<IDatabaseMediator>();
      _databaseTableMediator = new Mock<IDatabaseTableMediator>();
      _databaseTableEntityMediator = new Mock<IDatabaseTableEntityMediator>();

      _fakeFixPlan = _fakeDtoSeedFactory.CreateSeederFactory<FixPlanDocument>(new FixPlanDocument());
      _fakeFixPlanRequestDtoSeeder = _fakeDtoSeedFactory.CreateSeederFactory<FixPlanCreateRequestDto>(new FixPlanCreateRequestDto());
      _fakeFixPlanUpdateRequestDtoSeeder = _fakeDtoSeedFactory.CreateSeederFactory<FixPlanUpdateRequestDto>(new FixPlanUpdateRequestDto());
      _fakeFixPlanPhaseRequestDtoSeeder = _fakeDtoSeedFactory.CreateSeederFactory<FixPhaseStatusUpdateRequestDto>(new FixPhaseStatusUpdateRequestDto());
      _fakeFixPlanPhaseTaskRequestDtoSeeder = _fakeDtoSeedFactory.CreateSeederFactory<FixTaskStatusUpdateRequestDto>(new FixTaskStatusUpdateRequestDto());

      _databaseMediator.Setup(databaseMediator => databaseMediator.GetDatabase(_fixPlanDatabasebName))
                      .Returns(_databaseTableMediator.Object);
      _databaseTableMediator.Setup(databaseTableMediator => databaseTableMediator.GetContainer(_fixPlanDataTableName))
                            .Returns(_databaseTableEntityMediator.Object);

      _fixPlanMediator = new FixPlanMediator(_mapperConfiguration.CreateMapper(), _databaseMediator.Object, _fixPlanDatabasebName, _fixPlanDataTableName);
    }

    [TestMethod]
    public async Task CreateFixPlanAsync_CreateRequestSuccess_ReturnsSuccess()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      var documentCollection = new CreateDocumentDto<FixPlanDocument>()
      {
        Document = _fakeFixPlan.First(),
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixPlanCreateRequestDto = _fakeFixPlanRequestDtoSeeder.First();
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.CreateItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(documentCollection);
      
      //Act
      var result = await _fixPlanMediator.CreateFixPlanAsync(fixPlanCreateRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateFixPlanAsync_CreateRequestExceotion_ReturnsException()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      var documentCollection = new CreateDocumentDto<FixPlanDocument>()
      {
        Document = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      //Create document and wrap OpStatus
      var operationStatus = new OperationStatus() { IsOperationSuccessful = true };
      var fixPlanCreateRequestDto = _fakeFixPlanRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.CreateItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(documentCollection);

      //Act
      var result = await _fixPlanMediator.CreateFixPlanAsync(fixPlanCreateRequestDto, cancellationToken);

      //Assert
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName ="FixPlanId")]
    public async Task UpdateFixPlanAsync_UpdateRequestException_ReturnsException(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false,
        OperationException = new Exception()
        
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true,
        OperationException = new Exception()
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanUpdateRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

       var result = await _fixPlanMediator.UpdateFixPlanStructureAsync(fixPlanGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task UpdateFixPlanAsync_UpdateSuccess_ReturnsSuccess(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanUpdateRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateFixPlanStructureAsync(fixPlanGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsNotNull(result);
      Assert.IsNull(result.OperationException);
    }

    [TestMethod]
    public async Task UpdateFixPlanPhaseAsync_UpdateSuccess_ReturnsSuccess()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      string fixPlanId = "65e79f99-c4f9-4beb-81b8-7596c0ed9b3b";
      string fixPhaseId = "1caee959-9565-4354-8e93-89da24c1650e";
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid fixPhaseGuid = new Guid(fixPhaseId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateFixPlanPhaseStatusAsync(fixPlanGuid, fixPhaseGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsNull(result.OperationException);
      Assert.IsNull(result.OperationMessage);
    }

    [TestMethod]
    public async Task UpdateFixPlanPhaseAsync_UpdateFailure_ReturnsFailure()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      string fixPlanId = "65e79f99-c4f9-4beb-81b8-7596c0ed9b3b";
      string fixPhaseId = "ae3efbd9-9e48-4d54-9fb9-e137c19616a8";
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid fixPhaseGuid = new Guid(fixPhaseId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false,
        OperationException = new Exception()
        
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result =await _fixPlanMediator.UpdateFixPlanPhaseStatusAsync(fixPlanGuid, fixPhaseGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    public async Task UpdateFixPlanPhaseTaskAsync_UpdateFailure_ReturnsFailure()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      string fixPlanId = "65e79f99-c4f9-4beb-81b8-7596c0ed9b3b";
      string fixPhaseId = "ae3efbd9-9e48-4d54-9fb9-e137c19616a8";
      string fixPhaseTaskId = "662f4e87-b2e6-4bf9-9228-79cbba7b5c9c";
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid fixPhaseGuid = new Guid(fixPhaseId);
      Guid fixTaskGuid = new Guid(fixPhaseTaskId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseTaskRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateFixPlanTaskStatusAsync(fixPlanGuid, fixPhaseGuid, fixTaskGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    public async Task UpdateFixPlanPhaseTaskAsync_UpdateFailure_ReturnsException()
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      string fixPlanId = "65e79f99-c4f9-4beb-81b8-7596c0ed9b3b";
      string fixPhaseId = "ae3efbd9-9e48-4d54-9fb9-e137c19616a8";
      string fixPhaseTaskId = "662f4e87-b2e6-4bf9-9228-79cbba7b5c9c";
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid fixPhaseGuid = new Guid(fixPhaseId);
      Guid fixTaskGuid = new Guid(fixPhaseTaskId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseTaskRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateFixPlanTaskStatusAsync(fixPlanGuid, fixPhaseGuid, fixTaskGuid, fixPlanUpdateRequestDto, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task UpdateFixPlanClientState_UpdateSuccess_ReturnsSuccess(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanUpdateRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateClientFixPlanProposalStatusAsync(fixPlanGuid, cancellationToken);

      Assert.IsNotNull(result);
      Assert.IsNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task UpdateFixPlanClientState_UpdateFailure_ReturnsFailure(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
      };
      var fixPlanUpdateRequestDto = _fakeFixPlanUpdateRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateClientFixPlanProposalStatusAsync(fixPlanGuid, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task UpdateFixPlanClientPhase_UpdateSuccess_ReturnsSuccess(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid phaseId = new Guid("1caee959-9565-4354-8e93-89da24c1650e");
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = true,
      };

      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateClientFixPlanPhaseStatusAsync(fixPlanGuid, phaseId, cancellationToken);

      Assert.IsNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task UpdateFixPlanClientPhase_UpdateFailure_ReturnsFailure(string fixPlanId)
    {
      //Arrange
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanGuid = new Guid(fixPlanId);
      Guid phaseId = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea94");
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var operationStatus = new OperationStatus()
      {
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var fixPlanUpdateRequestDto = _fakeFixPlanPhaseRequestDtoSeeder.First();

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));
      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.UpsertItemAsync(It.IsAny<FixPlanDocument>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(operationStatus);

      var result = await _fixPlanMediator.UpdateClientFixPlanPhaseStatusAsync(fixPlanGuid, phaseId, cancellationToken);

      Assert.IsFalse(result.IsOperationSuccessful);
      Assert.IsNotNull(result.OperationException);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task GetFixPlanAsync_GetRequestSuccess_ReturnsSuccess(string fixPlanId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanIdGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { _fakeFixPlan.First() },
        IsOperationSuccessful = true
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixPlanMediator.GetFixPlanAsync(fixPlanIdGuid, cancellationToken);

      Assert.IsNotNull(result);
      Assert.IsTrue(result.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task GetFixPlanAsync_GetRequestException_ReturnsException(string fixPlanId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanIdGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixPlanMediator.GetFixPlanAsync(fixPlanIdGuid, cancellationToken);

      Assert.IsNotNull(result.OperationException);
      Assert.IsFalse(result.IsOperationSuccessful);
      
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task DeleteFixPlanAsync_DeleteFixPlanSuccess_ReturnsSuccess(string fixPlanId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanIdGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus { IsOperationSuccessful = true };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.DeleteItemAsync<FixPlanDocument>(fixPlanId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);


      var result = await _fixPlanMediator.DeleteFixPlanAsync(fixPlanIdGuid, cancellationToken);

      Assert.IsNull(result);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task DeleteFixPlanAsync_DeleteFixPlanFailure_ReturnsFailure(string fixPlanId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid fixPlanIdGuid = new Guid(fixPlanId);
      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
        OperationException = new Exception()
      };

      var operationStatus = new OperationStatus { IsOperationSuccessful = false, OperationException = new Exception() };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                  .ReturnsAsync((documentCollection, continuationToken));

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.DeleteItemAsync<FixPlanDocument>(fixPlanId, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(operationStatus);


      var result = await _fixPlanMediator.DeleteFixPlanAsync(fixPlanIdGuid, cancellationToken);

      Assert.IsNull(result);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "FixPlanId")]
    public async Task GetFixPlanHistoryAsync_GetRequestSuccess_ReturnsSuccess(string userId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid userIdGuid = new Guid(userId);

      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { },
        IsOperationSuccessful = true
      };

      var operationStatus = new OperationStatus { IsOperationSuccessful = true };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                   .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixPlanMediator.GetFixPlanHistoryAsync(userIdGuid, cancellationToken);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    [DataRow("56857377-5aa6-4a83-9fbd-867aa26e39d4", DisplayName = "userId")]
    public async Task GetFixPlanHistoryAsync_GetRequestFailure_ReturnsFailure(string userId)
    {
      var cancellationToken = CancellationToken.None;
      string continuationToken = null;
      Guid userIdGuid = new Guid(userId);

      var documentCollection = new DocumentCollectionDto<FixPlanDocument>()
      {
        Results = { },
        IsOperationSuccessful = false,
      };

      var operationStatus = new OperationStatus { IsOperationSuccessful = false };

      _databaseTableEntityMediator.Setup(databaseTableEntityMediator => databaseTableEntityMediator.GetItemQueryableAsync<FixPlanDocument>(continuationToken, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<FixPlanDocument, bool>>>(), null))
                                   .ReturnsAsync((documentCollection, continuationToken));

      var result = await _fixPlanMediator.GetFixPlanHistoryAsync(userIdGuid, cancellationToken);
      
      Assert.IsNotNull(result.ToList().Count);
    }

    #region TestCleanup
    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _configuration.Reset();
      _databaseMediator.Reset();
      _databaseTableMediator.Reset();
      _databaseTableEntityMediator.Reset();
    }
    #endregion

  }
}
