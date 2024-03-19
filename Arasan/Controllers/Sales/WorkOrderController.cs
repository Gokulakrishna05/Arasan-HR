using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Arasan.Controllers.Sales
{
    public class WorkOrderController : Controller
    {
        IWorkOrderService WorkOrderService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;

        public WorkOrderController(IWorkOrderService _WorkOrderService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            WorkOrderService = _WorkOrderService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult WorkOrder(string id)
        {
            WorkOrder ca = new WorkOrder();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();
            
            ca.Qolst = BindQuotation();
            ca.JopDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Location = Request.Cookies["LocationId"];
            ca.Emp= Request.Cookies["UserId"];
            ca.Loc = BindLocation(ca.Emp);
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
                    tda.taxlst = BindTax("");
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
                  
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                   
                    ca.CusNo = dt.Rows[0]["CREFNO"].ToString();
                    ca.Customer = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.ID = id;
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.OrderType = dt.Rows[0]["ORDTYPE"].ToString();
                    ca.TransAmount = dt.Rows[0]["TRANSAMOUNT"].ToString();
                    ca.CreditLimit = dt.Rows[0]["CRLIMIT"].ToString();
                    ca.RateCode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                    //ca.Quo = dt.Rows[0]["QUOID"].ToString();
                    ca.JopId = dt.Rows[0]["DOCID"].ToString();
                    ca.Cusdate = dt.Rows[0]["CREFDATE"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ViewBag.Quo = id;
                }
                DataTable dtt1 = new DataTable();
                dtt1 = WorkOrderService.GetWorkOrderDetails(id);
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

                        TData.Add(tda);
                    }
                }

            }
            ca.Worklst = TData;
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
            //IEnumerable<WorkOrder> cmp = WorkOrderService.GetAllWorkOrder(status);
            return View();
        }
        public ActionResult MyListWorkOrderGrid(string strStatus)
        {
            List<WorkOrderItems> Reg = new List<WorkOrderItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)WorkOrderService.GetAllListWorkOrderItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Close = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string Drum = string.Empty;
                string recept = string.Empty;

                recept = "<a href=Print?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                Close = "<a href=/WorkOrderShortClose/WorkOrderShortClose?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='close' /></a>";
                if(dtUsers.Rows[i]["IS_ALLOCATE"].ToString()=="Y")
                {
                    EditRow = "";
                    Drum = "";
                    DeleteRow = "";
                }
                else
                {
                    EditRow = "<a href=WorkOrder?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    Drum = "<a href=/WorkOrder/WDrumAllocation?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + "><img src='../Images/checklist.png' alt='Allocate' /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                }




                Reg.Add(new WorkOrderItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["JOBASICID"].ToString()),
                    //branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    enqno = dtUsers.Rows[i]["DOCID"].ToString(),
                    customer = dtUsers.Rows[i]["PARTY"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    clo = Close,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    drum = Drum,
                    recept = recept,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = WorkOrderService.StatusDeleteMR(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWorkOrder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWorkOrder");
            }
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
        public List<SelectListItem> BindTax(string id)
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetTax(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TARIFFID"].ToString(), Value = dtDesg.Rows[i]["TARIFFID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["QUOTE_NO"].ToString(), Value = dtDesg.Rows[i]["SALESQUOTEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation(string id)
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetLocation(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["loc"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetQuoDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string customer = "";
        //        string currency = "";
        //        if (ItemId != "edit")
        //        {
        //            dt = WorkOrderService.GetQuoDetails(ItemId);

        //            if (dt.Rows.Count > 0)
        //            {
        //                customer = dt.Rows[0]["PARTY"].ToString();
        //                currency = dt.Rows[0]["MAINCURR"].ToString();

        //            }
        //        }
                

        //        var result = new { customer = customer , currency = currency };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult WDrumAllocation(string id)
        {
            WDrumAllocation ca = new WDrumAllocation();
            DataTable dt = new DataTable();
            dt= WorkOrderService.GetWorkOrderByID(id);
            if(dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location= dt.Rows[0]["LOCID"].ToString();
                ca.JobId= dt.Rows[0]["DOCID"].ToString();
                ca.JobDate= dt.Rows[0]["DOCDATE"].ToString();
                ca.Customername= dt.Rows[0]["PARTY"].ToString();
                ca.CustomerId = dt.Rows[0]["CUSTOMERID"].ToString();
                ca.Locid = dt.Rows[0]["LOCMASTERID"].ToString();
                ca.JOId = dt.Rows[0]["JOBASICID"].ToString();
            }
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("er");
            if (dtv.Rows.Count > 0)
            {
                ca.DOCId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<WorkItem> TData = new List<WorkItem>();
            WorkItem tda = new WorkItem();
            DataTable dtt = new DataTable();
            dtt= WorkOrderService.GetWorkOrderDetails(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new WorkItem();
                    tda.itemid = dtt.Rows[i]["item"].ToString();
                    tda.items= dtt.Rows[i]["ITEMID"].ToString();
                    tda.orderqty= dtt.Rows[i]["QTY"].ToString();
                    tda.Jodetailid = dtt.Rows[i]["JODETAILID"].ToString();
                    List<Drumdetails> tlstdrum = new List<Drumdetails>();
                    Drumdetails tdrum = new Drumdetails();
                    DataTable dt3 = new DataTable();
                    dt3 = WorkOrderService.GetDrumDetails(tda.itemid, ca.Locid);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt3.Rows.Count; j++)
                        {
                            tdrum = new Drumdetails();
                            tdrum.lotno = dt3.Rows[j]["LOTNO"].ToString();
                            tdrum.drumno = dt3.Rows[j]["DRUMNO"].ToString();
                            tdrum.qty = dt3.Rows[j]["QTY"].ToString();
                            tdrum.rate = dt3.Rows[j]["RATE"].ToString();
                            tdrum.invid = dt3.Rows[j]["plstockvalueid"].ToString();
                            tlstdrum.Add(tdrum);
                        }
                    }
                    tda.drumlst = tlstdrum;
                    TData.Add(tda);
                }
            }
            ca.Worklst = TData;
            return View(ca);
        }
      

        //public JsonResult WDrumallocat(string json)
        //{
        //    var model = JsonConvert.DeserializeObject(json); 
        //    return null;
        //}

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]

        public ActionResult WDrumAllocation(WDrumAllocation cy, string id)
        {

            try
            {
                    cy.ID = id;
                    string Strout = WorkOrderService.DrumAllocationCRUD(cy);
                    if (string.IsNullOrEmpty(Strout))
                    {
                        if (cy.ID == null)
                        {
                            TempData["notice"] = "DrumAllocation Inserted Successfully...!";
                        }
                        else
                        {
                            TempData["notice"] = "DrumAllocation Updated Successfully...!";
                        }
                        return RedirectToAction("ListWDrumAllo");
                    }

                    else
                    {
                        ViewBag.PageTitle = "Edit WDrumAllocation";
                        TempData["notice"] = Strout;
                        //return View();
                    }

                    // }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public ActionResult MyListWDrumAllocationGrid()
        {
            List<ListWDrumAllocationItems> Reg = new List<ListWDrumAllocationItems>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)WorkOrderService.GetAllListWDrumAllocationItems();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Drum = string.Empty;

                Drum = "<a href=/WorkOrder/WDrumAllocation?id=" + dtUsers.Rows[i]["JOBASICID"].ToString() + "><img src='../Images/checklist.png' alt='Allocate' /></a>";


                Reg.Add(new ListWDrumAllocationItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["JOBASICID"].ToString()),
                    //branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    jopjd = dtUsers.Rows[i]["DOCID"].ToString(),
                    jopdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    customer = dtUsers.Rows[i]["PARTY"].ToString(),
                    drum = Drum,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ListWDrumAllocation()
        {
            //IEnumerable<WorkOrder> cmp = WorkOrderService.GetAllWorkOrder(status);
            return View();
        }
        public ActionResult MyListWDrumAlloGrid()
        {
            List<ListWDrumAlloItems> Reg = new List<ListWDrumAlloItems>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)WorkOrderService.GetAllListWDrumAlloItems();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string View = string.Empty;
                string deactive = string.Empty;

                View = "<a href=/WorkOrder/ViewDrumAllocation?id=" + dtUsers.Rows[i]["JODRUMALLOCATIONBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe' ><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                deactive = "<a href=StockRelease?id=" + dtUsers.Rows[i]["JODRUMALLOCATIONBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";


                Reg.Add(new ListWDrumAlloItems
                {
                    
                    id = dtUsers.Rows[i]["JODRUMALLOCATIONBASICID"].ToString(),
                    jobid = dtUsers.Rows[i]["jobid"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    customername = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    view = View,
                    deactive = deactive,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult StockRelease(string id)
        {
            DataTable lot = datatrans.GetData("SELECT PLSTOCKID FROM JODRUMALLOCATIONDETAIL WHERE JODRUMALLOCATIONBASICID='" + id +"'");
            string joid = datatrans.GetDataString("SELECT JOPID FROM JODRUMALLOCATIONBASIC WHERE JODRUMALLOCATIONBASICID='" + id + "'");
            string flag = "";
            if (lot.Rows.Count > 0)
            {
                for(int i=0;i<lot.Rows.Count;i++)
                {
                    string lotno = lot.Rows[i]["PLSTOCKID"].ToString();
                    flag = WorkOrderService.StatusStockRelease(lotno,joid, id);
                }
            }
            
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWDrumAllo");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWDrumAllo");
            }
        }
        public IActionResult ListWDrumAllo()
        {
            //IEnumerable<WDrumAllocation> cmp = WorkOrderService.GetAllWDrumAll();
            return View();
        }

        public ActionResult ViewDrumAllocation(string id)
        {
            WDrumAllocation ca = new WDrumAllocation();
            DataTable dt = new DataTable();
            dt = WorkOrderService.GetDrumAllByID(id);
            if (dt.Rows.Count > 0)
            {
                //ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.JobId = dt.Rows[0]["jobid"].ToString();
                //ca.JobDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Customername = dt.Rows[0]["PARTYNAME"].ToString();


                //ca.JOId = dt.Rows[0]["JOBASICID"].ToString();
                ca.DOCId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
            }

            List<WorkItem> TData = new List<WorkItem>();
            WorkItem tda = new WorkItem();
            DataTable dtt = new DataTable();
            dtt = WorkOrderService.GetDrumAllDetails(id);
            if (dtt.Rows.Count > 0)
            {
               
                  tda = new WorkItem();

                   tda.items = dtt.Rows[0]["ITEMID"].ToString();
                   //tda.orderqty = dtt.Rows[i]["QTY"].ToString();

            List<Drumdetails> tlstdrum = new List<Drumdetails>();
            Drumdetails tdrum = new Drumdetails();
            DataTable dt3 = new DataTable();
            dt3 = WorkOrderService.GetAllocationDrumDetails(id);
            if (dt3.Rows.Count > 0)
            {
                for (int j = 0; j < dt3.Rows.Count; j++)
                {
                    tdrum = new Drumdetails();
                    tdrum.lotno = dt3.Rows[j]["LOTNO"].ToString();
                    tdrum.drumno = dt3.Rows[j]["DRUMNO"].ToString();
                    tdrum.qty = dt3.Rows[j]["QTY"].ToString();
                    tdrum.rate = dt3.Rows[j]["RATE"].ToString();

                    tlstdrum.Add(tdrum);
                }
            }
            tda.drumlst = tlstdrum;
            TData.Add(tda);

        }
            ca.Worklst = TData;
            return View(ca);
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
                        tda.discount ="0";
                        tda.itemspec = "STD";
                        tda.freight = "0";
                        tda.freightamt ="0";
                        tda.disqty =  "0";
                        tda.packind = "0";
                        tda.matsupply = "OWN";
                       
                        tda.introdis = "0";
                        tda.cashdis = "0";
                        tda.spldis = "0";
                        tda.taxtype = "0'";
                        tda.tradedis ="0";
                        tda.qtydis = "0";
                        tda.additiondis = "0";
                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }
            }
           
            
            model.Worklst = Data;
            return Json(model.Worklst);

        }
        public async Task<IActionResult> Print(string id)
        {

            string mimtype = "";
            int extension = 1;
            string DrumID = datatrans.GetDataString("Select PARTYID from POBASIC where POBASICID='" + id + "' ");

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\JobOrder.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
          var order = await WorkOrderService.GetOrderItem(id);
          var orderDetail = await WorkOrderService.GetOrderItemDetail(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("workorderbasic", order);
            localReport.AddDataSource("JobDetail", orderDetail);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");
           
        }

        public IActionResult DirectWorkOrder(string id)
        {
            WorkOrder ca = new WorkOrder();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Curlst = BindCurrency();

            ca.Qolst = BindQuotation();
            ca.partylst = Bindparty();
            ca.JopDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Location = Request.Cookies["LocationId"];
            ca.Emp = Request.Cookies["UserId"];
            ca.Loc = BindLocation(ca.Emp);
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
                    tda.taxlst = BindTax("");
                    tda.itemlst = BindItem("");
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

                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();

                    ca.CusNo = dt.Rows[0]["CREFNO"].ToString();
                    ca.Customer = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.ID = id;
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.OrderType = dt.Rows[0]["ORDTYPE"].ToString();
                    ca.TransAmount = dt.Rows[0]["TRANSAMOUNT"].ToString();
                    ca.CreditLimit = dt.Rows[0]["CRLIMIT"].ToString();
                    ca.RateCode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                    //ca.Quo = dt.Rows[0]["QUOID"].ToString();
                    ca.JopId = dt.Rows[0]["DOCID"].ToString();
                    ca.Cusdate = dt.Rows[0]["CREFDATE"].ToString();
                    ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    ViewBag.Quo = id;
                }
                DataTable dtt1 = new DataTable();
                dtt1 = WorkOrderService.GetWorkOrderDetails(id);
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

                        TData.Add(tda);
                    }
                }

            }
            ca.Worklst = TData;
            return View(ca);
        }
        public List<SelectListItem> Bindparty()
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItem(string id)
        {
            try
            {
                DataTable dtDesg = WorkOrderService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindItem(loc));

        }
        public JsonResult GetTaxJSON(string ItemId)
        {
            string hsnid = "";

            string hsn = "";
            string sgstp = "";
            string cgstp = "";
            string igstp = "";
            double cgsta = 0;
            double sgsta = 0;
            double igsta = 0;
            double pers = 0;
            string gst = "";
             hsn = datatrans.GetDataString("select HSN from ITEMMASTER WHERE ITEMMASTERID = '" + ItemId + "'");

            hsnid = datatrans.GetDataString("select HSNCODEID from HSNCODE WHERE HSNCODE='" + hsn + "'");
 
            return Json(BindTax(hsnid));

        }
        public ActionResult GetItemDetail(string ItemId, string loc,string party)
        {
            try
            {
                string unit = "";
                string desc = "";
                
                DataTable d1 = datatrans.GetItemDetails(ItemId);
                if (d1.Rows.Count > 0)
                {
                    unit = d1.Rows[0]["UNITID"].ToString();
                    desc = d1.Rows[0]["ITEMDESC"].ToString();
                }

                string rate = datatrans.GetDataString("SELECT ROUND(AVG(RATE),2) as rate FROM PLSTOCKVALUE WHERE LOCID ='"+loc+"' AND ITEMID ='"+ ItemId + "'");
                string oldrate = datatrans.GetDataString("SELECT ED.RATE FROM EXINVBASIC E,EXINVDETAIL ED WHERE E.PARTYID ='" + party + "' AND ED.ITEMID ='"+ ItemId + "' ORDER BY DOCDATE DESC fetch  first 1 rows only");
                //if (plstock.Rows.Count > 0)
                //{
                //    rate = plstock.Rows[0]["rate"].ToString();
                     
                //}
                var result = new { unit = unit, desc= desc , rate = rate, oldrate= oldrate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
