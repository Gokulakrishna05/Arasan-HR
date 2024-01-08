using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IStateService
    {
        string StateCRUD(State by);
        //IEnumerable<State> GetAllState(string status);
        State GetStateById(string id);

        DataTable GetEditState(string id);
        DataTable Getcountry();

        //DataTable Getcountry();
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllState(string strStatus);
    }
}
