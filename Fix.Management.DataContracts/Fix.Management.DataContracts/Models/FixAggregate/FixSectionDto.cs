using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class FixSectionDto
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "details")]
        public List<FixSectionDetailsDto> Details { get; set; }
    }
}
