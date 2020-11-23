using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models
{
    [DataContract]
    class FixLocation
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "address")]
        public string Address { get; set; }

        [Required]
        [DataMember(Name = "city")]
        public string City { get; set; }

        [Required]
        [DataMember(Name = "province")]
        public string Province { get; set; }

        [Required]
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }

        [Required]
        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}
