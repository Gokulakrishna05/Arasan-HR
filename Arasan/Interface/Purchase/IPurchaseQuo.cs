using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IPurchaseQuo
    {
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        string PurQuotationCRUD(PurchaseQuo cy);
        IEnumerable<PurchaseQuo> GetAllPurQuotation();
        PurchaseQuo GetPurQuotationById(string id);
    }
}
