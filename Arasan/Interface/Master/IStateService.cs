using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface IStateService
    {
        string StateCRUD(State by);
        IEnumerable<State> GetAllState();
        State GetStateById(string id);

    }
}
