﻿using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IPurchaseImportQuo
    {
        //DataTable GetBranch();
        //DataTable GetSupplier();
        //DataTable GetCurency();
        //DataTable GetItem(string value);
        //DataTable GetItemGrp();
        DataTable GetItemCF(string ItemId, string unitid);

        //DataTable GetItemDetails(string ItemId);
        string PurQuotationCRUD(PurchaseImportQuo cy);
        //IEnumerable<PurchaseQuo> GetAllPurQuotation(string st, string ed);
        IEnumerable<ImportQoItem> GetAllPurQuotationItem(string id);
        //PurchaseQuo GetPurQuotationById(string id);
        DataTable GetPurQuotationByName(string name);
        DataTable GetPurQuotationName(string name);
        DataTable GetPurQuoteItem(string name);
        DataTable GetPurQuoteDetails(string name);
        DataTable GetItem();
        string QuotetoPO(string QuoteId);
        //DataTable GetItemSubGrp();
        DataTable GetPurchaseQuo(string id);
        //DataTable GetItemSubGroup(string id);
        DataTable GetPurchaseQuoItemDetails(string id);
        DataTable GetPurchaseQuoDetails(string id);
        //DataTable GetFolowup(string enqid);
        DataTable GetFolowup(string enqid);
        string PurchaseFollowupCRUD(QuoImportFollowup pf);
        IEnumerable<QuoImportFollowup> GetAllPurchaseFollowup();

        string StatusChange(string tag, string id);

        Task<IEnumerable<PQuoItemDetail>> GetPQuoItem(string id);
        DataTable GetAllPurchaseQuoItems(string strStatus);
    }
}