using AutoMapper;
using Sparky.WebApp.Api.Dtos;
using Sparky.WebApp.Api.Dtos.LoanDtos;
using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Helpers.MappingProfiles
{
    public class DtoMapperProfile : Profile
    {
        public DtoMapperProfile()
        {
            CreateMap<ChargingStation, ChargingStationDto>();
            CreateMap<PowerbankSlot, PowerBankSlotsDto>();
            CreateMap<Loan, LoanResponseDto>();
        }
    }
}
