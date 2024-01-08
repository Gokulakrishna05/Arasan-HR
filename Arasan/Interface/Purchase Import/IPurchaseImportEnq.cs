using Arasan.Models;
using System.Data;

namespace Arasan.Interface;

public interface IPurchaseImportEnqService
{
    DataTable GetBranch();
    DataTable GetSupplier();
    DataTable GetCurency();
    DataTable GetItem();
    DataTable GetPurchaseEnqByID(string id);
    DataTable GetPurchaseEnq(string id);
    DataTable GetItemGrp();
    DataTable GetEmp();
    DataTable GetItemDetails(string ItemId);
    DataTable GetItemCF(string ItemId, string unitid);
    string PurenquriyCRUD(PurchaseImportEnq cy);
    IEnumerable<PurchaseImportEnq> GetAllPurenquriy(string st, string ed);
    IEnumerable<ImportEnqItem> GetAllPurenquriyItem(string id);
    //PurchaseEnquiry GetPurenqServiceById(string id);
    string PurchaseFollowupCRUD(PurchaseImportFollowup pf);
    //IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup();
    //PurchaseFollowup GetPurchaseFollowupById(string id);
    DataTable GetFolowup(string enqid);

    string EnquirytoQuote(string id);

    //DataTable GetFollowupDetail(string id);
    DataTable GetPurchaseEnqDetails(string id);
    DataTable GetPurchaseEnqFolwDetails(string id);
    DataTable GetPurchaseEnqItemDetails(string id);
    DataTable GetPurchaseEnqItem(string id);
    DataTable GetItemSubGrp();
    DataTable GetItemSubGroup(string id);

    string StatusChange(string tag, string id);
    DataTable GetAllPurchaseEnquiryItems(string strStatus);
    string RegenerateCRUD(PurchaseImportEnq cy);
    DataTable GetRegenerateDetails(string id);
}


