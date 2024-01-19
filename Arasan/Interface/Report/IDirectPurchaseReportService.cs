using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IDirectPurchaseReportService
    {
        DataTable GetAllDPReport(string dtFrom, string dtTo, string Branch, string Item, string Customer);
        DataTable GetItem(string id);
    }
}
