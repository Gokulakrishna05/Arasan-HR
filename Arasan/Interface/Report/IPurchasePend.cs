﻿using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IPurchasePend
    {

        DataTable GetAllReport(string Branch, string Sdate, string Edate);
    }
}
