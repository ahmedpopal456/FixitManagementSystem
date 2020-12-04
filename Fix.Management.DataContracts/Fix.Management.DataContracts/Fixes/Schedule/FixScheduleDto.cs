using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Fix.Management.DataContracts.Fixes.Schedule
{
  [DataContract]
  public class FixScheduleDto
  {
    [DataMember]
    public IEnumerable<FixScheduleRangeDto> ScheduleRanges { get; set; }

    [DataMember]
    public IEnumerable<long> Workdays { get; set; }
  }
}
