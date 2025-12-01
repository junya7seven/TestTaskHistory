using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs;
using AutoMapper;
using Entities;

namespace Application.Helpers
{
    public class HistoryProfile : Profile
    {
        public HistoryProfile()
        {
            CreateMap<History, HistoryDTO>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => src.User.FullName)
                )

                .ForMember(
                    dest => dest.EventType,
                    opt => opt.MapFrom(src => src.EventType.Name)
                );
        }
    }
}
