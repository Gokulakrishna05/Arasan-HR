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
                DataTable dt = new DataTable();
                //dt = SalarySlipService.GetEditSalarySlip(id);
                dt = datatrans.GetData("Select SALARY_SLIP_ID,EMP_NAME,EMP_CODE,to_char(DOJ,'dd-MON-yyyy')DOJ,DEPT,DESIG,FATHER_NAME,to_char(DOB,'dd-MON-yyyy')DOB,BANK_NAME,ACC_NO,IFSC,PF_NO,ESI_NO,to_char(SAL_DIST_DATE,'dd-MON-yyyy')SAL_DIST_DATE,GROSS_SAL_DAY,BASIC_SALARY,DA,HRA,CONVEYANCE,OT_RATE,WASH_ALL,EDU_ALL,SPEC_ALL,PF,ESI,LOAN_ADV,INSURANCE,MEALS,FINE,TDS,OTH_DEDS,TOT_WORK_DAYS,NH_DAYS,WEEKOFF,WORKED_DAYS,LEAVE_DAYS,OP_CL,CL_TAKEN,CLO_CL,SALARY_DAYS from SALARY_SLIP WHERE SALARY_SLIP.SALARY_SLIP_ID='" + id + "'");
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["SALARY_SLIP_ID"].ToString();
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMP_NAME"].ToString();
                    ic.EmpCode = dt.Rows[0]["EMP_CODE"].ToString();
                    ic.DOJ = dt.Rows[0]["DOJ"].ToString();
                    ic.Dept = dt.Rows[0]["DEPT"].ToString();
                    ic.Desg = dt.Rows[0]["DESIG"].ToString();
                    ic.FatherName = dt.Rows[0]["FATHER_NAME"].ToString();
                    ic.DOB = dt.Rows[0]["DOB"].ToString();
                    ic.BankName = dt.Rows[0]["BANK_NAME"].ToString();
                    ic.AccNo = dt.Rows[0]["ACC_NO"].ToString();
                    ic.IFSC = dt.Rows[0]["IFSC"].ToString();
                    ic.PFNo = dt.Rows[0]["PF_NO"].ToString();
                    ic.ESINo = dt.Rows[0]["ESI_NO"].ToString();
                    ic.SalDistDate = dt.Rows[0]["SAL_DIST_DATE"].ToString();
                    ic.GrossSalaryDay = dt.Rows[0]["GROSS_SAL_DAY"].ToString();

                    ic.BasicSalary = (double)Convert.ToDecimal(dt.Rows[0]["BASIC_SALARY"]);
                    ic.DA = (double)Convert.ToDecimal(dt.Rows[0]["DA"]);
                    ic.HRA = (double)Convert.ToDecimal(dt.Rows[0]["HRA"].ToString());
                    ic.Conveyance = (double)Convert.ToDecimal(dt.Rows[0]["CONVEYANCE"]);
                    ic.OT = (double)Convert.ToDecimal(dt.Rows[0]["OT_RATE"]);
                    ic.WA = (double)Convert.ToDecimal(dt.Rows[0]["WASH_ALL"]);
                    ic.EA = (double)Convert.ToDecimal(dt.Rows[0]["EDU_ALL"]);
                    ic.SA = (double)Convert.ToDecimal(dt.Rows[0]["SPEC_ALL"]);
                    ic.PF = (double)Convert.ToDecimal(dt.Rows[0]["PF"]);
                    ic.ESI = (double)Convert.ToDecimal(dt.Rows[0]["ESI"]);
                    ic.LoanAdv = (double)Convert.ToDecimal(dt.Rows[0]["LOAN_ADV"]);
                    ic.Insurance = (double)Convert.ToDecimal(dt.Rows[0]["INSURANCE"]);
                    ic.Meals = (double)Convert.ToDecimal(dt.Rows[0]["MEALS"]);
                    ic.Fine = (double)Convert.ToDecimal(dt.Rows[0]["FINE"]);
                    ic.TDS = (double)Convert.ToDecimal(dt.Rows[0]["TDS"]);
                    ic.OtherDeductions = (double)Convert.ToDecimal(dt.Rows[0]["OTH_DEDS"]);

                    ic.TotWorkDays = dt.Rows[0]["TOT_WORK_DAYS"].ToString();
                    ic.NHDays = dt.Rows[0]["NH_DAYS"].ToString();
                    ic.WeekOff = dt.Rows[0]["WEEKOFF"].ToString();
                    ic.WorkedDays = dt.Rows[0]["WORKED_DAYS"].ToString();
                    ic.LeaveDays = dt.Rows[0]["LEAVE_DAYS"].ToString();
                    ic.OpCL = (double)Convert.ToDecimal(dt.Rows[0]["OP_CL"].ToString());
                    ic.CLTaken = (double)Convert.ToDecimal(dt.Rows[0]["CL_TAKEN"].ToString());
                    ic.CloCL = (double)Convert.ToDecimal(dt.Rows[0]["CLO_CL"].ToString());
                    ic.SalaryDays = dt.Rows[0]["SALARY_DAYS"].ToString();
                }
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
                    PDF = "<a href=SalarySlipPdf?id=" + dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                    ViewRow = "<a href=ViewSalarySlip?id=" + dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    EditRow = "<a href=SalarySlip?id=" + dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString() + "";
                }
                Reg.Add(new ListSalarySlip
                {
                    id = dtUsers.Rows[i]["SALARY_SLIP_ID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    dept = dtUsers.Rows[i]["DEPT"].ToString(),
                    desg = dtUsers.Rows[i]["DESIG"].ToString(),
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
        public IActionResult ViewSalarySlip(string id)
        {
            SalarySlip ic = new SalarySlip();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select EMPMAST.EMPNAME,EMP_CODE,to_char(DOJ,'dd-MON-yyyy')DOJ,DEPT,DESIG,FATHER_NAME,to_char(SS.DOB,'dd-MON-yyyy')DOB,BANK_NAME,ACC_NO,IFSC,PF_NO,ESI_NO,to_char(SAL_DIST_DATE,'dd-MON-yyyy')SAL_DIST_DATE,GROSS_SAL_DAY,BASIC_SALARY,DA,HRA,CONVEYANCE,OT_RATE,WASH_ALL,EDU_ALL,SPEC_ALL,PF,ESI,LOAN_ADV,INSURANCE,MEALS,FINE,TDS,OTH_DEDS,TOT_WORK_DAYS,NH_DAYS,WEEKOFF,WORKED_DAYS,LEAVE_DAYS,OP_CL,CL_TAKEN,CLO_CL,SALARY_DAYS from SALARY_SLIP SS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SS.EMP_NAME WHERE SS.SALARY_SLIP_ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.EmpCode = dt.Rows[0]["EMP_CODE"].ToString();
                ic.DOJ = dt.Rows[0]["DOJ"].ToString();
                ic.Dept = dt.Rows[0]["DEPT"].ToString();
                ic.Desg = dt.Rows[0]["DESIG"].ToString();
                ic.FatherName = dt.Rows[0]["FATHER_NAME"].ToString();
                ic.DOB = dt.Rows[0]["DOB"].ToString();
                ic.BankName = dt.Rows[0]["BANK_NAME"].ToString();
                ic.AccNo = dt.Rows[0]["ACC_NO"].ToString();
                ic.IFSC = dt.Rows[0]["IFSC"].ToString();
                ic.PFNo = dt.Rows[0]["PF_NO"].ToString();
                ic.ESINo = dt.Rows[0]["ESI_NO"].ToString();
                ic.SalDistDate = dt.Rows[0]["SAL_DIST_DATE"].ToString();
                ic.GrossSalaryDay = dt.Rows[0]["GROSS_SAL_DAY"].ToString();

                ic.BasicSalary = (double)Convert.ToDecimal(dt.Rows[0]["BASIC_SALARY"]);
                ic.DA = (double)Convert.ToDecimal(dt.Rows[0]["DA"]);
                ic.HRA = (double)Convert.ToDecimal(dt.Rows[0]["HRA"].ToString());
                ic.Conveyance = (double)Convert.ToDecimal(dt.Rows[0]["CONVEYANCE"]);
                ic.OT = (double)Convert.ToDecimal(dt.Rows[0]["OT_RATE"]);
                ic.WA = (double)Convert.ToDecimal(dt.Rows[0]["WASH_ALL"]);
                ic.EA = (double)Convert.ToDecimal(dt.Rows[0]["EDU_ALL"]);
                ic.SA = (double)Convert.ToDecimal(dt.Rows[0]["SPEC_ALL"]);
                ic.PF = (double)Convert.ToDecimal(dt.Rows[0]["PF"]);
                ic.ESI = (double)Convert.ToDecimal(dt.Rows[0]["ESI"]);
                ic.LoanAdv = (double)Convert.ToDecimal(dt.Rows[0]["LOAN_ADV"]);
                ic.Insurance = (double)Convert.ToDecimal(dt.Rows[0]["INSURANCE"]);
                ic.Meals = (double)Convert.ToDecimal(dt.Rows[0]["MEALS"]);
                ic.Fine = (double)Convert.ToDecimal(dt.Rows[0]["FINE"]);
                ic.TDS = (double)Convert.ToDecimal(dt.Rows[0]["TDS"]);
                ic.OtherDeductions = (double)Convert.ToDecimal(dt.Rows[0]["OTH_DEDS"]);
                ic.TotWorkDays = dt.Rows[0]["TOT_WORK_DAYS"].ToString();
                ic.NHDays = dt.Rows[0]["NH_DAYS"].ToString();
                ic.WeekOff = dt.Rows[0]["WEEKOFF"].ToString();
                ic.WorkedDays = dt.Rows[0]["WORKED_DAYS"].ToString();
                ic.LeaveDays = dt.Rows[0]["LEAVE_DAYS"].ToString();
                ic.OpCL = (double)Convert.ToDecimal(dt.Rows[0]["OP_CL"].ToString());
                ic.CLTaken = (double)Convert.ToDecimal(dt.Rows[0]["CL_TAKEN"].ToString());
                ic.CloCL = (double)Convert.ToDecimal(dt.Rows[0]["CLO_CL"].ToString());
                ic.SalaryDays = dt.Rows[0]["SALARY_DAYS"].ToString();
                ic.ID = id;
            }
            return View(ic);
        }

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
                DataTable dtt = new DataTable();
                DataTable dttt = new DataTable();
                string emplid = "", dept = "", desg = "", join = "", fath = "", bank = "", bankacc = "", pfno = "", esino = "", empdob = "", bassalary = "", da = "", hra = "", conv = "", wa = "", ea = "", sa = "", ot = "", pf = "", esi = "", loanadv = "", ins = "", meals = "", fine = "", tds = "", othdeds = "", woff = "", totleave = "", nhdays = "", cltaken = "";

                //dt = SalarySlipService.GetEmpDetails(empid);
                //dt = datatrans.GetData("SELECT EM.EMPMASTID,EM.EMPID,EM.EMPNAME,EM.EMPDOB,EM.JOINDATE,EM.EMPDESIGN,EM.PFNO,EM.ESINO,EM.FATHERNAME,EM.WOFF,EM.BANK,EM.BANKACCNO,SS.BASIC_SALARY,SS.DA,SS.HRA,SS.CONVEYANCE,SS.OT_RATE,SS.WASH_ALL,SS.EDU_ALL,SS.SPEC_ALL,SS.PF,SS.ESI,SS.LOAN_ADV,SS.INSURANCE,SS.MEALS,SS.FINE,SS.TDS,SS.OTH_DEDS,DB.DEPTNAME FROM EMPMAST EM LEFT OUTER JOIN SALARY_STRUCTURE SS ON SS.EMP_NAME=EM.EMPMASTID LEFT OUTER JOIN DDBASIC DB ON DB.DDBASICID=EM.EMPDEPT WHERE EM.EMPMASTID = '" + empid + "'");
                dt = datatrans.GetData("SELECT EM.EMPMASTID,EM.EMPID,EM.EMPNAME,to_char(EM.EMPDOB,'dd-MON-yyyy')EMPDOB,to_char(EM.JOINDATE,'dd-MON-yyyy')JOINDATE,EM.EMPDESIGN,EM.PFNO,EM.ESINO,EM.FATHERNAME,EM.WOFF,EM.BANK,EM.BANKACCNO,SS.BASIC_SALARY,SS.DA,SS.HRA,SS.CONVEYANCE,SS.OT_RATE,SS.WASH_ALL,SS.EDU_ALL,SS.SPEC_ALL,SS.PF,SS.ESI,SS.LOAN_ADV,SS.INSURANCE,SS.MEALS,SS.FINE,SS.TDS,SS.OTH_DEDS,DB.DEPTNAME FROM EMPMAST EM LEFT OUTER JOIN SALARY_STRUCTURE SS ON SS.EMP_NAME=EM.EMPMASTID LEFT OUTER JOIN DDBASIC DB ON DB.DDBASICID=EM.EMPDEPT WHERE EM.EMPMASTID = '" + empid + "'");
                dtt = datatrans.GetData("SELECT DAYOFWEEK, COUNT(*) AS TOTAL_DAYS FROM HOLIDAYMASTER WHERE IS_ACTIVE='Y' AND EXTRACT(MONTH FROM HOLIDAYDATE)=EXTRACT(MONTH FROM SYSDATE) AND EXTRACT(YEAR FROM HOLIDAYDATE)=EXTRACT(YEAR FROM SYSDATE) GROUP BY DAYOFWEEK ORDER BY DAYOFWEEK");
                dttt = datatrans.GetData("SELECT LR.TOTAL_DAYS, LTM.LEAVETYPENAME FROM LEAVEREQUEST LR LEFT OUTER JOIN LEAVETYPEMASTER LTM ON LTM.ID=LR.LEAVE_TYPE WHERE STATUS='Approve' AND EXTRACT(MONTH FROM FROM_DATE)=EXTRACT(MONTH FROM SYSDATE) AND EXTRACT(YEAR FROM FROM_DATE)=EXTRACT(YEAR FROM SYSDATE) AND LR.EMP_ID='" + empid + "'");
                if (dt.Rows.Count > 0)
                {
                    emplid = dt.Rows[0]["EMPID"].ToString(); dept = dt.Rows[0]["DEPTNAME"].ToString(); desg = dt.Rows[0]["EMPDESIGN"].ToString();
                    join = dt.Rows[0]["JOINDATE"].ToString(); fath = dt.Rows[0]["FATHERNAME"].ToString(); bank = dt.Rows[0]["BANK"].ToString();
                    bankacc = dt.Rows[0]["BANKACCNO"].ToString(); pfno = dt.Rows[0]["PFNO"].ToString(); esino = dt.Rows[0]["ESINO"].ToString();
                    empdob = dt.Rows[0]["EMPDOB"].ToString(); bassalary = dt.Rows[0]["BASIC_SALARY"].ToString(); da = dt.Rows[0]["DA"].ToString();
                    hra = dt.Rows[0]["HRA"].ToString(); conv = dt.Rows[0]["CONVEYANCE"].ToString(); wa = dt.Rows[0]["WASH_ALL"].ToString();
                    ea = dt.Rows[0]["EDU_ALL"].ToString(); sa = dt.Rows[0]["SPEC_ALL"].ToString(); ot = dt.Rows[0]["OT_RATE"].ToString();
                    pf = dt.Rows[0]["PF"].ToString(); esi = dt.Rows[0]["ESI"].ToString(); loanadv = dt.Rows[0]["LOAN_ADV"].ToString();
                    ins = dt.Rows[0]["INSURANCE"].ToString(); meals = dt.Rows[0]["MEALS"].ToString(); fine = dt.Rows[0]["FINE"].ToString();
                    tds = dt.Rows[0]["TDS"].ToString(); othdeds = dt.Rows[0]["OTH_DEDS"].ToString(); woff = dt.Rows[0]["WOFF"].ToString();
                     
                    if (dtt.Rows.Count > 0)
                    {
                        nhdays = dtt.Rows[0]["DAYOFWEEK"].ToString();
                    }
                    else
                    {
                        nhdays = "0";
                    }
                    if (dttt.Rows.Count > 0)
                    {
                        for(int i = 0; i < dttt.Rows.Count; i++)
                        {
                            if (dttt.Rows[i]["LEAVETYPENAME"].ToString() == "Casual Leave")
                            {
                                cltaken = dttt.Rows[i]["TOTAL_DAYS"].ToString();
                            }
                            else
                            {
                                cltaken = "0";
                            }

                            if(dttt.Rows[i]["LEAVETYPENAME"].ToString() != "Casual Leave")
                            {
                                totleave = dttt.Rows[i]["TOTAL_DAYS"].ToString();
                            }
                            else
                            {
                                totleave = "0";
                            }
                        }
                    }
                    else
                    {
                        totleave = "0";
                        cltaken = "0";
                    }

                }

                var response = new { emplid = emplid, dept = dept, desg = desg, join = join, fath = fath, bank = bank, bankacc = bankacc, pfno = pfno, esino = esino, empdob = empdob, bassalary = bassalary, da = da, hra = hra, conv = conv, wa = wa, ea = ea, sa = sa, ot = ot, pf = pf, esi = esi, loanadv = loanadv, ins = ins, meals = meals, fine = fine, tds = tds, othdeds = othdeds, woff = woff, totleave = totleave, nhdays = nhdays, cltaken = cltaken };
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
