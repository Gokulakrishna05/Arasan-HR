using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPurchaseReturn
    {
        DataTable GetPO();
        //DataTable GetItem();
        DataTable GetState();
        DataTable GetPODetails(string ItemId);
        string PurReturnCRUD(PurchaseReturn cy);
        DataTable GetPurchaseReturn(string id);
        DataTable GetPurchaseReturnDes(string id);
        IEnumerable<PurchaseReturn> GetAllPurReturn();
    }
}
