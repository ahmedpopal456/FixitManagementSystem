using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class ImageMetadataDto
    {
        [Required]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [Required]
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        [Required]
        [DataMember(Name = "media")]
        public string Media { get; set; } /// link to actual file
    }
}
