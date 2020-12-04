using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.FixPlans.Phases.Tasks.Enums;

namespace Fix.Management.DataContracts.FixPlans.Phases.Tasks.Schedule
{
  [DataContract]
  public class FixPhaseTaskScheduleDto
  {
    [DataMember]
    public FixPhaseTaskSummaryDto ScheduledTask { get; set; }

    [DataMember]
    public IEnumerable<long> AppointedTimestampsUtc { get; set; }
  }
}
