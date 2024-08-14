using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ResignationModel
    {
        public ResignationModel()
        {
            this.TReasonLst = new List<SelectListItem>();
            this.EmpIDLst = new List<SelectListItem>();
            //    this.DepLst = new List<SelectListItem>();

        }
        public string ID { get; set; }

        public string DocId { get; set; }
        public string Date { get; set; }
      
        public string EmpID { get; set; }
        public List<SelectListItem> EmpIDLst;
        public string EmpName { get; set; }
        public string EmpJoin { get; set; }
        public string EmpResignation { get; set; }
        public string TReason { get; set; }
        public List<SelectListItem> TReasonLst;
        public string Reason { get; set; }

        public List<EmployeeShift> EmpResignationlist { get; set; }


        public string ddlStatus { get; set; }

    }
    public class ResignationList
    {

        public string  id { get; set; }
        public string docid { get; set; }
        public string empid { get; set; }
        public string empname { get; set; }
        public string joindate { get; set; }
        public string resignationdate { get; set; }
        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }


    }
}
