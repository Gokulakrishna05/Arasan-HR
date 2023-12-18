using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IReceiptAgtRetDC
    {
        DataTable GetBranch();
        DataTable GetParty();
        DataTable Getbin();
        DataTable Getdocno();
        DataTable GetPartys(string id);
        DataTable GetdocnoS(string id);
        DataTable GetReceipt(string id);
        DataTable GetReceiptItem(string id);
        DataTable GetDCDetails(string id);
        DataTable Getdctype(string id);
        DataTable Getviewdctype(string id);
        DataTable GetItemDetail(string id);
        string ReceiptAgtRetDCCRUD(ReceiptAgtRetDC cy);

        DataTable GetAllReceipt(string strStatus);
        DataTable ViewGetReceipt(string id);
        DataTable ViewGetReceiptitem(string id);
        DataTable GetItemgrpDetail(string id);

        string StatusChange(string tag, int id);

        string RemoveChange(string tag, int id);
    }
}
