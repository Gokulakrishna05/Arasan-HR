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
        public List<QCResultItem> QCRLst;

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
        public string TestedBy { get; set; }
       
        
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
}
