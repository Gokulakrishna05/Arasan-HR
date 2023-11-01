using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PyroProduction
    {
        //public PyroProduction()
        //{
        //    this.Loclst = new List<SelectListItem>();
        //}
        //public string ID { get; set; }

        //public object APID { get; set; }
        //public string Location { get; set; }
        //public string Branch { get; set; }
        //public List<SelectListItem> Loclst { get; set; }
        //public string DocId { get; set; }
        //public string Docdate { get; set; }
        //public string Eng { get; set; }
        //public List<SelectListItem> Englst { get; set; }
        //public List<SelectListItem> Batchlst { get; set; }
        //public string BatchNo { get; set; }

        //public string batchcomplete { get; set; }
        //public string SchQty { get; set; }
        //public string ProdQty { get; set; }
        //public List<SelectListItem> Shiftlst { get; set; }

        //public string Shift { get; set; }

    

        public string ID { get; set; }
        public string change { get; set; }
        public object APID { get; set; }
        public string Location { get; set; }
        public string Approve { get; set; }
        public string Branch { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Eng { get; set; }
        public string super { get; set; }
        public bool Complete { get; set; }
        public string BatchNo { get; set; }
        public string batchcomplete { get; set; }
        public string SchQty { get; set; }
        public string ProdQty { get; set; }
        public List<SelectListItem> Shiftlst { get; set; }
        public List<SelectListItem> worklst { get; set; }
       
        public string Shift { get; set; }
        

        public string CLocation { get; set; }

        public List<SelectListItem> Wclst { get; set; }

    }
    public class PyroProductionentryDet
    {

        public string ID { get; set; }
        public string change { get; set; }
        public object APID { get; set; }
        public string Location { get; set; }
        public string Branch { get; set; }
        public string DocId { get; set; }
        public string super { get; set; }
        public string Docdate { get; set; }
        public string Eng { get; set; }
        public bool Complete { get; set; }
        public List<SelectListItem> Shiftlst { get; set; }
        public List<SelectListItem> worklst { get; set; }
        public List<SelectListItem> Wclst { get; set; }



        public string CLocation { get; set; }
        public string BatchNo { get; set; }
        public string batchcomplete { get; set; }
        public string SchQty { get; set; }
        public string ProdQty { get; set; }

        public List<PBreakDet> BreakLst { get; set; }
        public string Shift { get; set; }
        public List<PProInput> inplst { get; set; }
        public List<PProOutput> outlst { get; set; }
        public List<PAPProInCons> Binconslst { get; set; }
        public List<PEmpDetails> EmplLst { get; set; }
        public List<PLogDetails> LogLst { get; set; }

        public List<string> ShiftNames { get; set; }

        public string LOCID { get; set; }

        public string BranchId { get; set; }

    }
    public class PProInput
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
        public string Time { get; set; }
        public string inpid { get; set; }
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
    public class PProOutput
    {
        public string ItemId { get; set; }
        public string APID { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> shedlst { get; set; }

        public string ShedNo { get; set; }
        public string ShedOccu { get; set; }
        public string BinId { get; set; }
        public string outid { get; set; }
        public string outBin { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Bin { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public double batchqty { get; set; }
        public double Stock { get; set; }
        public double IssueQty { get; set; }
        public string MillLoadAdd { get; set; }
        public string Result { get; set; }

        public string Status { get; set; }
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
    public class PAPProInCons
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string APID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public double ConsStock { get; set; }
        public string consunit { get; set; }
        public string unitid { get; set; }
        public string consBin { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public double consQty { get; set; }
        public double Qty { get; set; }
        public string Isvalid { get; set; }
        public string BinId { get; set; }

        public string saveitemId { get; set; }

    }
    public class PEmpDetails
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
    public class PLogDetails
    {
        public string ID { get; set; }
        public string APID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string tothrs { get; set; }
        public string Reason { get; set; }

        public string Isvalid { get; set; }
    }
    public class PBreakDet
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




    //public class PAPItemDetail
    //{

    //    public string WCID { get; set; }
    //    public string DOCID { get; set; }
    //    public string DOCDATE { get; set; }
    //    public string SHIFT { get; set; }
    //    public string ITEMID { get; set; }
    //    public string QTY { get; set; }
    //    public string CHARGINGTIME { get; set; }
    //    public string UNIT { get; set; }
    //    public string DRUMNO { get; set; }
    //    public double OUTQTY { get; set; }
    //    public string FROMTIME { get; set; }
    //    public string TESTRESULT { get; set; }
    //    public string TOTIME { get; set; }
    //    public string EXPR7 { get; set; }
    //    public string EXPR8 { get; set; }
    //    public string EMPNAME { get; set; }


    //    public double EXPR5 { get; set; }
    //    public double CONSQTY { get; set; }
    //}

    //public class APItemDetails
    //{
    //    public string DOCID { get; set; }
    //    public string ITEMID { get; set; }
    //    public string QTY { get; set; }
    //    public string CHARGINGTIME { get; set; }
    //    public string UNIT { get; set; }
    //    public string DRUMNO { get; set; }
    //    public double OUTQTY { get; set; }
    //    public string FROMTIME { get; set; }
    //    public string TESTRESULT { get; set; }
    //    public string TOTIME { get; set; }
    //    public string SHIFT { get; set; }
    //    public string EXPR6 { get; set; }
    //    public string EXPR7 { get; set; }
    //    public string EMPNAME { get; set; }
    //    public double EXPR3 { get; set; }
    //    public double CONSQTY { get; set; }

    //}
    //public class APItemDetailsc
    //{
    //    public string DOCID { get; set; }
    //    public string ITEMID { get; set; }
    //    public string QTY { get; set; }
    //    public string CHARGINGTIME { get; set; }
    //    public string UNIT { get; set; }
    //    public string DRUMNO { get; set; }
    //    public double OUTQTY { get; set; }
    //    public string FROMTIME { get; set; }
    //    public string TESTRESULT { get; set; }
    //    public string TOTIME { get; set; }
    //    public string SHIFT { get; set; }
    //    public string EXPR6 { get; set; }
    //    public string EXPR7 { get; set; }
    //    public string EMPNAME { get; set; }
    //    public double EXPR3 { get; set; }
    //    public double CONSQTY { get; set; }

    //}
}

