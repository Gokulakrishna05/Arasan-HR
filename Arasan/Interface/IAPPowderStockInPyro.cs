
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IAPPowderStockInPyro
    {
        DataTable GetAllAPPowderStockInPyro(string dtFrom, string dtTo);
    }
}