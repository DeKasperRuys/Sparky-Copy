using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sparky.WebApp.Api.Contexts;
using Sparky.WebApp.Api.Dtos.LoanDtos;
using Sparky.WebApp.Api.Dtos.UserDtos;
using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Services
{
    public interface IUserService
    {
        Task<UserInfoDto> CreateUser(UserInfoDto req);
        Task<LoanResponseDto> StartLoan(int userId, int powerbankId, int stationId);
        Task<LoanResponseDto> StopLoan(int userId, int Level, int stationId);
        Task<List<LoanHistoryDto>> GetLoanHistory(int userId);
        Task<UserInfoDto> GetUser(string firebaseId);
    }
    public class UserService: IUserService
    {
        private SparkyDbContext _context;
        private readonly IMapper _mapper;

        public UserService(SparkyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> CreateUser(UserInfoDto req)
        {
            var existingUser = _context.UserInfo.FirstOrDefault(d => d.FirebaseUid == req.Uid);
            if(existingUser != null)
            {
                return req;
            }
            var user = new UserInfo();
            user.Email = req.Email;
            user.FirebaseUid = req.Uid;
            if (req.DisplayName == null) { user.UserName = "not yet given"; }
            else { user.UserName = req.DisplayName; }
            _context.Add(user);
            var x = await _context.SaveChangesAsync();
            if(x == 0)
            {
                return null;
            }
            return req;
            
        }
        public async Task<UserInfoDto> GetUser(string firebaseId)
        {
            var userInfo = _context.UserInfo.FirstOrDefault(d => d.FirebaseUid == firebaseId);
            var userDto = new UserInfoDto();
            userDto.Email = userInfo.Email;
            userDto.UserId = userInfo.UserInfoId;
            return userDto;
        }
        public async Task<List<LoanHistoryDto>> GetLoanHistory(int userId)
        {
            List<LoanHistoryDto> dtoList = new List<LoanHistoryDto>();
            var user = _context.UserInfo.Include(d => d.LoanHistory).FirstOrDefault(d => d.UserInfoId == userId);
            foreach (var loan in user.LoanHistory)
            {
                var LoanHistoryDto = new LoanHistoryDto();
                LoanHistoryDto.Date = UnixTimeToDateTime(loan.StartDateTime);//convert
                LoanHistoryDto.Duration = (loan.StopDateTime - loan.StartDateTime) /60; //convert
                LoanHistoryDto.Price = loan.Price;
                LoanHistoryDto.IsOngoing = loan.OnGoing;
                dtoList.Add(LoanHistoryDto);
            }
            return dtoList;
        }
        public async Task<LoanResponseDto> StartLoan(int userId, int powerbankId, int stationId)
        {
            var powerbank = _context.PowerBanks.First(d => d.PowerBankId == powerbankId);
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);
            if (!powerbank.isLent)
            {
                powerbank.isLent = true;
                powerbank.CurrentUserId = userId;
                powerbank.CurrentStationId = 0;
                var powerbankLent = new PowerbankLoanObj
                {
                    Powerbank = powerbank.PowerBankId,
                    Level = powerbank.Level,
                    
                };

                var loan = new Loan();
                loan.OnGoing = true;
                loan.PowerbankLoanObj = powerbankLent;
                DateTime unix = DateTime.UtcNow;
                long unixTime = ((DateTimeOffset)unix).ToUnixTimeSeconds();
                loan.StartDateTime = unixTime;
                loan.StopDateTime = 0;
                loan.Price = 0;              
                station.NumOfAvailableSlots += 1;
                var user = _context.UserInfo.First(d => d.UserInfoId == userId);
                user.LoanHistory.Add(loan);
                await _context.SaveChangesAsync();
                var loanReponseDto = _mapper.Map<LoanResponseDto>(loan);
                return loanReponseDto;
            }
            return null;
        }
        public async Task<LoanResponseDto> StopLoan(int userId,int Level, int stationId)
        {

            var user = _context.UserInfo.Include(d => d.LoanHistory).First(u => u.UserInfoId == userId);
            var loan =  user.LoanHistory.First(d => d.OnGoing == true);
            var powerbankObj = _context.PowerbankLoanObjs.First(d => d.Id == loan.PowerbankLoanObjId);
            var station = _context.ChargingStations.First(d => d.ChargingStationId == stationId);

            if (loan != null)
            {
                loan.OnGoing = false;
                DateTime unix = DateTime.UtcNow;
                long unixTime = ((DateTimeOffset)unix).ToUnixTimeSeconds();
                loan.StopDateTime = unixTime;
                loan.Price = CalculatePrice(loan.StartDateTime, loan.StopDateTime);
                powerbankObj.Level = Level;

                var powerbank = _context.PowerBanks.First(d => d.PowerBankId == loan.PowerbankLoanObj.Powerbank);
                powerbank.isLent = false;
                powerbank.CurrentStationId = stationId;
                powerbank.CurrentUserId = 0;
                powerbank.Level = CalculateLevel(loan.StartDateTime,loan.StopDateTime, powerbank.Level);
                station.NumOfAvailableSlots -= 1;
                //reset chargingstation
                station.ReturnOK = false;
                station.IsLoanRequest = false;


                await _context.SaveChangesAsync();
                var loanReponseDto = _mapper.Map<LoanResponseDto>(loan);
                loanReponseDto.Duration = loan.StopDateTime - loan.StartDateTime;
                return loanReponseDto;
            }
            return null;
        }
        private double CalculatePrice(long start, long stop)
        {
            var price = 0.25;
            var sec = stop - start;
            var min = sec / 60;
            var per15 = min / 15;
            var totprice = (double)per15 * price;
            return totprice + 0.25;
        }
        private int CalculateLevel(long start, long stop, int level)
        {
            var passed = stop - start;
            var sub = passed / 60;
            var charge = level - (int)sub;
            return charge;
        }

        private long DateTimeToUnix(DateTime myDateTime)
        {
            TimeSpan timeSpan = myDateTime - new DateTime(1970, 1, 1, 2, 0, 0, System.DateTimeKind.Utc);
            return (long)timeSpan.TotalSeconds;
        }
        private DateTime UnixTimeToDateTime(long unixTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime;
        }
    }
}
