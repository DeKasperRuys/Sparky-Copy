using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.LoanDtos
{
    public class StopLoanRequestDto
    {
        public int UserId { get; set; }
        public int Level { get; set; }
        public int LoanStation { get; set; }
    }
}
