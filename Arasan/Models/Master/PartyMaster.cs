using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PartyMaster
    {
        public PartyMaster()
        {
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
            this.Countrylst = new List<SelectListItem>();
        }
        public int ID { get; set; }
        public string CategoryType { get; set; }

        public string PartyCategory { get; set; }

        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyType { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> Statelst;

        public List<SelectListItem> Citylst;
        public List<SelectListItem> Countrylst;
        public int City { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string  GST { get; set; }
        public string PanNumber { get; set; }   
        public string TanNumber { get; set; }   
        public string CountryCode { get; set; } 
        public int TransationLimit { get; set; }    
        public  int CreditLimit { get; set; }   
        public string CreditDate { get; set; }

        public string JoinDate { get; set; }  
        public string LUTNumber { get; set; }
        public string LUTDate { get; set; }

        public int Pincode { get; set; }
        public string ContactPerson { get; set; }
        public int OverDueInterest { get; set; }    
        public string AccountNumber { get; set; }   
        public string CurrencyType { get; set; }    
        public  int TotalnoofBills { get; set; }
        public string LastbillDate { get; set; }
        public int  EntryBY { get; set; }   
        public string EntryOn { get; set; }
        public int UpdatedBY { get; set; }




    }
}
