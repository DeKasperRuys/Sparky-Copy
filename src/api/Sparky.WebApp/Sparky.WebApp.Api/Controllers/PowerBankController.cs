using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparky.WebApp.Api.Dtos.PowerBankDto;
using Sparky.WebApp.Api.Models;
using Sparky.WebApp.Api.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sparky.WebApp.Api.Controllers
{
    [Route("api/powerbank")]
    public class PowerBankController : Controller
    {
        private readonly IPowerbankService powerbankService;

        public PowerBankController(IPowerbankService powerbankService)
        {
            this.powerbankService = powerbankService;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<string> Post([FromBody] PowerBank powerbank)
        {
            var response = await powerbankService.AddPowerbank(powerbank);
            if (response == null)
            {
               throw new ArgumentOutOfRangeException("slots can not be null");
            }
            return response;
        }
       
        // SET POWERBANK SLOT CALL
    }
}
