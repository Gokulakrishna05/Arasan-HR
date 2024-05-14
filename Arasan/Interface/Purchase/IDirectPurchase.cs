using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IDirectPurchase
    {
        //DataTable GetBranch();
        //DataTable GetSupplier();
        //DataTable GetCurency();
        //DataTable GetLocation();
        //DataTable GetItem(string value);
       //DataTable GetItemSubGrp();
       // DataTable GetItemSubGroup(string id);

        //DataTable GetItemGrp();
        //DataTable GetItemSubGrp();
        //DataTable GetItemSubGroup(string id);
        //DataTable GetItemDetails(string ItemId);
        DataTable GetItemCF(string ItemId, string unitid);
        string DirectPurCRUD(DirectPurchase cy);
       
      
        IEnumerable<DirectPurchase> GetAllDirectPur(string status);
        DataTable GetDirectPurchase(string id);
        DataTable GetDirectPurchaseItemDetails(string id);
        IEnumerable<DirItem> GetAllDirectPurItem(string id);

        string StatusChange(string tag, string id);
        DataTable GetVocher();
        DataTable GetAllDirectPurchases(string id);
        string DirectPurchasetoGRN(string id);
        DataTable GetDirectPurchaseGrn(string id);
        DataTable GetDirectPurchaseItemGRN(string id);
        DataTable GetHsn(string id);
        DataTable GethsnDetails(string id);
        DataTable GetgstDetails(string id);
        DataTable GetTariff(string id);


        //Task<IEnumerable<DpItemDetail>> GetdpItem(string id, string s);
        //Task<IEnumerable<DpDetItemDetail>> GetdpdetItem(string id, string s);
        //DataTable GetItem();
    }
}
