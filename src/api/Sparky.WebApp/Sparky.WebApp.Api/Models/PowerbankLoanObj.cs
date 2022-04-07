using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class PowerbankLoanObj
    {
     
        public int Id { get; set; }
        public int Powerbank { get; set; }
        public int Level { get; set; }
        public Loan Loan { get; set; }
    }
}
