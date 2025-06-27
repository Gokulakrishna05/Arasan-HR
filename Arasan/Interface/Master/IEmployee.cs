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
        string Changepass(Employee emp );
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
        Task<IEnumerable<EmployeeDetails>> GetEmployeeDetails(string id);
        Task<IEnumerable<EmpEduDetails>> GetEmpEduDetails(string id);
        Task<IEnumerable<EmpOthDetails>> GetEmpOthDetails(string id);

        string AddBankCRUD(string id);
        string AddBloodGroupCRUD(string id);
        string AddCommunityCRUD(string id);
        string AddDispCRUD(string id);


        DataTable GetPayCodeDeatils(string id);
        DataTable GetBranchDeatils(string id);
        DataTable GetDepCodeDeatils(string id);
        DataTable GetPeforDeatils(string id);
        DataTable GetEmrgConDeatils(string id);
        DataTable GetInsuranceDeatils(string id);
        DataTable GetPrvHisDeatils(string id);
        DataTable GetEmpAtted(string id);
          DataTable GetBr();
        DataTable GetDCode();


    }
}
