using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IDepartment
    {
        string DepartmentCRUD(Department ss);
        IEnumerable<Department> GetAllDepartment();

        DataTable GetDepartment(string id);
        string StatusChange(string tag, int id);

    }
}
