using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class RateController : Controller
    {
        IRateService RateService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public RateController(IRateService _RateService, IConfiguration _configuratio)
        {
            RateService = _RateService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Rate(string id)
        {
            Rate ca = new Rate();
            ca.Brlst = BindBranch();
            ca.Ratelst = BindRateCode();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("ratem");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            //ca.createby = Request.Cookies["UserId"];
            List<RateItem> TData = new List<RateItem>();
            RateItem tda = new RateItem();

            if (id == null)
            {
                tda = new RateItem();

                tda.Itemlst = BindItemlst();
                tda.Ratelst = BindRateCode();
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            else
            {
                //ca = LocationService.GetLocationsById(id);
                DataTable dt = new DataTable();
                //double total = 0;
                dt = RateService.GetEditRate(id);
                if (dt.Rows.Count > 0)
                {
                   
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Ratecode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateName = dt.Rows[0]["RATENAME"].ToString();
                    ca.ValidFrom = dt.Rows[0]["VALIDFROM"].ToString();
                    ca.ValidTo = dt.Rows[0]["VALIDTO"].ToString();
                    ca.UF = dt.Rows[0]["UF"].ToString();
                    ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = RateService.GetEditRateDeatil(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new RateItem();
                       
                        tda.Ratelst = BindRateCode();
                        tda.RCode = dt2.Rows[i]["RCODE"].ToString();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Validfrom = dt2.Rows[i]["VFROM"].ToString();
                        tda.Validto = dt2.Rows[i]["VTO"].ToString();
                        tda.Type = dt2.Rows[i]["RTYPE"].ToString();
                        TData.Add(tda);
                    }
                }

            }
            ca.RATElist = TData;
            return View(ca);
        }
        public IActionResult RateRivision(string id)
        {
            Rate ca = new Rate();
            ca.Brlst = BindBranch();
            ca.Ratelst = BindRateCode();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("ratem");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<RateItem> TData = new List<RateItem>();
            RateItem tda = new RateItem();
       
                DataTable dt = new DataTable();
                //double total = 0;
                dt = RateService.GetEditRate(id);
                if (dt.Rows.Count > 0)
                {

                   // ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Ratecode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateName = dt.Rows[0]["RATENAME"].ToString();
                    ca.ValidFrom = dt.Rows[0]["VALIDFROM"].ToString();
                    ca.ValidTo = dt.Rows[0]["VALIDTO"].ToString();
                    ca.UF = dt.Rows[0]["UF"].ToString();
                    ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                    ca.ID = id;
               }
                DataTable dt2 = new DataTable();

                dt2 = RateService.GetEditRateDeatil(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new RateItem();
                        //tda.Ratelst = BindRateCode();
                        tda.RCode = dt2.Rows[i]["RCODE"].ToString();
                        //tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Validfrom = dt2.Rows[i]["VFROM"].ToString();
                        tda.Validto = dt2.Rows[i]["VTO"].ToString();
                        tda.Type = dt2.Rows[i]["RTYPE"].ToString();
                        TData.Add(tda);
                    }
                ca.VRATElist = TData;
                }
            List<RateItem> TData2 = new List<RateItem>();
            RateItem tda2 = new RateItem();
            tda2 = new RateItem();
            tda2.Itemlst = BindItemlst();
            tda2.Isvalid = "Y";
            TData2.Add(tda2);

            ca.RATElist = TData2;
            return View(ca);
        }
        public JsonResult GetRateJSON()
        {
            return Json(BindRateCode());
        }
        public IActionResult AddRateCode(string id)
        {
            RateCode ca = new RateCode();
           // ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            return View(ca);
        }
        
        public JsonResult SaveRateCode(string ratecode,string ratedesc)
        {
            string Strout = RateService.RateCodeCRUD(ratecode, ratedesc);
            var result = new { msg = Strout };
            return Json(result);
        }
        [HttpPost]
        public ActionResult Rate(Rate Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = RateService.RateCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Rate Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Rate Updated Successfully...!";
                    }
                    return RedirectToAction("ListRate");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Rate";
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
        public IActionResult ListRate()
        {
            return View();
        }
        public ActionResult MyListRateGrid(string strStatus)
        {
            List<ListRateItem> Reg = new List<ListRateItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)RateService.GetAllListRateItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string revision = string.Empty;


                View = "<a href=ViewRate?id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                EditRow = "<a href=Rate?id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                revision = "<a href=RateRivision?id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "><img src='../Images/revision.png' alt='Edit' /></a>";

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewRate?id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=Rate?id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["RATEBASICID"].ToString() + "";
                }

                Reg.Add(new ListRateItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["RATEBASICID"].ToString()),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    ratecode = dtUsers.Rows[i]["RATECODE"].ToString(),
                    ratename = dtUsers.Rows[i]["RATENAME"].ToString(),
                    view = View,
                    edit = EditRow,
                    delrow = DeleteRow,
                    revision= revision
                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewRate(string id)
        {
            Rate ca = new Rate();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = RateService.GetEditRate(id);
            if (dt.Rows.Count > 0)
            {

                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Ratecode = dt.Rows[0]["RATECODE"].ToString();
                ca.RateName = dt.Rows[0]["RATENAME"].ToString();
                ca.ValidFrom = dt.Rows[0]["VALIDFROM"].ToString();
                ca.ValidTo = dt.Rows[0]["VALIDTO"].ToString();
                ca.UF = dt.Rows[0]["UF"].ToString();
                ca.RateType = dt.Rows[0]["RATETYPE"].ToString();
                ca.ID = id;

            }
            List<RateItem> TData = new List<RateItem>();
            RateItem tda = new RateItem();

            dtt = RateService.GetEditRateDeatils(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new RateItem();

                    tda.Ratelst = BindRateCode();
                    tda.RCode = dtt.Rows[i]["RCODE"].ToString();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                    tda.rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Validfrom = dtt.Rows[i]["VFROM"].ToString();
                    tda.Validto = dtt.Rows[i]["VTO"].ToString();
                    tda.Type = dtt.Rows[i]["RTYPE"].ToString();
                    TData.Add(tda);
                }
            }

            ca.RATElist = TData;
            return View(ca);
        }
        public ActionResult DeleteItem(string tag, String id)
        {

            string flag = RateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRate");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = RateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRate");
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
        public List<SelectListItem> BindRateCode()
        {
            try
            {
                DataTable dtDesg = RateService.GetRateCode();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["RATECODE"].ToString(), Value = dtDesg.Rows[i]["RATECODE"].ToString() });
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
                DataTable dtDesg = datatrans.GetItem();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable stock = new DataTable();

                string unit = "";
                string price = "";

                dt = RateService.GetItemDetails(ItemId);

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
            RateItem model = new RateItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public JsonResult GetItemRateJSON()
        {
            RateItem model = new RateItem();
            model.Ratelst = BindRateCode();
            return Json(BindRateCode());

        }
    }
}
