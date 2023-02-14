using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Sales
{
    public interface ISalesQuotationService
    {
        IEnumerable<SalesQuotation> GetAllSalesQuotation();
        DataTable GetBranch();
        string SalesQuotationCRUD(SalesQuotation cy);
    }
}
