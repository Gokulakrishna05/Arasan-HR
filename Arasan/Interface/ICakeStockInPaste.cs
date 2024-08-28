
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Report
{
    public interface ICakeStockInPaste
    {
        DataTable GetAllCakeStockInPaste(string dtFrom, string dtTo, string Branch );
    }
}