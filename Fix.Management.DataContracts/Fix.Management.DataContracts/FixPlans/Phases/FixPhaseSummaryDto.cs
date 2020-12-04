using System.Collections.Generic;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.FixPlans.Phases.Tasks.Schedule;

namespace Fix.Management.DataContracts.FixPlans.Phases
{
  [DataContract]
  public class FixPhaseSummaryDto
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public IEnumerable<FixPhaseTaskScheduleDto> TaskSchedule { get; set; }
  }
}
