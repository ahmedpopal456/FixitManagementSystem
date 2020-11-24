using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class PriceRangeDto
    {
        [Required]
        [DataMember]
        public int From { get; set; }

        [Required]
        [DataMember]
        public int To { get; set; }
    }
}
