using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;                        

namespace Arasan.Interface.Master

{
    public interface IUnitService
    {
        string UnitCRUD(Unit cy);
        IEnumerable<Unit> GetAllUnit();
        DataTable GetUnit(string id);
        string StatusChange(string tag, int id);
    }
}
