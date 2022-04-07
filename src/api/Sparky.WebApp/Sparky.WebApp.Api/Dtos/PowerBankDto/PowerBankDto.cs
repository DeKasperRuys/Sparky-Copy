using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.PowerBankDto
{
    public class PowerBankDto
    {
        public int Id { get; set; }
        public string CurrentStationName { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int SlotId { get; set; }
    }
}
