using System.Collections.Generic;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.FixPlans.BillingDetails.Enums;
using Fix.Management.DataContracts.FixPlans.Phases.Cost;

namespace Fix.Management.DataContracts.FixPlans.BillingDetails
{
  [DataContract]
  public class FixPlanBillingDetailsDto
  {
    [DataMember]
    public float InitialCost { get; set; }

    [DataMember]
    public IEnumerable<FixPhaseEstimateCostDto> PhaseCosts { get; set; }

    [DataMember]
    public FixPlanBillingTypes BillingType { get; set; }

    [DataMember]
    public float EndCost { get; set; }
  }
}
