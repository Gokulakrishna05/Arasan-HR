using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class PayPeriodController : Controller
    {
        IPayPeriodService payperiodService;
         IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public PayPeriodController(IPayPeriodService _payperiodService, IConfiguration _configuratio)
        {
            payperiodService = _payperiodService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PayPeriod(string id)
        {
            PayPeriod py = new PayPeriod();
            py.Set = DateTime.Now.ToString("dd-MMM-yyyy");
            py.PPLists = BindPayPeriodType();
            List<Pay> TData = new List<Pay>();
            Pay tda = new Pay();

            DataTable dtv = datatrans.GetSequence("paypr");
            if (dtv.Rows.Count > 0)
            {
                py.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new Pay();
                   
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = payperiodService.GetPayPeriod(id);
                if (dt.Rows.Count > 0)
                {

                    py.DocId = dt.Rows[0]["DOCID"].ToString();
                    py.Set = dt.Rows[0]["DOCDATE"].ToString();
                    py.PayPeriodType = dt.Rows[0]["PAYPERIODTYPE"].ToString();
                    py.StartingDate = dt.Rows[0]["STARTINGDATE"].ToString();
                    py.EndingDate = dt.Rows[0]["ENDINGDATE"].ToString();
                    py.SalaryDate = dt.Rows[0]["SALDATE"].ToString();

                    py.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = payperiodService.GetEditPayPeriod(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new Pay();

                    tda.PayPeriods = dt2.Rows[i]["PAYPERIOD"].ToString();
                    tda.StartsAt = dt2.Rows[i]["STARTSAT"].ToString();
                    tda.EndsAt = dt2.Rows[i]["ENDSAT"].ToString();
                    tda.SalaryDate = dt2.Rows[i]["SALARYDATE"].ToString();
                    tda.PayPeriodDays = dt2.Rows[i]["PAYPERIODDAYS"].ToString();
                    tda.WeeklyHolidays = dt2.Rows[i]["NOOFWEEKLYHOLIDAYS"].ToString();
                    tda.MonthlyHolidays = dt2.Rows[i]["MONTHLYHOLIDAYS"].ToString();
                    tda.OtherHols = dt2.Rows[i]["OTHERHOLS"].ToString();
                    tda.WorkingDays = dt2.Rows[i]["WORKINGDAYS"].ToString();
                     tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            py.PayLists = TData;
            return View(py);
        }
        public List<SelectListItem> BindPayPeriodType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MONTHLY", Value = "MONTHLY" });
                lstdesg.Add(new SelectListItem() { Text = "WEEKLY", Value = "WEEKLY" });
                lstdesg.Add(new SelectListItem() { Text = "FORTNIGHT", Value = "FORTNIGHT" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = payperiodService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayPeriod");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPayPeriod");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = payperiodService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayPeriod");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPayPeriod");
            }
        }
        public JsonResult GetPayPeriodJSON()
        {
            Pay M = new Pay();

            return Json(BindPayPeriodType());
        }

        [HttpPost]

        public ActionResult PayPeriod(PayPeriod by, string id)
        {

            try
            {
                by.ID = id;
                string Strout = payperiodService.PayPeriodCRUD(by);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (by.ID == null)
                    {
                        TempData["notice"] = "PayPeriod Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PayPeriod Updated Successfully...!";
                    }
                    return RedirectToAction("ListPayPeriod");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PayPeriod";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(by);
        }
        public IActionResult ViewPayPeriod(string id, List<Pay> data)
        {
            PayPeriod M = new PayPeriod();
            List<Pay> TData = new List<Pay>();
            Pay tda = new Pay();

            DataTable dt = new DataTable();


            dt = datatrans.GetData("Select  DOCID,to_char (DOCDATE,'dd-MON-yyyy')DOCDATE,PAYPERIODTYPE,to_char (STARTINGDATE,'dd-MON-yyyy')STARTINGDATE,to_char (ENDINGDATE,'dd-MON-yyyy')ENDINGDATE,to_char (SALDATE,'dd-MON-yyyy')SALDATE from PPBASIC  WHERE PPBASICID='" + id + "'");


            if (dt.Rows.Count > 0)
            {
                M.DocId = dt.Rows[0]["DOCID"].ToString();
                M.Set = dt.Rows[0]["DOCDATE"].ToString();
                M.PayPeriodType = dt.Rows[0]["PAYPERIODTYPE"].ToString();
                M.StartingDate = dt.Rows[0]["STARTINGDATE"].ToString();
                M.EndingDate = dt.Rows[0]["ENDINGDATE"].ToString();
                M.SalaryDate = dt.Rows[0]["SALDATE"].ToString();



            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("Select  PPBASICID,PAYPERIOD,to_char (STARTSAT,'dd-MON-yyyy')STARTSAT, to_char (ENDSAT,'dd-MON-yyyy')ENDSAT, to_char (SALARYDATE,'dd-MON-yyyy')SALARYDATE, PAYPERIODDAYS, NOOFWEEKLYHOLIDAYS,MONTHLYHOLIDAYS,OTHERHOLS,WORKINGDAYS from PPDETAIL WHERE PPBASICID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {



                    tda = new Pay();

                    tda.PayPeriods = dt2.Rows[i]["PAYPERIOD"].ToString();
                    tda.StartsAt = dt2.Rows[i]["STARTSAT"].ToString();
                    tda.EndsAt = dt2.Rows[i]["ENDSAT"].ToString();
                    //tda.SalaryDate = BindAdd();
                    tda.SalaryDate = dt2.Rows[i]["SALARYDATE"].ToString();
                    tda.PayPeriodDays = dt2.Rows[i]["PAYPERIODDAYS"].ToString();
                    tda.WeeklyHolidays = dt2.Rows[i]["NOOFWEEKLYHOLIDAYS"].ToString();
                    tda.MonthlyHolidays = dt2.Rows[i]["MONTHLYHOLIDAYS"].ToString();
                    tda.OtherHols = dt2.Rows[i]["OTHERHOLS"].ToString();
                    tda.WorkingDays = dt2.Rows[i]["WORKINGDAYS"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                  
                }
            }


            M.PayLists = TData;

            return View(M);
        }
        public IActionResult ListPayPeriod()
        {
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<IPayPeriodGrid> Reg = new List<IPayPeriodGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = payperiodService.GetAlLPayPeriod(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=PayPeriod?id=" + dtUsers.Rows[i]["PPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                    ViewRow = "<a href=ViewPayPeriod?id=" + dtUsers.Rows[i]["PPBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";

                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PPBASICID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["PPBASICID"].ToString() + "";

                }

                Reg.Add(new IPayPeriodGrid
                {
                     docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    payperiodtype = dtUsers.Rows[i]["PAYPERIODTYPE"].ToString(),
                    startingdate = dtUsers.Rows[i]["STARTINGDATE"].ToString(),
                    endingdate = dtUsers.Rows[i]["ENDINGDATE"].ToString(),
                    saldate = dtUsers.Rows[i]["SALDATE"].ToString(),

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

    }
}
