using Arasan.Models;
using System.Data;
using System.Collections.Generic;
namespace Arasan.Interface
{
    public interface ISalesReturn
    {
        DataTable GetInvoice();
        DataTable GetInvoiceDetails(string invoiceid);
        DataTable GetInvoiceItem(string invoiceid);
        string SalesReturnCRUD(SalesReturn Cy);
        IEnumerable<SalesReturn> GetAllSalesReturn();
        DataTable GetSalesRetDetails(string invoiceid);
        DataTable GetSalesRet (string invoiceid);
        //DataTable GetSalesReturn(string invoiceid);
    }
}
