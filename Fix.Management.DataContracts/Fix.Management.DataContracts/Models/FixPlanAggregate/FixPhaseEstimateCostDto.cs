using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixPlanAggregate
{
    [DataContract]
    class FixPhaseEstimateCostDto
    {
        [Required]
        [DataMember(Name = "phaseId")]
        public Guid PhaseId { get; set; }

        [Required]
        [DataMember(Name = "phaseEstimateCost")]
        public float PhaseEstimateCost { get; set; }
    }
}
