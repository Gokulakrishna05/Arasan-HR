namespace Arasan.Interface;
using System.Data;
using Arasan.Models;
public interface IActivitiesCO
{
    DataTable GetBranch();
    DataTable GetLocation();
    DataTable GetMachine();
    DataTable GetActivity();
    DataTable GetItem();
    DataTable GetEmplId();
    DataTable GetParty();
    string ActivitiesCOCRUD(ActivitiesCO cy);

    string StatusChange(string tag, string id);
    string RemoveChange(string tag, string id);
    DataTable GetAllActivitiesCO(string strStatus);

    DataTable GetEditActivityCo(string id);
    DataTable GetEditCons(string id);
    DataTable GetEditEmp(string id);
    DataTable GetEditSerdetail(string id);
    DataTable GetEditChkl(string id);




}

