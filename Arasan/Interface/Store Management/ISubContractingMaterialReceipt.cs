using Arasan.Models;
using System.Data;


namespace Arasan.Interface.Store_Management
{
    public interface ISubContractingMaterialReceipt
    {
        DataTable GetAllSubContractingMaterialItem(string strStatus);
        DataTable GetSubDelivItemDetails(string itemId);
        DataTable GetDrum();
        DataTable LedgerList();
        DataTable AccconfigLst();
        DataTable GetDrumItemDetails(string id);
        DataTable GetPartyItem(string id);
        DataTable GetItemDetails(string itemId);
        DataTable GetItems(string itemId);
        DataTable GetWCDetails(string itemId);
        string SubContractingMaterialReceiptCRUD(SubContractingMaterialReceipt cy);
        DataTable GetSubRecvItemDetails(string itemId);
        DataTable GetDC();
        DataTable GetSupplier( );
        DataTable GetSubContract(string id);
        DataTable FetchAccountRec(string id);
        DataTable GetMaterialReceipt(string id);
        DataTable GetReceiptItem(string id);
        DataTable GetDrumdetails(string id);
        string StatusChange(string tag, string id);
        string StatusActChange(string tag, string id);
        Task<IEnumerable<SubMrdc>> GetSubMrdc(string id);
        Task<IEnumerable<SubMrdcdet>> GetSubMrdcdet(string id);
        Task<IEnumerable<SubActMrdcdet>> GetSubActMrdcdet(string id);
    }
}
