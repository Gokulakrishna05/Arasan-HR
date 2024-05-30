using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface IDirectAddition
    {
        string DirectAdditionCRUD(DirectAddition by);
        IEnumerable<DirectAddition> GetAllDirectAddition(string st, string ed);
        //DirectAddition GetDirectAdditionById(string id);
        DataTable GetLocation();
        DataTable GetBranch();
        //DataTable GetItem(string value);
        DataTable GetDirectAdditionDetails(string id);
        DataTable GetDAItemDetails(string id);
        DataTable GetDirectAddition(string id);
        DataTable GetDirectAdditionItem(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        IEnumerable<DirectItem> GetAllDirectAdditionItem(string id);

        string StatusChange(string tag, String id);
        string StatusActChange(string tag, String id);
        DataTable GetAllListDirectAdditionItems(string strStatus);
        //DataTable GetItemSubGrp();
        DataTable GetItem(string id);
        DataTable BindProcess();
    }
}
