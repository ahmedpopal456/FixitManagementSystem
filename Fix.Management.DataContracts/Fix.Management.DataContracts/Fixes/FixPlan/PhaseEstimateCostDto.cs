using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.FixPlan
{
    [DataContract]
    class PhaseEstimateCostDto
    {
        [Required]
        [DataMember]
        public Guid PhaseId { get; set; }

        [Required]
        [DataMember]
        public float PhaseEstimateCost { get; set; }
    }
}
