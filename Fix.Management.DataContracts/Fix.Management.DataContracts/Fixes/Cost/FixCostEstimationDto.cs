using System.Runtime.Serialization;

namespace Fix.Management.DataContracts.Fixes.Cost
{
  [DataContract]
  public class FixCostEstimationDto
  {
    [DataMember]
    public float Cost { get; set; }

    [DataMember]
    public string Comment { get; set; }
  }
}
