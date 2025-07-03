using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AllowanceMasterController : Controller
    {
        IAllowanceMaster AllowanceMasterService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public AllowanceMasterController(IAllowanceMaster _AllowanceMasterService, IConfiguration _configuratio)
        {
            AllowanceMasterService = _AllowanceMasterService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AllowanceMaster(string id)
        {
            AllowanceMaster ic = new AllowanceMaster();
            ic.AllowanceTypelst = BindAllowanceType();
            ic.ApplicableLevellst = BindApplicableLevel();
            if(id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = AllowanceMasterService.GetEditAllowanceMaster(id);
                if(dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.AllowanceName = dt.Rows[0]["ALLOWANCE_NAME"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                    ic.AllowanceTypelst = BindAllowanceType();
                    ic.AllowanceType = dt.Rows[0]["ALLOWANCE_TYPE"].ToString();
                    ic.ApplicableLevellst = BindApplicableLevel();
                    ic.ApplicableLevel = dt.Rows[0]["APPLICABLE_LEVEL"].ToString();
                    ic.IsRecurring = dt.Rows[0]["IS_RECURRING"].ToString();
                    ic.EffectiveDate = dt.Rows[0]["EFFECTIVE_DATE"].ToString();
                    
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult AllowanceMaster(AllowanceMaster Cy, string id)
        {
            try
            {
                id = Cy.ID;
                string Strout = AllowanceMasterService.AllowanceMasterCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Allowance Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Allowance Updated Successfully...!";
                    }
                    return RedirectToAction("ListAllowanceMaster");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Allowance";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ListAllowanceMaster()
        {
            return View();
        }
        public ActionResult MyListAllowanceMastergrid(string strStatus)
        {
            List<AllowanceMastergrid> Reg = new List<AllowanceMastergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = AllowanceMasterService.GetAllAllowanceMasterGrid(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=AllowanceMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewAllowanceMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new AllowanceMastergrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    allowancename = dtUsers.Rows[i]["ALLOWANCE_NAME"].ToString(),
                    allowancetype = dtUsers.Rows[i]["ALLOWANCE_TYPE"].ToString(),
                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                });
            }

            return Json(new
            {
                Reg
            });
        }

        public IActionResult ViewAllowanceMaster(string id)
        {
            AllowanceMaster ic = new AllowanceMaster();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select ALLOWANCE_NAME,DESCRIPTION,ALLOWANCE_TYPE,APPLICABLE_LEVEL,IS_RECURRING,to_char(EFFECTIVE_DATE,'dd-MON-yyyy')EFFECTIVE_DATE from ALLOWANCE_MASTER WHERE ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.AllowanceName = dt.Rows[0]["ALLOWANCE_NAME"].ToString();
                ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                ic.AllowanceType = dt.Rows[0]["ALLOWANCE_TYPE"].ToString();
                ic.ApplicableLevel = dt.Rows[0]["APPLICABLE_LEVEL"].ToString();
                ic.IsRecurring = dt.Rows[0]["IS_RECURRING"].ToString();
                ic.EffectiveDate = dt.Rows[0]["EFFECTIVE_DATE"].ToString();
                ic.ID = id;
            }
            return View(ic);
        }

        private List<SelectListItem> BindAllowanceType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Fixed", Value = "Fixed" });
                lstdesg.Add(new SelectListItem() { Text = "Percentage", Value = "Percentage" });
                lstdesg.Add(new SelectListItem() { Text = "Custom", Value = "Custom" });
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SelectListItem> BindApplicableLevel()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Department", Value = "Department" });
                lstdesg.Add(new SelectListItem() { Text = "Employee", Value = "Employee" });
                lstdesg.Add(new SelectListItem() { Text = "Both", Value = "Both" });
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = AllowanceMasterService.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAllowanceMaster");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAllowanceMaster");
            }

        }

    }
}
