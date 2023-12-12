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
        DataTable GetRetItemDetail(string id);
        DataTable GetReturnable(string id);
        DataTable GetReturnableItems(string id);

        DataTable GetAllReturn(string strStatus);

        string StatusChange(string tag, int id);

        string RemoveChange(string tag, int id);
    }
}
