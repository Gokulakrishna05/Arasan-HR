using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface.Store_Management
{
    public interface IDirectDeductionService
    {

        string DirectDeductionCRUD(DirectDeduction by);
        IEnumerable<DirectDeduction> GetAllDirectDeduction(string st, string ed);
        //DirectDeduction GetDirectDeductionById(string id);
        DataTable GetLocation();
        DataTable GetBranch();
        //DataTable GetItem(string value);
        ////DataTable GetItemGrp();
        DataTable GetDirectDeductionDetails(string id);
        DataTable GetDDByName(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetDDItem(string id);
        DataTable GetDDItemDetails(string id);
        IEnumerable<DeductionItem> GetAllStoreIssueItem(string id);

        string StatusChange(string tag, string id);
        DataTable GetAllListDirectDeductionItems(string strStatus);
    }

}
