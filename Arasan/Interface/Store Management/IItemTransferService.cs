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
        DataTable GetItem();
        DataTable GetItemTransferDetails(string id);

    }
}
