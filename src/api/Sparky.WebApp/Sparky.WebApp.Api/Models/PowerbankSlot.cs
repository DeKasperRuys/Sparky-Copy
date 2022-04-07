using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class PowerbankSlot
    {
        public int PowerbankSlotId { get; set; }
        public bool IsEmpty { get; set; }
 
        public int LevedCharged { get; set; }
        public int PowerbankId { get; set; }
        public int StationKey { get; set; }
        public int SlotIdentifier { get; set; }
        public ChargingStation Station { get; set; }
    }
}
