using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixActivityHistoryAggregate
{
    [DataContract]
    class FixActivityDto
    {
        [Required]
        [DataMember(Name = "lastFixStatus")]
        public FixActivityStatusEnum LastFixStatus { get; set; }

        [Required]
        [DataMember(Name = "currentFixStatus")]
        public FixActivityStatusEnum CurrentFixStatus { get; set; }

        [Required]
        [DataMember(Name = "activityDetails")]
        public ActivityDetailsDto ActivityDetails { get; set; }
    }
}
