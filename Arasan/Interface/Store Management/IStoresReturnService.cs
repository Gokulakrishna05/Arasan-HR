using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Stores_Management
{
    public interface IStoresReturnService
    {
        IEnumerable<StoresReturn> GetAllStoresReturn(string st, string ed);
        DataTable GetLocation();
        DataTable GetBin();
        DataTable GetBranch();
        string StoresReturnCRUD(StoresReturn cy);
        string StatusChange(string tag, string id);
        DataTable GetItem(string id);
        DataTable GetStoresReturn(string id);
        DataTable GetSRItemDetails(string id);
        IEnumerable<StoreItem> GetAllStoresReturnItem(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable Getstkqty(string ItemId, string loc, string branch);
        DataTable Getloc(string Poid);
        DataTable GetAllListStoresReturnItems(string strStatus);
    }
}
