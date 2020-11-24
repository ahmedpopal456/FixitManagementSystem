using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.BillingActivity
{
    [DataContract]
    class BillingActivity
    {
        [Required]
        [DataMember]
        public Guid Id { get; set; }

        // TODO
    }
}
