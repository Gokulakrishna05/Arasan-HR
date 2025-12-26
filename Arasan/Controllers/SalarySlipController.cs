using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SalarySlipController : Controller
    {
        ISalarySlipService SalarySlipService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public SalarySlipController(ISalarySlipService _SalarySlipService, IConfiguration _configuratio)
        {
            SalarySlipService = _SalarySlipService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult SalarySlip(string id)
        {
            SalarySlip ic = new SalarySlip();
            ic.EmpNamelst = BindEmpName();
            if (id == null)
            {

            }
            else
            {
                //DataTable dt = new DataTable();
                //dt = SalarySlipService.GetEditSalarySlip(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ic.ID = dt.Rows[0]["ID"].ToString();
                //    ic.EmpNamelst = BindEmpName();
                //    ic.EmpName = dt.Rows[0]["EMP_NAME"].ToString();
                //    ic.Salary = dt.Rows[0]["BASIC_SALARY"].ToString();
                //    ic.HRA = dt.Rows[0]["HRA"].ToString();
                //    ic.AllowanceAmt = dt.Rows[0]["ALLOWANCE_AMT"].ToString();
                //    ic.OTRate = dt.Rows[0]["OT_RATE"].ToString();
                //    ic.Incentive = dt.Rows[0]["INCENTIVE"].ToString();
                //    ic.Bonus = dt.Rows[0]["BONUS_ISELIGIBLE"].ToString();
                //}
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult SalarySlip(SalarySlip Cy, string id)
        {
            try
            {
                id = Cy.ID;
                string Strout = SalarySlipService.SalarySlipCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Salary Slip Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Salary Slip Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalarySlip");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Salary Slip";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ListSalarySlip()
        {
            return View();
        }

        public ActionResult MyListSalarySlipgrid(string strStatus)
        {
            List<ListSalarySlip> Reg = new List<ListSalarySlip>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalarySlipService.GetAllSalarySlipGrid(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                string PDF = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=SalarySlip?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewSalarySlip?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new ListSalarySlip
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    dept = dtUsers.Rows[i]["BONUS_ISELIGIBLE"].ToString(),
                    desg = dtUsers.Rows[i]["BONUS_ISELIGIBLE"].ToString(),
                    pdf = PDF,
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
        //public IActionResult ViewSalarySlip(string id)
        //{
        //    SalarySlip ic = new SalarySlip();
        //    DataTable dt = new DataTable();
        //    dt = datatrans.GetData("Select EMPMAST.EMPNAME,BASIC_SALARY,HRA,ALLOWANCE_AMT,OT_RATE,INCENTIVE,BONUS_ISELIGIBLE from SALARY_STRUCTURE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SALARY_STRUCTURE.EMP_NAME WHERE SALARY_STRUCTURE.ID='" + id + "'");

        //    if (dt.Rows.Count > 0)
        //    {
        //        ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
        //        ic.Salary = dt.Rows[0]["BASIC_SALARY"].ToString();
        //        ic.HRA = dt.Rows[0]["HRA"].ToString();
        //        ic.AllowanceAmt = dt.Rows[0]["ALLOWANCE_AMT"].ToString();
        //        ic.OTRate = dt.Rows[0]["OT_RATE"].ToString();
        //        ic.Incentive = dt.Rows[0]["INCENTIVE"].ToString();
        //        ic.Bonus = dt.Rows[0]["BONUS_ISELIGIBLE"].ToString();
        //        ic.ID = id;
        //    }
        //    return View(ic);
        //}

        private List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = SalarySlipService.GetEmpName();
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

        public ActionResult GetEmpDetailsJSON(string empid)
        {
            try
            {
                DataTable dt = new DataTable();
                string amt = "";

                dt = SalarySlipService.GetEmpDetails(empid);

                if (dt.Rows.Count > 0)
                {
                    amt = dt.Rows[0]["AMT_PERC"].ToString();

                }

                var response = new { amt = amt };
                return Json(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = SalarySlipService.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalarySlip");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalarySlip");
            }

        }
    }
}
