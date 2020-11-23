using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class FixAvailableWorkdaysDto
    {
        [Required]
        [DataMember(Name = "workdayRanges")]
        public List<WorkdayRangeDto> WorkdayRangeDtos { get; set; }

        [Required]
        [DataMember(Name = "workdays")]
        public List<long> Workdays { get; set; }
    }
}
