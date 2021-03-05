using Fixit.Core.DataContracts.Seeders;
using System.Collections.Generic;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IList<T> CreateSeederFactory<T>(IFakeSeederAdapter<T> fakeSeederAdapter) where T : class
    {
      return fakeSeederAdapter.SeedFakeDtos();
    }
  }
}
