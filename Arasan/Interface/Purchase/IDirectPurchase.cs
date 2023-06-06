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
       
      
        IEnumerable<DirectPurchase> GetAllDirectPur();
        DataTable GetDirectPurchase(string id);
        DataTable GetDirectPurchaseItemDetails(string id);
        IEnumerable<DirItem> GetAllDirectPurItem(string id);

        string StatusChange(string tag, int id);
    }
}
