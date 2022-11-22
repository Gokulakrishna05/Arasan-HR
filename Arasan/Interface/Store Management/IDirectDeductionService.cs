using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface.Store_Management
{
    public interface IDirectDeductionService
    {

        string DirectDeductionCRUD(DirectDeduction by);
        IEnumerable<DirectDeduction> GetAllDirectDeduction();
        DirectDeduction GetDirectDeductionById(string id);
        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetItem();
        DataTable GetItemGrp();
        DataTable GetDirectDeductionDetails(string id);
    }
}
