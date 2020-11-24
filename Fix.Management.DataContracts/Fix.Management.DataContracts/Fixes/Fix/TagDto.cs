using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class TagDto
    {
        [Required]
        [DataMember]
        public string ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public int Counter { get; set; }
    }
}
