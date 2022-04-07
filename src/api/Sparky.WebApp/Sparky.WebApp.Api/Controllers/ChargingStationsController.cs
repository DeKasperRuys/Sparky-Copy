using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sparky.WebApp.Api.Dtos;
using Sparky.WebApp.Api.Dtos.PowerBankDto;
using Sparky.WebApp.Api.Dtos.SequenceRequests;
using Sparky.WebApp.Api.Models;
using Sparky.WebApp.Api.Services;

namespace Sparky.WebApp.Api.Controllers
{
    [Route("api/stations")]
    [ApiController]
    public class ChargingStationsController : ControllerBase
    {
        private readonly IChargingStationService stationsvc;

        public ChargingStationsController(IChargingStationService _stationsvc)
        {
            stationsvc = _stationsvc;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<ChargingStationDto>>> GetAll()
        {
            var response = await stationsvc.GetAllStations();
            if(response == null)
            {
                return BadRequest("no stations found");
            }
            return Ok(response);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Post([FromBody] ChargingStation chargingStation)
        {
            var response = await stationsvc.PostAsync(chargingStation);
            if (response == null)
            {
                return BadRequest("slots can not be null");
            }
            return CreatedAtAction("Post","succes");
        }
        [HttpPost("addstations")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AddStations([FromBody] AddStationsDto dto)
        {
            var response = await stationsvc.PostListAsync(dto.stations);
            if (response ==  0)
            {
                return BadRequest("slots can not be null");
            }
            return CreatedAtAction("Post", "succes");
        }
        #region //IsLoanRequested
        [HttpGet("poll/loanRequest/{QrId}")]
        public async Task<ActionResult<bool>> PollIsLoanRequested(int QrId)
        {
            var response = await stationsvc.PollIsLoanRequested(QrId);
            return response;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("loanRequest/{QrId}")]
        public ActionResult Put(int QrId, [FromBody] SetBool isRequested)
        {
            stationsvc.SetIsLoanRequest(QrId, isRequested.SetBoolean);
            return Ok(new { }); ;
        }
        #endregion

        #region //AvailablePowerbank
        [HttpGet("poll/availablepowerbank/{Id}")]
        public ActionResult<PowerBankDto> PollAvailablePowerbank(int Id)
        {
            var response = stationsvc.PollAvailablePowerBank(Id);
            return response;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("availablepowerbank/{Id}")]
        public ActionResult PutAvailablePowerbank(int Id, [FromBody] PowerbankId powerbank)
        {
            stationsvc.SetAvailablePwbId(Id, powerbank.Id,powerbank.SlotId);
            return Ok(new { }); ;
        }
        #endregion

        #region //OpenStation
        [HttpGet("poll/openstation/{Id}")]
        public ActionResult<bool> PollOpenStation(int Id)
        {
            var response = stationsvc.PollOpenStation(Id);
            return response;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("openstation/{Id}")]
        public ActionResult PutOpenStation(int Id, [FromBody] SetBool open)
        {
            stationsvc.SetOpenStation(Id, open.SetBoolean);
            return Ok(new { }); ;
        }
        #endregion




        #region //IsReturnOk
        [HttpGet("poll/returnOk/{Id}")]
        public ActionResult<bool> PollIsReturnOk(int Id)
        {
            var response = stationsvc.PollIsReturnOk(Id);
            return response;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("returnOk/{Id}")]
        public ActionResult PutIsReturnOk(int Id, [FromBody] SetBool isReturnOk)
        {
            stationsvc.SetisReturnOk(Id, isReturnOk.SetBoolean);
            return Ok(new { }); ;
        }
        #endregion

        #region //ReturnRequest
        [HttpGet("poll/isreturnrequest/{Id}")]
        public async Task<ActionResult<bool>> PollReturnRequest(int Id)
        {
            var response = await stationsvc.PollIsReturnRequested(Id);
            return response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("isreturnrequest/{Id}")]
        public ActionResult PutReturnRequest(int Id, [FromBody] SetBool returnRequest)
        {
            stationsvc.SetIsReturnRequested(Id, returnRequest.SetBoolean);
            return Ok(new { }); ;
        }
        #endregion

        #region //CloseStation
        [HttpGet("poll/closeStation/{Id}")]
        public ActionResult<bool> PollCloseStation(int Id)
        {
            var response = stationsvc.PollCloseStation(Id);
            return response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("closeStation/{Id}")]
        public ActionResult PutCloseStation(int Id, [FromBody] SetBool close)
        {
            stationsvc.SetCloseStation(Id, close.SetBoolean);
            return Ok(new { }); 
        }
        #endregion


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("update-slot/{Id}")]
        public ActionResult UpdateSlot(int Id, [FromBody] UpdateSlotDto updateDto)
        {
            stationsvc.UpdateSlot(Id, updateDto.slotId, updateDto.powerbankId);
            return Ok(new { }); ;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("empty-slot/{Id}")]
        public async Task<ActionResult<int>> GetEmptySlots(int Id)
        {
            var slot =  await stationsvc.GetEmptySlots(Id);
            if(slot == null)
            {
                return Ok(0);
            }
            return Ok( slot.SlotIdentifier);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("slots/{Id}")]
        public async Task<ActionResult<List<PowerBankSlotsDto>>> GetStationSlots(int Id)
        {
            var slots = await stationsvc.GetStationSlots(Id);
            return Ok(slots);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("get-fullestpwb/{Id}")]
        public async Task<ActionResult<FullestPwbRequest>> GetFullestPwb(int Id)
        {
            var slot = await stationsvc.GetFullestPowerbank(Id);
            if (slot == null)
            {
                return Ok(0);
            }
            return Ok(slot);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("set-empty-slot/{Id}")]
        public ActionResult SetEmptySlot(int Id, [FromBody] SetEmptySlotDto emptySlotDto)
        {
            stationsvc.PutEmptySlot(Id, emptySlotDto.SlotId, emptySlotDto.IsEmpty);
            return Ok(new { }); ;
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
