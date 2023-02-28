using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Sales
{
    public class WorkOrderController : Controller
    {
        IWorkOrderService WorkOrderService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public WorkOrderController(IWorkOrderService _WorkOrderService, IConfiguration _configuratio)
        {
            WorkOrderService = _WorkOrderService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WorkOrder(string id)
        {
            WorkOrder ca = new WorkOrder();
            ca.Brlst = BindBranch();
            ca.Curlst = BindCurrency();
            ca.Loc = BindLocation();
            if (id == null)
            {

            }
            else
            {
                //ca = LocationService.GetLocationsById(id);

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult WorkOrder(WorkOrder Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkOrderService.WorkOrderCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "WorkOrder Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "WorkOrder Updated Successfully...!";
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
        public IActionResult ListWorkOrder()
        {
            //IEnumerable<WorkOrder> cmp = WorkOrderService.GetAllWorkOrder();
            return View();
        }
        public List<SelectListItem> BindCurrency()
            {
                try
                {
                    DataTable dtDesg = datatrans.GetCurency();
                    List<SelectListItem> lstdesg = new List<SelectListItem>();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Cur"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                    }
                    return lstdesg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
