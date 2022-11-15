using Arasan.Models;
using System.Data;

namespace Arasan.Interface;

public interface IPurchaseEnqService
{
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        DataTable GetItem(string value);


        DataTable GetItemGrp();
        DataTable GetEmp();
        DataTable GetItemDetails(string ItemId);
        DataTable GetItemCF(string ItemId, string unitid);
        string PurenquriyCRUD(PurchaseEnquiry cy);
        IEnumerable<PurchaseEnquiry> GetAllPurenquriy();
        IEnumerable<EnqItem> GetAllPurenquriyItem(string id);
        PurchaseEnquiry GetPurenqServiceById(string id);
        string PurchaseFollowupCRUD(PurchaseFollowup pf);
        IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup();
        PurchaseFollowup GetPurchaseFollowupById(string id);
        DataTable GetFolowup(string enqid);
        string EnquirytoQuote(string id);


        DataTable GetPurchaseEnqDetails(string id);
        DataTable GetPurchaseEnqItemDetails(string id);
        DataTable GetItemSubGrp();
        DataTable GetItemSubGroup(string id);

    
    
}


    //}



//    DataTable GetBranch();
//    DataTable GetSupplier();
//    DataTable GetCurency();
//    DataTable GetItem(string value);


//    DataTable GetItemGrp();
//    DataTable GetEmp();
//    DataTable GetItemDetails(string ItemId);
//    DataTable GetItemCF(string ItemId, string unitid);
//    string PurenquriyCRUD(PurchaseEnquiry cy);
//    IEnumerable<PurchaseEnquiry> GetAllPurenquriy();
//    IEnumerable<EnqItem> GetAllPurenquriyItem(string id);
//    PurchaseEnquiry GetPurenqServiceById(string id);
//    string PurchaseFollowupCRUD(PurchaseFollowup pf);
//    IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup();
//    PurchaseFollowup GetPurchaseFollowupById(string id);
//    DataTable GetFolowup(string enqid);
//    string EnquirytoQuote(string id);


//    DataTable GetPurchaseEnqDetails(string id);
//    DataTable GetPurchaseEnqItemDetails(string id);
//    DataTable GetItemSubGrp();
//    DataTable GetItemSubGroup(string id);
//}

   


