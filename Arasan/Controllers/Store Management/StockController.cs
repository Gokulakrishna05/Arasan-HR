using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class StockController : Controller
    {
        IStockService StockService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public StockController(IStockService _StockService, IConfiguration _configuratio)
        {
            StockService = _StockService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult IndentStockIssue(string id)
        {
            return View();
        }
        public ActionResult ListIndentStockIssue()
        {
           List<Stock> EnqChkItem = new List<Stock>();
            DataTable dtEnq = new DataTable();
            dtEnq = StockService.GetIndentDeatils();
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                string Approval = string.Empty;
                int stk = datatrans.GetDataId("select SUM(BALANCE_QTY) AS qty from INVENTORY_ITEM WHERE ITEM_ID='"+ dtEnq.Rows[i]["ITEMMASTERID"].ToString() + "' AND LOCATION_ID='10001000000827' AND BRANCH_ID='" + dtEnq.Rows[i]["BRANCH_ID"].ToString() + "'");
                EnqChkItem.Add(new Stock
                {
                    id = Convert.ToInt64(dtEnq.Rows[i]["PINDDETAILID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = Convert.ToDouble(dtEnq.Rows[i]["QTY"].ToString()),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    branchname = dtEnq.Rows[i]["BRANCHID"].ToString(),
                    indentno = dtEnq.Rows[i]["DOCID"].ToString(),
                    indentdate = dtEnq.Rows[i]["DOCDATE"].ToString(),
                    stockavailable = stk
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
    }
}
