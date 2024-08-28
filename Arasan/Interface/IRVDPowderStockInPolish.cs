
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface IRVDPowderStockInPolish
    {
        DataTable GetAllRVDPowderStockInPolish(string dtFrom, string dtTo);
    }
}