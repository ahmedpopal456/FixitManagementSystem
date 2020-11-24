using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class WorkdayRangeDto
    {
        [Required]
        [DataMember]
        public long From { get; set; }

        [Required]
        [DataMember]
        public long To { get; set; }
    }
}
