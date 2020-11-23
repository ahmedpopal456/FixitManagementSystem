using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.BillingActivity
{
    [DataContract]
    class PlanBillingActivityDto
    {
        [Required]
        [DataMember]
        public Guid InvoiceId { get; set; }

        [Required]
        [DataMember]
        public BillingStatusEnum Status { get; set; }

        [Required]
        [DataMember]
        public long CreatedTimestampUtc { get; set; }
    }
}
