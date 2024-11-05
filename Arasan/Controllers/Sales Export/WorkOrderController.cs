using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Sales_Export
{
    public class WorkOrderController : Controller
    {
        IWorkOrder WorkOrder;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public WorkOrderController(IWorkOrder _WorkOrder, IConfiguration _configuratio)
        {

            WorkOrder = _WorkOrder;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Work_Order(string id)
        {
            WorkOrder ca = new WorkOrder();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Loclst = BindWorkCenter();
            ca.Suplst = BindSupplier();
            ca.Reasonlst = BindReason();
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("vchsl");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<OrderItem> TData = new List<OrderItem>();
            OrderItem tda = new OrderItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new OrderItem();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                //double total = 0;
                dt = WorkOrder.GetWorkOrder(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                    ca.Reason = dt.Rows[0]["REASON"].ToString();
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = WorkOrder.GetWorkOrderItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new OrderItem();
                        tda.JobId = dt2.Rows[i]["JOBORDID"].ToString();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.OrdQty = dt2.Rows[i]["ORDQTY"].ToString();
                        tda.Dc = dt2.Rows[i]["DCQTY"].ToString();
                        tda.Excise = dt2.Rows[i]["EXCISEQTY"].ToString();
                        tda.PendQty = dt2.Rows[i]["PENDQTY"].ToString();
                        tda.ShortQty = dt2.Rows[i]["PRECLQTY"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        TData.Add(tda);
                    }
                }
            }
         
            ca.OrderLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult Work_Order(WorkOrder Cy, string id)
        {

            try
            {
                Cy.ID = id;
                Cy.Branch = Request.Cookies["BranchId"];
                string Strout = WorkOrder.WorkOrderCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Export WorkOrder Short Close Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Export WorkOrder Short Close Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkOrder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Work_Order";
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = WorkOrder.GetWorkCenter();
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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = WorkOrder.GetSupplier();
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
        public List<SelectListItem> BindReason()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "SAMPLE CLOSING", Value = "SAMPLECLOSING" });
                lstdesg.Add(new SelectListItem() { Text = "ORDER CLOSING", Value = "ORDERCLOSING" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = WorkOrder.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string price = "";
                dt = WorkOrder.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListWorkOrder()
        {
            return View();
        }
        public ActionResult MyListWorkOrderGrid(string strStatus)
        {
            List<ListWorkOrderItems> Reg = new List<ListWorkOrderItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)WorkOrder.GetAllListWorkOrder(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["EJOCLBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                    EditRow = "<a href=Work_Order?id=" + dtUsers.Rows[i]["EJOCLBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewWorkOrder?id=" + dtUsers.Rows[i]["EJOCLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EJOCLBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["EJOCLBASICID"].ToString() + "";
                }
                Reg.Add(new ListWorkOrderItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EJOCLBASICID"].ToString()),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    customer = dtUsers.Rows[i]["PARTYID"].ToString(),
                    sendmail = SendMail,
                    view = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewWorkOrder(string id)
        {
            WorkOrder ca = new WorkOrder();

            DataTable dt = new DataTable();
            //double total = 0;
            dt = WorkOrder.GetWorkOrder(id);
            if (dt.Rows.Count > 0)
            {
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                ca.Reason = dt.Rows[0]["REASON"].ToString();
                ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                ca.ID = id;


                List<OrderItem> TData = new List<OrderItem>();
                OrderItem tda = new OrderItem();

                DataTable dt2 = new DataTable();
                dt2 = WorkOrder.GetWorkOrderItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new OrderItem();
                        tda.JobId = dt2.Rows[i]["JOBORDID"].ToString();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.OrdQty = dt2.Rows[i]["ORDQTY"].ToString();
                        tda.Dc = dt2.Rows[i]["DCQTY"].ToString();
                        tda.Excise = dt2.Rows[i]["EXCISEQTY"].ToString();
                        tda.PendQty = dt2.Rows[i]["PENDQTY"].ToString();
                        tda.ShortQty = dt2.Rows[i]["PRECLQTY"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        TData.Add(tda);
                    }
                }
                ca.OrderLst = TData;

            }

            return View(ca);
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = WorkOrder.StatusChange(tag, id);
            }
            else
            {
                flag = WorkOrder.ActStatusChange(tag, id);
            }

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
    }
}
