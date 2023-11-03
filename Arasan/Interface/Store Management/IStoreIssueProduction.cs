using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IStoreIssueProduction
    {
        //DataTable GetBranch();
        //DataTable GetLocation();
        string StoreIssueProCRUD(StoreIssueProduction cy);
        IEnumerable<StoreIssueProduction> GetAllStoreIssuePro(string st,string ed);
        //DataTable GetEmp();
        DataTable EditSIPbyID(string Poid);
        DataTable GetItem(string Poid);
        DataTable Getloc(string Poid);
        DataTable Getwork(string Poid);
        DataTable GetProcess(string Poid);
        //DataTable GetItem(string value);
        //DataTable GetItemSubGrp();
        // DataTable GetItemSubGroup(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable Getstkqty(string ItemId, string loc,string branch);
        DataTable GetSICItemDetails(string id);
        IEnumerable<SIPItem> GetAllStoreIssueItem(string id);

        //DataTable GetItemDetails(string ItemId);
        //DataTable GetItemGrp();
    }
}
