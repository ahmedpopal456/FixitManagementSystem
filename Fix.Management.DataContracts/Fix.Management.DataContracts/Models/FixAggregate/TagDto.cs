using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class TagDto
    {
        [Required]
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "counter")]
        public int Counter { get; set; }
    }
}
