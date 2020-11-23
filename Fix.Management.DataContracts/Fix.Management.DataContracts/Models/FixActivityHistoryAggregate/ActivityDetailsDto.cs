using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixActivityHistoryAggregate
{
    [DataContract]
    class ActivityDetailsDto
    {
        [Required]
        [DataMember(Name = "modifiedByUser")]
        public UserShortDto ModifiedByUser { get; set; }
        
        [Required]
        [DataMember(Name = "modifiedTimestampUtc")]
        public long ModifiedTimestampUtc { get; set; }

        [Required]
        [DataMember(Name = "fixItemId")]
        public Guid FixItemId { get; set; }

        [Required]
        [DataMember(Name = "fixItemType")]
        public FixItemTypeEnum FixItemType { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }
    }
}
