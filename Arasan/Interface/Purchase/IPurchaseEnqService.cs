using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPurchaseEnqService
    {
        string PurchaseEnquiryCRUD(PurchaseEnquiry by);
        IEnumerable<PurchaseEnquiry> GetAllPurchaseEnquiry();
        PurchaseEnquiry GetPurchaseEnquiryById(string id);
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        DataTable GetItem(string value);

        DataTable GetItemGrp();
        DataTable GetEmp();
        DataTable GetItemDetails(string ItemId);
        DataTable GetItemCF(string ItemId,string unitid);
    }
}
