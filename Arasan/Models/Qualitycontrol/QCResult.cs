using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class QCResult
    {
        public QCResult()
        {
            //this.Typlst = new List<SelectListItem>();
            this.lst = new List<SelectListItem>();
          
            this.Supplst = new List<SelectListItem>();
            
            this.assignList = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();
        }
       

        public List<SelectListItem> assignList;

        //public List<SelectListItem> Typlst;
        public List<SelectListItem> lst;
      
        public List<SelectListItem> Supplst;
     
        public List<SelectListItem> Loc;

        //public List<QCResultItem> QCRLst;
        

        public string Branch { get; set; }
        public string Location { get; set; }

        public string QcLocation { get; set; }
        public string ID { get; set; }
     
        public string Party { get; set; }
        public string GRNNo { get; set; }
        public string Remarks { get; set; }
        //public string Type { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string GRNDate { get; set; }
        public string ddlStatus { get; set; }
        public string TestedBy { get; set; }
        public string Stat { get; set; }

        public List<QCResultItem> QResLst { get; set; }
    }
    public class QCResultItem
    {
       
        public string GrnQty { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }

        public List<SelectListItem> Itemlst;
        public string InsQty { get; set; }
        public string RejQty { get; set; }
        public string AccQty { get; set; }
        public string Unit { get; set; }
        public string CostRate { get; set; }
        public string Isvalid { get; set; }

    }
    public class qcresultItem
    {
        public long id { get; set; }
        public string doc { get; set; }
        public string item { get; set; }
        public string party { get; set; }
        public string work { get; set; }
        public string process { get; set; }
        public string schno { get; set; }
        public string docDate { get; set; }
        public string grn { get; set; }
        public string loc { get; set; }
        public string test { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
        public string Accrow { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}
