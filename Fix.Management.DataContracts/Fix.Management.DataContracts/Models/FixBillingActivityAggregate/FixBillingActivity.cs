using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class FixBillingActivity
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        // TODO
    }
}
