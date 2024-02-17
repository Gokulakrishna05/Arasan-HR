using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface 
{
	public interface IAPProductionEntry
	{


		DataTable ShiftDeatils();
		string APProductionEntryCRUD(APProductionentry Cy);

        string APProductionEntryDetCRUD(APProductionentryDet Cy );

        string APProEntryCRUD(APProductionentryDet Cy);

        string ApproveAPProEntryCRUD(APProductionentryDet Cy);

        IEnumerable<APProductionentry>  GetAllAPProductionentry();
		DataTable GetItemDetails(string id);

        DataTable SaveOutDetails(string id, string item,string drum, string time, string qty, string totime, string exqty, string stat, string stock,string loc, string work, string process, string shift, string schedule,string doc);

        DataTable GetOutItemDetails(string id);
        DataTable GetConItemDetails(string id);
        
		DataTable GetItem();

        DataTable GetOutItem();
        DataTable GetEmp();
        DataTable GetReason();
        DataTable GetSupEmp(string id);
        DataTable GetBatch(string id);
        DataTable GetAPProd(string id);
        DataTable GetBatchInput(string id);
        DataTable GetBatchOutput(string id);

        DataTable GetInput(string id);
        DataTable GetOutput(string id);
        DataTable GetLogdetail(string id);
        DataTable GetOutsdetail(string id);
        DataTable GetBatchItem(string id);
        DataTable GetBatchOutItem(string id);

        DataTable GetResult(string id);
        DataTable GetCons(string id);
        DataTable GetEmpdet(string id);
        DataTable GetBreak(string id);
        DataTable GetItemCon(string id);
        DataTable GetInpDrum(string ItemId,string loc);
        DataTable GetDrumBatch(string ItemId,string loc,string item);
        DataTable GetStockBatch(string ItemId,string loc );

        DataTable GetDrum();

        DataTable GetAPWorkCenter();
        DataTable GetProcess();
		DataTable GetMachineDetails(string id);

        
        DataTable GetEmployeeDetails(string id);


        DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch,string drum, int r);
        DataTable SaveConsDetails(string id, string item, string unit, string qty, string usedqty, string work, string process, int l);
        DataTable SaveEmpDetails(string id, string empname, string code, string depat, string sdate, string stime, string edate, string etime, string ot, string et, string normal, string now);
        DataTable SaveOutsDetails(string id, string noofemp, string sdate, string stime, string edate, string etime, string workhrs, string cost, string expence, string now);
        DataTable SaveBreakDetails(string id, string machine, string des, string dtype, string mtype, string stime, string etime, string pb,string all, string reason);
        DataTable SaveLogDetails(string id, string sdate, string stime, string edate, string etime, string tot, string reason);
        Task<IEnumerable<APItemDetail>> GetAPItem( string aid);
        Task<IEnumerable<APItemDetails>> GetAPItems(string bid );
        Task<IEnumerable<APItemDetailsc>> GetAPItemsc(string cid);
        DataTable GetAPProductionentryName(string id);
        DataTable GetInputDeatils(string id);
        DataTable GetConsDeatils(string id);
        DataTable GetEmpdetDeatils(string id);
        DataTable GetBreakDeatils(string id);
        DataTable GetOutputDeatils(string id);
        DataTable GetLogdetailDeatils(string id);

        DataTable GetStkDetails(string Lot, string brid, string loc,string item);
        DataTable Getstkqty(string ItemId, string locid );
        DataTable GetConstkqty(string ItemId, string locid, string brid);
        DataTable GetLotNo(string item, string loc, string branch);
        DataTable GetAllAPProductionentryItems();
    }
}
