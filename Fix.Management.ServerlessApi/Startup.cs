using AutoMapper;
using Fix.Management.ServerlessApi;
using Fix.Management.ServerlessApi.Managers;
using Fix.Management.ServerlessApi.Mediators;
using Fix.Management.ServerlessApi.Mappers;
using Fixit.Core.Database;
using Fixit.Core.Database.Mediators;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Fix.Management.ServerlessApi
{
  public class Startup : FunctionsStartup
  {
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
      _configuration = (IConfiguration)builder.Services.BuildServiceProvider().GetService(typeof(IConfiguration));

      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new FixPlanManagementMapper());
      });

      builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());
      builder.Services.AddSingleton<IFixPlanMediator, FixPlanMediator>(provider =>
      {
        var mapper = provider.GetService<IMapper>();
        var databaseMediator = provider.GetService<IDatabaseMediator>();
        var configuration = provider.GetService<IConfiguration>();
        return new FixPlanMediator(mapper, configuration, databaseMediator);
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
