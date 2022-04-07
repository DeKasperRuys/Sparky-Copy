using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sparky.WebApp.Api.Models
{
    public class ChargingStation
    {
        public int ChargingStationId{ get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int NumOfSlots { get; set; }
        public int NumOfAvailableSlots { get; set; }



        //lock params
        public bool IsReturnRequest { get; set; }
        public bool ReturnOK { get; set; }
        public bool CloseStation { get; set; }




        //unlock params
        public int AvailablePowerbank { get; set; }
        public bool IsLoanRequest { get; set; }
        public bool OpenStation { get; set; }


        public ICollection<PowerbankSlot> Slots { get; set; }
    }
}
