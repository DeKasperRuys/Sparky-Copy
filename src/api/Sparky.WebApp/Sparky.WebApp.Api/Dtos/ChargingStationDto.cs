using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos
{
    public class ChargingStationDto
    {
        
        public int ChargingStationId { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int NumOfSlots { get; set; }
        public List<PowerBankSlotsDto> Slots { get; set; }    
    }
}
