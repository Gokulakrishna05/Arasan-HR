using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Master
{
    public interface IEmpMultipleAllocationService
    {
        string EmpMultipleAllocationCRUD(EmpMultipleAllocation cy);
        //IEnumerable<EmpMultipleAllocation> GetAllEmpMultipleAllocation();
        DataTable GetEmp(string action);
        DataTable GetEmpAllocation(string strStatus);
        DataTable GetEmpLocation(string id);
        DataTable GetEmpMultipleAllocationReassign(string id);
        DataTable GetEmpMultipleAllocationServiceName(string id);
        DataTable GetEmpMultipleItem(string PRID, string strStatus);
        DataTable GetMlocation();
        long GetMregion(string? v, string id);
        string StatusChange(string tag, int id);

        string RemoveChange(string tag, int id);

        string ReassignEmpMultipleAllocation(EmpReasign cy);
    }
}
