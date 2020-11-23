using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Tags
{
    [DataContract]
    class TagActivityDto
    {
        [Required]
        [DataMember]
        public UserShortDto ModifiedByUser { get; set; }

        [Required]
        [DataMember]
        public long ModifiedTimestampUtc { get; set; }

        // TODO

        [DataMember]
        public string Note { get; set; }
    }
}
