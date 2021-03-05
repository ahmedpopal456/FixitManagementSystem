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
using Fixit.Core.Storage.Queue.Mediators;
using Fix.Management.ServerlessApi.Mediators.FixLocations;
using Fix.Management.ServerlessApi.Mediators;
using Fix.Management.ServerlessApi.Mediators.FixTag;

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

      builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());
      builder.Services.AddSingleton<IDatabaseMediator>(factory.CreateCosmosClient());
      builder.Services.AddSingleton<IQueueServiceClientMediator>(storageFactory.CreateQueueServiceClientMediator());
      builder.Services.AddSingleton<IFixMediator, FixMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var queueMediator = provider.GetService<IQueueServiceClientMediator>();
        var configuration = provider.GetService<IConfiguration>();
        return new FixMediator(mapper, configuration, queueMediator, databaseMediator);
      });


      builder.Services.AddSingleton<IFixPlanMediator, FixPlanMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();

        return new FixPlanMediator(mapper, configuration, databaseMediator);
      });
      
      builder.Services.AddSingleton<IFixLocationMediator, FixLocationMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();

        return new FixLocationMediator(mapper, configuration, databaseMediator);
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
