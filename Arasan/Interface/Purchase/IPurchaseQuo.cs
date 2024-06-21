using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IPurchaseQuo
    {
        //DataTable GetBranch();
        //DataTable GetSupplier();
        //DataTable GetCurency();
        //DataTable GetItem(string value);
        //DataTable GetItemGrp();
        DataTable GetItemCF(string ItemId, string unitid);

        //DataTable GetItemDetails(string ItemId);
        string PurQuotationCRUD(PurchaseQuo cy);
        //IEnumerable<PurchaseQuo> GetAllPurQuotation(string st, string ed);
        IEnumerable<QoItem> GetAllPurQuotationItem(string id);
        //PurchaseQuo GetPurQuotationById(string id);
        DataTable GetPurQuotationByName(string name);
        DataTable GetPurQuotationName(string name);
        DataTable GetPurQuoteItem(string name);
        DataTable GetPurQuoteDetails(string name);
        DataTable GetItem( );
        string QuotetoPO(PurchaseQuo QuoteId);
        //DataTable GetItemSubGrp();
        DataTable GetPurchaseQuo(string id);
        //DataTable GetItemSubGroup(string id);
        DataTable GetPurchaseQuoItemDetails(string id);
        DataTable GetPurchaseQuoDetails(string id);
        //DataTable GetFolowup(string enqid);
        DataTable GetFolowup(string enqid);
        string PurchaseFollowupCRUD(QuoFollowup pf);
        IEnumerable<QuoFollowup> GetAllPurchaseFollowup();

        string StatusChange(string tag, string id);
        string StatusActChange(string tag, string id);

        Task<IEnumerable<PQuoItemDetail>> GetPQuoItem(string id);

        IEnumerable <PQuoItemDetail> GetPQuoItemD(string id);
        DataTable GetAllPurchaseQuoItems(string strStatus);
    }
}
