using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class Fix
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid ID { get; set; }

        [Required]
        [DataMember(Name = "acceptedByCraftsman")]
        public UserShortDto AcceptedByCraftsman { get; set; }

        [Required]
        [DataMember(Name = "tags")]
        public List<TagDto> Tags { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [Required]
        [DataMember(Name = "category")]
        public FixCategoryEnum Category { get; set; }

        [Required]
        [DataMember(Name = "type")]
        public FixTypeEnum Type { get; set; }

        [Required]
        [DataMember(Name = "sections")]
        public List<FixSectionDto> Sections { get; set; }

        [DataMember(Name = "images")]
        public List<ImageDto> Images { get; set; }

        [Required]
        [DataMember(Name = "location")]
        public FixLocationDto Location { get; set; }

        [Required]
        [DataMember(Name = "availableWorkdays")]
        public FixAvailableWorkdaysDto AvailableWorkdays { get; set; }

        [Required]
        [DataMember(Name = "clientEstimatedBudget")]
        public FixPriceRangeDto ClientEstimatedBudget { get; set; }

        [Required]
        [DataMember(Name = "systemCalculatedCost")]
        public float SystemCalculatedCost { get; set; }

        [Required]
        [DataMember(Name = "craftsmanCostSuggestion")]
        public CostSuggestionDto CraftsmanCostSuggestion { get; set; }

        [Required]
        [DataMember(Name = "createdTimestampUtc")]
        public long CreatedTimestampUtc { get; set; }

        [Required]
        [DataMember(Name = "createdByUser")]
        public UserShortDto CreatedByUser { get; set; }

        [Required]
        [DataMember(Name = "updatedTimestampUtc")]
        public long UpdatedTimestampUtc { get; set; }

        [Required]
        [DataMember(Name = "updatedByUser")]
        public UserShortDto UpdatedByUser { get; set; }

        [Required]
        [DataMember(Name = "status")]
        public FixStatusEnum Status { get; set; }

        [DataMember(Name = "projectPlanSummary")]
        public ProjectPlanSummaryDto ProjectPlanSummary { get; set; }
    }
}
