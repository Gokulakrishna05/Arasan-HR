using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPurchaseIndent
    {
        DataTable GetBranch();
        DataTable GetLocation();
        DataTable GetSLocation();
        DataTable GetItemSubGrp();
        DataTable GetItem();
        DataTable GetItemDetails(string ItemId);
        DataTable GetEmp();
        string IndentCRUD(PurchaseIndent cy);
        DataTable GetIndent();
        DataTable GetIndentItem(string PRID);
        DataTable GetIndentItemApprove();
        DataTable GetIndentItemApproved();
        DataTable GetIndentItemSupp();
        DataTable GetIndentItemSuppDetail();
        DataTable GetHistory(string id);
        DataTable GetIndentItemSuppEnq(string id);
        DataTable GetLasttwoSupp(string id);

        DataTable GetSuppPurchaseDetails(string Partyid, string itemid);
        DataTable GetSupplier();
        string GenerateEnquiry(string[] selectedRecord, string supid);
        DataTable GetIndentItembyItemd(string ItemId);
        DataTable GetIndetnPlacedDetails(string ItemId);

    }
}
