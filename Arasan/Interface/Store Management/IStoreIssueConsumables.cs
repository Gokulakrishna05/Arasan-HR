using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IStoreIssueConsumables
    {
        DataTable GetBranch();
        DataTable GetLocation();
        string StoreIssueCRUD(StoreIssueConsumables cy);
        IEnumerable<StoreIssueConsumables> GetAllStoreIssue();
        DataTable GetEmp();
        DataTable EditSICbyID(string Poid);
        DataTable GetItem(string value);
        //DataTable GetItemSubGrp();
        // DataTable GetItemSubGroup(string id);

        DataTable GetItemGrp();


    }
}
