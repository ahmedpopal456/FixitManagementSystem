using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes
{
    [DataContract]
    class Location
    {
        [Required]
        [DataMember]
        public Guid Id { get; set; }

        [Required]
        [DataMember]
        public string Address { get; set; }

        [Required]
        [DataMember]
        public string City { get; set; }

        [Required]
        [DataMember]
        public string Province { get; set; }

        [Required]
        [DataMember]
        public string PostalCode { get; set; }

        [Required]
        [DataMember]
        public string Country { get; set; }
    }
}
