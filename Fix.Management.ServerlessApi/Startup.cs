using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fixit.Core.Database;
using Fixit.Core.Database.Mediators;
using Fix.Management.ServerlessApi.Mappers;
using Fix.Management.ServerlessApi.Mediators.FixPlans;
using Fix.Management.ServerlessApi.Mediators.Fixes;
using Fix.Management.ServerlessApi;
using Fixit.Core.Storage;
using Fix.Management.ServerlessApi.Mediators;
using Fix.Management.ServerlessApi.Mediators.FixTag;
using Fixit.Core.DataContracts.Decorators.Extensions;
using Fixit.Core.Networking.Extensions;
using Fixit.Core.Networking.Local.NMS;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Fix.Management.ServerlessApi
{
  public class Startup : FunctionsStartup
  {
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {

      _configuration = (IConfiguration)builder.Services.BuildServiceProvider()
                                                       .GetService(typeof(IConfiguration));

      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new FixManagementMapper());
        mc.AddProfile(new FixPlanManagementMapper());
        mc.AddProfile(new OnFixCreateUpdateMapper());

      });

      DatabaseFactory factory = new DatabaseFactory(_configuration["FIXIT-FMS-DB-EP"], _configuration["FIXIT-FMS-DB-KEY"]);
      StorageFactory storageFactory = new StorageFactory(_configuration["FIXIT-FMS-STORAGEACCOUNT-CS"]);
      StorageFactory chatStorageFactory = new StorageFactory(_configuration["FIXIT-CMS-STORAGEACCOUNT-CS"]);

      builder.Services.AddNmsServices("https://fixit-dev-nms-api.azurewebsites.net/");
      builder.Services.AddFixitCoreDecoratorServices();
      builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());
      builder.Services.AddSingleton<IDatabaseMediator>(factory.CreateCosmosClient());
      builder.Services.AddSingleton<IFixMediator, FixMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();
        var fixNmsHttpClient = provider.GetService<IFixNmsHttpClient>();

        var queueMediator = storageFactory.CreateQueueServiceClientMediator();
        var chatQueueMediator = chatStorageFactory.CreateQueueServiceClientMediator();

        return new FixMediator(mapper, configuration, queueMediator, chatQueueMediator, databaseMediator, fixNmsHttpClient);
      });


      builder.Services.AddSingleton<IFixPlanMediator, FixPlanMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();

        return new FixPlanMediator(mapper, configuration, databaseMediator);
      });
      

      builder.Services.AddSingleton<IFixTagMediator, FixTagMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();

        return new FixTagMediator(mapper, configuration, databaseMediator);
      });

      builder.Services.AddTransient<IDatabaseMediator>(configurationProvider =>
      {
        var endpoint = _configuration["FIXIT-FMS-DB-EP"];
        var key = _configuration["FIXIT-FMS-DB-KEY"];

        return new DatabaseFactory(endpoint, key).CreateCosmosClient();
      });
    }
  }
}
