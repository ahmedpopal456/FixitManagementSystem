using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.ActivityHistory
{
    [DataContract]
    class ActivityHistory
    {
        [Required]
        [DataMember]
        public Guid Id { get; set; }

        // TODO
    }
}
