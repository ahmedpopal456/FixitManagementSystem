using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Fix.Management.DataContracts.Models.FixAggregate.ImageAggregate;

namespace Fix.Management.DataContracts.Models.FixAggregate
{
    [DataContract]
    class ImageDto
    {
        [Required]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [Required]
        [DataMember(Name = "imageMetadata")]
        public ImageMetadataDto ImageMetadata { get; set; }
    }
}
