
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Fixes.Details;
using Fixit.Core.DataContracts.Fixes.Files;
using Fixit.Core.DataContracts.Fixes.Locations;
using Fixit.Core.DataContracts.Fixes.Schedule;
using Fixit.Core.DataContracts.Users;
using Fixit.Core.DataContracts.Fixes.Tags;
using System;

namespace Fix.Management.ServerlessApi.FakeDataProviders.Adapters.Internal.Fixes
{
  public class FakeFixCreateRequestDtoSeeder : IFakeSeederAdapter<FixCreateRequestDto>
  {
    public IList<FixCreateRequestDto> SeedFakeDtos()
    {
      FixCreateRequestDto firstFixDocument = new FixCreateRequestDto
      {
        Details = new List<FixDetailsDto>()
        {
          new FixDetailsDto
          {
            Name = "Shower",
            Description = "Need to change shower",
            Category = "Bathroom",
            Type = "New"
          }
        },
        Tags = new List<TagDto>()
        {
          new TagDto
          {
            Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea98"),
            Name = "Bathroom"
          },
          new TagDto
          {
            Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea99"),
            Name = "Toilet"
          }
        },
        Images = new List<FileDto>()
        {
          new FileDto
          {
            Name = "image-bathroom",
            Url = "/image.png"
          }

        },
        Location = new FixLocationDto()
        {
          Address = "123 Something",
          City = "Montreal",
          Province = "Quebec",
          PostalCode = "H4S 202"
        },
        Schedule = new List<FixScheduleRangeDto>()
        {
          new FixScheduleRangeDto
          {
            EndTimestampUtc = 1609102412,
            StartTimestampUtc = 1609102532
          }

        },
        CreatedByClient = new UserSummaryDto()
        {
          Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea93"),
          FirstName = "Mary",
          LastName = "Lamb"
        },
        UpdatedByUser = new UserSummaryDto()
        {
          Id = new Guid("8b418766-4a99-42a8-b6d7-9fe52b88ea93"),
          FirstName = "Mary",
          LastName = "Lamb"
        }
      };

      FixCreateRequestDto secondFixDocument = null;

      return new List<FixCreateRequestDto>
      {
        firstFixDocument,
        secondFixDocument
      };
    }
  }
}
