using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ServicePOModel
    {
        public ServicePOModel()
        {
            this.CurrLst = new List<SelectListItem>();
            this.PartyIdLst = new List<SelectListItem>();
            this.PreparedLst = new List<SelectListItem>();
            this.TermLst = new List<SelectListItem>();
            this.SendLst = new List<SelectListItem>();
            this.ReceiveLst = new List<SelectListItem>();

        }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string ID { get; set; }
        public string Curr { get; set; }
        public List<SelectListItem> CurrLst;
        public string Symbol { get; set; }
        public string ExRate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string PartyId { get; set; }
        public List<SelectListItem> PartyIdLst;
        public string Party { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tax { get; set; }
        public string Prepared { get; set; }
        public List<SelectListItem> PreparedLst;
        public string Term { get; set; }
        public List<SelectListItem> TermLst;
        public string DueDate { get; set; }



        public List<ServicePO> ServicePOlist { get; set; }
        public List<ServicePOAdditional> ServicePOAdditionallist { get; set; }
        public List<TermsAndCondition> TermsAndConditionlist { get; set; }

        public string ddlStatus { get; set; }
        public string Isvalid { get; set; }



        //tab1
        public string Gross { get; set; }
        public string Net { get; set; }
        public string AmountInWords { get; set; }
        //tab3
        public string Send { get; set; }
        public List<SelectListItem> SendLst;
        public string Receive { get; set; }
        public List<SelectListItem> ReceiveLst;
        public string FollowupDate { get; set; }
        public string FollowupDetails { get; set; }




    }
    //table1
    public class ServicePO
    {

        public string ServiceID { get; set; }
        public List<SelectListItem> ServiceIDlst { get; set; }
        public string ServiceDesc { get; set; }
      
        public string Unit { get; set; }
        public List<SelectListItem> Unitlst { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
 
        public string Isvalid { get; set; }

    }
    //table2
    public class ServicePOAdditional
    {


        public string AddDedec { get; set; }
        public List<SelectListItem> AddDedeclst { get; set; }
        public string Amounts { get; set; }
        public string Isvalid { get; set; }

    }
    // tab2 table3
    public class TermsAndCondition
    {
        public string TAC { get; set; }
        public List<SelectListItem> TAClst { get; set; }

        public string Isvalid { get; set; }
    }
    public class ServicePOList
    {
        public string id { get; set; }
        public string pono { get; set; }
        public string curr { get; set; }
        public string term { get; set; }
        public string duedate { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        public string rrow { get; set; }

    }
}
