using Arasan.Models;
using System.Data;
using System.Collections.Generic;

namespace Arasan.Interface.Sales
{
    public interface IProFormaInvoiceService
    {
        IEnumerable<ProFormaInvoice> GetAllProFormaInvoice(string status);
        DataTable GetBranch();
        DataTable GetJob();
        DataTable GetEditProFormaInvoice(string id);
        DataTable GetDrumParty(string id);
        DataTable GetDrumAll(string id);
      
        string ProFormaInvoiceCRUD(ProFormaInvoice cy);
        string StatusChange(string tag, int id);
        DataTable GetWorkOrderDetail(string id);
        //DataTable GetProFormaInvoiceDetails(string id);
        DataTable EditProFormaInvoiceDetails(string id);
    }
}
