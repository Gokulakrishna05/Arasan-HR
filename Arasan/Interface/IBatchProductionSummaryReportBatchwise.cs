using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface IBatchProductionSummaryReportBatchwise
    {
        DataTable GetAllBatchProductionSummaryReportBatchwise(string dtFrom, string stTo);
    }
}
