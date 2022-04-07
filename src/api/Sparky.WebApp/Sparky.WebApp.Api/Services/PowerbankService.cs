using AutoMapper;
using Sparky.WebApp.Api.Contexts;
using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Services
{
    public interface IPowerbankService
    {
        Task<string> AddPowerbank(PowerBank powerBank);
    }
    public class PowerbankService: IPowerbankService
    {
        private SparkyDbContext _context;
        private readonly IChargingStationService chargingStationService;
        private readonly IMapper _mapper;

        public PowerbankService(IMapper mapper, SparkyDbContext context,IChargingStationService chargingStationService)
        {
            _context = context;
            this.chargingStationService = chargingStationService;
            this._mapper = mapper;
        }


        public async Task<string> AddPowerbank(PowerBank powerBank)
        {
            
            ChargingStation chargingStation = chargingStationService.GetChargingStation(powerBank.CurrentStationId);
            if(chargingStation == null)
            {
                return "No ChargingStation Found";
            }

            if(chargingStation.NumOfAvailableSlots == 0)
            {
                return "No More Slots Available";
            }
            chargingStation.NumOfAvailableSlots -= 1;
            var x = await _context.AddAsync(powerBank);
            _context.SaveChanges();
            return "succes";

        }

    }
}
