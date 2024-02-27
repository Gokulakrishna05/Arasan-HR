using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPyroProduction
    {

        //string StatusChange(string tag, string id);
        //string RemoveChange(string tag, string id);
        DataTable GetAllPyro(string strStatus);
        DataTable GetWork();
        DataTable ShiftDeatils();
        DataTable GetReason();
        DataTable GetWorkedit(string id);
        DataTable GetProdSch(string id);

        DataTable GetItemDetails(string id);
        DataTable GetStockDetails(string ItemId,string item);

        string  PyroProductionEntry(PyroProduction Cy);

        DataTable GetProduction(string id);
        DataTable GetOutItemDetails(string id);
        DataTable GetConItemDetails(string id);
        DataTable GetItem(string id);
        DataTable GetDrumBatch(string ItemId, string loc, string item);
        DataTable GetOutItem(string id);
        DataTable GetEmp();
       
          
        DataTable GetItemCon(string id);

        DataTable GetDrum();
        DataTable GetShedNo();
        DataTable GetDrum(string item,string loc);
  
        DataTable GetMachineDetails(string id);
       DataTable GetEmployeeDetails(string id);

        DataTable GetAPProd(string id);

        DataTable GetPyroProd(string id);
        DataTable GetInput(string id);
        DataTable GetOutput(string id);
        DataTable GetLogdetail(string id);
        DataTable GetOutsdetail(string id);
        DataTable GetCons(string id);
        DataTable GetEmpdet(string id);
        DataTable GetBreak(string id);
        string GetBinOPBal(string ProcLotNo, string DocID, string ProcessMastID, string WcMastID);
        string GetMLOPBal(string ProcLotNo, string DocID, string ProcessMastID, string WcMastID);

            //IEnumerable<PyroProduction> GetAllPyro();

            DataTable GetPyroProductionName(string id);
        DataTable GetInputDeatils(string id);
        DataTable GetConsDeatils(string id);
        DataTable GetEmpdetDeatils(string id);
        DataTable GetBreakDeatils(string id);
        DataTable GetOutputDeatils(string id);
        DataTable CuringsetDetails(string id);

        Task<IEnumerable<PyroDetail>> Getpyropdf(string id);

        DataTable GetLogdetailDeatils(string id);

        string SaveBasicDetail(string schno, string docid, string docdate, string loc, string proc, string shift, string schqty, string prodqty, string wcid, string proclot, string branchid,string enterd);

        DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch, string drum,int r);
        DataTable SaveOutputDetails(string id, string item, string stime, string ttime, string qty, string drum,string status, string stock, string excess,string shed);
        DataTable SaveBunkDetails(string id, string opbin, string powder, string grase, string totip, string top, string oxd, string trm, string clbin, string mlop, string mladd, string mlded, string mlcl);
        DataTable SaveConsDetails(string id, string item, string bin, string unit, string usedqty, string qty, string stock,int l);
        DataTable SaveEmpDetails(string id, string empname, string code, string depat, string sdate, string stime, string edate, string etime, string ot, string et, string normal, string now);
        DataTable SaveBreakDetails(string id, string machine, string des, string dtype, string mtype, string stime, string etime, string pb, string all, string reason);
        DataTable SaveLogDetails(string id, string sdate, string stime, string edate, string etime, string tot, string reason);
        DataTable SaveOutsDetails(string id, string noofemp, string sdate, string stime, string edate, string etime, string workhrs, string cost, string expence, string now);

        string PyroProEntryCRUD(PyroProductionentryDet Cy);


        string ApprovePyroProductionEntryGURD(PyroProductionentryDet Cy);

    }
}
