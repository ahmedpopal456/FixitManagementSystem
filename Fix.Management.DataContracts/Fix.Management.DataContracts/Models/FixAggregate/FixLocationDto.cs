using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class FixLocationDto
    {
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
    }
}
