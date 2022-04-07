using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos
{
    public class AddStationsDto
    {
        public List<ChargingStation> stations { get; set; }
    }
}
