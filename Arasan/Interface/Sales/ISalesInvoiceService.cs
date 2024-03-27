using Arasan.Models;
using System.Data;
using System.Collections.Generic;
namespace Arasan.Interface
{
    public interface ISalesInvoiceService
    {
        string DirectPurCRUD(SalesInvoice cy);
        DataTable GetAllSalesInvoice(string status);
        DataTable GetSalesInvoiceDeatils(string id);
        DataTable GetEditItemDetails(string id);
        string StatusChange(string tag, string id);
        //DataTable GetSalesInvoiceItemDetails(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetFGItem(string locid);
        DataTable GetDrumDetails(string Itemid, string locid);
        string GetDrumStock(string Itemid, string locid);
        DataTable GetSIDetails(string SIID);
        DataTable GetTerms();
        DataTable GetHsn(string id);
        DataTable GetGSTDetails(string id);
        DataTable GetTrefficDetails(string id);
        DataTable GetItemSpecDetails(string id);
        DataTable GetArea(string custid);
        DataTable GetAreaDetails(string id);
        DataTable GetTname();
        DataTable ViewDepot(string id);
        DataTable Depotdetail(string id);
        DataTable TermsDetail(string id);
        DataTable AreaDetail(string id);
        DataTable GetSIITEMDetails(string SIID);
        DataTable GetNarr(string id);
        DataTable GetAllListSalesInvoiceItems();
        DataTable GetJob();
        DataTable Getjobdetails(string jobid);
        DataTable GetSchedule(string jobid);
        DataTable GetDrumDetails(string id);
        Task<IEnumerable<ExinvBasicItem>> GetBasicItem(string id);
        Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id);
    }
}
