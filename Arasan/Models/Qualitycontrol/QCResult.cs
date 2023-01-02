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
            this.Typlst = new List<SelectListItem>();
            this.lst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public List<SelectListItem> Typlst;

        public List<SelectListItem> Itemlst;

        public List<SelectListItem> lst;

        public List<SelectListItem> assignList;

        public string POGRN { get; set; }

        public string ID { get; set; }
        public string Type { get; set; }
        public string Party { get; set; }
        public string GRNNo { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string GRNDate { get; set; }
        public string TestedBy { get; set; }
    }
}
