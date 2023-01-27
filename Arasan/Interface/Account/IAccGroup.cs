using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IAccGroup
    {
        IEnumerable<AccGroup> GetAllAccGroup();
        string AccGroupCRUD(AccGroup cy);
        DataTable GetAccGroup(string id);
    }
}
