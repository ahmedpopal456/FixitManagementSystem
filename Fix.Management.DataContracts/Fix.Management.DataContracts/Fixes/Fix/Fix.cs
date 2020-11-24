using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Fix.Management.DataContracts.Models.Fix;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class Fix
    {
        [Required]
        [DataMember]
        public Guid ID { get; set; }

        [Required]
        [DataMember]
        public UserShortDto AcceptedByCraftsman { get; set; }

        [Required]
        [DataMember]
        public List<TagDto> Tags { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string Description { get; set; }

        [Required]
        [DataMember]
        public CategoryEnum Category { get; set; }

        [Required]
        [DataMember]
        public TypeEnum Type { get; set; }

        [Required]
        [DataMember]
        public List<SectionDto> Sections { get; set; }

        [DataMember]
        public List<ImageDto> Images { get; set; }

        [Required]
        [DataMember]
        public LocationDto Location { get; set; }

        [Required]
        [DataMember]
        public AvailableWorkdaysDto AvailableWorkdays { get; set; }

        [Required]
        [DataMember]
        public PriceRangeDto ClientEstimatedBudget { get; set; }

        [Required]
        [DataMember]
        public float SystemCalculatedCost { get; set; }

        [Required]
        [DataMember]
        public CostSuggestionDto CraftsmanCostSuggestion { get; set; }

        [Required]
        [DataMember]
        public long CreatedTimestampUtc { get; set; }

        [Required]
        [DataMember]
        public UserShortDto CreatedByUser { get; set; }

        [Required]
        [DataMember]
        public long UpdatedTimestampUtc { get; set; }

        [Required]
        [DataMember]
        public UserShortDto UpdatedByUser { get; set; }

        [Required]
        [DataMember]
        public StatusEnum Status { get; set; }

        [DataMember]
        public ProjectPlanSummaryDto ProjectPlanSummary { get; set; }
    }
}
