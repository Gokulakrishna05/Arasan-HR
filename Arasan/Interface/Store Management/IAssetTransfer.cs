using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface IAssetTransfer
    {
        string AssetTransferCRUD(AssetTransfer ss);
        DataTable BindBinID();
        DataTable GetAllListAssetTransferItemItems(string strStatus);
        DataTable GetAssetTransfer(string id);
        DataTable GetAssetTransferItem(string id);
        DataTable GetItem();
        DataTable GetItemDetails(string itemId);
        string StatusChange(string tag, string id);
    }
}
