using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class PhaseBillingActivityDto
    {
        [Required]
        [DataMember(Name = "InvoiceId")]
        public Guid InvoiceId { get; set; }

        [Required]
        [DataMember(Name = "status")]
        public BillingStatusEnum Status { get; set; }

        [Required]
        [DataMember(Name = "createdTimestampUtc")]
        public long CreatedTimestampUtc { get; set; }
    }
}
