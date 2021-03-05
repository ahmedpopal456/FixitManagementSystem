using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Database;
using Fixit.Core.DataContracts.Fixes.Tags;
using Fixit.Core.DataContracts.Seeders;

namespace Fix.Management.Lib.Models.Document
{
  public class FixTagDocument : DocumentBase, IFakeSeederAdapter<FixTagDocument>
  {
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public Guid TagId { get; set; }

    [DataMember]
    public string GroupId { get; set; }

    [DataMember]
    public TagStatisticsDto Statistics { get; set; }

    public IList<FixTagDocument> SeedFakeDtos()
    {
      return new List<FixTagDocument>
      {
        new FixTagDocument
        {
           TagId = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea98"),
           Name = "Bathroom"
        }
      };
    }
  }
}
