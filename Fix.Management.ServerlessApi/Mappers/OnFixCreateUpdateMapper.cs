using AutoMapper;
using Fix.Management.Lib.Models.Document;
using Fixit.Core.DataContracts.Fixes.Tags;
using Fixit.Core.DataContracts.Users.Address;

namespace Fix.Management.ServerlessApi.Mappers
{
  public class OnFixCreateUpdateMapper : Profile
  {
    public OnFixCreateUpdateMapper()
    {
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
