using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Dtos.LoanDtos
{
    public class LoanResponseDto
    {
        public long StartDateTime { get; set; }
        public long StopDateTime { get; set; }
        public double Price { get; set; }
        public long Duration { get; set; }
    }
}
