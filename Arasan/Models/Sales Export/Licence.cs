using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Licence
    {
        public Licence()
        {
            this.Brlst = new List<SelectListItem>();
            
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }
        public string DocDate { get; set; }
        public string DocId { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceDate { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public List<ImportDeatils> ImportLst { get; set; }
        public List<ExportDeatils> ExportLst { get; set; }
    }
    public class ImportDeatils
    {
        public string ID { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
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
    public class ExportDeatils
    {
        public string ID { get; set; }
        public string ExpNo { get; set; }
        public string ExpDate { get; set; }
        public string ItemId { get; set; }
        public string Customer { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> ExportItemlst { get; set; }
        public List<SelectListItem> Suplst { get; set; }
        public string Des { get; set; }
        public string Isvalid1 { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
    }
    public class ListLicenceItems
    {
        public long id { get; set; }
        public string docno { get; set; }
        public string docdate { get; set; }
        public string licno { get; set; }
        public string sendmail { get; set; }
        public string followup { get; set; }
        public string move { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }

    }
}
