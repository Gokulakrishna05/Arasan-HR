using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Arasan.Interface.Master
{
    public interface IEmployee
    {

        string EmployeeCRUD(Employee emp, List<IFormFile> file1);
        DataTable GetState();
        DataTable GetCity(string id);
        DataTable GetCityst(string id);
        DataTable GetEMPDept();
        DataTable GetDesign();
        DataTable GetEmployee(string id);
        DataTable GetEmpEduDeatils(string data);
        DataTable GetEmpPersonalDeatils(string id);
        DataTable GetEmpSkillDeatils(string id);
        DataTable GetCurrentUser(string id);
        string GetMultipleLocation(MultipleLocation mp);
        long GetMregion(string regionid, string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

        DataTable GetAllEmployee(string strStatus);


        string AddBankCRUD(string id);
        string AddBloodGroupCRUD(string id);
        string AddCommunityCRUD(string id);
        string AddDispCRUD(string id);


    }
}
