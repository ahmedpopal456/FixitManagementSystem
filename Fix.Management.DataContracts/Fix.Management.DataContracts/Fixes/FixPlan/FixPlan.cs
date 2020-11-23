using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Fix.Management.DataContracts.Fixes.BillingActivity;

namespace Fix.Management.DataContracts.Fixes.FixPlan
{
    [DataContract]
    class FixPlan
    {
        [Required]
        [DataMember]
        public Guid Id { get; set; }

        [Required]
        [DataMember]
        public Guid FixId { get; set; }

        [Required]
        [DataMember]
        public long CreatedTimestampUtc { get; set; }

        [Required]
        [DataMember]
        public bool IsBookmarked { get; set; }

        [Required]
        [DataMember]
        public PlanProposalStateEnum PlanProposalState { get; set; }

        [Required]
        [DataMember]
        public float FinalCost { get; set; }

        [Required]
        [DataMember]
        public PlanBillingDetailsDto PlanBillingDetails { get; set; }

        [DataMember]
        public Guid ActivePhaseId { get; set; }

        [DataMember]
        public List<FixPhaseDto> FixPhases { get; set; }

    }
}
