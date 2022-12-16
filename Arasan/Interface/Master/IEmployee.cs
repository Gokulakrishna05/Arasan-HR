using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IEmployee
    {

       string EmployeeCRUD(Employee emp);
        DataTable GetState();
        DataTable GetCity();
        IEnumerable<Employee> GetAllEmployee();
        DataTable GetEmployee(string id);
    }
}
