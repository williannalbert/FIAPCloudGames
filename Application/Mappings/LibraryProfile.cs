using Application.DTOs.Library;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class LibraryProfile : Profile
{
    public LibraryProfile()
    {
        CreateMap<Library, LibraryDTO>()
            .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Sales.Select(s => s.Game)));
        CreateMap<Library, CreateLibraryDTO>().ReverseMap();

    }
}
