using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.Fix
{
    [DataContract]
    class LocationDto
    {
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
    }
}
