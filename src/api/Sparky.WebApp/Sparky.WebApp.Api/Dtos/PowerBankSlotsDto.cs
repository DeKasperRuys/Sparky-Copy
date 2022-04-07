using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos
{
    public class PowerBankSlotsDto
    {
        
        public int PowerbankSlotId { get; set; }
        public bool IsEmpty { get; set; }
        public int SlotIdentifier { get; set; }
        public int LevedCharged { get; set; }
        public int PowerbankId { get; set; }

    }
}
