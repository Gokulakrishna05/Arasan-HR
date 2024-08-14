namespace Arasan.Interface;
 using System.Data;
using Arasan.Models;


public interface IEmployeeAttendanceDetails
    {

            DataTable GetEmp();
            DataTable GetEmployee();
           
           string GetInsEmp(EmployeeAtttendanceDetailsModel E);
           DataTable GetEmployee(string id);
           DataTable GetAllEmployeeDetail(string strStatus);
          DataTable GetEmployeeAttendanceBasicEdit(string id);
          DataTable GetEmployeeAttendanceDetailEdit(string id);

          string StatusChange(string tag, string id);






}

