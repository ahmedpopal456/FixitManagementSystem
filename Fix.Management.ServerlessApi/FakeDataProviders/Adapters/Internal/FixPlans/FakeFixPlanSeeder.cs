using System;
using System.Collections.Generic;
using Fix.Management.ServerlessApi.Models.Document;
using Fixit.Core.DataContracts.FixPlans.BillingDetails;
using Fixit.Core.DataContracts.FixPlans.Enums;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Enums;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks.Enums;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Users;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.FixPlans
{
  public class FakeFixPlanSeeder : IFakeSeederAdapter<FixPlanDocument>
  {
    public IList<FixPlanDocument> SeedFakeDtos()
    {
      FixPlanDocument firstFixPlan = new FixPlanDocument
      {
        ActivePhaseId = new Guid("eb60f570-6b60-48bb-8588-3c2d7df38478"),
        FixId = new Guid("78941fbb-0fd3-4005-aa5a-10a1f575132c"),
        IsBookmarked = false,
        ProposalState = FixPlanProposalStates.Tentative,
        CreatedByCraftsman = new UserSummaryDto
        {
          FirstName = "John",
          LastName = "Doe",
          Id = new Guid("18e6ffdb-b39f-469f-85eb-e7646fa20fd3")
        },
        Phases = new List<FixPhaseDto>()
        {
          new FixPhaseDto
          {
            Name = "John",
            Status = PhaseStatuses.New,
            Id = new Guid("1caee959-9565-4354-8e93-89da24c1650e"),
            Tasks = new List<FixPhaseTaskDto>
            {
              new FixPhaseTaskDto()
              {
                Description="Testing",
                Id = new Guid("e0ed5fcf-fcc1-4fe5-8f27-344443f17073"),
                Name = "Test",
                Order =0,
                Status= TaskStatuses.New
              }
            }
          }
        },
        BillingDetails = new FixPlanBillingDetailsDto
        {
          InitialCost = 100,
          BillingType = 0,
          EndCost = 100
        },
        TotalCost = 100
      };

      FixPlanDocument secondFixPlan = null;

      return new List<FixPlanDocument> { firstFixPlan, secondFixPlan };
    }
  }
}
