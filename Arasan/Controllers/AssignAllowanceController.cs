using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AssignAllowanceController : Controller
    {
        IAssignAllowance AssignAllowanceService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public AssignAllowanceController(IAssignAllowance _AssignAllowanceService, IConfiguration _configuratio)
        {
            AssignAllowanceService = _AssignAllowanceService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AssignAllowance(string id)
        {
            AssignAllowance ic = new AssignAllowance();
            ic.EmpNamelst = BindEmpName();
            ic.AllowanceNamelst = BindAllowanceName();
            ic.AllowanceTypelst = BindAllowanceType();
            if(id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = AssignAllowanceService.GetEditAssignAllowance(id);
                if(dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMP_NAME"].ToString();
                    ic.AllowanceNamelst = BindAllowanceName();
                    ic.AllowanceName = dt.Rows[0]["ALLOWANCE_NAME_ID"].ToString();
                    ic.AllowanceTypelst = BindAllowanceType();
                    ic.AllowanceType = dt.Rows[0]["ALLOWANCE_TYPE_ID"].ToString();
                    ic.AmtPerc = dt.Rows[0]["AMT_PERC"].ToString();
                    ic.EffectiveDate = dt.Rows[0]["EFFECTIVE_DATE"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult AssignAllowance(AssignAllowance Cy, string id)
        {
            try
            {
                id = Cy.ID;
                string Strout = AssignAllowanceService.AssignAllowanceCRUD(Cy);
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
                    return RedirectToAction("ListAssignAllowance");
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

        public IActionResult ListAssignAllowance()
        {
            return View();
        }
        public ActionResult MyListAssignAllowancegrid(string strStatus)
        {
            List<AssignAllowancegrid> Reg = new List<AssignAllowancegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = AssignAllowanceService.GetAllAssignAllowanceGrid(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=AssignAllowance?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewAssignAllowance?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new AssignAllowancegrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
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

        public IActionResult ViewAssignAllowance(string id)
        {
            AssignAllowance ic = new AssignAllowance();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select EMPMAST.EMPNAME,ALLOWANCE_MASTER.ALLOWANCE_NAME,ALLOWANCE_MASTER.ALLOWANCE_TYPE,AMT_PERC,EFFECTIVE_DATE,DESCRIPTION from ASSIGN_ALLOWANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = ASSIGN_ALLOWANCE.EMP_NAME LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_NAME_ID LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_TYPE_ID WHERE ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.AllowanceName = dt.Rows[0]["ALLOWANCE_NAME"].ToString();
                ic.AllowanceType = dt.Rows[0]["ALLOWANCE_TYPE"].ToString();
                ic.AmtPerc = dt.Rows[0]["AMT_PERC"].ToString();
                ic.EffectiveDate = dt.Rows[0]["EFFECTIVE_DATE"].ToString();
                ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                ic.ID = id;
            }
            return View(ic);
        }

        private List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = AssignAllowanceService.GetEmpName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SelectListItem> BindAllowanceName()
        {
            try
            {
                DataTable dtDesg = AssignAllowanceService.GetAllowanceName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ALLOWANCE_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SelectListItem> BindAllowanceType()
        {
            try
            {
                DataTable dtDesg = AssignAllowanceService.GetAllowanceType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ALLOWANCE_TYPE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = AssignAllowanceService.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAssignAllowance");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAssignAllowance");
            }

        }
    }
}
