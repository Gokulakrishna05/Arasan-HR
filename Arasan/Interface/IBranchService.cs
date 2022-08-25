using Arasan.Models;
using System.Collections.Generic;
using System.Collections;

namespace Arasan.Interface
{
    public interface IBranchService
    {
        IEnumerable<Branch> GetAllBranch();
        void AddBranch(Branch _branch);

        //void EditStudent(Branch _branch);
        //void DeleteStudent(Branch _branch);
    }
}
