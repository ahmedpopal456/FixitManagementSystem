using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixActivityHistoryAggregate
{
    [DataContract]
    class FixActivityHistory
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        // TODO
    }
}
