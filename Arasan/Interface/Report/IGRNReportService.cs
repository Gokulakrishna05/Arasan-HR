using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IGRNReportService
    {
        DataTable GetAllReport(string dtFrom, string dtTo, string Branch, string Item, string Customer);
        DataTable GetBranch();
        DataTable GetItem(string id);
        //long GetMregion(string? v, string id);
    }
}
