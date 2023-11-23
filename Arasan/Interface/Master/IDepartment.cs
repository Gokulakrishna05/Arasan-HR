using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IDepartment
    {
        string DepartmentCRUD(Department ss);

        DataTable GetDepartment(string id);

        //DataTable GetDepartmentDetail(string id);

        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllDEPARTMENT(string strStatus);

    }
}
