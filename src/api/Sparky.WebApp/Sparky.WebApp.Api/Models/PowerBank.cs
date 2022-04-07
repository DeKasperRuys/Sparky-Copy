using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class PowerBank
    {
        public int PowerBankId { get; set; }
        public string Identifier { get; set; }
        public bool isLent { get; set; }
        public int CurrentStationId { get; set; }
        public int CurrentUserId { get; set; }
        public int Level { get; set; }
        public ICollection<Loan> LoanHistory { get; set; }
        public int SlotId { get; set; }
    }
}
