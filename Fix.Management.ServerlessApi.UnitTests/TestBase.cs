using AutoMapper;
using AutoMapper.Configuration;
using Fix.Management.ServerlessApi.FakeDataProviders.Adapters;
using Fix.Management.ServerlessApi.Mappers;
using Fixit.Core.Database.Mediators;
using Fixit.Core.DataContracts.Seeders;
using Fixit.Core.Storage.Queue.Mediators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fix.Management.ServerlessApi.UnitTests
{
  [TestClass]
  public class TestBase
  {
    public IFakeSeederFactory _fakeDtoSeedFactory;

    // Main Object Mocks
    protected Mock<IConfiguration> _configuration;

    // Database System Mocks
    protected Mock<IDatabaseMediator> _databaseMediator;
    protected Mock<IDatabaseTableMediator> _databaseTableMediator;
    protected Mock<IDatabaseTableEntityMediator> _databaseTableEntityMediator;

    //Queue Storage Mock
    protected Mock<IQueueServiceClientMediator> _queueStorageMediator;
    protected Mock<IQueueClientMediator> _queueStorageEntityMediator;
    protected Mock<IQueueServiceClientMediator> _chatQueueStorageMediator;
    protected Mock<IQueueClientMediator> _chatQueueStorageEntityMediator;

    // Mapper
    protected MapperConfiguration _mapperConfiguration = new MapperConfiguration(config =>
    {
      config.AddProfile(new FixPlanManagementMapper());
      config.AddProfile(new FixManagementMapper());
      config.AddProfile(new OnFixCreateUpdateMapper());
    });

    public TestBase()
    {
      _fakeDtoSeedFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
    }

    [AssemblyCleanup]
    public static void AfterSuiteTests()
    {
    }

  }
}
