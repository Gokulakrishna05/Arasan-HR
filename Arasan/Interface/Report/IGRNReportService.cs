﻿using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IGRNReportService
    {
        DataTable GetAllReport(string Branch,string Customer,string Item,string dtFrom,string dtTo);
        DataTable GetBranch();
        DataTable GetItem(string id);
        //long GetMregion(string? v, string id);
    }
}