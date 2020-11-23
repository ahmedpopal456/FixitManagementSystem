using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixBillingActivityAggregate
{
    [DataContract]
    class FixPlanBillingActivityDto
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
