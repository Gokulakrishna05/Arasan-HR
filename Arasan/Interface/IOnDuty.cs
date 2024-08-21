namespace Arasan.Interface;
using System.Data;
using Arasan.Models;
public interface IOnDuty
{
    DataTable GetEmplId();
    string OnDutyCRUD(OnDuty cy);
    DataTable GetAllOnDuty(string strStatus);

    string StatusChange(string tag, string id);
    string RemoveChange(string tag, string id);

    DataTable GetEditOnDuty(string id);
    DataTable GetEditDutDet(string id);

}

