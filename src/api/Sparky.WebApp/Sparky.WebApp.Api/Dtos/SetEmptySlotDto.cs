using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos
{
    public class SetEmptySlotDto
    {
        public int SlotId { get; set; }
        public bool IsEmpty { get; set; }
    }
}
