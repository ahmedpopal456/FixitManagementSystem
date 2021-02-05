using System.Collections.Generic;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases.Enums;
using Fixit.Core.DataContracts;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans
{
  public class FakeFixPlanPhaseRequestDtoSeeder : IFakeSeederAdapter<FixPhaseStatusUpdateRequestDto>
  {
    public IList<FixPhaseStatusUpdateRequestDto> SeedFakeDtos()
    {
      FixPhaseStatusUpdateRequestDto firstFixPhaseRequestDto = new FixPhaseStatusUpdateRequestDto
      {
        Status = PhaseStatuses.InProgress,
      };

      FixPhaseStatusUpdateRequestDto secondFixPhaseRequestDto = null;

      return new List<FixPhaseStatusUpdateRequestDto> { firstFixPhaseRequestDto, secondFixPhaseRequestDto };
    }
  }
}
