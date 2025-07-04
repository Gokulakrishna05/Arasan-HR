﻿namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

    public interface IEmpShiftSchedule
    {
        DataTable GetEmplId();
        DataTable GetShift();
        DataTable GetCategory();
        DataTable GetDep();
        DataTable GetEmployeeDetail(string ItemId);
        string GetInsEmp(EmpShiftScheduleModel E);
        DataTable GetAllEmpShift(string strStatus);
        DataTable GetShiftScheduleBasicEdit(string id);
        DataTable GetShiftScheduleDetailEdit(string id);
        string StatusChange(string tag, string id);

        //DataTable GetAllShiftType(string sitid, string endid);
        //DataTable GetAllShiftTypeDetail(string id);
    //DataTable GetAllShiftType(string id);
    //DataTable GetShiftType(string id);
}

