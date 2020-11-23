using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class CostSuggestionDto
    {
        [Required]
        [DataMember(Name = "cost")]
        public float Cost { get; set; }

        [Required]
        [DataMember(Name = "comments")]
        public string Comments { get; set; }
    }
}
