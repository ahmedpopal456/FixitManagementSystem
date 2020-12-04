using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Files
{
  [DataContract]
  public class FileDto
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public FileMetadataDto Metadata { get; set; }
  }
}
