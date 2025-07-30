using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class EmpAdvanceController : Controller
    {
        IEmpAdvance EmpAdvanceMast;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public EmpAdvanceController(IEmpAdvance _EmpAdvanceMast, IConfiguration _configuratio)
        {
            EmpAdvanceMast = _EmpAdvanceMast;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult EmpAdvance(String id)
        {
            EmpAdvance ic = new EmpAdvance();
            ic.EmpIDLst = BindEmpId();
            ic.AdvIDLst = BindAdvId();

            if (id == null)
            {

            }
            else
            { 
                DataTable dt = new DataTable();

                  dt = EmpAdvanceMast.GetEmpAdvanceEdit(id);
                  if (dt.Rows.Count > 0)
                  {
                     ic.EmpIDLst = BindEmpId();
                     ic.AdvIDLst = BindAdvId();
                     //ic.ID = dt.Rows[0]["ID"].ToString();
                     //ic.Adv = dt.Rows[0]["ADVANCE_ID"].ToString();
                     ic.Empe = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                     ic.AdvTp = dt.Rows[0]["ADVANCE_TYPE"].ToString();
                     ic.Advamt = dt.Rows[0]["ADVANCE_AMOUNT"].ToString();
                     ic.Emi = dt.Rows[0]["EMI_AMOUNT"].ToString();
                     ic.SMn = dt.Rows[0]["START_MONTH"].ToString();
                     ic.Emid = dt.Rows[0]["PAID_EMI_COUNT"].ToString();
                     ic.Rmks = dt.Rows[0]["REMARKS"].ToString();

                  }
            }
            return View(ic);

        }
        public IActionResult EmpAdvanceList()
        {
            return View();
        }
        public ActionResult MyListEmpAdvancegrid(string strStatus)
        {
            List<EmpAdvanceList> Res = new List<EmpAdvanceList>();

            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = EmpAdvanceMast.GetAllEmpAdvance(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=EmpAdvance?id=" + dtUsers.Rows[i]["ADVANCE_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewEmpAdvance?id=" + dtUsers.Rows[i]["ADVANCE_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ADVANCE_ID"].ToString() + "";

                    //DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Act&id=" + dtUsers.Rows[i]["ADVANCE_ID"].ToString() + "";

                }

                Res.Add(new EmpAdvanceList
                {
                    empe = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    //adv = dtUsers.Rows[i]["ADVANCE_ID"].ToString(),
                    dvTp = dtUsers.Rows[i]["ADVANCE_TYPE"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Res
            });

        }
        [HttpPost]
        public ActionResult EmpAdvance(EmpAdvance ic, string id)
        {
            try
            {
                ic.ID = id;
                string Strout = EmpAdvanceMast.EmpAdvanceCRUD(ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ic.ID == null)
                    {
                        TempData["notice"] = "EmpAdvance  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "EmpAdvance  Updated Successfully...!";
                    }
                    return RedirectToAction("EmpAdvanceList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit EmpAdvance";

                    return View(ic);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ic);

        }
        public List<SelectListItem> BindEmpId()
        {
            try
            {
                DataTable dtDesg = EmpAdvanceMast.GetEmpId();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindAdvId()
        {
            try
            {
                DataTable dtDesg = EmpAdvanceMast.GetAdvId();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ATYPE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

       

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = EmpAdvanceMast.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("EmpAdvanceList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("EmpAdvanceList");
            }

        }

        public IActionResult ViewEmpAdvance(string id)
        {
            EmpAdvance Emp = new EmpAdvance();

            DataTable dt = new DataTable();

            //dt = datatrans.GetData("SELECT HR_EMPLOYEE_ADVANCE.ADVANCE_ID AS ID, EMPMAST.EMPNAME, ADVTYPEMASTER.ATYPE AS ADVANCE_TYPE, HR_EMPLOYEE_ADVANCE.ADVANCE_AMOUNT, HR_EMPLOYEE_ADVANCE.EMI_AMOUNT,HR_EMPLOYEE_ADVANCE.to_char(START_MONTH,'dd-MON-yyyy')START_MONTH, HR_EMPLOYEE_ADVANCE.PAID_EMI_COUNT, HR_EMPLOYEE_ADVANCE.REMARKS FROM HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID LEFT OUTER JOIN ADVTYPEMASTER ON ADVTYPEMASTER.ID = HR_EMPLOYEE_ADVANCE.ADVANCE_TYPE WHERE HR_EMPLOYEE_ADVANCE.ADVANCE_ID = '" + id + "' ");

            dt = datatrans.GetData("SELECT HR_EMPLOYEE_ADVANCE.ADVANCE_ID AS ID, EMPMAST.EMPNAME, ADVTYPEMASTER.ATYPE AS ADVANCE_TYPE, HR_EMPLOYEE_ADVANCE.ADVANCE_AMOUNT, HR_EMPLOYEE_ADVANCE.EMI_AMOUNT,to_char(START_MONTH,'dd-MON-yyyy')START_MONTH, HR_EMPLOYEE_ADVANCE.PAID_EMI_COUNT, HR_EMPLOYEE_ADVANCE.REMARKS FROM HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID LEFT OUTER JOIN ADVTYPEMASTER ON ADVTYPEMASTER.ID = HR_EMPLOYEE_ADVANCE.ADVANCE_TYPE WHERE HR_EMPLOYEE_ADVANCE.ADVANCE_ID = '" + id + "' ");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                //Emp.ID = dt.Rows[0]["ID"].ToString();
                Emp.Empe = dt.Rows[0]["EMPNAME"].ToString();
                //Emp.Adv = dt.Rows[0]["ATYPE"].ToString();
               
                Emp.AdvTp = dt.Rows[0]["ADVANCE_TYPE"].ToString();
                Emp.Advamt = dt.Rows[0]["ADVANCE_AMOUNT"].ToString();
                Emp.Emi = dt.Rows[0]["EMI_AMOUNT"].ToString();
                Emp.SMn = dt.Rows[0]["START_MONTH"].ToString();
                Emp.Emid = dt.Rows[0]["PAID_EMI_COUNT"].ToString();
                Emp.Rmks = dt.Rows[0]["REMARKS"].ToString();
                Emp.ID = id;
            }
            return View(Emp);

        }
    }
}
