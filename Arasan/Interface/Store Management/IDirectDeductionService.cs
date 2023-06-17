using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface.Store_Management
{
    public interface IDirectDeductionService
    {

        string DirectDeductionCRUD(DirectDeduction by);
        IEnumerable<DirectDeduction> GetAllDirectDeduction(string status);
        //DirectDeduction GetDirectDeductionById(string id);
        DataTable GetLocation();
        DataTable GetBranch();
        //DataTable GetItem(string value);
        ////DataTable GetItemGrp();
        DataTable GetDirectDeductionDetails(string id);
        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetDDItemDetails(string id);
        IEnumerable<DeductionItem> GetAllStoreIssueItem(string id);

        string StatusChange(string tag, int id);
    }

}
