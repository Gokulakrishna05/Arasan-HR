using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ILedger
    {
        string  LedgerCRUD(Ledger Cy);
        IEnumerable<Ledger> GetAllLedger();
        DataTable GetLedger(string id);
        DataTable GetAccType();

        DataTable GetGroupDetails(string id);

    }

}