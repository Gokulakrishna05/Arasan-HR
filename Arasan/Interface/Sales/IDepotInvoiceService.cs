using Arasan.Models;
using System.Data;
using System.Collections.Generic;

namespace Arasan.Interface.Sales
{
    public interface IDepotInvoiceService
    {
        string DirectPurCRUD(DepotInvoice cy);
        IEnumerable<DepotInvoice> GetAllDepotInvoice();
        DataTable GetDepotInvoiceDeatils(string id);
        DataTable GetEditItemDetails(string id);
        string StatusChange(string tag, int id);
        //DataTable GetDepotInvoiceItemDetails(string id);
        DataTable GetItemCF(string ItemId, string unitid);
    }
}
