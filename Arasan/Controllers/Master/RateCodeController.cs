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
    public class RateCodeController : Controller
    {
        IRateCodeService RateCodeService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public RateCodeController(IRateCodeService _RateCodeService, IConfiguration _configuratio)
        {
            RateCodeService = _RateCodeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult RateCode(string id)
        {
            RateCode ca = new RateCode();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            //ca.createby = Request.Cookies["UserId"];

            if (id == null)
            {

            }
            else
            {
                //ca = LocationService.GetLocationsById(id);
                DataTable dt = new DataTable();

                dt = RateCodeService.GetEditRateCode(id);
                if (dt.Rows.Count > 0)
                {
                   
                    ca.Ratecode = dt.Rows[0]["RATECODE"].ToString();
                    ca.RateDsc = dt.Rows[0]["RATEDESC"].ToString();
                   
                    ca.ID = id;

                }

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult RateCode(RateCode Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = RateCodeService.RateCodeCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "RateCode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "RateCode Updated Successfully...!";
                    }
                    return RedirectToAction("ListRateCode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit RateCode";
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
        public IActionResult ListRateCode()
        {
            return View();
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
        public ActionResult MyRateCodeGrid(string strStatus)
        {
            List<ListRateCodeItem> Reg = new List<ListRateCodeItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)RateCodeService.GetAllListRateCodeItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //string View = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                //View = "<a href=ViewSalesTarget?id=" + dtUsers.Rows[i]["SALFCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                EditRow = "<a href=RateCode?id=" + dtUsers.Rows[i]["RATECODEMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["RATECODEMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ListRateCodeItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["RATECODEMASTID"].ToString()),
                    ratecode = dtUsers.Rows[i]["RATECODE"].ToString(),
                    ratedsc = dtUsers.Rows[i]["RATEDESC"].ToString(),
                    edit = EditRow,
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

            string flag = RateCodeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRateCode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRateCode");
            }
        }
    }
}
