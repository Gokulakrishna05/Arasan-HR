using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IEnquiryQuotation
    {
        string EnquiryQuotationCRUD(EnquiryQuotation cy);
        DataTable GetCondition();
        DataTable GetSupplier();
        DataTable Gettemplete();
        DataTable GetExRateDetails(string id);
        DataTable GetCustomerDetails(string id);
        DataTable GetItem();
        DataTable GetItemDetails(string id);
        DataTable GetAllListEnquiryQuotation(string strStatus);
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        IEnumerable<QuotationItem> GetAllEnquiryQuotationItem(string id);
        DataTable GetEnquiryQuotation(string id);
        DataTable GetParty(string id);
        DataTable GetEnquiryQuotationItem(string id);
        DataTable GetEnquiryQuotationView(string id);
        DataTable GetPartyName(string id);
        DataTable GetEnquiryItem(string id);
    }
}
