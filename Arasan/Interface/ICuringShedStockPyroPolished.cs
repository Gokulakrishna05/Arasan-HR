
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface ICuringShedStockPyroPolished
    {
        DataTable GetAllCuringShedStockPyroPolished(string dtFrom, string dtTo);
    }
}