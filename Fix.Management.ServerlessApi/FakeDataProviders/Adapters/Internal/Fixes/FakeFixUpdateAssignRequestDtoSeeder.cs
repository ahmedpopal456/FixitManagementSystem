using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Fixes.Cost;
using Fixit.Core.DataContracts.Users;
using System;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Fixes
{
  public class FakeFixUpdateAssignRequestDtoSeeder : IFakeSeederAdapter<FixUpdateAssignRequestDto>
  {
    public IList<FixUpdateAssignRequestDto> SeedFakeDtos()
    {
      FixUpdateAssignRequestDto firstFixDocument = new FixUpdateAssignRequestDto
      {
        AssignedToCraftsman = new UserSummaryDto()
        {
          Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea69"),
          FirstName = "Old",
          LastName = "McDonald"
        },
        ClientEstimatedCost = new FixCostRangeDto()
        {
          MaximumCost = 9000,
          MinimumCost = 300
        },
        SystemCalculatedCost = (float)123.45,
        CraftsmanEstimatedCost = new FixCostEstimationDto()
        {
          Cost = (float) 5000,
          Comment = "Take it or leave it"
        },
        UpdatedByUser = new UserSummaryDto()
        {
          Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea93"),
          FirstName = "Mary",
          LastName = "Lamb"
        }
      };

      FixUpdateAssignRequestDto secondFixDocument = null;

      return new List<FixUpdateAssignRequestDto>
      {
        firstFixDocument,
        secondFixDocument
      };
    }
  }
}
