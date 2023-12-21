using Arasan.Interface;
using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Sales;
using Arasan.Services.Store_Management;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Sales
{
    public class SalesTargetController : Controller
    {
        ISalesTargetService SalesTargetService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public SalesTargetController(ISalesTargetService _SalesTargetService, IConfiguration _configuratio)
        {
            SalesTargetService = _SalesTargetService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SalesTarget(string id)
        {
            SalesTarget ca = new SalesTarget();
            ca.Brlst = BindBranch();
            //ca.Partylst = BindGParty();
            DataTable dtv = datatrans.GetSequence("Dbnot");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            //ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<SalesTargetItem> TData = new List<SalesTargetItem>();
            SalesTargetItem tda = new SalesTargetItem();
            if (id == null)
            {
                tda = new SalesTargetItem();

                tda.Itemlst = BindItemlst();
                tda.Partylst = BindGParty();
                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            else
            {


            }

            ca.Targetlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult SalesTarget(SalesTarget Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = SalesTargetService.SalesTargetCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesTarget Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesTarget Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesTarget");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesTarget";
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
        public IActionResult ListSalesTarget()
        {
            return View();
        }
        public IActionResult ViewSalesTarget(string id)
        {
            SalesTarget ca = new SalesTarget();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = SalesTargetService.GetSalesTarget(id)
;
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.FDay = dt.Rows[0]["FDAY"].ToString();
                ca.TDay = dt.Rows[0]["EDAY"].ToString();
                ca.FMonth = dt.Rows[0]["MON"].ToString();
                ca.FYear = dt.Rows[0]["FINYR"].ToString();
                ca.ID = id;
            }
            List<SalesTargetItem> TData = new List<SalesTargetItem>();
            SalesTargetItem tda = new SalesTargetItem();

            dtt = SalesTargetService.GetSalesTargetItem(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SalesTargetItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.PartyId = dtt.Rows[i]["PARTYNAME"].ToString();
                    tda.Quantity = dtt.Rows[i]["QTY"].ToString();
                    tda.rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["SAMOUNT"].ToString();
                    TData.Add(tda);
                }
            }

            ca.Targetlst = TData;
            return View(ca);
        }
        public ActionResult MySalesTargetGrid(string strStatus)
        {
            List<ListSalesTargetItem> Reg = new List<ListSalesTargetItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)SalesTargetService.GetAllListSalesTargetItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;

                string DeleteRow = string.Empty;

                View = "<a href=ViewSalesTarget?id=" + dtUsers.Rows[i]["SALFCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SALFCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListSalesTargetItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SALFCBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    mon = dtUsers.Rows[i]["MON"].ToString(),
                    view = View,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, String id)
        {

            string flag = SalesTargetService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesTarget");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesTarget");
            }
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = SalesTargetService.GetItem();
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
        public List<SelectListItem> BindGParty()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable stock = new DataTable();

                string unit = "";
                string price = "";

                dt = SalesTargetService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }
               

                var result = new { unit = unit, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON()
        {
            SalesTargetItem model = new SalesTargetItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public JsonResult GetItemPartyJSON()
        {
            SalesTargetItem model = new SalesTargetItem();
            model.Partylst = BindGParty();
            return Json(BindGParty());

        }
    }
}
