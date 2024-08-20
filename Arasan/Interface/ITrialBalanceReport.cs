using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface ITrialBalanceReport
    {
        DataTable GetAllTrialBalanceReport(string dtFrom,string Branch,string Master);
         DataTable GetMaster(string id);
    }
}
