using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
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
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();
            ca.Loc = BindLocation();
            ca.Qolst = BindQuotation();
            ca.JopDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("er");
            if (dtv.Rows.Count > 0)
            {
                ca.JopId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<WorkItem> TData = new List<WorkItem>();
            WorkItem tda = new WorkItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new WorkItem();
                    tda.taxlst = BindTax();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = WorkOrderService.GetWorkOrder(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.JopDate = dt.Rows[0]["DOCDATE"].ToString();
                  
                    ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                   
                    ca.CusNo = dt.Rows[0]["CREFNO"].ToString();
                    ca.Customer = dt.Rows[0]["PARTY"].ToString();
                    ca.ID = id;
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.OrderType = dt.Rows[0]["ORDTYPE"].ToString();
                    ca.TransAmount = dt.Rows[0]["TRANSAMOUNT"].ToString();
                    ca.CreditLimit = dt.Rows[0]["CRLIMIT"].ToString();
                    ca.RateCode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                    ca.Quo = dt.Rows[0]["QUOID"].ToString();
                    ca.JopId = dt.Rows[0]["DOCID"].ToString();
                    ca.Cusdate = dt.Rows[0]["CREFDATE"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ViewBag.Quo = id;
                }
              
            }
          //  ca.Worklst = TData;
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
            IEnumerable<WorkOrder> cmp = WorkOrderService.GetAllWorkOrder();
            return View(cmp);
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
        public List<SelectListItem> BindTax()
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetTax();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TAX"].ToString(), Value = dtDesg.Rows[i]["TAXMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindQuotation()
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetQuo();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["QUOTE_NO"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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
        public ActionResult GetQuoDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string customer = "";
                string currency = "";
                if (ItemId != "edit")
                {
                    dt = WorkOrderService.GetQuoDetails(ItemId);

                    if (dt.Rows.Count > 0)
                    {
                        customer = dt.Rows[0]["PARTY"].ToString();
                        currency = dt.Rows[0]["MAINCURR"].ToString();

                    }
                }
                

                var result = new { customer = customer , currency = currency };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSalesQuoDetails(string id,string jobid)
        {
            WorkOrder model = new WorkOrder();
            DataTable dtt = new DataTable();
            List<WorkItem> Data = new List<WorkItem>();
            WorkItem tda = new WorkItem();
            if (id == "edit")
            {
                DataTable dtt1 = new DataTable();
                dtt1 = WorkOrderService.GetWorkOrderDetails(jobid);
                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tda = new WorkItem();
                        tda.items = dtt1.Rows[i]["ITEMID"].ToString();
                        tda.itemid = dtt1.Rows[i]["item"].ToString();
                        tda.unit = dtt1.Rows[i]["UNITID"].ToString();
                        tda.orderqty = dtt1.Rows[i]["QTY"].ToString();

                        tda.rate = dtt1.Rows[i]["RATE"].ToString();
                        tda.amount = dtt1.Rows[i]["AMOUNT"].ToString();
                        tda.discount = dtt1.Rows[i]["DISCOUNT"].ToString();
                        tda.taxtype = dtt1.Rows[i]["TAXTYPE"].ToString();
                        tda.tradedis = dtt1.Rows[i]["TDISC"].ToString();
                        tda.qtydis = dtt1.Rows[i]["QDISC"].ToString();
                        tda.additiondis = dtt1.Rows[i]["ADISC"].ToString();

                        tda.itemspec = dtt1.Rows[i]["ITEMSPEC"].ToString();
                        tda.freight = dtt1.Rows[i]["FREIGHT"].ToString();
                        tda.freightamt = dtt1.Rows[i]["FREIGHTAMT"].ToString();
                        tda.disqty = dtt1.Rows[i]["DCQTY"].ToString();
                        tda.packind = dtt1.Rows[i]["PACKSPEC"].ToString();
                        tda.matsupply = dtt1.Rows[i]["MATSUPP"].ToString();
                        tda.Isvalid = "Y";
                        tda.introdis = dtt1.Rows[i]["IDISC"].ToString();
                        tda.cashdis = dtt1.Rows[i]["CDISC"].ToString();
                        tda.spldis = dtt1.Rows[i]["SDISC"].ToString();

                        Data.Add(tda);
                    }
                }
            }
            else
            {
                dtt = WorkOrderService.GetSatesQuoDetails(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new WorkItem();

                        tda.items = dtt.Rows[i]["ITEMID"].ToString();
                        tda.itemid = dtt.Rows[i]["itemi"].ToString();
                        tda.unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.orderqty = dtt.Rows[i]["QTY"].ToString();

                        tda.rate = dtt.Rows[i]["RATE"].ToString();
                        tda.amount = dtt.Rows[i]["TOTAMT"].ToString();
                        tda.discount = dtt.Rows[i]["DISCAMOUNT"].ToString();

                        
                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }
            }
           
            
            model.Worklst = Data;
            return Json(model.Worklst);

        }
    }
}
