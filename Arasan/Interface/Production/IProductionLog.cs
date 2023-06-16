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
        DataTable GetProConsDet(string id);
        DataTable GetProOutDet(string id);
        DataTable GetProOutsDet(string id);
        DataTable GetProBunkDet(string id);
        DataTable GetProParamDet(string id);
        DataTable GetProProcessDet(string id);
        DataTable GetProWasteDet(string id);
        DataTable GetEmployeeDetails(string id);
        DataTable GetProductionLog(string id);
        DataTable GetMachineDetails(string id);
        IEnumerable<ProductionLog> GetAllProductionLog();
        string ProductionLogCRUD(ProductionLog id);
        DataTable GetProductionLogByName(string name);
        DataTable ProWorkCenterDet(string name);
        DataTable ProMachineDet(string name);
        DataTable ProEmpDet(string name);
        DataTable ProBreakDet(string name);
        DataTable ProInpDet(string name);
        DataTable ProConsDet(string name);

        DataTable ProOutDet(string name);
        DataTable ProWasteDet(string name);
        DataTable ProOutsDet(string name);
        DataTable ProBunkDet(string name);
        DataTable ProParamDet(string name);
        DataTable ProProcessDet(string name);

        string StatusChange(string tag, int id);

    }
}
