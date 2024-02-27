using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IBatchReportService
    {
        DataTable GetAllBatchReport(string dtFrom, string dtTo);
        DataTable GetAllSchReportItems(string dtFrom, string dtTo, string WorkCenter, string Process, string Pschno);
        DataTable GetProcess();
        DataTable GetWorkCenter();
        DataTable GetPschno();
    }
}
