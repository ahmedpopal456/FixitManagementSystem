using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.ActivityHistory
{
    [DataContract]
    class ActivityDetailsDto
    {
        [Required]
        [DataMember]
        public UserShortDto ModifiedByUser { get; set; }
        
        [Required]
        [DataMember]
        public long ModifiedTimestampUtc { get; set; }

        [Required]
        [DataMember]
        public Guid FixItemId { get; set; }

        [Required]
        [DataMember]
        public ItemTypeEnum FixItemType { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
