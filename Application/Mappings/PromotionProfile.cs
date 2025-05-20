using Application.DTOs.Promotion;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class PromotionProfile : Profile
{
    public PromotionProfile()
    {
        CreateMap<Promotion, PromotionDTO>().ReverseMap();
        CreateMap<Promotion, CreatePromotionDTO>().ReverseMap();
    }
}
