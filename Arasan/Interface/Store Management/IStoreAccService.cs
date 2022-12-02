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
        DataTable GetItem(string value);
        DataTable GetStoreAccDetails(string id);
        DataTable GetStoreAccItemDetails(string id);
        DataTable GetItemCF(string ItemId, string Unitid);
        IEnumerable<StoItem> GetAllStoreAccItem(string id);
    }

}
