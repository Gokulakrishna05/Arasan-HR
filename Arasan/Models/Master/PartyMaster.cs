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
            this.assignList = new List<SelectListItem>();
            this.Categorylst = new List<SelectListItem>();
            this.Ledgerlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Ledgerlst;

        public string Ledger { get; set; }
        public string ID { get; set; }
        public List<SelectListItem> Categorylst;

        public string CategoryType { get; set; }
        public string Regular { get; set; }
        public string PartyCategory { get; set; }

        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        //public string PartyType { get; set; }
        public string PartyGroup { get; set; }
        public string Comm { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> Statelst;
        public string State { get; set; }
        public List<SelectListItem> Citylst;
        public string City { get; set; }

        public List<SelectListItem> Countrylst;
       
        public string Country { get; set; }
        public List<SelectListItem> assignList;

        public string Phone { get; set; }
        public string Remark { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string  GST { get; set; }
        public string Http { get; set; }
        public string Type { get; set; }
        public string Excise { get; set; }
        public string EccID { get; set; }
        public string Range { get; set; }
        public string Fax { get; set; }
        public string Intred { get; set; }
        public string Commisionerate { get; set; }
        public string PanNumber { get; set; }   
        public string TanNumber { get; set; }
        public string AccName { get; set; }
        public string CountryCode { get; set; } 
        public string TransationLimit { get; set; }    
        public string CreditLimit { get; set; }   
        public string CreditDate { get; set; }
        public string RateCode{ get; set; }
        public string Active { get; set; }
        public string SectionID { get; set; }
        public string ConPartyID { get; set; }
        public string JoinDate { get; set; }  
        public string LUTNumber { get; set; }
        public string LUTDate { get; set; }

        public string Pincode { get; set; }
      
        public string OverDueInterest { get; set; }    
        public string AccountNumber { get; set; }   
        public string CurrencyType { get; set; }    
        public  int TotalnoofBills { get; set; }
        public string LastbillDate { get; set; }
        public int  EntryBY { get; set; }   
        public string EntryOn { get; set; }
        public int UpdatedBY { get; set; }
        //public List<PartyItem> PartyLst { get; set; }

        public string CID { get; set; }
        public string ContactPerson { get; set; }
        public string Purpose { get; set; }

        public string Designation { get; set; }
        public string CPhone { get; set; }

        public string CEmail { get; set; }
        public string Isvalid { get; set; }
        public string status { get; set; }

    }
    //public class PartyItem
    //{
    //    public string ID { get; set; }
    //    public string ContactPerson { get; set; }
    //    public string Purpose { get; set; }
      
    //    public string Designation { get; set; }
    //    public string Phone { get; set; }
       
    //    public string Email { get; set; }
    //    public string Isvalid { get; set; }
    //}
    }
