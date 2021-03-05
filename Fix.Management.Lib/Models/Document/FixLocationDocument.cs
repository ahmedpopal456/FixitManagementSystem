using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Database;
using Fixit.Core.DataContracts.Seeders;

namespace Fix.Management.Lib.Models.Document
{
  public class FixLocationDocument : DocumentBase, IFakeSeederAdapter<FixLocationDocument>
  {
    [DataMember]
    public string Address { get; set; }

    [DataMember]
    public string City { get; set; }

    [DataMember]
    public string Province { get; set; }

    [DataMember]
    public string PostalCode { get; set; }

    [DataMember]
    public string LastUsedTimeStampUtc { get; set; }

    public IList<FixLocationDocument> SeedFakeDtos()
    {
      FixLocationDocument firstFixLocationDocument = new FixLocationDocument
      {
        Address = "123 Something",
        City = "Montreal",
        Province = "Quebec",
        PostalCode = "H4S 202"
      };

      FixLocationDocument secondFixLocationDocument = null;

      return new List<FixLocationDocument> { firstFixLocationDocument, secondFixLocationDocument };
    }
  }
}
