using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface 
{
    public interface IProductionLog
    {
        DataTable ShiftDeatils();
        DataTable BindProcess();
        DataTable GetWorkCenter();
        DataTable GetReason();
        DataTable GetMachine();
        DataTable DrumDetails();
        DataTable GetItem();
        DataTable GetProWorkCenterDet(string id);
        DataTable GetProMachineDet(string id);
        DataTable GetProEmpDet(string id);
        DataTable GetProBreakDet(string id);
        DataTable GetProInpDet(string id);
        DataTable GetEmployeeDetails(string id);
        DataTable GetProductionLog(string id);
        DataTable GetMachineDetails(string id);
        IEnumerable<ProductionLog> GetAllProductionLog();
        string ProductionLogCRUD(ProductionLog id);
    }
}
