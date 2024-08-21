//namespace Arasan.Interface
//{
//    public interface ICuringShedStockPyro
//    {
//    }
//}
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface ICuringShedStockPyro
    {
        DataTable GetAllCuringShedStockPyro(string dtFrom, string stTo);
    }
}
