using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }
        public string UserName { get; set; }
        public string FirebaseUid { get; set; }
        public string Email { get; set; }
        public ICollection<Loan> LoanHistory { get; set; }
        public UserInfo()
        {
            LoanHistory = new Collection<Loan>();
        }
        
    }
}
