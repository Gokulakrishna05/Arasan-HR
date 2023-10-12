using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPyroProduction
    {
        DataTable GetWork(string id);
        DataTable ShiftDeatils();
        DataTable GetWorkedit(string id);

        DataTable GetItemDetails(string id);
        DataTable GetStockDetails(string ItemId,string item);

        string  PyroProductionEntry(PyroProduction Cy);


        DataTable GetOutItemDetails(string id);
        DataTable GetConItemDetails(string id);
        DataTable GetItem();

        DataTable GetOutItem();
        DataTable GetEmp();
       
          
        DataTable GetItemCon();

        DataTable GetDrum();
        DataTable GetShedNo();
        DataTable GetDrum(string item);
  
        DataTable GetMachineDetails(string id);
       DataTable GetEmployeeDetails(string id);

        DataTable GetAPProd(string id);

        DataTable GetPyroProd(string id);
        DataTable GetInput(string id);
        DataTable GetOutput(string id);
        DataTable GetLogdetail(string id);
        DataTable GetCons(string id);
        DataTable GetEmpdet(string id);
        DataTable GetBreak(string id);

        IEnumerable<PyroProduction> GetAllPyro();

        DataTable GetPyroProductionName(string id);
        DataTable GetInputDeatils(string id);
        DataTable GetConsDeatils(string id);
        DataTable GetEmpdetDeatils(string id);
        DataTable GetBreakDeatils(string id);
        DataTable GetOutputDeatils(string id);
        DataTable GetLogdetailDeatils(string id);

        DataTable SaveInputDetails(string id, string item, string bin, string time, string qty, string stock, string batch, string drum);
        DataTable SaveOutputDetails(string id, string item, string bin, string stime, string ttime, string qty, string drum);
        DataTable SaveConsDetails(string id, string item, string bin, string unit, string usedqty, string qty, string stock);
        DataTable SaveEmpDetails(string id, string empname, string code, string depat, string sdate, string stime, string edate, string etime, string ot, string et, string normal, string now);
        DataTable SaveBreakDetails(string id, string machine, string des, string dtype, string mtype, string stime, string etime, string pb, string all, string reason);
        DataTable SaveLogDetails(string id, string sdate, string stime, string edate, string etime, string tot, string reason);

        string PyroProEntryCRUD(PyroProductionentryDet Cy);


        string ApprovePyroProductionEntryGURD(PyroProductionentryDet Cy);

    }
}
