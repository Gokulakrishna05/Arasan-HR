using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPyroProduction
    {
        DataTable GetWork(string id);
        DataTable ShiftDeatils();
         
     
        DataTable GetItemDetails(string id);
        string  PyroProductionEntry(PyroProduction Cy);


        DataTable GetOutItemDetails(string id);
        DataTable GetConItemDetails(string id);
        DataTable GetItem();

        DataTable GetOutItem();
        DataTable GetEmp();
       
          
        DataTable GetItemCon();

        DataTable GetDrum();
  
        DataTable GetMachineDetails(string id);
       DataTable GetEmployeeDetails(string id);

        DataTable GetAPProd(string id);

        DataTable GetInput(string id);
        DataTable GetOutput(string id);
        DataTable GetLogdetail(string id);
        DataTable GetCons(string id);
        DataTable GetEmpdet(string id);
        DataTable GetBreak(string id);

    }
}
