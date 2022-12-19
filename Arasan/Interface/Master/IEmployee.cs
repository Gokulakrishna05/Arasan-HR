using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Arasan.Interface.Master
{
    public interface IEmployee
    {

       string EmployeeCRUD(Employee emp);
        DataTable GetState();
        DataTable GetCity(string value);
        IEnumerable<Employee> GetAllEmployee();
        DataTable GetEmployee(string id);
        DataTable GetEmpEduDeatils(string data);
    }
}
