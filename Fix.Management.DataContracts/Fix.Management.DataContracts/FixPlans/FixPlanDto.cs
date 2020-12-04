using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.Fixes;
using Fix.Management.DataContracts.FixPlans.BillingDetails;
using Fix.Management.DataContracts.FixPlans.Enums;
using Fix.Management.DataContracts.FixPlans.Phases;

namespace Fix.Management.DataContracts.FixPlans
{
  [DataContract]
  public class FixPlanDto
  {
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public Guid FixId { get; set; }

    [DataMember]
    public long CreatedTimestampUtc { get; set; }

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
  }
}
