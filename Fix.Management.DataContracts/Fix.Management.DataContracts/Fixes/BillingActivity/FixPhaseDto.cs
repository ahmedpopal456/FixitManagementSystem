using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.BillingActivity
{
    [DataContract]
    class FixPhaseDto
    {
        [Required]
        [DataMember]
        public Guid Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public PhaseStatusEnum Status { get; set; }

        [Required]
        [DataMember]
        public PhaseBillingActivityDto PhaseBillingActivity { get; set; }

        [Required]
        [DataMember]
        public List<PhaseTaskDto> FixTasks { get; set; }
    }
}
