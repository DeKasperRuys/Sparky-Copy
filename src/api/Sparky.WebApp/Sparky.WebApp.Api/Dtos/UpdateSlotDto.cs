using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos
{
    public class UpdateSlotDto
    {
        public int slotId { get; set; }
        public int powerbankId { get; set; }
    }
}
