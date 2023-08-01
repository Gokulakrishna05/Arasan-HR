using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models 
{	 
    public class APProductionentry
    {
        public APProductionentry()
        {
            this.Loclst = new List<SelectListItem>();
        }
        public string ID { get; set; }

		public object APID { get; set; }
		public string Location { get; set; }
        public List<SelectListItem> Loclst { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Eng { get; set; } 
        public List<SelectListItem> Englst { get; set; }
        public List<SelectListItem> Batchlst { get; set; }
        public string BatchNo { get;set; }
        public string batchcomplete { get; set; }
        public string SchQty { get; set; }
        public string ProdQty { get; set; }
        public List<SelectListItem> Shiftlst { get; set; }
        
        public string Shift { get; set; }
      
    }
    public class APProductionentryDet
    {
      
        public string ID { get; set; }

        public object APID { get; set; }
        public string Location { get; set; }
     
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Eng { get; set; }
       
        public string BatchNo { get; set; }
        public string batchcomplete { get; set; }
        public string SchQty { get; set; }
        public string ProdQty { get; set; }
       
        public List<BreakDet> BreakLst { get; set; }
        public string Shift { get; set; }
        public List<ProInput> inplst { get; set; }
        public List<ProOutput> outlst { get; set; }
        public List<APProInCons> Binconslst { get; set; }
        public List<EmpDetails> EmplLst { get; set; }

    }
    public class ProInput
    {
        public string ItemId { get; set; }
        public string APID { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemGrouplst { get; set; }
        public string ItemGroupId { get; set; }
        public string BinId { get; set; }
		public string Bin { get; set; }
		public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public double batchqty { get; set; }
        public double StockAvailable { get; set; }
        public double IssueQty { get; set; }
        public string MillLoadAdd { get; set; }
        public string Output { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> outputlst;
        public string Purchasestock { get; set; }
        public string drumid { get; set; }
        public string Proinid { get; set; }
        public List<SelectListItem> lotlist { get; set; }
        public string Lotno { get; set; }
        public double totalqty { get; set; }
    }
    public class ProOutput
    {
        public string ItemId { get; set; }
        public string APID { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
      
        public string BinId { get; set; }
        public string Time { get; set; }
        public string Bin { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public double batchqty { get; set; }
        public double Stock  { get; set; }
        public double IssueQty { get; set; }
        public string MillLoadAdd { get; set; }
        public double OutputQty { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> outputlst;
        public string Purchasestock { get; set; }
        public string drumid { get; set; }
        public string Proinid { get; set; }
        public List<SelectListItem> lotlist { get; set; }
        public string Lotno { get; set; }
        public double totalqty { get; set; }
    }
    public class APProInCons
	{
		public string ID { get; set; }
		public string ItemId { get; set; }
        public string APID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
		public double ConsStock { get; set; }
		public string consunit { get; set; }
		public string unitid { get; set; }
		public string consBin { get; set; }
		public double consQty { get; set; }
		public double Qty { get; set; }
		public string Isvalid { get; set; }
		public string BinId { get; set; }

		public string saveitemId { get; set; }

	}
	public class EmpDetails
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string EmpCode { get; set; }
        public string saveItemId { get; set; }

        public string APID { get; set; }
        public List<SelectListItem> Employeelst { get; set; }

        public string Employee { get; set; }
        public string NOW { get; set; }

        public string Depart { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string OTHrs { get; set; }
        public string Normal { get; set; }
        public string ETOther { get; set; }
        public string Isvalid { get; set; }
    }
    public class BreakDet
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }

        public string saveItemId { get; set; }
        public List<SelectListItem> Reasonlst { get; set; }
        public string Reason { get; set; }

        public List<SelectListItem> Machinelst { get; set; }
        public string APID { get; set; }
        public string MachineId { get; set; }
        public string MachineDes { get; set; }
        public List<SelectListItem> Emplst { get; set; }
        public string Alloted { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DType { get; set; }
        public string MType { get; set; }
        public string PB { get; set; }
        public string Isvalid { get; set; }
    }
}

