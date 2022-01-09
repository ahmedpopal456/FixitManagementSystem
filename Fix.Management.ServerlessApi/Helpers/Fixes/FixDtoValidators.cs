using Newtonsoft.Json;
using System.Net.Http;
using System;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Users;
using Fixit.Core.DataContracts.Fixes.Files;
using System.Collections.Generic;
using Fixit.Core.DataContracts.Fixes.Details;
using Fixit.Core.DataContracts.Fixes.Tags;
using Fixit.Core.DataContracts.Fixes.Enums;
using Fixit.Core.DataContracts.Users.Address;

namespace Fixit.FixManagement.ServerlessApi.Helpers.Fixes
{
  public static class FixDtoValidators
  {
    public static bool TryValidatingGuid(string id, out Guid resultingGuid)
    {
      bool isValid = Guid.TryParse(id, out var guidId) && !Guid.Empty.Equals(guidId);
      resultingGuid = guidId;

      return isValid;
    }

    public static bool IsNotValidLocationDto(AddressDto locationDto)
    {
      return string.IsNullOrWhiteSpace(locationDto.FormattedAddress);
    }

    public static bool IsNotValidUserSummaryDto(UserSummaryDto userSummaryDto)
    {
      return string.IsNullOrWhiteSpace(userSummaryDto.Id.ToString())
        || string.IsNullOrWhiteSpace(userSummaryDto.FirstName)
        || string.IsNullOrWhiteSpace(userSummaryDto.LastName);
    }

    public static bool IsNotValidImageDto(IEnumerable<FileDto> imageDto)
    {
      foreach (var imageDtoMember in imageDto)
      {
        if (string.IsNullOrWhiteSpace(imageDtoMember.Name) || string.IsNullOrWhiteSpace(imageDtoMember.Url))
        {
          return true;
        }
      }
      return false;
    }

    public static bool IsNotValidDetailsDto(FixDetailsDto detailsDto)
    {
        if (string.IsNullOrWhiteSpace(detailsDto.Name)
          || string.IsNullOrWhiteSpace(detailsDto.Description)
          || string.IsNullOrWhiteSpace(detailsDto.Category)
          || string.IsNullOrWhiteSpace(detailsDto.Type))
        {
          return true;
        }
      return false;
    }

    public static bool IsNotValidTagsDto(IEnumerable<TagDto> tagsDto)
    {
      foreach (var tagsDtoMember in tagsDto)
      {
        if (string.IsNullOrWhiteSpace(tagsDtoMember.Id.ToString()) || string.IsNullOrWhiteSpace(tagsDtoMember.Name))
        {
          return true;
        }
      }
      return false;
    }


    #region FixCreationRequest
    public static bool IsValidFixCreationRequest(HttpContent httpContent, out FixCreateRequestDto fixDto)
    {
      bool isValid = false;
      fixDto = null;

      try
      {
        var fixDeserialized = JsonConvert.DeserializeObject<FixCreateRequestDto>(httpContent.ReadAsStringAsync().Result);

        if (fixDeserialized != null &&
          fixDeserialized.Tags != null && !IsNotValidTagsDto(fixDeserialized.Tags) &&
          fixDeserialized.Details != null && !IsNotValidDetailsDto(fixDeserialized.Details) &&
          fixDeserialized.Location != null && !IsNotValidLocationDto(fixDeserialized.Location) &&
          fixDeserialized.CreatedByClient != null && !IsNotValidUserSummaryDto(fixDeserialized.CreatedByClient) &&
          fixDeserialized.UpdatedByUser != null && !IsNotValidUserSummaryDto(fixDeserialized.UpdatedByUser)
          )
        {
          isValid = true;
          fixDto = fixDeserialized;
        }

      }
      catch
      {
        // Fall through
      }
      return isValid;
    }
    #endregion

    #region FixUpdateRequest
    public static bool IsValidFixUpdateRequest(HttpContent httpContent, out FixUpdateRequestDto fixDto)
    {
      bool isValid = false;
      fixDto = null;

      try
      {
        var fixDeserialized = JsonConvert.DeserializeObject<FixUpdateRequestDto>(httpContent.ReadAsStringAsync().Result);

        if (fixDeserialized != null
            && fixDeserialized.Tags != null && !IsNotValidTagsDto(fixDeserialized.Tags)
            && fixDeserialized.Details != null && !IsNotValidDetailsDto(fixDeserialized.Details)
            && fixDeserialized.Location != null && !IsNotValidLocationDto(fixDeserialized.Location)
            && fixDeserialized.UpdatedByUser != null && !IsNotValidUserSummaryDto(fixDeserialized.UpdatedByUser)
            && (fixDeserialized.ClientEstimatedCost == null || fixDeserialized.Status != FixStatuses.New))
        {
          isValid = true;
          fixDto = fixDeserialized;
        }

      }
      catch
      {
        // Fall through 
      }
      return isValid;
    }
    #endregion

    #region FixAssignUpdateRequest
    public static bool IsValidFixAssignUpdateRequest(HttpContent httpContent, out FixUpdateAssignRequestDto fixDto)
    {
      bool isValid = false;
      fixDto = null;

      try
      {
        var fixDeserialized = JsonConvert.DeserializeObject<FixUpdateAssignRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixDeserialized != null &&
          fixDeserialized.AssignedToCraftsman != null && !IsNotValidUserSummaryDto(fixDeserialized.AssignedToCraftsman) &&
          fixDeserialized.UpdatedByUser != null && !IsNotValidUserSummaryDto(fixDeserialized.UpdatedByUser) &&
          !fixDeserialized.ClientEstimatedCost.Equals(null) && !fixDeserialized.ClientEstimatedCost.MaximumCost.Equals(null) && !fixDeserialized.ClientEstimatedCost.MinimumCost.Equals(null) &&
          !fixDeserialized.CraftsmanEstimatedCost.Equals(null) && !fixDeserialized.CraftsmanEstimatedCost.Cost.Equals(null) &&
          !fixDeserialized.SystemCalculatedCost.Equals(null))
        {
          isValid = true;
          fixDto = fixDeserialized;
        }

      }
      catch
      {
        // Fall through 
      }
      return isValid;
    }
    #endregion

  }
}
