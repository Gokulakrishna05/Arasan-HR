using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface IItemTransferService
    {
        string ItemTransferCRUD(ItemTransfer by);
        IEnumerable<ItemTransfer> GetAllItemTransfer();
        ItemTransfer GetItemTransferById(string id);

        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetItem(string value);
        DataTable GetItemTransferDetails(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetItemTransferItemDetails(string id);
        IEnumerable<Itemtran> GetAllItemTransferItem(string id);
    }
}
