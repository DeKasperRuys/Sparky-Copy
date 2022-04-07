using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sparky.WebApp.Api.Contexts;
using Sparky.WebApp.Api.Dtos;
using Sparky.WebApp.Api.Dtos.PowerBankDto;
using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Services
{
    public interface IChargingStationService
    {
        Task<ChargingStationDto> PostAsync(ChargingStation chargingStation);
        Task<List<ChargingStationDto>> GetAllStations();
        Task<int> PostListAsync(List<ChargingStation> stations);

        // set bools
        bool SetIsLoanRequest(int stationId, bool isRequested);
        bool SetIsReturnRequested(int stationId, bool req);
        void SetAvailablePwbId(int stationId, int pwdId, int slotId);
        void SetisReturnOk(int stationId, bool retunOk);
        void SetOpenStation(int stationId, bool open);
        void SetCloseStation(int stationId, bool close);

        // Polling
        Task<bool> PollIsLoanRequested(int stationId);
        PowerBankDto PollAvailablePowerBank(int stationId);
        bool PollIsReturnOk(int stationId);
        Task<bool> PollIsReturnRequested(int stationId);
        bool PollOpenStation(int stationId);
        bool PollCloseStation(int stationId);
        ChargingStation GetChargingStation(int id);
        Task<string> UpdateSlot(int stationId, int slotId, int powerbankId);
        Task<PowerbankSlot> GetEmptySlots(int stationId);
        Task<List<PowerBankSlotsDto>> GetStationSlots(int stationId);
        Task<FullestPwbRequest> GetFullestPowerbank(int stationId);
        Task<bool> PutEmptySlot(int stationId, int slotId, bool isEmpty);
    }

    public class ChargingStationService : IChargingStationService
    {
        private SparkyDbContext _context;
        private readonly IMapper _mapper;

        public ChargingStationService(SparkyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ChargingStation GetChargingStation(int id)
        {
            var chargingStation = _context.ChargingStations
                .First(d => d.ChargingStationId == id);
            return chargingStation;
        }
        public async Task<List<ChargingStationDto>> GetAllStations()
        {
            var result = await _context.ChargingStations.Include(b => b.Slots).ToListAsync();
            if (result != null)
            {
                var powerbanks = _context.PowerBanks.Where(x => !x.isLent);
                foreach (var powerbank in powerbanks)
                {
                    var station = result.First(x => x.ChargingStationId == powerbank.CurrentStationId);
                    var slot = station.Slots.First(x => x.SlotIdentifier == powerbank.SlotId);
                    slot.LevedCharged = powerbank.Level;
                }
                var dtoList = _mapper.Map<List<ChargingStationDto>>(result);
                return dtoList;
            }
            return null;
        }
        public async Task<ChargingStationDto> PostAsync(ChargingStation chargingStation)
        {

            var slotsList = new List<PowerbankSlot>();
            if (chargingStation.NumOfSlots > 0)
            {
                for (int i = 0; i < chargingStation.NumOfSlots; i++)
                {
                    var slot = new PowerbankSlot
                    {
                        IsEmpty = false,
                        LevedCharged = 100
                    };
                    slotsList.Add(slot);
                }
                chargingStation.Slots = slotsList;
                _context.Add(chargingStation);
                var x = await _context.SaveChangesAsync();
                var dto = _mapper.Map<ChargingStationDto>(chargingStation);

                return dto;
            }

            return null;

        }
        public async Task<int> PostListAsync(List<ChargingStation> stations)
        {
            foreach (var station in stations)
            {
                var slotsList = new List<PowerbankSlot>();
                for (int i = 0; i < station.NumOfSlots; i++)
                {
                    var slot = new PowerbankSlot
                    {
                        IsEmpty = false,
                        LevedCharged = 100,
                        SlotIdentifier = i + 1

                    };
                    slotsList.Add(slot);
                }
                station.Slots = slotsList;
            }
            stations.ForEach(s => _context.Add(s));
            var x = await _context.SaveChangesAsync();

            return x;
        }

        public async Task<string> StagePowerbank(int Level, int stationId, int slotId)
        {
            var station = _context.ChargingStations.Include(d => d.Slots).First(d => d.ChargingStationId == stationId);
            var slot = station.Slots.First(d => d.PowerbankSlotId == slotId);
            slot.IsEmpty = false;
            slot.LevedCharged = Level;
            station.IsReturnRequest = false;
            station.ReturnOK = true;
            station.NumOfAvailableSlots += 1;
            if (station.NumOfAvailableSlots < station.NumOfSlots)
            {
                await _context.SaveChangesAsync();
                return "staged";
            }
            return "Full";
        }

        #region //IsLoanRequest
        public bool SetIsLoanRequest(int stationId, bool isRequested)
        {

            var chargingStation = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            if (chargingStation.NumOfAvailableSlots <= chargingStation.NumOfSlots)
            {
                chargingStation.IsLoanRequest = isRequested;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> PollIsLoanRequested(int stationId)
        {
            var station = await _context.ChargingStations.FirstAsync(d => d.ChargingStationId == stationId);
            return station.IsLoanRequest;
        }
        #endregion

        #region //AvailablePwdId
        public void SetAvailablePwbId(int stationId, int pwdId, int slotId)
        {
            var chargingStation = _context.ChargingStations.Include(x => x.Slots).First(d => d.ChargingStationId == stationId);
            chargingStation.AvailablePowerbank = pwdId;
            if (pwdId != 0)
            {
                var powerbankToLoan = _context.PowerBanks.First(x => x.PowerBankId == pwdId);
                powerbankToLoan.SlotId = slotId;
            }        
            _context.SaveChanges();
        }
        public PowerBankDto PollAvailablePowerBank(int stationId)
        {
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            var powerbank = _context.PowerBanks.FirstOrDefault(d => d.PowerBankId == station.AvailablePowerbank);
            var powerbankDto = new PowerBankDto();
            if (powerbank != null)
            {
                powerbankDto.CurrentStationName = station.Name;
                powerbankDto.Level = powerbank.Level;
                powerbankDto.Name = powerbank.Identifier;
                powerbankDto.Id = powerbank.PowerBankId;
                powerbankDto.SlotId = powerbank.SlotId;
            }


            return powerbankDto;
        }
        #endregion

        #region //Openstation
        public void SetOpenStation(int stationId, bool open)
        {
            var chargingStation = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            chargingStation.OpenStation = open;
            _context.SaveChanges();
        }
        public bool PollOpenStation(int stationId)
        {
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            return station.OpenStation;
        }
        #endregion



        #region //IsReturnOk
        public void SetisReturnOk(int stationId, bool retunOk)
        {
            var chargingStation = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            chargingStation.ReturnOK = retunOk;
            _context.SaveChanges();
        }
        public bool PollIsReturnOk(int stationId)
        {
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            return station.ReturnOK;
        }
        #endregion

        #region //IsReturnRequested

        public bool SetIsReturnRequested(int stationId, bool req)
        {

            var chargingStation = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            chargingStation.IsReturnRequest = req;
            _context.SaveChanges();
            return true;

            return false;
        }
        public async Task<bool> PollIsReturnRequested(int stationId)
        {
            var station = await _context.ChargingStations.FirstAsync(d => d.ChargingStationId == stationId);
            return station.IsReturnRequest;
        }
        #endregion
        public async Task<bool> PutEmptySlot(int stationId, int slotId, bool isEmpty)
        {
            var station = await _context.ChargingStations.FirstAsync(d => d.ChargingStationId == stationId);
            var slot = station.Slots.First(x => x.SlotIdentifier == slotId);
            slot.IsEmpty = isEmpty;
            _context.SaveChanges();
            return true;
        }

        
        #region //CloseStation
        public void SetCloseStation(int stationId, bool close)
        {
            var chargingStation = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            chargingStation.CloseStation = close;
            _context.SaveChanges();
        }
        public bool PollCloseStation(int stationId)
        {
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            return station.CloseStation;
        }
        #endregion
        public async Task<string> UpdateSlot(int stationId, int slotId, int powerbankId)
        {

            var station = _context.ChargingStations.Include(x => x.Slots).First(x => x.ChargingStationId == stationId);
            var slot = station.Slots.First(x => x.SlotIdentifier == slotId);
            
            if (powerbankId == 0)
            {
                var powerbank = _context.PowerBanks.First(x => x.SlotId == slotId);
                powerbank.SlotId = 0;
                slot.PowerbankId = 0;
                slot.IsEmpty = true;
                slot.LevedCharged = 0;
            } else
            {
                var powerbank = _context.PowerBanks.First(x => x.PowerBankId == powerbankId);
                powerbank.SlotId = slotId;
                slot.PowerbankId = powerbankId;
                slot.IsEmpty = false;
                slot.LevedCharged = powerbank.Level;
            }
            await _context.SaveChangesAsync();
            return "succes";
        }
        public async Task<PowerbankSlot> GetEmptySlots(int stationId)
        {
            var station = _context.ChargingStations.Include(x => x.Slots).First(x => x.ChargingStationId == stationId);
            
            var slot = station.Slots.First(x => x.IsEmpty == true);
            if(slot == null)
            {
                return null;
            }
            return slot;
        }
        public async Task<List<PowerBankSlotsDto>> GetStationSlots(int stationId)
        {
            var station = _context.ChargingStations.Include(x => x.Slots).First(x => x.ChargingStationId == stationId);
            var slots = _mapper.Map<List<PowerBankSlotsDto>>(station.Slots.ToList());
            return slots;
        }
        public async Task<FullestPwbRequest> GetFullestPowerbank(int stationId)
        {
            var station = _context.ChargingStations.Include(x => x.Slots).First(x => x.ChargingStationId == stationId);
            var powerbankInStationList = station.Slots.Select(x => {return x.PowerbankId;}).ToList() ;
            var pwbList = _context.PowerBanks.Where(x => powerbankInStationList.Contains(x.PowerBankId));
            var fullest = await pwbList.OrderByDescending(x => x.Level).FirstAsync();
            var slot = station.Slots.First(x => x.PowerbankId == fullest.PowerBankId);
            return new FullestPwbRequest {
                PowerbankId = fullest.PowerBankId,
                SlotId = slot.SlotIdentifier
            };
        }
        private void ChargePowerbanks()
        {
            var powerbanks = _context.PowerBanks.Where(d => d.isLent == false);
            var stations = _context.ChargingStations.Include(x => x.Slots);
            var slots = stations.Select(x => x.Slots);
            foreach (var powerbank in powerbanks)
            {

            }

        }
    }

}
