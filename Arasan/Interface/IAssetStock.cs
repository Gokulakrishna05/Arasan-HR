  
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IAssetStock
    {
        DataTable GetAllAssetStock(string dtFrom, string dtTo, string Branch, string Location);
        DataTable GetLocation(string BranchID);
    }
}