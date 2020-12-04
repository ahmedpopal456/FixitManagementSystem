using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Fix.Management.DataContracts.Fixes
{
  // TODO: This class will temporary reside here, as it should be 
  //       imported from the User Management System Data Contracts

  [DataContract]
  public class UserSummaryDto
  {
    [Required]
    [DataMember]
    public Guid ID { get; set; }

    [Required]
    [DataMember]
    public string FirstName { get; set; }

    [Required]
    [DataMember]
    public string LastName { get; set; }
  }
}
