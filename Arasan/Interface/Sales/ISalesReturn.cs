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
    }
}
