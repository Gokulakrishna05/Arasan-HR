using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalesEnquiry
    {
        public SalesEnquiry()
    {
        this.Brlst = new List<SelectListItem>();
        this.Suplst = new List<SelectListItem>();
        this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Prilst = new List<SelectListItem>();
            this.Enqlst = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();
        }
    public List<SelectListItem> Brlst;
    public string ID { get; set; }

    public string Branch { get; set; }
        public string EnqDate { get; set; }
        public string Customer { get; set; }

    public List<SelectListItem> Suplst;

    public string Currency { get; set; }
      
        public List<SelectListItem> Curlst;
        public List<SelectListItem> RecList;
        public string Assign { get; set; }
        public List<SelectListItem> assignList;
        public string Recieved { get; set; }
        public List<SelectListItem> Prilst;
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> Enqlst;
        public List<SelectListItem> Typelst;
        public string CustomerType { get; set; }
        public string EnqType { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string EnqNo { get; set; }
        public string ContactPerson { get; set; }
        public string Priority { get; set; }
        public string status { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string TotalQty { get; set; }
        public List<SalesItem> SalesLst { get; set; }
    }
    public class SalesItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string Des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
    }
    public class EnqFollowup
    {
        public string FolID { get; set; }
        public string EnqNo { get; set; }
        public string CusName { get; set; }
        public string Enqdate { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
        public List<SelectListItem> EnqassignList;


        public List<SalesEnqFollowupDetails> qflst { get; set; }


    }
    public class SalesEnqFollowupDetails
    {
        public string ID { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
    }
}
   