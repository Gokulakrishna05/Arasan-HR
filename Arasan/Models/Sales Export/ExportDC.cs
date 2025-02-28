using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ExportDC
    {
        public ExportDC()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Despatchlst = new List<SelectListItem>();
            this.Inspectedlst = new List<SelectListItem>();
            this.Doclst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }
        public string Branch { get; set; }
        public string Customer { get; set; }
        public string Customerid { get; set; }

        public List<SelectListItem> Suplst;
        public List<SelectListItem> joplst;

        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public List<SelectListItem> RecList;
        public string Recieved { get; set; }

        public List<SelectListItem> Despatchlst;
        public string Despatch { get; set; }
        public List<SelectListItem> Inspectedlst;
        public List<SelectListItem> arealist;
        public string Inspected { get; set; }
        public List<SelectListItem> Doclst;
        public string Doc { get; set; }
        public string DCNo { get; set; }
        public string DCdate { get; set; }
        public string Ref { get; set; }
        public string Refdate { get; set; }
        public string Jobid { get; set; }
        public string CName { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Pincode { get; set; }
        public string Fax { get; set; }
        public string Emaildate { get; set; }
        public string Send { get; set; }
        public string Deliver { get; set; }
        public string Email { get; set; }
        public string Narration { get; set; }
        public string other { get; set; }
        public string ddlStatus { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public List<ExportDCItem> ExportDCLst { get; set; }
        public List<ScrapItem> ScrapLst { get; set; }

       

    }
    public class ExportDCItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
      //  public string Qty { get; set; }
       // public string Rate { get; set; }
        public string CF { get; set; }
        public string Amount { get; set; }
        public string Lot { get; set; }
        public string Serial { get; set; }
        public string Seal { get; set; }
        public string Order { get; set; }
        public string Current { get; set; }
        public string DC { get; set; }
        public string UnitPrimary { get; set; }
        public string QtyPrimary { get; set; }
       
        public string CashDisc { get; set; }
        public string Introduction { get; set; }
        public string Trade { get; set; }
        public string Addition { get; set; }
        public string Special { get; set; }
        public string Fright { get; set; }
        public string Drum { get; set; }
        public string Container { get; set; }
        public string Tare { get; set; }
        public string Vechile { get; set; }
        public string Material { get; set; }





        public string workid { get; set; }
        public string schid { get; set; }
        public string jobschid { get; set; }
        public string DrumIds { get; set; }
        public string jodetid { get; set; }
        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
       
        public string shedate { get; set; }
      
        public double quantity { get; set; }
        public double rate { get; set; }
        
        public string ItemType { get; set; }
        public string ItemSpec { get; set; }
        public string binid { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public double Disc { get; set; }
        public double DiscAmount { get; set; }
    
       
     
     
        
        public double introdisc { get; set; }
      
      
     
      
      
   
        public string Drumsdesc { get; set; }
        public string tarrifid { get; set; }
        public string FrieghtItemId { get; set; }
        public string HSNcode { get; set; }
        public string Frieght { get; set; }
        public string FriQty { get; set; }
        public string FrieghtAmount { get; set; }

        public string itemss { get; set; }
        public string saveitem { get; set; }
        public string hsncodes { get; set; }

        public string work { get; set; }

        public string drumds { get; set; }
        public double frigcharges { get; set; }
         
        public string unitname { get; set; }
        public double qty { get; set; }
        public double amountt { get; set; }
        public string itemtypes { get; set; }
        public string itemdesc { get; set; }
        public double disct { get; set; }
        public double discamt { get; set; }
        public double discountamounts { get; set; }
        public double cashamt { get; set; }
      
        public double introDis { get; set; }
        public double cashdis { get; set; }
        public double tradedis { get; set; }
        public double adddis { get; set; }
        public double specdis { get; set; }
        public double totalamt { get; set; }
        public double currentstk { get; set; }
        public string drumsdes { get; set; }
        public string frieghtttem { get; set; }
        public string hsnc { get; set; }
        public string frieghta { get; set; }
        public string friquatity { get; set; }
        public string frieghtamt { get; set; }

    }
    public class ScrapItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Des { get; set; }
        public string Isvalid1 { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string CF { get; set; }
        public string Amount { get; set; }
        public string Rejected { get; set; }
        public string Stock { get; set; }
    }
    public class ListExportDCItems
    {
        public long id { get; set; }
        public string dcno { get; set; }
        public string dcdate { get; set; }
        public string loc { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
    }

 
}
