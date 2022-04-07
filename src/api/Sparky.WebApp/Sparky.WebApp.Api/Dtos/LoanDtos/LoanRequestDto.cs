using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.LoanDtos
{
    public class LoanRequestDto
    {
        public int UserId { get; set; }
        public int PowerBankId { get; set; }
        public int StationId { get; set; }
    }
}
