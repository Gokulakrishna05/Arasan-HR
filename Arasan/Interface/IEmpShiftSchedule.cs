namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

    public interface IEmpShiftSchedule
    {
        DataTable GetEmplId();
        DataTable GetShift();
        DataTable GetCategory();
        DataTable GetDep();
        string GetInsEmp(EmpShiftScheduleModel E);
        DataTable GetAllEmpShift(string strStatus);
    DataTable GetShiftScheduleBasicEdit(string id);
    DataTable GetShiftScheduleDetailEdit(string id);
    string StatusChange(string tag, string id);



}

