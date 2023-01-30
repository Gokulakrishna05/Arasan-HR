using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPurchaseReturn
    {
        DataTable GetGRN();
        DataTable GetItem(string id);
        DataTable GetState();
        DataTable GetGRNDetails(string ItemId);
        DataTable GetGRNBlDetails(string ItemId);
        DataTable GetSupplier(string ItemId);
        DataTable GetCurrency(string ItemId);
        string PurReturnCRUD(PurchaseReturn cy);
        DataTable GetPurchaseReturn(string id);
        DataTable GetPurchaseReturnDetail(string id);
        DataTable GetPurchaseReturnReason(string id);
        DataTable GetPurchaseReturnDes(string id);
        IEnumerable<PurchaseReturn> GetAllPurReturn();
        DataTable GetCity(string id);
        DataTable Getstkqty(string grnid,string locid,string brid);
    }
}
