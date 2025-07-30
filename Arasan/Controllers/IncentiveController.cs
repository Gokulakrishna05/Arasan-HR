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
    public class IncentiveController : Controller
    {
        IIncentive IncentiveMast;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public IncentiveController(IIncentive _IncentiveMast, IConfiguration _configuratio)
        {
            IncentiveMast = _IncentiveMast;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult Incentive(String id)
        {
            Incentive ic = new Incentive();
            ic.EmpIDLst = BindEmpId();

            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();

                dt = IncentiveMast.GetIncentiveEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.EmpIDLst = BindEmpId();
                    //ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Emp = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                    ic.Des = dt.Rows[0]["DESIGNATION_ID"].ToString();
                    ic.Dpt = dt.Rows[0]["DEPARTMENT_ID"].ToString();
                    ic.Icem = dt.Rows[0]["INCENTIVE_NAME"].ToString();
                    ic.Ictpe = dt.Rows[0]["INCENTIVE_TYPE"].ToString();
                    ic.Amt = dt.Rows[0]["AMOUNT"].ToString();
                    ic.Rean = dt.Rows[0]["REASON"].ToString();

                }
               
            }
            return View(ic);
        }
    
        public IActionResult IncentiveList()
        {
            return View();
        }
        public ActionResult MyListIncentivegrid(string strStatus)
        {
            List<IncentiveList> Res = new List<IncentiveList>();

            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = IncentiveMast.GetAllIncentive(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=Incentive?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewIncentive?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                    //DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Act&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                }

                Res.Add(new IncentiveList
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    empid = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    desg = dtUsers.Rows[i]["DESIGNATION_ID"].ToString(),
                    dpt = dtUsers.Rows[i]["DEPARTMENT_ID"].ToString(),
                   
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

        public ActionResult Incentive(Incentive ic, string id)
        {
            try
            {
                ic.ID = id;
                string Strout = IncentiveMast.IncentiveCRUD(ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ic.ID == null)
                    {
                        TempData["notice"] = "Incentive  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Incentive  Updated Successfully...!";
                    }
                    return RedirectToAction("IncentiveList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Incentive";

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
                DataTable dtDesg = IncentiveMast.GetEmpId();
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
        public ActionResult GetEmpDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string empdn = "";
                string joindpt = "";

                dt = datatrans.GetData("SELECT EMPMAST.EMPDESIGN,DDBASIC.DEPTNAME FROM EMPMAST  LEFT OUTER JOIN DDBASIC ON DDBASIC.DDBASICID = EMPMAST.EMPDEPT WHERE EMPMASTID='" + ItemId + "' ");


                if (dt.Rows.Count > 0)
                {

                    empdn = dt.Rows[0]["EMPDESIGN"].ToString();
                    joindpt = dt.Rows[0]["DEPTNAME"].ToString();

                }

                var result = new { empdn = empdn, joindpt = joindpt };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = IncentiveMast.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("IncentiveList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("IncentiveList");
            }

        }

        public IActionResult ViewIncentive(string id)
        {
            Incentive Emp = new Incentive();

            DataTable dt = new DataTable();

            dt = datatrans.GetData(" Select ID,EMPMAST.EMPNAME,DESIGNATION_ID,DEPARTMENT_ID,INCENTIVE_NAME,INCENTIVE_TYPE,AMOUNT,REASON  from HR_INCENTIVE_MASTER LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_INCENTIVE_MASTER.EMPLOYEE_ID WHERE ID='" + id + "'  ");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                //Emp.ID = dt.Rows[0]["ID"].ToString();
                Emp.Emp = dt.Rows[0]["EMPNAME"].ToString();
                Emp.Des = dt.Rows[0]["DESIGNATION_ID"].ToString();
                Emp.Dpt = dt.Rows[0]["DEPARTMENT_ID"].ToString();
                Emp.Icem = dt.Rows[0]["INCENTIVE_NAME"].ToString();
                Emp.Ictpe = dt.Rows[0]["INCENTIVE_TYPE"].ToString();
                Emp.Amt = dt.Rows[0]["AMOUNT"].ToString();
                Emp.Rean = dt.Rows[0]["REASON"].ToString();
                Emp.ID = id;
            }
            return View(Emp);

        }

    }
}
