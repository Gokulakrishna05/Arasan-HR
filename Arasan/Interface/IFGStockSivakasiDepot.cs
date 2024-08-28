using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface IFGStockSivakasiDepot
    {
        DataTable GetAllFGStockSivakasiDepot(string dtFrom);
    }
}
