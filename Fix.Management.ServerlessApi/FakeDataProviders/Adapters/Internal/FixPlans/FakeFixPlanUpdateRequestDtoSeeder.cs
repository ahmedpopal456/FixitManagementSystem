using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Fixit.Core.DataContracts;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans
{
  public class FakeFixPlanUpdateRequestDtoSeeder : IFakeSeederAdapter<FixPlanUpdateRequestDto>
  {

    public IList<FixPlanUpdateRequestDto> SeedFakeDtos()
    {
      FixPlanUpdateRequestDto firstFixPlan = new FixPlanUpdateRequestDto
      {
        Phases = new List<FixPhaseDto>()
        {
          new FixPhaseDto
          {
            Name = "Start",
            Id = Guid.Parse("3e888ecb-0454-448a-90cd-6d16cc4d44eb"),
            Tasks = new List<FixPhaseTaskDto>()
            {
              new FixPhaseTaskDto()
              {
                Description="Testing",
                Id = Guid.Parse("1e811863-dea8-44c1-a95d-4b0b34a9970e"),
                Name = "Test",
                Order =0
              }         
            }
          }
        }
      };

      FixPlanUpdateRequestDto secondFixPlan = null;

      return new List<FixPlanUpdateRequestDto> { firstFixPlan, secondFixPlan };
    }
  }
}
