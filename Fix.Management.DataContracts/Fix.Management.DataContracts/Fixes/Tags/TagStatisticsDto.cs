using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Tags
{
  public class TagStatisticsDto
  {
    [DataMember]
    public long CreatedTimestampUtc { get; set; }

    [DataMember]
    public long UpdatedTimeStampUtc { get; set; }


  }
}
