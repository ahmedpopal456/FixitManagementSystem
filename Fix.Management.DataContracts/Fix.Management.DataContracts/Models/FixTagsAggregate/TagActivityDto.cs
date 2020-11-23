using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixTagsAggregate
{
    [DataContract]
    class TagActivityDto
    {
        [Required]
        [DataMember(Name = "modifiedByUser")]
        public UserShortDto ModifiedByUser { get; set; }

        [Required]
        [DataMember(Name = "modifiedTimestampUtc")]
        public long ModifiedTimestampUtc { get; set; }

        // TODO

        [DataMember(Name = "note")]
        public string Note { get; set; }
    }
}
