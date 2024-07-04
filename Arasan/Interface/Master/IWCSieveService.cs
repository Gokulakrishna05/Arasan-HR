using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface.Master
{
    public interface IWCSieveService
    {
        DataTable GetAllWCSieve(string strStatus);
        DataTable GetEditWCSieve(string id);
        DataTable GetSieve();
        DataTable GetViewEditWCSieve(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        string WCSieveCRUD(WCSieve ss);
        DataTable WCSieveDeatils(string id);
        DataTable WCSieveViewDeatils(string id);
    }
}
