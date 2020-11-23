using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models
{
    [DataContract]
    class UserShortDto
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid ID { get; set; }

        [Required]
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
    }
}
