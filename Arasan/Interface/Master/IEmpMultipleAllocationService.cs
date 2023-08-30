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
        DataTable GetEmp();
        DataTable GetEmpAllocation();
        DataTable GetEmpMultipleItem(string PRID);
        DataTable GetMlocation();
    }
}
