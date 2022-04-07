using Microsoft.EntityFrameworkCore;
using Sparky.WebApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Contexts
{
    public class SparkyDbContext : DbContext
    {
        public SparkyDbContext(DbContextOptions<SparkyDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChargingStation> ChargingStations { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<PowerBank> PowerBanks { get; set; }
        public DbSet<PowerbankLoanObj> PowerbankLoanObjs { get; set; }
    }
}
