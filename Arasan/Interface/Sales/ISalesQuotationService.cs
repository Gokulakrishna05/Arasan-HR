using Arasan.Models;
using System.Data;
using System.Collections.Generic;

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
        //string QuotetoOrder(string cy);
        DataTable GetItemCF(string ItemId, string unitid);
        IEnumerable<QuoItem> GetAllSalesQuotationItem(string id);
        DataTable GetCustomerDetails(string itemId);
        DataTable GetCusType();
        DataTable GetPurchaseQuotationDetails(string id);
        string SalesQuotationFollowupCRUD(QuotationFollowup pf);
        DataTable GetFolowup(string enqid);
        //DataTable GetSalesQuotationByName(string id);
        //DataTable GetSalesQuotationItem(string id);
        string StatusChange(string tag, int id);
        DataTable GetSalesQuo(string id);
        DataTable GetSalesQuoItem(string id);
    }
}
