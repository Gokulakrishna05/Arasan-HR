using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IStop
    {
        string StopCRUD(Stop ss);

        DataTable GetStop(string id);

        DataTable GetviewStop(string id);
        //DataTable GetDepartmentDetail(string id);

        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

        DataTable GetAllStop(string strStatus);

    }
}
