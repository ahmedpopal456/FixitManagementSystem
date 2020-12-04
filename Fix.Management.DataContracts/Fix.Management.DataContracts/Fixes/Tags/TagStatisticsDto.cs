using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Tags
{
  /// <summary>
  /// TODO: find more info
  /// </summary>
  public class TagStatisticsDto
  {
    [DataMember]
    public long CreatedTimestampUtc { get; set; }

    [DataMember]
    public long UpdatedTimeStampUtc { get; set; }


  }
}
