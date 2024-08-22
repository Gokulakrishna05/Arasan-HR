using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IFinishedGoodsStockDetailsDrumwise
    {
        DataTable GetAllFinishedGoodsStockDetailsDrumwise(string dtFrom,string Location);
        DataTable GetLocation(string BranchID);
    }
}