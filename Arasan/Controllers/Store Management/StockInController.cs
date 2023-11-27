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
            dt = StackService.GetStockInItem(ItemID);
            if(dt.Rows.Count > 0)
            {
                S.Item = dt.Rows[0]["ITEMID"].ToString();
                S.ItemID = ItemID;
                S.Location= dt.Rows[0]["LOCATION_ID"].ToString();
                S.Locationname= dt.Rows[0]["LOCID"].ToString();
                S.Branch= dt.Rows[0]["BRANCH_ID"].ToString();
                S.Qty = dt.Rows[0]["QTY"].ToString() + " " + dt.Rows[0]["UNITID"].ToString();
                S.QtyS = dt.Rows[0]["QTY"].ToString();
            }
            DataTable dt2 = new DataTable();
            dt2= StackService.GetIndentItem(ItemID);
            List<IndentList> TData = new List<IndentList>();
            IndentList tda = new IndentList();
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new IndentList();
                    tda.IndentID= dt2.Rows[i]["PINDDETAILID"].ToString();
                    tda.IndentNo = dt2.Rows[i]["DOCID"].ToString();
                    tda.IndentDate = dt2.Rows[i]["DOCDATE"].ToString();
                    tda.ItemName = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ItemId= dt2.Rows[i]["ITEM_ID"].ToString();
                    tda.Quantity = dt2.Rows[i]["QTY"].ToString();
                    tda.qty= dt2.Rows[i]["QTY"].ToString();
                    tda.Unit= dt2.Rows[i]["UNITID"].ToString();
                    tda.LocationName = dt2.Rows[i]["LOCID"].ToString();
                    tda.LocationID= dt2.Rows[i]["DEPARTMENT"].ToString();
                    TData.Add(tda);
                }
            }
            S.Indentlist = TData;
            return View(S);
        }
        [HttpPost]
        public IActionResult IssueToIndent(StockIn Cy, string id)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                Cy.ID = id;
                string Strout = StackService.IssueToStockCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Stock Issued Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Stock Issued Successfully...!";
                    }
                    return RedirectToAction("ListStockIn");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Indent";
                    TempData["notice"] = Strout;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            return View(Cy);
        }

        public IActionResult ListStockIn()
        {
            //IEnumerable<StockIn> cmp = StackService.GetAllStock();
            return View();
        }
        public ActionResult MyListStockInGrid()
        {
            List<StockItems> Reg = new List<StockItems>();
            DataTable dtUsers = new DataTable();
            dtUsers = (DataTable)StackService.GetAllListStockItem();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //string Qc = string.Empty;
                //string GRNStatus = string.Empty;
                string Account = string.Empty;
                //string View = string.Empty;
                //string EditRow = string.Empty;
                //string DeleteRow = string.Empty;
                Account = "<a href=IssueToIndent?ItemID=" + dtUsers.Rows[i]["ITEM_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";

                //Account = "<a href=IssueToIndent?id=" + dtUsers.Rows[i]["INVENTORY_ITEM_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";
                ////Account = "<a href=IssueToIndent?id=" + dtUsers.Rows[i]["INVENTORY_ITEM_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";
              
              
                Reg.Add(new StockItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["INVENTORY_ITEM_ID"].ToString()),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    acc = Account,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
