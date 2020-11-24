using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class CostSuggestionDto
    {
        [Required]
        [DataMember]
        public float Cost { get; set; }

        [Required]
        [DataMember]
        public string Comments { get; set; }
    }
}
