using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class FixPhaseDto
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "status")]
        public PhaseStatusEnum Status { get; set; }

        [Required]
        [DataMember(Name = "phaseBillingActivity")]
        public PhaseBillingActivityDto PhaseBillingActivity { get; set; }

        [Required]
        [DataMember(Name = "fixTasks")]
        public List<FixPhaseTaskDto> FixTasks { get; set; }
    }
}
