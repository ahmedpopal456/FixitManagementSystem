using System;
using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.FixPlans.Phases.Cost
{
  [DataContract]
  public class FixPhaseEstimateCostDto
  {
    public Guid PhaseId { get; set; }

    public float PhaseEstimatedCost { get; set; }
  }
}
