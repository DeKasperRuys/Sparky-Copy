using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sparky.WebApp.Api.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Sparky.WebApp.Api.Services;
using Sparky.WebApp.Api.Dtos.LoanDtos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sparky.WebApp.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserInfoController : Controller
    {
        private readonly IUserService _usersvc;
        public UserInfoController(IUserService usersvc)
        {
            _usersvc = usersvc;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserInfoDto> CreateUser([FromBody] UserInfoDto request)
        {
            var response = await _usersvc.CreateUser(request);
            if (response == null)
            {
                return null;
            }
            return response;
        }
        [HttpPost("startloan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<LoanResponseDto> StartLoan([FromBody] LoanRequestDto request)
        {
            var response = await _usersvc.StartLoan(request.UserId, request.PowerBankId, request.StationId);
            if (response == null)
            {
                return null;
            }
            return response;
        }
        [HttpPost("stoploan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<LoanResponseDto> StopLoan([FromBody] StopLoanRequestDto request)
        {
            var response = await _usersvc.StopLoan(request.UserId,request.Level,request.LoanStation);
            if (response == null)
            {
                return null;
            }
            return response;
        }
        [HttpGet("loan-history/{id}")]
        public async Task<List<LoanHistoryDto>> GetLoanHistory(int id)
        {
            var response = await _usersvc.GetLoanHistory(id) ;
            if (response == null)
            {
                return null;
            }
            return response;
        }
        [HttpGet("getuserid/{fbId}")]
        public async Task<UserInfoDto> GetUserId(string fbId)
        {
            var response = await _usersvc.GetUser(fbId);
            if (response == null)
            {
                return null;
            }
            return response;
        }
    }
}
