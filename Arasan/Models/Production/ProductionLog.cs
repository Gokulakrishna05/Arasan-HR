using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models 
{
    public class ProductionLog
    {
        public ProductionLog()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.EmpList = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string Itemname { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkId { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ProcessId { get; set; }
        public string Shift { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> RecList;
        public List<SelectListItem>EmpList;
        public string Supervised { get; set; }
        public double FuelQty { get; set; }
        public string ProdLog { get; set; }
        public string ProcessLot { get; set; }
        public double InputQty { get; set; }
        public double OutputQty { get; set; }
        public double ConsQty { get; set; }
        public double TotalPowder { get; set; }
        public double TotalDust { get; set; }
        public double TotalWaste { get; set; }
        public double Rmvalue { get; set; }
        public string Entered { get; set; }
        public string ComplYN { get; set; }
        public string MainValue { get; set; }
        public string ProdSieve { get; set; }
        public string Remark { get; set; }
        public double EUnit { get; set; }
        public double Melting { get; set; }
        public List<WorkCenter> WorkLst { get; set; }
        public List<MachineItem> MachineLst { get; set; }
        public List<EmpDetail> EmpLst { get; set; }
        public List<BreakDetail> BreakLst { get; set; }
        public List<InputDetail> InputLst { get; set; }
        public List<ConsumDetail> ConsLst { get; set; }
        public List<OutputDetail> OutLst { get; set; }
        public List<WasteDetail> WasteLst { get; set; }
        public List<SourcingDetail> SourcingLst { get; set; }
        public List<BunkerDetail> BunkLst { get; set; }
        public List<ParameterDetail> ParamLst { get; set; }
        public List<ProcessDetail> ProcessLst { get; set; }
    }
    public class WorkCenter
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string Status { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Reasonlst { get; set; }
        public string Reason { get; set; }

        public List<SelectListItem> WorkCenterlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> PTypelst { get; set; }
        public string PType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double TotalHrs { get; set; }
        public string Isvalid { get; set; }
    }
    public class MachineItem
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string Status { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Reasonlst { get; set; }
        public string Reason { get; set; }

        public List<SelectListItem> Maclst { get; set; }

        public string Machine { get; set; }
        public string MachineId { get; set; }
        public List<SelectListItem> PTypelst { get; set; }
        public string PType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double TotalHrs { get; set; }
        public double TotalMachineCost { get; set; }
        public double TotalMins { get; set; }
        public string Isvalid { get; set; }
    }
    public class EmpDetail
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string EmpCode { get; set; }
        public string saveItemId { get; set; }
       

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
    public class BreakDetail
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
       
        public string saveItemId { get; set; }
        public List<SelectListItem> Reasonlst { get; set; }
        public string Reason { get; set; }

        public List<SelectListItem> Machinelst { get; set; }

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
    public class InputDetail
    {
        public string ID { get; set; }
        public List<SelectListItem> Drumlst { get; set; }
        public string DrumNo { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }

        public List<SelectListItem> DrumYNlst { get; set; }

        public string Drumyn { get; set; }
   
        public List<SelectListItem> Lotlst { get; set; }
        public string LotYN { get; set; }

        public string Batch { get; set; }
        public double BatchQty { get; set; }
        public double Stock { get; set; }
        public double IQty { get; set; }
        public double IBRate { get; set; }
        public string Isvalid { get; set; }
    }
    public class ConsumDetail
    {
        public string ID { get; set; }
        
        public string Unit { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }

       

        public List<SelectListItem> Lotlst { get; set; }
        public string LotYN { get; set; }

        
        public double ConsQty { get; set; }
        public double Stock { get; set; }
        public double Value { get; set; }
        public double Rate { get; set; }
        public string Isvalid { get; set; }
    }
    public class OutputDetail
    {
        public string ID { get; set; }
        public List<SelectListItem> Drumlst { get; set; }
        public string DrumNo { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }

        public List<SelectListItem> DrumYNlst { get; set; }

        public string Drumyn { get; set; }

        public List<SelectListItem> Lotlst { get; set; }
        public string LotYN { get; set; }

        public string Batch { get; set; }
        public double OQty { get; set; }
        public double Stock { get; set; }
       
        public double Hrs { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Isvalid { get; set; }
    }
    public class WasteDetail
    {
        public string ID { get; set; }
      
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }

     
      
       

        public string WBatch { get; set; }
        public double WQty { get; set; }
       
        public double WRate { get; set; }
       
        public string Isvalid { get; set; }
    }
    public class SourcingDetail
    {
        public string ID { get; set; }
      
        public string saveItemId { get; set; }




        public string NOW { get; set; }
        public string NoOfEmp { get; set; }
        public double EmpCost { get; set; }
        public double Expence { get; set; }

        public double WorkHrs { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Isvalid { get; set; }
    }
    public class BunkerDetail
    {
        public string ID { get; set; }

       




        public double OPBin { get; set; }
        public double PIP { get; set; }
        public double GIP { get; set; }
        public double CLBin { get; set; }

        public double MLOP { get; set; }
        public double MLAdd { get; set; }
        public double MLDed { get; set; }
        public double MLCL { get; set; }
      
        
    }
    public class ParameterDetail
    {
        public string ID { get; set; }






        public string Param { get; set; }
        public string Unit { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Value { get; set; }
       
        public string Isvalid { get; set; }


    }
    public class ProcessDetail
    {
        public string ID { get; set; }




        public List<SelectListItem> Procelst { get; set; }
        public string Process { get; set; }

        public double DistNo { get; set; }
        public double DistNo1 { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double RunHrs { get; set; }
        public double TotHrs { get; set; }
        public double BreakHrs { get; set; }
        public string Seq { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Batch { get; set; }
        public string Narr { get; set; }
        public string Isvalid { get; set; }


    }
}
