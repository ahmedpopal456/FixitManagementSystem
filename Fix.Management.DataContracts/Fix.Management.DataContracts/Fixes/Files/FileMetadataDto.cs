using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.Files
{
  public class FileMetadataDto
  {
    [DataMember]
    public long CreatedTimestampUtc { get; set; }

    [DataMember]
    public long UpdatedTimeStampUtc { get; set; }
      
  }
}
