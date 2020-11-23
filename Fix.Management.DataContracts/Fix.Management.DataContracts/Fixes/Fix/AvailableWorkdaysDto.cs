using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class AvailableWorkdaysDto
    {
        [Required]
        [DataMember]
        public List<WorkdayRangeDto> WorkdayRangeDtos { get; set; }

        [Required]
        [DataMember]
        public List<long> Workdays { get; set; }
    }
}
