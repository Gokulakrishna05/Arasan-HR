using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ILedger
    {
        string LedgerCRUD(Ledger Cy);
        IEnumerable<Ledger> GetAllLedger();
        DataTable GetLedger(string id);
        DataTable GetAccType();
        string StatusChange(string tag, string id);

        //DataTable GetGroupDetails(string id);
        DataTable GetAccGroup(string id);
        DataTable GetGroupCodeDetails(string id);
        DataTable GetAllLedgers(string strStatus);
        DataTable GetAllListDayBookItem(string strfrom, string strTo);
        DataTable GetAllListDayBookItems(string id);
        //DataTable GetAllListDayBookItemDetails();
    }

}