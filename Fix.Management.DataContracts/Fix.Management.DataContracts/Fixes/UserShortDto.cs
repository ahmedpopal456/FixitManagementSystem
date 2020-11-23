using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes
{
    [DataContract]
    class UserShortDto
    {
        [Required]
        [DataMember]
        public Guid ID { get; set; }

        [Required]
        [DataMember]
        public string FirstName { get; set; }

        [Required]
        [DataMember]
        public string LastName { get; set; }
    }
}
