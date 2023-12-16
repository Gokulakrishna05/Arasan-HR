using Arasan.Models;
using System.Data;


namespace Arasan.Interface.Store_Management
{
    public interface ISubContractingMaterialReceipt
    {
        DataTable GetAllSubContractingMaterialItem(string strStatus);
        DataTable GetSubDelivItemDetails(string itemId);
        DataTable GetDrum();
        DataTable GetDrumItemDetails(string id);
        DataTable GetItemDetails(string itemId);
        DataTable GetItems(string itemId);
        DataTable GetWCDetails(string itemId);
        string SubContractingMaterialReceiptCRUD(SubContractingMaterialReceipt cy);
        DataTable GetSubRecvItemDetails(string itemId);
        DataTable GetDC();
        DataTable GetSupplier(string id);
        DataTable GetSubContract(string id);
        DataTable GetMaterialReceipt(string id);
        DataTable GetReceiptItem(string id);
        DataTable GetDrumdetails(string id);
    }
}
