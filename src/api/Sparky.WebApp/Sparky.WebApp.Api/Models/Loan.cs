using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public long StartDateTime { get; set; }
        public long StopDateTime { get; set; }
        public bool OnGoing { get; set; }
        public double Price { get; set; }

        public int PowerbankLoanObjId { get; set; }
        public PowerbankLoanObj PowerbankLoanObj { get; set; }
        public UserInfo Borrower { get; set; }
    }
}
