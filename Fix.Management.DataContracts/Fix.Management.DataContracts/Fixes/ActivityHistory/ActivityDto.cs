using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.ActivityHistory
{
    [DataContract]
    class ActivityDto
    {
        [Required]
        [DataMember]
        public ActivityStatusEnum LastFixStatus { get; set; }

        [Required]
        [DataMember]
        public ActivityStatusEnum CurrentFixStatus { get; set; }

        [Required]
        [DataMember]
        public ActivityDetailsDto ActivityDetails { get; set; }
    }
}
