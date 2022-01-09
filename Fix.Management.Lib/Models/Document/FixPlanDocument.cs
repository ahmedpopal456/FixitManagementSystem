using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Database;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Details;
using Fixit.Core.DataContracts.Fixes.Tags;
using Fixit.Core.DataContracts.FixPlans.BillingDetails;
using Fixit.Core.DataContracts.FixPlans.Enums;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Enums;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks.Enums;
using Fixit.Core.DataContracts.Seeders;
using Fixit.Core.DataContracts.Users;

namespace Fix.Management.Lib.Models.Document
{
  [DataContract]
  public class FixPlanDocument : DocumentBase, IFakeSeederAdapter<FixPlanDocument>
  {
    [DataMember]
    public Guid FixId { get; set; }

    [DataMember]
    public long CreatedTimestampUtc { get; set; }

    [DataMember]
    public long UpdatedTimestampUtc { get; set; }

    [DataMember]
    public bool IsBookmarked { get; set; }

    [DataMember]
    public FixPlanProposalStates ProposalState { get; set; }

    [DataMember]
    public float TotalCost { get; set; }

    [DataMember]
    public Guid ActivePhaseId { get; set; }

    [DataMember]
    public FixPlanBillingDetailsDto BillingDetails { get; set; }

    [DataMember]
    public IEnumerable<FixPhaseDto> Phases { get; set; }

    [DataMember]
    public UserSummaryDto CreatedByCraftsman { get; set; }

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
