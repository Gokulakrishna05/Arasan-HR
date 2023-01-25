using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Stores_Management
{
    public interface IStoresReturnService
    {
        IEnumerable<StoresReturn> GetAllStoresReturn();
        DataTable GetLocation();
        DataTable GetBranch();
        string StoresReturnCRUD(StoresReturn cy);
        DataTable GetStoresReturn(string id);
        DataTable GetSRItemDetails(string id);
        IEnumerable<StoreItem> GetAllStoresReturnItem(string id);
        DataTable GetItemCF(string ItemId, string unitid);
    }
}
