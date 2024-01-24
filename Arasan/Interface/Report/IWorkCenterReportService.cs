using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IWorkCenterReportService
    {
        DataTable GetAllWorkCenterReport(string dtFrom, string dtTo, string WorkCenter, string Process);
        DataTable GetProcess();
        DataTable GetWorkCenter();
    }
}
