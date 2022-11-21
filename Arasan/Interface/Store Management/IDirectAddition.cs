using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface IDirectAddition
    {
        string DirectAdditionCRUD(DirectAddition by);
        IEnumerable<DirectAddition> GetAllDirectAddition();
        DirectAddition GetDirectAdditionById(string id);

        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetItem();
        DataTable GetDirectAdditionDetails(string id);
    }
}
