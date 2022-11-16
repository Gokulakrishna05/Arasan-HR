using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IPurchaseQuo
    {
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        DataTable GetItem(string value);
        DataTable GetItemGrp();
        DataTable GetItemDetails(string ItemId);
        string PurQuotationCRUD(PurchaseQuo cy);
        IEnumerable<PurchaseQuo> GetAllPurQuotation();
        PurchaseQuo GetPurQuotationById(string id);
        DataTable GetPurQuotationByName(string name);
        DataTable GetPurQuoteItem(string name);
        string QuotetoPO(string QuoteId);
    }
}
