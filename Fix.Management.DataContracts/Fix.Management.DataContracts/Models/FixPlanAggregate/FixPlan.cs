using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Fix.Management.DataContracts.Models.FixBillingActivityAggregate;
using Fix.Management.DataContracts.Models.FixBillingActivityAggregate;

namespace Fix.Management.DataContracts.Models
{
    [DataContract]
    class FixPlan
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "fixId")]
        public Guid FixId { get; set; }

        [Required]
        [DataMember(Name = "createdTimestampUtc")]
        public long CreatedTimestampUtc { get; set; }

        [Required]
        [DataMember(Name = "isBookmarked")]
        public bool IsBookmarked { get; set; }

        [Required]
        [DataMember(Name = "planProposalState")]
        public PlanProposalStateEnum PlanProposalState { get; set; }

        [Required]
        [DataMember(Name = "finalCost")]
        public float FinalCost { get; set; }

        [Required]
        [DataMember(Name = "planBillingDetails")]
        public FixPlanBillingDetailsDto PlanBillingDetails { get; set; }

        [DataMember(Name = "activePhaseId")]
        public Guid ActivePhaseId { get; set; }

        [DataMember(Name = "fixPhases")]
        public List<FixPhaseDto> FixPhase { get; set; }

    }
}
