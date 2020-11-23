using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Models.FixTagsAggregate
{
    [DataContract]
    class FixTags
    {
        [Required]
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "activityDetails")]
        public TagActivityDto ActivityDetails { get; set; }
    }
}
