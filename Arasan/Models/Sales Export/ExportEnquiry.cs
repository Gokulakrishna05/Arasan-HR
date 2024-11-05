using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
	public class ExportEnquiry
	{
		public ExportEnquiry()
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
		public string Phone { get; set; }
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
		public string Rate { get; set; }
		public string CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public string TotalQty { get; set; }
		public string ddlStatus { get; set; }
		public string Emailid { get; set; }
		public string Send { get; set; }
		public string Deatails { get; set; }
		public string Emaildate { get; set; }
		public string FollowUp { get; set; }
		public string Time { get; set; }
        public List<ExportItem> ExportLst { get; set; }
        public List<TermsItem> TermsLst { get; set; }

    }
	public class ExportItem
	{
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Des { get; set; }
        public string Isvalid { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }

    }
	public class TermsItem
	{
		public string ID { get; set; }
		public string Isvalid1 { get; set; }
        public string Template { get; set; }
        public List<SelectListItem> Tandclst { get; set; }
        public string Conditions { get; set; }
        public List<SelectListItem> Condlst { get; set; }
    }

    public class ExportEnquiryItems
	{
        public long id { get; set; }
        public string enqno { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string party { get; set; }
        public string sendmail { get; set; }
        public string followup { get; set; }
        public string move { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
    }
}
