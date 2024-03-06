using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface.Sales
{
    public interface ISalesQuotationService
    {
        IEnumerable<SalesQuotation> GetAllSalesQuotation(string status);
        DataTable GetBranch();
        DataTable Getcountry();
        DataTable GetEnquiry();

        DataTable GetSupplier();
        DataTable GetSalesQuotation(string id);
        DataTable GetSalesQuotationItemDetails(string id);
        string SalesQuotationCRUD(SalesQuotation cy);
       
        
        DataTable GetItemCF(string ItemId, string unitid);
        IEnumerable<QuoItem> GetAllSalesQuotationItem(string id);
        DataTable GetCustomerDetails(string itemId);
        DataTable GetEnqDetails(string itemId);
        
        DataTable GetQuobyId(string itemId);
        DataTable GetCurrbyId(string itemId);
        DataTable GetCustypebyId(string itemId);
        DataTable GetTypelstbyId(string itemId);
        DataTable GetPribyId(string itemId);

        //DataTable GetItemgroupbyId(string itemId);
        //DataTable SalesQuotationDetail(string id, string item);
        DataTable GetItemgrpDetail(string id);
        DataTable GetCusType();
        //DataTable GetCusName();
        DataTable GetPurchaseQuotationDetails(string id);
        string SalesQuotationFollowupCRUD(QuotationFollowup pf);
        DataTable GetFollowup(string enqid);
        //DataTable GetSalesQuotationByName(string id);
        //DataTable GetSalesQuotationItem(string id);
        //string StatusChange(string tag, int id);
        DataTable GetSalesQuo(string id);
        DataTable GetSalesQuoItem(string id);

        Task<IEnumerable<SQuoItemDetail>> GetSQuoItem(string id);
        DataTable GetAllListSalesQuotationItems(string strStatus);
        string StatusDeleteMR(string tag, int id);
        DataTable GetSalesQuotationByName(string id);
        DataTable GetSalesQuotationItem(string id);

        string SalesQuotationWorkOrder(string QuoId);

    }
}
