using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class WorkOrderShortCloseController : Controller
    {
        IWorkOrderShortClose ShortClose;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public WorkOrderShortCloseController(IWorkOrderShortClose _ShortClose, IConfiguration _configuratio)
        {
            ShortClose = _ShortClose;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WorkOrderShortClose(string id)
        {
            WorkOrderShortClose ca = new WorkOrderShortClose();
            //ca.Branch = Request.Cookies["BranchId"];
            //ca.Curlst = BindCurrency();
            //ca.Loc = BindLocation();
            //ca.Qolst = BindQuotation();
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("OfcCl");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<WorkCloseItem> TData = new List<WorkCloseItem>();
            WorkCloseItem tda = new WorkCloseItem();
            DataTable dt = new DataTable();
            dt = ShortClose.GetWorkOrder(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Customer = dt.Rows[0]["PARTY"].ToString();
                ca.CustomerId = dt.Rows[0]["customerId"].ToString();
                ca.ID = id;
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.LocationId = dt.Rows[0]["loc"].ToString();
                ca.OrderType = dt.Rows[0]["ORDTYPE"].ToString();
                ca.JopId = dt.Rows[0]["DOCID"].ToString();
                
               
            }
            DataTable dtt1 = new DataTable();
            dtt1 = ShortClose.GetWorkOrderDetails(id);
            if (dtt1.Rows.Count > 0)
            {
                for (int i = 0; i < dtt1.Rows.Count; i++)
                {
                    tda = new WorkCloseItem();
                    
                    tda.ItemId = dtt1.Rows[i]["ITEMID"].ToString();
                    tda.unit = dtt1.Rows[i]["UNITID"].ToString();
                    tda.orderqty = dtt1.Rows[i]["QTY"].ToString();
                    tda.UnitId = dtt1.Rows[i]["units"].ToString();
                    tda.rate = dtt1.Rows[i]["RATE"].ToString();
                    //tda.PendQty = dtt1.Rows[i]["AMOUNT"].ToString();
                    //tda.clQty = dtt1.Rows[i]["DISCOUNT"].ToString();
                    tda.Isvalid = "Y";


                    TData.Add(tda);
                }
            }


            ca.Closelst = TData;
            return View(ca);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WorkOrderShortClose(WorkOrderShortClose Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ShortClose.WorkShortCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "WorkOrderShortClose Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "WorkOrderShortClose Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkOrder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkOrder";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
    }
}
