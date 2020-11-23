using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Fix.Management.DataContracts.Models.FixPlanAggregate;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class FixPlanBillingDetailsDto
    {
        [Required]
        [DataMember(Name = "planBillingType")]
        public FixPlanBillingTypeEnum PlanBillingType { get; set; }

        [Required]
        [DataMember(Name = "startCost")]
        public float StartCost { get; set; }

        [DataMember(Name = "phaseCosts")]
        public List<FixPhaseEstimateCostDto> PhaseCosts { get; set; }

        [Required]
        [DataMember(Name = "endCost")]
        public float EndCost { get; set; }
    }
}
