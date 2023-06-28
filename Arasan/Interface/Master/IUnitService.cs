using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;                        

namespace Arasan.Interface.Master

{
    public interface IUnitService
    {
        string UnitCRUD(Unit cy);
        IEnumerable<Unit> GetAllUnit(string status);
        DataTable GetUnit(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
