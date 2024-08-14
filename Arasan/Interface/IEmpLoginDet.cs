namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

public interface IEmpLoginDet
{

    DataTable GetAllEmpLog(string strStatus);
    string GetInslog(EmpLoginDetModel E);

    DataTable GetEmpName();
    DataTable GetCategory();
    DataTable GetEmploginDetBasicEdit(string id);
    DataTable GetEmploginDetDetailEdit(string id);
    string StatusChange(string tag, string id);



}

