using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IStoreIssueConsumables
    {
        //DataTable GetBranch();
       // DataTable GetLocation();
        string StoreIssueCRUD(StoreIssueConsumables cy);
        IEnumerable<StoreIssueConsumables> GetAllStoreIssue(string st, string ed);
       // DataTable GetEmp();
        DataTable EditSICbyID(string Poid);
        //DataTable GetItem(string value);
        //DataTable GetItemSubGrp();
        // DataTable GetItemSubGroup(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetSICItemDetails(string id);
        IEnumerable<SICItem> GetAllStoreIssueItem(string id);
        DataTable Getstkqty(string ItemId, string loc, string branch);

        DataTable GetItem(string Poid);
        DataTable Getloc(string Poid);
        DataTable Getwork(string Poid);
        DataTable GetProcess(string Poid);
        string StatusChange(string tag, string id);
        DataTable GetListStoreIssueItems(string strStatus);
    }
}
