using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IBranchService


    {
        DataTable GetCompany();
        DataTable Getcountry();
        DataTable GetState();
    }
}


