using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Sales

{
    public interface IDebitNoteBillService
    {
        IEnumerable<DebitNoteBill> GetAllDebitNoteBill();
        string DebitNoteBillCRUD(DebitNoteBill cy);
        //DataTable GetParty();
        DataTable GetGrn(string id);
        DataTable GetItemDetails(string itemId, string grnid);
        //DataTable GetInvoDates(string itemId);

        DataTable EditProEntry(string PROID);
        DataTable GetPartyLedger(string PROID);
        DataTable GetDebitNoteBillDetail(string id);
        DataTable GetDebitNoteBillItem(string id);
        DataTable GetPurRet(string id);
        DataTable GetPurRetDetail(string id);
        DataTable GetPurRetDoc(string id);
        string CreditNoteStock(DebitNoteBill cy);
        DataTable GetItem(string id);
        DataTable GetLedger(string id);
        DataTable GetVocher();
        DataTable GetAccGrp();
        //DataTable GetItemDetails(string itemId);
    }
}
