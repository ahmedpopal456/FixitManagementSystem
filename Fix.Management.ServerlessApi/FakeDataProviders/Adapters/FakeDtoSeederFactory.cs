using Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Fixes;
using Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.Fixes;
using Fix.Management.ServerlessApi.Models.Document;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IFakeSeederAdapter<T> CreateFakeSeeder<T>() where T : class
    {
      string type = typeof(T).Name;

      switch (type)
      {
        case nameof(FixDocument):
          return (IFakeSeederAdapter<T>)new FakeFixDocumentSeeder();
        case nameof(FixCreateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixCreateRequestDtoSeeder();
        case nameof(FixUpdateRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixUpdateRequestDtoSeeder();
        case nameof(FixUpdateAssignRequestDto):
          return (IFakeSeederAdapter<T>)new FakeFixUpdateAssignRequestDtoSeeder();
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
