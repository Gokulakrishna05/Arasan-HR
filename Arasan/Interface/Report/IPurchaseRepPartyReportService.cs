using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IPurchaseRepPartyReportService
    {
        DataTable GetAllPurchasePartyReport(string dtFrom, string dtTo, string Branch, string Customer, string Item);

        DataTable GetItem(string id);
    }
}
