using Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans;
using Fix.Management.ServerlessApi.Models;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.Seeders;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IFakeSeederAdapter<T> CreateFakeSeeder<T>() where T : class
    {
      string type = typeof(T).Name;

      switch (type)
      {
        case nameof(FixPlanDocument):
          return (IFakeSeederAdapter<T>)new FakeFixPlanSeeder();
        case nameof(FixPlanCreateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixPlanCreateRequestDtoSeeder();
        case nameof(FixPlanUpdateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixPlanUpdateRequestDtoSeeder();
        case nameof(FixPhaseStatusUpdateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixPlanPhaseRequestDtoSeeder();
        case nameof(FixTaskStatusUpdateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixPlanPhaseTaskRequestDtoSeeder();
        default:
          return null;
      }
    }
  }
}
