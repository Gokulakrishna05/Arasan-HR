using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface ISalesEnq
    {
        DataTable GetSupplier();
        IEnumerable<SalesEnquiry> GetAllSalesEnq();
        string SalesEnqCRUD(SalesEnquiry cy);
        DataTable GetSalesEnquiry(string id);
    }
}
