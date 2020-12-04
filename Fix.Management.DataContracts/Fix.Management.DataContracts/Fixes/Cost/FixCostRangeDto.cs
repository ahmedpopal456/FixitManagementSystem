using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Cost
{
  [DataContract]
  public class FixCostRangeDto
  {
    [DataMember]
    public int MaxCost { get; set; }

    [DataMember]
    public int MinCost { get; set; }
  }
}
