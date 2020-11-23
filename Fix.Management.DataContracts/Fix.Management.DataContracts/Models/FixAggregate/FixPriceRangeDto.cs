using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class FixPriceRangeDto
    {
        [Required]
        [DataMember(Name = "from")]
        public int From { get; set; }

        [Required]
        [DataMember(Name = "to")]
        public int To { get; set; }
    }
}
