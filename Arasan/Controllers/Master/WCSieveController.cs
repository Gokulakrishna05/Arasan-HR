using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Arasan.Services.Sales;
using Arasan.Services.Store_Management;
//using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class WCSieveController : Controller
    {
        IWCSieveService WCSieveService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public WCSieveController(IWCSieveService _WCSieveService, IConfiguration _configuratio)
        {
            WCSieveService = _WCSieveService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WCSieve(string id)
        {
            WCSieve ca = new WCSieve();
            ca.createby = Request.Cookies["UserId"];
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            List<WCSItem> TData = new List<WCSItem>();
            WCSItem tda = new WCSItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new WCSItem();
                    tda.Sievelst = BindSieve();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {
                DataTable dt = new DataTable();
                dt = WCSieveService.GetEditWCSieve(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Brlst = BindBranch();
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Worklst = BindWorkCenter();
                    ca.WorkCenter = dt.Rows[0]["WCBASICID"].ToString();
                    //ca.sie = dt.Rows[0]["SIEVEID"].ToString();
                    //ca.countryid = dt.Rows[0]["PRATE"].ToString();
                    //ca.countryid = dt.Rows[0]["ITEMTYPE"].ToString();
                    ca.ID = id;
                }
                DataTable dt2 = new DataTable();

                dt2 = WCSieveService.WCSieveDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new WCSItem();
                        tda.Sievelst = BindSieve();
                        tda.Sieve = dt2.Rows[i]["SIEVEID"].ToString();
                        tda.Rate = dt2.Rows[i]["PRATE"].ToString();
                        tda.Type = dt2.Rows[i]["ITEMTYPE"].ToString();
                        TData.Add(tda);
                    }
                }

            }
            ca.WCSLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult WCSieve(WCSieve ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = WCSieveService.WCSieveCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " WCSieve Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " WCSieve Updated Successfully...!";
                    }
                    return RedirectToAction("ListWCSieve");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WCSieve";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public IActionResult ListWCSieve()
        {
            return View();
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = datatrans.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
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
        public List<SelectListItem> BindSieve()
        {
            try
            {
                DataTable dtDesg = WCSieveService.GetSieve();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SIEVE"].ToString(), Value = dtDesg.Rows[i]["SIEVEMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListWCSieveGrid(string strStatus)
        {
            List<WCSieveItem> Reg = new List<WCSieveItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)WCSieveService.GetAllWCSieve(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string view = string.Empty;

                view = "<a href=ViewWCSieve?id=" + dtUsers.Rows[i]["WCSPRODDETAILID"].ToString() + "><img src='../Images/view_icon.png' alt='View' /></a>";
                EditRow = "<a href=WCSieve?id=" + dtUsers.Rows[i]["WCSPRODDETAILID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["WCSPRODDETAILID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new WCSieveItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["WCSPRODDETAILID"].ToString()),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
                    sieve = dtUsers.Rows[i]["SIEVE"].ToString(),
                    rate = dtUsers.Rows[i]["PRATE"].ToString(),
                    type = dtUsers.Rows[i]["ITEMTYPE"].ToString(),
                    view = view,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewWCSieve(string id)
        {
            WCSieve ca = new WCSieve();
            DataTable dt = new DataTable();
            dt = WCSieveService.GetViewEditWCSieve(id);
            if (dt.Rows.Count > 0)
            {
                ca.Brlst = BindBranch();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Worklst = BindWorkCenter();
                ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                ca.ID = id;
            }
            List<WCSItem> TData = new List<WCSItem>();
            WCSItem tda = new WCSItem();

            DataTable dt2 = new DataTable();
            dt2 = WCSieveService.WCSieveViewDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new WCSItem();
                    tda.Sievelst = BindSieve();
                    tda.Sieve = dt2.Rows[i]["SIEVE"].ToString();
                    tda.Rate = dt2.Rows[i]["PRATE"].ToString();
                    tda.Type = dt2.Rows[i]["ITEMTYPE"].ToString();
                    TData.Add(tda);
                }
            }

              ca.WCSLst = TData;
            return View(ca);
        }
        public JsonResult GetSieveJSON()
        {
            WCSItem model = new WCSItem();
            model.Sievelst = BindSieve();
            return Json(BindSieve());

        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = WCSieveService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWCSieve");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWCSieve");
            }
        }
    }
}
