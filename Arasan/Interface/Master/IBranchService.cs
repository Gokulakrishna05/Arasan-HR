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
        IEnumerable<Branch> GetAllBranch(string status);
        DataTable GetCompany();
        //DataTable Getcountry();
        DataTable GetCity(string id);
        DataTable GetEditBranch(string id);
        string CityCRUD(string id);
        DataTable GetState();
        DataTable GetBranch(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllBranches(string strStatus);



    }
}


