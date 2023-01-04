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
          
            this.assignList = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();
        }
       

        public List<SelectListItem> assignList;

        public List<SelectListItem> Loc;
        public string Location { get; set; }

      
        public string ID { get; set; }
     
        public string Party { get; set; }
        public string GRNNo { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string GRNDate { get; set; }
        public string TestedBy { get; set; }
    }
}
