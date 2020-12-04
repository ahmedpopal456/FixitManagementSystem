using System;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.FixPlans.Phases.Enums;
using Fix.Management.DataContracts.FixPlans.Phases.Tasks;

namespace Fix.Management.DataContracts.FixPlans.Phases
{
  [DataContract]
  public class FixPhaseDto
  {
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public PhaseStatuses Status { get; set; }

    [DataMember]
    public FixPhaseTaskDto Tasks { get; set; }
  }
}
