using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IExportInvoice
    {
        DataTable GetSupplier();
        DataTable GetSupplier2();
        DataTable GetWorkCenter();
        DataTable GetCustomerDetails(string id);
        DataTable GetExRateDetails(string id);
        DataTable GetItem();
    }
}
