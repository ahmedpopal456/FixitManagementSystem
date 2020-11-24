using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes.Fix
{
    [DataContract]
    class ImageDto
    {
        [Required]
        [DataMember]
        public string Id { get; set; }

        [Required]
        [DataMember]
        public ImageMetadataDto ImageMetadata { get; set; }
    }
}
