using AutoMapper;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts.Fixes.Locations;
using Fixit.Core.DataContracts.Fixes.Tags;

namespace Fix.Management.ServerlessApi.Mappers
{
  public class OnFixCreateUpdateMapper : Profile
  {
    public OnFixCreateUpdateMapper()
    {
      CreateMap<FixLocationResponseDto, FixLocationDocument>()
        .ForMember(fixLocation => fixLocation.Address, opts => opts.MapFrom(dto => dto.Address))
        .ForMember(fixLocation => fixLocation.City, opts => opts.MapFrom(dto => dto.City))
        .ForMember(fixLocation => fixLocation.PostalCode, opts => opts.MapFrom(dto => dto.PostalCode))
        .ForMember(fixLocation => fixLocation.Province, opts => opts.MapFrom(dto => dto.Province))
        .ForMember(fixLocation => fixLocation.id, opts => opts.Ignore())
        .ForMember(fixLocation => fixLocation.EntityId, opts => opts.Ignore())
        .ReverseMap();

      CreateMap<FixLocationDto, FixLocationDocument>()
        .ForMember(fixLocation => fixLocation.Address, opts => opts.MapFrom(dto => dto.Address))
        .ForMember(fixLocation => fixLocation.City, opts => opts.MapFrom(dto => dto.City))
        .ForMember(fixLocation => fixLocation.PostalCode, opts => opts.MapFrom(dto => dto.PostalCode))
        .ForMember(fixLocation => fixLocation.Province, opts => opts.MapFrom(dto => dto.Province))
        .ForMember(fixLocation => fixLocation.id, opts => opts.Ignore())
        .ForMember(fixLocation => fixLocation.EntityId, opts => opts.Ignore())
        .ReverseMap();

      CreateMap<TagResponseDto, FixTagDocument>()
        .ForMember(fixTag => fixTag.Name, opts => opts.MapFrom(dto => dto.Name))
        .ForMember(fixTag => fixTag.TagId, opts => opts.MapFrom(dto => dto.Id))
        .ForMember(fixTag => fixTag.GroupId, opts => opts.MapFrom(dto => dto.GroupId))
        .ForMember(fixTag => fixTag.Statistics, opts => opts.MapFrom(dto => dto.Statistics))
        .ForMember(fixTag => fixTag.id, opts => opts.Ignore())
        .ForMember(fixTag => fixTag.EntityId, opts => opts.Ignore())
        .ReverseMap();

      CreateMap<TagDto, FixTagDocument>()
        .ForMember(fixTag => fixTag.Name, opts => opts.MapFrom(dto => dto.Name))
        .ForMember(fixTag => fixTag.TagId, opts => opts.MapFrom(dto => dto.Id))
        .ForMember(fixTag => fixTag.GroupId, opts => opts.MapFrom(dto => dto.GroupId))
        .ForMember(fixTag => fixTag.Statistics, opts => opts.MapFrom(dto => dto.Statistics))
        .ForMember(fixTag => fixTag.id, opts => opts.Ignore())
        .ForMember(fixTag => fixTag.EntityId, opts => opts.Ignore())
        .ReverseMap();
    }
  }
}
