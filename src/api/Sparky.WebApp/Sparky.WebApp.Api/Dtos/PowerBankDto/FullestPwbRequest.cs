using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.PowerBankDto
{
    public class FullestPwbRequest
    {
        public int PowerbankId { get; set; }
        public int SlotId { get; set; }
    }
}
