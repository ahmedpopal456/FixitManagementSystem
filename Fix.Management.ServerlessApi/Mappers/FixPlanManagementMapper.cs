using System;
using Fixit.Core.DataContracts.FixPlans;
using AutoMapper;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts.FixPlans.Operations.Requests.FixPlans;
using Fixit.Core.DataContracts.FixPlans.Phases;
using Fixit.Core.DataContracts.FixPlans.Phases.Tasks;

namespace Fix.Management.ServerlessApi.Mappers
{
  public class FixPlanManagementMapper : Profile
  {
    public FixPlanManagementMapper()
    {
      CreateMap<FixPlanCreateRequestDto, FixPlanDocument>()
        .ForMember(fixPlan => fixPlan.FixId, opts => opts.MapFrom(dto => dto.FixId))
        .ForMember(fixPlan => fixPlan.BillingDetails, opts => opts.MapFrom(dto=>dto.BillingDetails))
        .ForMember(fixPlan => fixPlan.CreatedByCraftsman, opts => opts.MapFrom(dto=>dto.CreatedByCraftsman))
        .ForMember(fixPlan => fixPlan.TotalCost, opts => opts.MapFrom(dto => dto.TotalCost))
        .ForMember(fixPlan => fixPlan.CreatedTimestampUtc, opts => opts.MapFrom(dto => dto.CreatedTimestampUtc))
        .ForMember(fixPlan => fixPlan.UpdatedTimestampUtc, opts => opts.MapFrom(dto => dto.UpdatedTimeStampUtc))
        .ForMember(fixPlan => fixPlan.IsBookmarked, opts => opts.MapFrom(dto => dto != null ? dto.IsBookmarked : false))
        .ForMember(fixPlan => fixPlan.Phases, opts => opts.MapFrom(dto => dto != null ? dto.Phases : default))
        .ReverseMap();

      CreateMap<FixPlanUpdateRequestDto, FixPlanDocument>()
        .ForMember(fixPlan => fixPlan.IsBookmarked, opts => opts.MapFrom(dto => dto != null ? dto.IsBookmarked : false))
        .ForMember(fixPlan => fixPlan.UpdatedTimestampUtc, opts => opts.MapFrom(dto => dto.UpdatedTimestampUtc))
        .ForMember(fixPlan => fixPlan.Phases, opts => opts.MapFrom(dto => dto != null ? dto.Phases : default))
        .ReverseMap();

      CreateMap<FixPhaseDto, FixPhaseDto>()
        .ForMember(fixPlan => fixPlan.Name, opts => opts.MapFrom(dto => dto.Name))
        .ForMember(fixPlan => fixPlan.Status, opts => opts.MapFrom(dto => dto.Status))
        .ReverseMap();

      CreateMap<FixPhaseTaskDto, FixPhaseTaskDto>()
        .ForMember(fixPlan => fixPlan.Name, opts => opts.MapFrom(dto => dto.Name))
        .ForMember(fixPlan => fixPlan.Status, opts => opts.MapFrom(dto => dto.Status))
        .ForMember(fixPlan => fixPlan.Description, opts => opts.MapFrom(dto => dto.Description))
        .ReverseMap();

      CreateMap<FixPlanDocument, FixPlanDto>()
        .ForMember(fixPlan => fixPlan.FixId, opts => opts.MapFrom(dto => dto.FixId))
        .ForMember(fixPlan => fixPlan.ActivePhaseId, opts => opts.MapFrom(dto => dto != null ? dto.ActivePhaseId : Guid.Empty))
        .ForMember(fixPlan => fixPlan.BillingDetails, opts => opts.MapFrom(dto => dto.BillingDetails))
        .ForMember(fixPlan => fixPlan.CreatedByCraftsman, opts => opts.MapFrom(dto => dto.CreatedByCraftsman))
        .ForMember(fixPlan => fixPlan.CreatedTimestampUtc, opts => opts.MapFrom(dto => dto.CreatedTimestampUtc))
        .ForMember(fixPlan => fixPlan.UpdatedTimestampUtc, opts => opts.MapFrom(dto => dto.UpdatedTimestampUtc))      
        .ForMember(fixPlan => fixPlan.ProposalState, opts => opts.MapFrom(dto => dto != null ? dto.ProposalState : default))
        .ForMember(fixPlan => fixPlan.TotalCost, opts => opts.MapFrom(dto => dto != null ? dto.TotalCost : default))
        .ForMember(fixPlan => fixPlan.IsBookmarked, opts => opts.MapFrom(dto => dto != null ? dto.IsBookmarked : false))
        .ForMember(fixPlan => fixPlan.Phases, opts => opts.MapFrom(dto => dto != null ? dto.Phases : default))
        .ReverseMap();
    }
  }
}
