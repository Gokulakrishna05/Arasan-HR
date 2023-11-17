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

        string StatusChange(string tag, int id);
       

        //DataTable GetGroupDetails(string id);
        DataTable GetAccGroup(string id);
        DataTable GetGroupCodeDetails(string id);
        DataTable GetAllLedgers();
    }

}