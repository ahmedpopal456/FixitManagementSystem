using System;
using System.Net.Http;
using Newtonsoft.Json;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases.Enums;
using System.Linq;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks.Enums;

namespace Fix.Management.ServerlessApi.Helpers
{
  public static class FixPlansDtoValidators
  {
    public static bool TryValidatingGuid(string id, out Guid resultingGuid)
    {
      bool isValid = Guid.TryParse(id, out var guidId) && !Guid.Empty.Equals(guidId);
      resultingGuid = guidId;

      return isValid;
    }

    #region FixPlanDocument 

    public static bool IsValidFixPlanRequest(HttpContent httpContent, out FixPlanCreateRequestDto fixPlanCreateRequestDto)
    {
      bool isFixPlanValid = false;
      fixPlanCreateRequestDto = null;

      try
      {
        var fixPlanDeserialized = JsonConvert.DeserializeObject<FixPlanCreateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixPlanDeserialized != null)
        {
          var fixPlanDeserializedPhase = fixPlanDeserialized.Phases.FirstOrDefault();
          var fixPlanDeserializedTask = fixPlanDeserializedPhase.Tasks.FirstOrDefault();
          if (!fixPlanDeserialized.FixId.Equals(Guid.Empty) && 
              !fixPlanDeserialized.CreatedByCraftsman.Equals(null) && !fixPlanDeserialized.CreatedByCraftsman.Id.Equals(Guid.Empty) && !fixPlanDeserialized.CreatedByCraftsman.FirstName.Equals(string.Empty) &&
              !fixPlanDeserialized.Phases.Equals(null) && !fixPlanDeserializedPhase.Name.Equals(string.Empty) && !fixPlanDeserializedPhase.Id.Equals(Guid.Empty) &&
              !fixPlanDeserializedTask.Equals(null) && !fixPlanDeserializedTask.Id.Equals(Guid.Empty) && !fixPlanDeserializedTask.Name.Equals(string.Empty) &&
              !fixPlanDeserialized.BillingDetails.Equals(null) && fixPlanDeserialized.BillingDetails.InitialCost > 0 && fixPlanDeserialized.BillingDetails.EndCost > 0 && fixPlanDeserialized.TotalCost >= 0 )
          {
            fixPlanCreateRequestDto = fixPlanDeserialized;
            isFixPlanValid = true;
          }
        }

      }
      catch
      {
        // Fall through 
      }
      return isFixPlanValid;
    }
    #endregion

    #region FixPhase
    public static bool IsValidFixPlanPhaseRequest(HttpContent httpContent, out FixPhaseStatusUpdateRequestDto fixPhaseRequestDto)
    {
      bool isStatusValid = false;
      fixPhaseRequestDto = null;

      try
      {
        var fixPlanDeserialized = JsonConvert.DeserializeObject<FixPhaseStatusUpdateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixPlanDeserialized != null)
        {
          isStatusValid = true;
          if (!fixPlanDeserialized.Status.Equals(null))
          {
            fixPhaseRequestDto = fixPlanDeserialized;
          }
        }

      }
      catch
      {
        // Fall through 
      }
      return isStatusValid;
    }
    #endregion

    #region FixTask
    public static bool IsValidFixPlanTaskRequest(HttpContent httpContent, out FixTaskStatusUpdateRequestDto fixTaskRequestDto)
    {
      fixTaskRequestDto = null;
      bool isStatusValid = false; ;
      try
      {
        var fixTaskDeserialized = JsonConvert.DeserializeObject<FixTaskStatusUpdateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixTaskDeserialized != null)
        {
          if (!fixTaskDeserialized.Status.Equals(null))
          {
            fixTaskRequestDto = fixTaskDeserialized;
            isStatusValid = true;
          } 
        }

      }
      catch
      {
        // Fall through 
      }
      return isStatusValid;
    }

    #endregion

    #region UpdatePlan
    public static bool IsValidUpdateFixPlanRequest(HttpContent httpContent, out FixPlanUpdateRequestDto fixPlanRequestDto)
    {
      bool isUpdateValid = false;
      fixPlanRequestDto = null;
      
      try
      {
        var fixPlanDeserialized = JsonConvert.DeserializeObject<FixPlanUpdateRequestDto>(httpContent.ReadAsStringAsync().Result);
        if (fixPlanDeserialized != null)
        {
          foreach(var fixPlanPhaseItem in fixPlanDeserialized.Phases)
          {
            if(!fixPlanPhaseItem.Name.Equals(string.Empty) || fixPlanPhaseItem.Tasks != null || fixPlanPhaseItem.Id.Equals(Guid.Empty) ||
               Enum.IsDefined(typeof(PhaseStatuses), fixPlanPhaseItem.Status) || fixPlanPhaseItem.Status != PhaseStatuses.Done ||
               Enum.IsDefined(typeof(PhaseStatuses), fixPlanPhaseItem.Status) || fixPlanPhaseItem.Tasks.FirstOrDefault().Status != TaskStatuses.Done ||
               !fixPlanPhaseItem.Tasks.FirstOrDefault().Name.Equals(string.Empty) || fixPlanPhaseItem.Tasks.FirstOrDefault().Id.Equals(Guid.Empty))
            {
              fixPlanRequestDto = fixPlanDeserialized;
              isUpdateValid = true;
            }
          }
        }

      }
      catch
      {
        // Fall through 
      }
      return isUpdateValid;
    }
    #endregion
  }
}
