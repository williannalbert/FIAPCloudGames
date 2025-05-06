using Application.DTOs.GamePromotion;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class GamePromotionProfile : Profile
{
    public GamePromotionProfile()
    {
        CreateMap<GamePromotion, GamePromotionDTO>().ReverseMap();
    }
}
