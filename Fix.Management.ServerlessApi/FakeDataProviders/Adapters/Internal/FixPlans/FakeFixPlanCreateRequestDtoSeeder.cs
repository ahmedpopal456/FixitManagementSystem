using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts.FixPlans.BillingDetails;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Cost;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Fixit.Core.DataContracts.Seeders;
using Fixit.Core.DataContracts.Users;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans
{
  public class FakeFixPlanCreateRequestDtoSeeder : IFakeSeederAdapter<FixPlanCreateRequestDto>
  {
    public IList<FixPlanCreateRequestDto> SeedFakeDtos()
    {
      FixPlanCreateRequestDto firstFixPlan = new FixPlanCreateRequestDto
      {
        FixId = Guid.Parse("6d757ef9-8fcb-4e6c-83bd-419dafa661fe"),
        CreatedByCraftsman = new UserSummaryDto
        {
          FirstName = "John",
          LastName = "Doe",
          Id = Guid.Parse("495a22d8-f524-498d-beb9-e749a9942859")
          },
          Phases = new List<FixPhaseDto>()
        {
          new FixPhaseDto
          {
            Name = "Start",
            Id = Guid.Parse("8b418766-4a99-42a8-b6d7-9fe52b88ea94"),
            Tasks = new List<FixPhaseTaskDto>
            {
              new FixPhaseTaskDto
              {
                Description="Testing",
                Id= Guid.Parse("21c2c3bb-6d70-4096-899b-085207cae1a5"),
                Name = "Test"
              }
              
            }
          }
        },
          BillingDetails = new FixPlanBillingDetailsDto
          {
            InitialCost = 100,
            PhaseCosts = new List<FixPhaseEstimateCostDto>
            {
              new FixPhaseEstimateCostDto
              {
                PhaseEstimatedCost=2
              }
            },
            EndCost = 100
          },
          TotalCost = 100
        };

      FixPlanCreateRequestDto secondFixPlan = null;

        return new List<FixPlanCreateRequestDto> { firstFixPlan, secondFixPlan };
    }
  }
}
