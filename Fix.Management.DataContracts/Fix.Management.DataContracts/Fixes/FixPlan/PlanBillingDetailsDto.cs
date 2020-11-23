using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.FixPlan
{
    [DataContract]
    class PlanBillingDetailsDto
    {
        [Required]
        [DataMember]
        public BillingTypeEnum PlanBillingType { get; set; }

        [Required]
        [DataMember]
        public float StartCost { get; set; }

        [DataMember]
        public List<PhaseEstimateCostDto> PhaseCosts { get; set; }

        [Required]
        [DataMember]
        public float EndCost { get; set; }
    }
}
