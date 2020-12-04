using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Schedule
{
  [DataContract]
  public class FixScheduleRangeDto
  {
    [DataMember]
    public long StartTimestampUtc { get; set; }

    [DataMember]
    public long EndTimestampUtc { get; set; }
  }
}
