using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fix.Management.DataContracts.Fixes.Cost;
using Fix.Management.DataContracts.Fixes.Details;
using Fix.Management.DataContracts.Fixes.Enums;
using Fix.Management.DataContracts.Fixes.Files;
using Fix.Management.DataContracts.Fixes.Locations;
using Fix.Management.DataContracts.Fixes.Schedule;
using Fix.Management.DataContracts.Fixes.Tags;
using Fix.Management.DataContracts.FixPlans;

namespace Fix.Management.DataContracts.Fixes
{
  [DataContract]
  public class FixDto
  {
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public UserSummaryDto AssignedToCraftsman { get; set; }

    [DataMember]
    public IEnumerable<TagDto> Tags { get; set; }

    [DataMember]
    public IEnumerable<FixDetailsDto> Details { get; set; }

    [DataMember]
    public IEnumerable<FileDto> Images { get; set; }

    [DataMember]
    public FixLocationDto Location { get; set; }

    [DataMember]
    public IEnumerable<FixScheduleRangeDto> Schedule { get; set; }

    [DataMember]
    public FixCostRangeDto ClientEstimatedCost { get; set; }

    [DataMember]
    public float SystemCalculatedCost { get; set; }

    [DataMember]
    public FixCostEstimationDto CraftsmanEstimatdCost { get; set; }

    [DataMember]
    public long CreatedTimestampUtc { get; set; }

    [DataMember]
    public UserSummaryDto CreatedByUser { get; set; }

    [DataMember]
    public long UpdatedTimestamp { get; set; }

    [DataMember]
    public UserSummaryDto UpdatedByUser { get; set; }

    [DataMember]
    public FixStatuses Status { get; set; }

    [DataMember]
    public Guid ActivityHistoryId { get; set; }

    [DataMember]
    public Guid PlanBillingId { get; set; }

    [DataMember]
    public FixPlanSummaryDto PlanSummary { get; set; }

    [DataMember]
    public string ClientId { get; set; }
  }
}
