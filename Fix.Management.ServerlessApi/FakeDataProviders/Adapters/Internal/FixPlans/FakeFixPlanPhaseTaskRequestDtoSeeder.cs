using System.Collections.Generic;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks.Enums;
using Fixit.Core.DataContracts.Seeders;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans
{
  public class FakeFixPlanPhaseTaskRequestDtoSeeder : IFakeSeederAdapter<FixTaskStatusUpdateRequestDto>
  {
    public IList<FixTaskStatusUpdateRequestDto> SeedFakeDtos()
    {
      FixTaskStatusUpdateRequestDto firstFixPhaseTaskRequestDto = new FixTaskStatusUpdateRequestDto
      {
        Status = TaskStatuses.New
      };

      FixTaskStatusUpdateRequestDto secondFixPhaseTaskRequestDto = null;

      return new List<FixTaskStatusUpdateRequestDto> { firstFixPhaseTaskRequestDto, secondFixPhaseTaskRequestDto };
    }
  }
}
