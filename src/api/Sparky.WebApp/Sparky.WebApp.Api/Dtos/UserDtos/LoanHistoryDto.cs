using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.UserDtos
{
    public class LoanHistoryDto
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public long Duration { get; set; }
        public bool IsOngoing { get; set; }
    }
}
