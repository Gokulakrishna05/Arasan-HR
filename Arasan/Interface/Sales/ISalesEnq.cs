using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface ISalesEnq
    {
        DataTable GetCusType();
        DataTable GetSupplier();
       
        IEnumerable<SalesEnquiry> GetAllSalesEnq();
        string SalesEnqCRUD(SalesEnquiry cy);
        DataTable GetSalesEnquiry(string id);
        DataTable GetCustomerDetails(string id);
        DataTable GetItemDetails(string id);
        DataTable GetSalesEnquiryItem(string id);
        string EnquirytoQuote(string id);
        DataTable GetEnqByName(string id);
        DataTable GetEnqItem(string id);
        DataTable GetEnqDetail(string id);
        DataTable GetFolowup(string id);
        string PurchaseFollowupCRUD(EnqFollowup pf);
        IEnumerable<SalesItem> GetAllSalesenquriyItem(string id);
    }
}
