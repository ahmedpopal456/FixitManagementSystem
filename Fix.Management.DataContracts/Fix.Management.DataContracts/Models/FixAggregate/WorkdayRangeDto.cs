using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class WorkdayRangeDto
    {
        [Required]
        [DataMember(Name = "from")]
        public long From { get; set; }

        [Required]
        [DataMember(Name = "to")]
        public long To { get; set; }
    }
}
