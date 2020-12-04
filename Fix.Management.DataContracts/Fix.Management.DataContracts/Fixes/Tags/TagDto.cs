using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Tags
{
  [DataContract]
  public class TagDto
  {
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string GroupId { get; set; }

    [DataMember]
    public TagStatisticsDto Statistics { get; set; }
  }
}
