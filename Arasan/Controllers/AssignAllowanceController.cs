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

            List<SelectAllowance> TData = new List<SelectAllowance>();
            SelectAllowance tda = new SelectAllowance();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SelectAllowance();
                    tda.AllowanceNamelst = BindAllowanceName();
                    tda.AllowanceTypelst = BindAllowanceType("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = AssignAllowanceService.GetEditAssignAllowance(id);
                if(dt.Rows.Count > 0)
                {
                    ic.ID = id;
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMP_NAME"].ToString();
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = AssignAllowanceService.GetEditAssignAllowanceDetails(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new SelectAllowance();

                    tda.AllowanceNamelst = BindAllowanceName();
                    tda.AllowanceName = dt2.Rows[i]["ALLOWANCE_NAME_ID"].ToString();
                    tda.AllowanceTypelst = BindAllowanceType(tda.AllowanceName);
                    tda.AllowanceType = dt2.Rows[i]["ALLOWANCE_TYPE"].ToString();
                    tda.AmtPerc = dt2.Rows[i]["AMT_PERC"].ToString();
                    tda.EffectiveDate = dt2.Rows[i]["EFFECTIVE_DATE"].ToString();
                    tda.Description = dt2.Rows[i]["REMARKS"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ic.Allowancelst = TData;
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
            dt = datatrans.GetData("Select EMPMAST.EMPNAME from ASSIGN_ALLOWANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = ASSIGN_ALLOWANCE.EMP_NAME WHERE ASSIGN_ALLOWANCE.ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.ID = id;
            }
            List<SelectAllowance> TData = new List<SelectAllowance>();
            SelectAllowance tda = new SelectAllowance();

            DataTable dtt = new DataTable();
            dtt = datatrans.GetData("SELECT ALLOWANCE_MASTER.ALLOWANCE_NAME,ALLOWANCE_MASTER.ALLOWANCE_TYPE,ASSIGN_ALLOWNACE_DETAILS.AMT_PERC,to_char(ASSIGN_ALLOWNACE_DETAILS.EFFECTIVE_DATE,'dd-MON-yyyy') AS EFFECTIVE_DATE,ASSIGN_ALLOWNACE_DETAILS.REMARKS FROM ASSIGN_ALLOWNACE_DETAILS LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWNACE_DETAILS.ALLOWANCE_NAME_ID LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWNACE_DETAILS.ALLOWANCE_TYPE WHERE ASSIGN_ALLOWNACE_DETAILS.ID='" + id + "'");

            if(dtt.Rows.Count > 0)
            {
                for(int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SelectAllowance();
                    tda.AllowanceName = dt.Rows[0]["ALLOWANCE_NAME"].ToString();
                    tda.AllowanceType = dt.Rows[0]["ALLOWANCE_TYPE"].ToString();
                    tda.AmtPerc = dt.Rows[0]["AMT_PERC"].ToString();
                    tda.EffectiveDate = dt.Rows[0]["EFFECTIVE_DATE"].ToString();
                    tda.Description = dt.Rows[0]["REMARKS"].ToString();
                    TData.Add(tda);
                }
            }
            ic.Allowancelst = TData;
            return View(ic);
        }

        //public List<SelectListItem> BindAllowanceType(string alltypeid)
        //{
        //    try
        //    {
        //        DataTable dtDesg = AssignAllowanceService.GetAllowanceType(alltypeid);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["CITY_NAME"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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

        public JsonResult GetAllNameJSON()
        {
            return Json(BindAllowanceName());
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

        public JsonResult GetAllTypeJSON(string alltypeid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindAllowanceType(alltypeid));
        }

        private List<SelectListItem> BindAllowanceType(string alltypeid)
        {
            try
            {
                DataTable dtDesg = AssignAllowanceService.GetAllowanceType(alltypeid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ALLOWANCE_TYPE"].ToString(), Value = dtDesg.Rows[i]["ALLOWANCE_TYPE"].ToString() });


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
