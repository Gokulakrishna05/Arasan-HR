using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IStoreIssueProduction
    {
        DataTable GetBranch();
        DataTable GetLocation();
        string StoreIssueProCRUD(StoreIssueProduction cy);
        IEnumerable<StoreIssueProduction> GetAllStoreIssuePro();
        //DataTable GetEmp();
        DataTable EditSIPbyID(string Poid);
        DataTable GetItem(string value);
        //DataTable GetItemSubGrp();
        // DataTable GetItemSubGroup(string id);

        DataTable GetItemGrp();
    }
}
