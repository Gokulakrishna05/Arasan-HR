//namespace Arasan.Interface
//{
//    public interface IBalanceSheet
//    {
//    }
//}
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface
{
    public interface IBalanceSheet
    {
        DataTable GetAllBalanceSheet(string dtFrom, string Branch);
        DataTable GetAllBalanceSheet1(string dtFrom, string Branchs);
    }
}
