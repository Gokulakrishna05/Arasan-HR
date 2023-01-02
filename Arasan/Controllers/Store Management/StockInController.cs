using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;


namespace Arasan.Controllers
{
    public class StockInController : Controller
    {
        IStockIn StackService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        public StockInController(IStockIn _StackService, IConfiguration _configuratio)
        {
            StackService = _StackService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
           

        }
        public IActionResult IssueToIndent(string ItemID)
        {
            StockIn S = new StockIn();
            DataTable dt = new DataTable(); 

            return View(S);
        }
        public IActionResult ListStockIn()
        {

            IEnumerable<StockIn> cmp = StackService.GetAllStock();
            return View(cmp);
        }
    }
}
