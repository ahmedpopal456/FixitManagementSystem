using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Details.Sections
{
  [DataContract]
  public class FixSectionDetailsDto
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Value { get; set; }
  }
}
