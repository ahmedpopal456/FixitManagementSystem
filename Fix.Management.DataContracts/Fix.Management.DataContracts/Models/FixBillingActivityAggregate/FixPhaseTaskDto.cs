using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class FixPhaseTaskDto
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [Required]
        [DataMember(Name = "order")]
        public int Order { get; set; }

        [Required]
        [DataMember(Name = "status")]
        public TaskStatusEnum Status { get; set; }

        [Required]
        [DataMember(Name = "selectedWorkdays")]
        public List<long> SelectedWorkdays { get; set; }
    }
}
