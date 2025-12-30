using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SalaryStructureController : Controller
    {
        ISalaryStructure SalaryStructureService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public SalaryStructureController(ISalaryStructure _SalaryStructureService, IConfiguration _configuratio)
        {
            SalaryStructureService = _SalaryStructureService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult SalaryStructure(string id)
        {
            SalaryStructure ic = new SalaryStructure();
            ic.EmpNamelst = BindEmpName();
            ic.Bonus = "No";
            if(id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = SalaryStructureService.GetEditSalaryStructure(id);
                if(dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMP_NAME"].ToString();
                    ic.BasicSalary = (double)Convert.ToDecimal(dt.Rows[0]["BASIC_SALARY"]);
                    ic.DA = (double)Convert.ToDecimal(dt.Rows[0]["DA"]);
                    ic.HRA = (double)Convert.ToDecimal(dt.Rows[0]["HRA"].ToString());
                    ic.Conveyance = (double)Convert.ToDecimal(dt.Rows[0]["CONVEYANCE"]);
                    ic.OT = (double)Convert.ToDecimal(dt.Rows[0]["OT_RATE"]);
                    ic.WA = (double)Convert.ToDecimal(dt.Rows[0]["WASH_ALL"]);
                    ic.EA = (double)Convert.ToDecimal(dt.Rows[0]["EDU_ALL"]);
                    ic.SA = (double)Convert.ToDecimal(dt.Rows[0]["SPEC_ALL"]);
                    ic.Bonus = dt.Rows[0]["BONUS_ISELIGIBLE"].ToString();
                    ic.BonusAmt = (double)Convert.ToDecimal(dt.Rows[0]["BONUS_AMT"]);
                    ic.PF = (double)Convert.ToDecimal(dt.Rows[0]["PF"]);
                    ic.ESI = (double)Convert.ToDecimal(dt.Rows[0]["ESI"]);
                    ic.LoanAdv = (double)Convert.ToDecimal(dt.Rows[0]["LOAN_ADV"]);
                    ic.Insurance = (double)Convert.ToDecimal(dt.Rows[0]["INSURANCE"]);
                    ic.Meals = (double)Convert.ToDecimal(dt.Rows[0]["MEALS"]);
                    ic.Fine = (double)Convert.ToDecimal(dt.Rows[0]["FINE"]);
                    ic.TDS = (double)Convert.ToDecimal(dt.Rows[0]["TDS"]);
                    ic.OtherDeductions = (double)Convert.ToDecimal(dt.Rows[0]["OTH_DEDS"]);
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult SalaryStructure(SalaryStructure Cy, string id)
        {
            try
            {
                id = Cy.ID;
                string Strout = SalaryStructureService.SalaryStructureCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Salary Structure Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Salary Structure Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalaryStructure");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Salary Structure";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ListSalaryStructure()
        {
            return View();
        }
        public ActionResult MyListSalaryStructuregrid(string strStatus)
        {
            List<SalaryStructuregrid> Reg = new List<SalaryStructuregrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalaryStructureService.GetAllSalaryStructureGrid(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=SalaryStructure?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewSalaryStructure?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new SalaryStructuregrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    bonus = dtUsers.Rows[i]["BONUS_ISELIGIBLE"].ToString(),
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

        public IActionResult ViewSalaryStructure(string id)
        {
            SalaryStructure ic = new SalaryStructure();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select EMPMAST.EMPNAME,BASIC_SALARY,DA,HRA,CONVEYANCE,OT_RATE,WASH_ALL,EDU_ALL,SPEC_ALL,BONUS_ISELIGIBLE,BONUS_AMT,PF,ESI,LOAN_ADV,INSURANCE,MEALS,FINE,TDS,OTH_DEDS from SALARY_STRUCTURE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SALARY_STRUCTURE.EMP_NAME WHERE SALARY_STRUCTURE.ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.BasicSalary = (double)Convert.ToDecimal(dt.Rows[0]["BASIC_SALARY"]);
                ic.DA = (double)Convert.ToDecimal(dt.Rows[0]["DA"]);
                ic.HRA = (double)Convert.ToDecimal(dt.Rows[0]["HRA"].ToString());
                ic.Conveyance = (double)Convert.ToDecimal(dt.Rows[0]["CONVEYANCE"]);
                ic.OT = (double)Convert.ToDecimal(dt.Rows[0]["OT_RATE"]);
                ic.WA = (double)Convert.ToDecimal(dt.Rows[0]["WASH_ALL"]);
                ic.EA = (double)Convert.ToDecimal(dt.Rows[0]["EDU_ALL"]);
                ic.SA = (double)Convert.ToDecimal(dt.Rows[0]["SPEC_ALL"]);
                ic.Bonus = dt.Rows[0]["BONUS_ISELIGIBLE"].ToString();
                ic.BonusAmt = (double)Convert.ToDecimal(dt.Rows[0]["BONUS_AMT"]);
                ic.PF = (double)Convert.ToDecimal(dt.Rows[0]["PF"]);
                ic.ESI = (double)Convert.ToDecimal(dt.Rows[0]["ESI"]);
                ic.LoanAdv = (double)Convert.ToDecimal(dt.Rows[0]["LOAN_ADV"]);
                ic.Insurance = (double)Convert.ToDecimal(dt.Rows[0]["INSURANCE"]);
                ic.Meals = (double)Convert.ToDecimal(dt.Rows[0]["MEALS"]);
                ic.Fine = (double)Convert.ToDecimal(dt.Rows[0]["FINE"]);
                ic.TDS = (double)Convert.ToDecimal(dt.Rows[0]["TDS"]);
                ic.OtherDeductions = (double)Convert.ToDecimal(dt.Rows[0]["OTH_DEDS"]);
                ic.ID = id;
            }
            return View(ic);
        }

        private List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = SalaryStructureService.GetEmpName();
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

        public ActionResult GetAllAmtJSON(string allamtid)
        {
            try
            {
                DataTable dt = new DataTable();
                string da = "", hra = "", conv = "", ot = "", wa = "", ea = "", sa = "";
                
                //dt = SalaryStructureService.GetAllAmtDetails(allamtid);
                dt = datatrans.GetData("SELECT AAD.ID, AAD.ASSALLDETAILID, AM.ALLOWANCE_NAME, AAD.AMT_PERC FROM ASSIGN_ALLOWNACE_DETAILS AAD LEFT OUTER JOIN ALLOWANCE_MASTER AM ON AM.ID=AAD.ALLOWANCE_NAME_ID WHERE EMP_ID = '" + allamtid + "' ");

                if (dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Dearness Allowance(DA)")
                        {
                            da = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "House Rent Allowance(HRA)")
                        {
                            hra = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Conveyance")
                        {
                            conv = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Over Time Allowance(OT)")
                        {
                            ot = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Washing Allowance(WA)")
                        {
                            wa = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Education Allowance(EA)")
                        {
                            ea = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                        if (dt.Rows[i]["ALLOWANCE_NAME"].ToString() == "Special Allowance(SA)")
                        {
                            sa = dt.Rows[i]["AMT_PERC"].ToString();
                        }
                    }
                }

                var response = new { da = da, hra = hra, conv = conv, ot = ot, wa = wa, ea = ea, sa = sa };
                return Json(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = SalaryStructureService.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalaryStructure");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalaryStructure");
            }

        }
    }
}
