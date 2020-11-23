using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.BillingActivity
{
    [DataContract]
    class PhaseBillingActivityDto
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
