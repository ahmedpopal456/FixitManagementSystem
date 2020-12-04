using System.Runtime.Serialization;
using Fix.Management.DataContracts.FixPlans.Phases;

namespace Fix.Management.DataContracts.FixPlans
{
  [DataContract]
  public class FixPlanSummaryDto
  {
    [DataMember]
    public FixPhaseSummaryDto Phases { get; set; }
    
    [DataMember]
    public float CompletionPercentage { get; set; }

    [DataMember]
    public int TotalPhaseCount { get; set; }
  }
}
