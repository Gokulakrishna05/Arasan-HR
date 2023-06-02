using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface
{
    public interface IBranchService


    {
        string BranchCRUD(Branch cy);
        IEnumerable<Branch> GetAllBranch();
        DataTable GetCompany();
        DataTable Getcountry();
        DataTable GetEditBranch(string id);
        DataTable GetState(string id);
        DataTable GetBranch(string id);
        string StatusChange(string tag, int id);

    }
}


