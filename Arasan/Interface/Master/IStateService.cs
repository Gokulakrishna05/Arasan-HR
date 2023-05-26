using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IStateService
    {
        string StateCRUD(State by);
        IEnumerable<State> GetAllState();
        State GetStateById(string id);

        DataTable Getcountry();
        string StatusChange(string tag, int id);
    }
}
