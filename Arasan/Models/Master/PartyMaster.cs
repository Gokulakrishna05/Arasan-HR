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
        public List<SelectListItem> ratelst;
        public List<SelectListItem> commlst;
        public List<SelectListItem> typelst;

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
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public List<SelectListItem> Statelst;
        public string State { get; set; }
        public List<SelectListItem> Citylst;
        public string City { get; set; }

        public List<SelectListItem> Countrylst;
       
        public string Country { get; set; }
        public List<SelectListItem> assignList;
        public List<SelectListItem> saleloclst;
        public List<SelectListItem> saleperlst;
        public List<SelectListItem> concodelst;

        public string salloc { get; set; }
        public string salper { get; set; }
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
       

        public string CID { get; set; }
    
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }
        public string branch { get; set; }
        public List<PartyItem> PartyLst { get; set; }
        public List<ratedet> rateLst { get; set; }
        public List<shipping> shLst { get; set; }

    }
    public class PartyItem
    {
        public string ContactPerson { get; set; }
        public string Purpose { get; set; }
        public List<SelectListItem> purlst;
        public string Designation { get; set; }
        public string CPhone { get; set; }

        public string CEmail { get; set; }
        public string Isvalid { get; set; }
    }
    public class shipping
    {
        public string city { get; set; }
        public string state { get; set; }
        public List<SelectListItem> statelst;
        public string add1 { get; set; }
        public string add2 { get; set; }

        public string add3 { get; set; }
        public string consingn { get; set; }
        public string addtype { get; set; }
        public string pincode { get; set; }
        public string gstno { get; set; }
        public string shippingdistance { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string Isvalid { get; set; }
    }
    public class ratedet
    {
        public string ratetype { get; set; }
        public string ratecode { get; set; }
        public List<SelectListItem> ratelist;
        public List<SelectListItem> ratecodelist;
        public string ratedece { get; set; }
        public string acco { get; set; }

        
        public string Isvalid { get; set; }
    }
    public class PartyGrid
    {
        public string id { get; set; }
        public string partyname { get; set; }
        public string partycategory { get; set; }
        public string partygroup { get; set; }

        public string joindate { get; set; }
        public string ratecode { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
