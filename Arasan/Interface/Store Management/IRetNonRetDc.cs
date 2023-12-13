using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IRetNonRetDc
    {
        DataTable GetBranch();
        DataTable GetParty();
        string RetNonRetDcCRUD(RetNonRetDc cy);

        DataTable GetPartyDetails(string id);
        //DataTable GetSubGroup(string id);
        DataTable GetPartyitems(string id);
        DataTable GetRetItemDetail(string id);
        DataTable GetRetItem(string id);
        DataTable GetReturnable(string id);
        DataTable ViewGetReturnable(string id);
        DataTable GetReturnableItems(string id);
       // DataTable GetItemSubGroup(string id);
        DataTable GetViewReturnableItems(string id);
        DataTable GetAllReturn(string strStatus);

        string StatusChange(string tag, int id);

        string RemoveChange(string tag, int id);

       Task<IEnumerable<ReturnDetail>> GetReturns(string id);
    }
}
