using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Sales

{
    public interface IDebitNoteBillService
    {
        //IEnumerable<DebitNoteBill> GetAllDebitNoteBill();
        string DebitNoteBillCRUD(DebitNoteBill cy);
        string DebitNoteBillDetCRUD(DebitNoteBill cy);

        string DebitNoteAcc(DebitNoteBill cy);
        //DataTable GetParty();
        DataTable GetGrn(string id);
        DataTable GetItemDetails(string itemId );
        //DataTable GetInvoDates(string itemId);
        DataTable AccconfigLst();
        DataTable EditProEntry(string PROID);
        DataTable GetPartyLedger(string PROID);
        DataTable GetDebitNoteBillDetail(string id);
        DataTable GetDebitNoteBillItem(string id);
        DataTable GetInvoiceDetails(string id);
        DataTable GetInvoiceDate(string id);
        DataTable GetPurRet(string id);
        DataTable GetPurRetDetail(string id);
        DataTable GetGrnRet(string id);
        DataTable GetGRNRetDetail(string id);
        DataTable GetPurRetDoc(string id);
        string CreditNoteStock(DebitNoteBill cy);
        DataTable GetItem(string id);
        DataTable GetLedger(string id);
        DataTable GetVocher();
        DataTable GetAccGrp();

        DataTable GetDNDetails(string id);
        //DataTable GetItemDetails(string itemId);

        DataTable GetAllDebitNoteBill(string strStatus);
    }
}
