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

        public IActionResult Storestock(string id)
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
                EnqChkItem.Add(new Stock
                {
                    //id = Convert.ToInt64(dtEnq.Rows[i]["PINDDETAILID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                     
                    quantity = Convert.ToDouble(dtEnq.Rows[i]["stk"].ToString()),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                     
        
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public ActionResult ListIndentStockIssues()
        {
            List<StockGrid> EnqChkItem = new List<StockGrid>();
            DataTable dtEnq = new DataTable();
            //DataTable dt2 = new DataTable();
            dtEnq = StockService.GetStockDeatils();
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {

                string Approval = string.Empty;
                EnqChkItem.Add(new StockGrid
                {
                    //id = Convert.ToInt64(dtEnq.Rows[i]["PINDDETAILID"].ToString()),
                    itemname = dtEnq.Rows[i]["ITEMID"].ToString(),
                    binid = dtEnq.Rows[i]["BINID"].ToString(),
                    unit = dtEnq.Rows[i]["UNITID"].ToString(),
                    quantity = Convert.ToDouble(dtEnq.Rows[i]["QTY"].ToString()),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                    branchname = dtEnq.Rows[i]["BRANCHID"].ToString(),

                    
            });

            }

            return Json(new
            {
                EnqChkItem
            });
        }

        public IActionResult Assetstock(string id)
        {
            return View();
        }
        public ActionResult ListAssetstock()
        {
            List<Asset> EnqChkItem = new List<Asset>();
            DataTable dtEnq = new DataTable();
            DataTable dtEnq1 = new DataTable();
            dtEnq = StockService.GetAssetDeatils();
            
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                 
                string p = datatrans.GetDataString("Select SUM(QTY) from ASSTOCKVALUE where ITEMID='" + dtEnq.Rows[i]["item"].ToString() + "' AND LOCID='" + dtEnq.Rows[i]["loc"].ToString() + "' and PLUSORMINUS='p' ");
                string m = datatrans.GetDataString("Select SUM(QTY) from ASSTOCKVALUE where ITEMID='" + dtEnq.Rows[i]["item"].ToString() + "' AND LOCID='" + dtEnq.Rows[i]["loc"].ToString() + "' and PLUSORMINUS='m' ");
                if(p=="")
                {
                    p = "0";
                }
                if (m == "")
                {
                    m = "0";
                }
                double pm1 = Convert.ToDouble(p);
                double pm2 = Convert.ToDouble(m);
                double pm = pm1 - pm2;
                string Approval = string.Empty;
                if (pm > 0)
                {
                    EnqChkItem.Add(new Asset
                    {
                        //id = Convert.ToInt64(dtEnq.Rows[i]["PINDDETAILID"].ToString()),
                        itemname = dtEnq.Rows[i]["ITEMID"].ToString(),

                        quantity = pm,
                        loc = dtEnq.Rows[i]["LOCID"].ToString(),


                    });
                }
            }

            return Json(new
            {
                EnqChkItem
            });
        }
    }
}
