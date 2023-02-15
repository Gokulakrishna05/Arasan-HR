using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Sales
{
    public interface ISalesQuotationService
    {
        IEnumerable<SalesQuotation> GetAllSalesQuotation();
        DataTable GetBranch();
        DataTable Getcountry();
        DataTable GetSalesQuotation(string id);
        DataTable GetSalesQuotationItemDetails(string id);
        string SalesQuotationCRUD(SalesQuotation cy);

        DataTable GetItemCF(string ItemId, string unitid);
    }
}
