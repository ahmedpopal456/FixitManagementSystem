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
    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public string FirstName { get; set; }

    [DataMember]
    public string LastName { get; set; }
  }
}
