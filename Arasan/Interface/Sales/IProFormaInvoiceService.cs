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
        DataTable GetDrumDetails(string id);
        DataTable GetDrumAll(string id);
      
        string ProFormaInvoiceCRUD(ProFormaInvoice cy);
        string StatusChange(string tag, string id);
        DataTable GetWorkOrderDetail(string id);
        DataTable GetProFormaInvoiceDetails(string id);
        DataTable GetgstDetails(string id);
        DataTable EditProFormaInvoiceDetails(string id);
        DataTable GetArea(string custid);
        DataTable GetTerms();
        DataTable GetAllListProFormaInvoiceItems(string strStatus);

        Task<IEnumerable<PinvBasicItem>> GetBasicItem(string id);
        Task<IEnumerable<PinvDetailitem>> GetPinvItemDetail(string id);
        Task<IEnumerable<PinvTermsitem>> GetPinvtermsDetail(string id);
    }
}
