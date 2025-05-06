using Application.DTOs.Game;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDTO>().ReverseMap();
        CreateMap<Game, CreateGameDTO>().ReverseMap();
        CreateMap<Game, UpdateGameDTO>().ReverseMap();
    }
}
