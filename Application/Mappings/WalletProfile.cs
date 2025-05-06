using Application.DTOs.Wallet;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<CreateWalletDTO, Wallet>().ReverseMap();
        CreateMap<WalletDTO, Wallet>().ReverseMap();
    }
}
