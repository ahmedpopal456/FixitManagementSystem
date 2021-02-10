﻿using AutoMapper;
using Fixit.Core.DataContracts.Fixes.Operations.Requests;
using Fixit.Core.DataContracts.Fixes.Operations.Responses;
using Fix.Management.ServerlessApi.Models.Document;

namespace Fix.Management.ServerlessApi.Mappers
{
  public class FixManagementMapper : Profile
  {
    public FixManagementMapper()
    {
      // Fix Response
      CreateMap<FixDocument, FixResponseDto>()
        .ForMember(fix => fix.AssignedToCraftsman, opts => opts.MapFrom(dto => dto.AssignedToCraftsman))
        .ForMember(fix => fix.Details, opts => opts.MapFrom(dto => dto.Details))
        .ForMember(fix => fix.Images, opts => opts.MapFrom(dto => dto.Images))
        .ForMember(fix => fix.Tags, opts => opts.MapFrom(dto => dto.Tags))
        .ForMember(fix => fix.Location, opts => opts.MapFrom(dto => dto.Location))
        .ForMember(fix => fix.Schedule, opts => opts.MapFrom(dto => dto.Schedule))
        .ForMember(fix => fix.ClientEstimatedCost, opts => opts.MapFrom(dto => dto.ClientEstimatedCost))
        .ForMember(fix => fix.SystemCalculatedCost, opts => opts.MapFrom(dto => dto.SystemCalculatedCost))
        .ForMember(fix => fix.CraftsmanEstimatedCost, opts => opts.MapFrom(dto => dto.CraftsmanEstimatedCost))
        .ForMember(fix => fix.CreatedTimestampUtc, opts => opts.MapFrom(dto => dto.CreatedTimestampUtc))
        .ForMember(fix => fix.CreatedByClient, opts => opts.MapFrom(dto => dto.CreatedByClient))
        .ForMember(fix => fix.UpdatedTimestampUtc, opts => opts.MapFrom(dto => dto.UpdatedTimestampUtc))
        .ForMember(fix => fix.UpdatedByUser, opts => opts.MapFrom(dto => dto.UpdatedByUser))
        .ForMember(fix => fix.Status, opts => opts.MapFrom(dto => dto.Status))
        .ReverseMap();

      // Create Fixes Request
      CreateMap<FixCreateRequestDto, FixDocument>()
        .ForMember(document => document.Tags, opts => opts.MapFrom(dto => dto.Tags))
        .ForMember(document => document.Details, opts => opts.MapFrom(dto => dto.Details))
        .ForMember(document => document.Images, opts => opts.MapFrom(dto => dto.Images))
        .ForMember(document => document.Location, opts => opts.MapFrom(dto => dto.Location))
        .ForMember(document => document.Schedule, opts => opts.MapFrom(dto => dto.Schedule))
        .ForMember(document => document.CreatedByClient, opts => opts.MapFrom(dto => dto.CreatedByClient))
        .ForMember(document => document.UpdatedByUser, opts => opts.MapFrom(dto => dto.UpdatedByUser))
        .ForMember(document => document.Status, opts => opts.MapFrom(dto => dto.Status))
        .ForMember(document => document.id, opts => opts.Ignore())
        .ReverseMap();

      // Update Fixes Request
      CreateMap<FixUpdateRequestDto, FixDocument>()
        .ForMember(document => document.AssignedToCraftsman, opts => opts.Ignore())
        .ForMember(document => document.Tags, opts => opts.MapFrom(dto => dto.Tags))
        .ForMember(document => document.Details, opts => opts.MapFrom(dto => dto.Details))
        .ForMember(document => document.Images, opts => opts.MapFrom(dto => dto.Images))
        .ForMember(document => document.Location, opts => opts.MapFrom(dto => dto.Location))
        .ForMember(document => document.Schedule, opts => opts.MapFrom(dto => dto.Schedule))
        .ForMember(document => document.ClientEstimatedCost, opts => opts.Ignore())
        .ForMember(document => document.SystemCalculatedCost, opts => opts.Ignore())
        .ForMember(document => document.CraftsmanEstimatedCost, opts => opts.Ignore())
        .ForMember(document => document.UpdatedByUser, opts => opts.MapFrom(dto => dto.UpdatedByUser))
        .ForMember(document => document.Status, opts => opts.Ignore())
        .ForMember(document => document.id, opts => opts.Ignore())
        .ReverseMap();

      // Update Fix Assigned Request
      CreateMap<FixUpdateAssignRequestDto, FixDocument>()
        .ForMember(document => document.AssignedToCraftsman, opts => opts.MapFrom(dto => dto.AssignedToCraftsman))
        .ForMember(document => document.ClientEstimatedCost, opts => opts.MapFrom(dto => dto.ClientEstimatedCost))
        .ForMember(document => document.SystemCalculatedCost, opts => opts.MapFrom(dto => dto.SystemCalculatedCost))
        .ForMember(document => document.CraftsmanEstimatedCost, opts => opts.MapFrom(dto => dto.CraftsmanEstimatedCost))
        .ForMember(document => document.UpdatedByUser, opts => opts.MapFrom(dto => dto.UpdatedByUser))
        .ForMember(document => document.Status, opts => opts.Ignore())
        .ForMember(document => document.id, opts => opts.Ignore())
        .ReverseMap();

      // Get Fix Cost Response
      CreateMap<FixDocument, FixCostResponseDto>()
        .ForMember(fix => fix.ClientEstimatedCost, opts => opts.MapFrom(dto => dto.ClientEstimatedCost))
        .ForMember(fix => fix.SystemCalculatedCost, opts => opts.MapFrom(dto => dto.SystemCalculatedCost))
        .ForMember(fix => fix.CraftsmanEstimatedCost, opts => opts.MapFrom(dto => dto.CraftsmanEstimatedCost))
        .ReverseMap();
    }
  }
}