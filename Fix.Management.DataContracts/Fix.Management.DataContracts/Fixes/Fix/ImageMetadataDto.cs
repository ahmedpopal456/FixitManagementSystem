using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class ImageMetadataDto
    {
        [Required]
        [DataMember]
        public string Id { get; set; }

        [Required]
        [DataMember]
        public string ContentType { get; set; }

        [Required]
        [DataMember]
        public string Media { get; set; } /// link to actual file
    }
}
