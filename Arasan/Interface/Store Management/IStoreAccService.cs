using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IStoreAccService
    {
        string StoreAccCRUD(StoreAcc by);
        IEnumerable<StoreAcc> GetAllStoreAcc();
        StoreAcc GetStoreAccById(string id);

        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetItem();
        DataTable GetStoreAccDetails(string id);
    }

}

