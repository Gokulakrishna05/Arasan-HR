using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection.PortableExecutable;
using Intuit.Ipp.Data;
using Arasan.Services;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.VisualBasic;
using Nest;
using System.Reflection;

namespace Arasan.Controllers
{
    public class EmpLoginDetController : Controller
    {
        IEmpLoginDet log;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public EmpLoginDetController(IEmpLoginDet _log, IConfiguration _configuratio)
        {
            log = _log;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult EmpLoginDet(String id)
        {
            EmpLoginDetModel L = new EmpLoginDetModel();
            L.MonthLst = BindMonth();
            L.PayCategoryLst = BindPayCategory();
            L.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");



            List<EmployeeLogin> TData = new List<EmployeeLogin>();
            EmployeeLogin tda = new EmployeeLogin();

            DataTable dtv = datatrans.GetSequence("attnp");
            if (dtv.Rows.Count > 0)
            {
                L.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new EmployeeLogin();
                    tda.EmpNamelst = BindEmpName();
                    tda.WeekOfflst = BindWeekoff();
                    tda.HAlst = BindHA();
                    tda.Statuslst = BindStatus();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = log.GetEmploginDetBasicEdit(id);
                if (dt.Rows.Count > 0)
                {
                    L.DocId = dt.Rows[0]["DOCID"].ToString();
                    L.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    L.AttDate = dt.Rows[0]["ATTDATE"].ToString();
                    L.Holiday = dt.Rows[0]["HOLIDAY"].ToString();
                    L.Month = dt.Rows[0]["MONTH"].ToString();
                    L.PayCategory = dt.Rows[0]["PAYCATEGORY"].ToString();

                }
                DataTable dt2 = new DataTable();

                dt2 = log.GetEmploginDetDetailEdit(id);


                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)  
                    {
                        tda = new EmployeeLogin();
                        tda.EmpNamelst = BindEmpName();
                        tda.WeekOfflst = BindWeekoff();
                        tda.HAlst = BindHA();
                        tda.Statuslst = BindStatus();
                        tda.Isvalid = "Y";

                        tda.EmpName = dt2.Rows[0]["EMPNAME"].ToString();
                        tda.Mission = dt2.Rows[0]["MISSION"].ToString();
                        tda.ODTaken = dt2.Rows[0]["ODTAKEN"].ToString();
                        tda.ODHrs = dt2.Rows[0]["ODHRS"].ToString();
                        tda.WeekOff = dt2.Rows[0]["WEEKOFF"].ToString();
                        tda.ShiftLoginDate = dt2.Rows[0]["SHIFTLOGINDATE"].ToString();
                        tda.ShiftLoginTime = dt2.Rows[0]["SHIFTLOGINTIME"].ToString();
                        tda.LoginDate = dt2.Rows[0]["LOGINDATE"].ToString();
                        tda.LoginTime = dt2.Rows[0]["LOGINTIME"].ToString();
                        tda.ShiftLogoutDate = dt2.Rows[0]["SHIFTLOGOUTDATE"].ToString();
                        tda.ShiftLogoutTime = dt2.Rows[0]["SHIFTLOGOUTTIME"].ToString();
                        tda.LogoutDate = dt2.Rows[0]["LOGOUTDATE"].ToString();
                        tda.LogoutTime = dt2.Rows[0]["LOGOUTTIME"].ToString();
                        tda.IntDiff = dt2.Rows[0]["INTDIFF"].ToString();
                        tda.OuttDiff = dt2.Rows[0]["OUTTDIFF"].ToString();
                        tda.TimeDiff = dt2.Rows[0]["TIMEDIFF"].ToString();
                        tda.rmission = dt2.Rows[0]["RMISSION"].ToString();
                        tda.WHrs1 = dt2.Rows[0]["WHRS1"].ToString();
                        tda.WHrs2 = dt2.Rows[0]["WHRS2"].ToString();
                        tda.WorkedHrs = dt2.Rows[0]["WORKEDHOURS"].ToString();
                        tda.HA = dt2.Rows[0]["HA"].ToString();
                        tda.OTHrs = dt2.Rows[0]["OTHRS"].ToString();
                        tda.ensation = dt2.Rows[0]["ENSATION"].ToString();
                        tda.Status = dt2.Rows[0]["STATUS"].ToString();
                        TData.Add(tda);

                    }

                }

            }
            L.EmpLoginDetlists = TData;


            return View(L);
        }
        public IActionResult EmpLoginDetlist()
        {
            return View();
        }
        public List<SelectListItem> BindMonth()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "JAN2023", Value = "JAN2023" });
                lstdesg.Add(new SelectListItem() { Text = "FEB2023", Value = "FEB2023" });
                lstdesg.Add(new SelectListItem() { Text = "MAR2023", Value = "MAR2023" });
                lstdesg.Add(new SelectListItem() { Text = "APR2023", Value = "APR2023" });
                lstdesg.Add(new SelectListItem() { Text = "MAY2023", Value = "MAY2023" });
                lstdesg.Add(new SelectListItem() { Text = "JUN2023", Value = "JUN2023" });
                lstdesg.Add(new SelectListItem() { Text = "JUL2023", Value = "JUL2023" });
                lstdesg.Add(new SelectListItem() { Text = "AUG2023", Value = "AUG2023" });
                lstdesg.Add(new SelectListItem() { Text = "SEP2023", Value = "SEP2023" });
                lstdesg.Add(new SelectListItem() { Text = "OCT2023", Value = "OCT2023" });
                lstdesg.Add(new SelectListItem() { Text = "NOV2023", Value = "NOV2023" });
                lstdesg.Add(new SelectListItem() { Text = "DEC2023", Value = "DEC2023" });
                lstdesg.Add(new SelectListItem() { Text = "JAN2024", Value = "JAN2024" });
                lstdesg.Add(new SelectListItem() { Text = "FEB2024", Value = "FEB2024" });
                lstdesg.Add(new SelectListItem() { Text = "MAR2024", Value = "MAR2024" });
                lstdesg.Add(new SelectListItem() { Text = "APR2024", Value = "APR2024" });
                lstdesg.Add(new SelectListItem() { Text = "MAY2024", Value = "MAY2024" });
                lstdesg.Add(new SelectListItem() { Text = "JUN2024", Value = "JUN2024" });
                lstdesg.Add(new SelectListItem() { Text = "JUL2024", Value = "JUL2024" });
                lstdesg.Add(new SelectListItem() { Text = "AUG2024", Value = "AUG2024" });
                lstdesg.Add(new SelectListItem() { Text = "SEP2024", Value = "SEP2024" });
                lstdesg.Add(new SelectListItem() { Text = "OCT2024", Value = "OCT2024" });
                lstdesg.Add(new SelectListItem() { Text = "NOV2024", Value = "NOV2024" });
                lstdesg.Add(new SelectListItem() { Text = "DEC2024", Value = "DEC2024" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPayCategory()
        {
            try
            {
                DataTable dtDesg = log.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PAYCATEGORY"].ToString(), Value = dtDesg.Rows[i]["PCBASICID"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = log.GetEmpName();
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
        public List<SelectListItem> BindWeekoff()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Y", Value = "Y" });
                lstdesg.Add(new SelectListItem() { Text = "N", Value = "N" });
               
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindHA()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "T", Value = "T" });
                lstdesg.Add(new SelectListItem() { Text = "F", Value = "F" });
               
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "F", Value = "F" });
                lstdesg.Add(new SelectListItem() { Text = "H", Value = "H" });
                lstdesg.Add(new SelectListItem() { Text = "L", Value = "L" });
                lstdesg.Add(new SelectListItem() { Text = "A", Value = "A" });
                lstdesg.Add(new SelectListItem() { Text = "W", Value = "W" });
                lstdesg.Add(new SelectListItem() { Text = "O", Value = "O" });
                lstdesg.Add(new SelectListItem() { Text = "W/O", Value = "W/O" });
                lstdesg.Add(new SelectListItem() { Text = "NH", Value = "NH" });
                lstdesg.Add(new SelectListItem() { Text = "OD", Value = "OD" });
                lstdesg.Add(new SelectListItem() { Text = "NH/OT", Value = "NH/OT" });
                lstdesg.Add(new SelectListItem() { Text = "NH/W", Value = "NH/W" });
                lstdesg.Add(new SelectListItem() { Text = "LEFT", Value = "LEFT" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListEmpLogindetgrid(string strStatus)
        {
            List<EmployeeLoginDet> Reg = new List<EmployeeLoginDet>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = log.GetAllEmpLog(strStatus); //Join for list page GetAllEmpLog in server

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=EmpLoginDet?id=" + dtUsers.Rows[i]["EMPLOGINDETBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    ViewRow = "<a href=ViewEmployeeLoginDet?id=" + dtUsers.Rows[i]["EMPLOGINDETBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["EMPLOGINDETBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["EMPLOGINDETBASICID"].ToString() + "";
                }




                Reg.Add(new EmployeeLoginDet
                {
                    id = dtUsers.Rows[i]["EMPLOGINDETBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    attdate = dtUsers.Rows[i]["ATTDATE"].ToString(),
                    holiday = dtUsers.Rows[i]["HOLIDAY"].ToString(),
                    month = dtUsers.Rows[i]["MONTH"].ToString(),
                    paycategory = dtUsers.Rows[i]["PAYCATEGORY"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    rrow = Regenerate
                });
            }
            return Json(new
            {
                Reg
            });
        }
        [HttpPost]
        public ActionResult EmpLoginDet(EmpLoginDetModel E, string id)
        {


            try
            {
                E.ID = id;
                string Strout = log.GetInslog(E);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (E.ID == null)
                    {
                        TempData["notice"] = "Employee Login Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Employee Login Updated Successfully...!";
                    }
                    return RedirectToAction("EmpLoginDetlist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit EmpLoginDetlist";

                    return View(E);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(E);

        }
        public JsonResult GetEmpShiftJSON()
        {

            return Json(BindEmpName());
        }
        public JsonResult GetEmpShift2JSON()
        {

            return Json(BindWeekoff());
        }
        public JsonResult GetEmpShift3JSON()
        {

            return Json(BindHA());
        }
        public JsonResult GetEmpShift4JSON()
        {

            return Json(BindStatus());
        }
        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = log.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("EmpLoginDetlist");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("EmpLoginDetlist");
            }

        }

        public IActionResult ViewEmployeeLoginDet(string id)
        {
            EmpLoginDetModel L = new EmpLoginDetModel();

            List<EmployeeLogin> TData = new List<EmployeeLogin>();
            EmployeeLogin tda = new EmployeeLogin();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select EMPLOGINDETBASICID,EMPLOGINDETBASIC.DOCID,to_char(EMPLOGINDETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(ATTDATE,'dd-MON-yyyy')ATTDATE,HOLIDAY,EMPLOGINDETBASIC.MONTH,PCBASIC.PAYCATEGORY from EMPLOGINDETBASIC left outer join PCBASIC ON PCBASICID=EMPLOGINDETBASIC.PAYCATEGORY WHERE EMPLOGINDETBASICID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                L.ID = dt.Rows[0]["EMPLOGINDETBASICID"].ToString();
                L.DocId = dt.Rows[0]["DOCID"].ToString();
                L.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                L.AttDate = dt.Rows[0]["ATTDATE"].ToString();
                L.Holiday = dt.Rows[0]["HOLIDAY"].ToString();
                L.Month = dt.Rows[0]["MONTH"].ToString();
                L.PayCategory = dt.Rows[0]["PAYCATEGORY"].ToString();
 

            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("Select EMPLOGINDETDETAILID,EMPLOGINDETBASICID,EMPMAST.EMPNAME,MISSION,ODTAKEN,ODHRS,WEEKOFF,to_char(SHIFTLOGINDATE,'dd-MON-yyyy')SHIFTLOGINDATE,SHIFTLOGINTIME,to_char(LOGINDATE,'dd-MON-yyyy')LOGINDATE,LOGINTIME,to_char(SHIFTLOGOUTDATE,'dd-MON-yyyy')SHIFTLOGOUTDATE,SHIFTLOGOUTTIME,to_char(LOGOUTDATE,'dd-MON-yyyy')LOGOUTDATE,LOGOUTTIME,INTDIFF,OUTTDIFF,TIMEDIFF,RMISSION,WHRS1,WHRS2,WORKEDHOURS,HA,OTHRS,ENSATION,STATUS from  EMPLOGINDETDETAIL  left outer join EMPMAST ON EMPMASTID=EMPLOGINDETDETAIL.EMPNAME WHERE EMPLOGINDETBASICID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {



                    tda = new EmployeeLogin();
                    tda.EmpNamelst = BindEmpName();
                    tda.WeekOfflst = BindWeekoff();
                    tda.HAlst = BindHA();
                    tda.Statuslst = BindStatus();
                    tda.Isvalid = "Y";

                    tda.EmpName = dt2.Rows[0]["EMPNAME"].ToString();
                    tda.Mission = dt2.Rows[0]["MISSION"].ToString();
                    tda.ODTaken = dt2.Rows[0]["ODTAKEN"].ToString();
                    tda.ODHrs = dt2.Rows[0]["ODHRS"].ToString();
                    tda.WeekOff = dt2.Rows[0]["WEEKOFF"].ToString();
                    tda.ShiftLoginDate = dt2.Rows[0]["SHIFTLOGINDATE"].ToString();
                    tda.ShiftLoginTime = dt2.Rows[0]["SHIFTLOGINTIME"].ToString();
                    tda.LoginDate = dt2.Rows[0]["LOGINDATE"].ToString();
                    tda.LoginTime = dt2.Rows[0]["LOGINTIME"].ToString();
                    tda.ShiftLogoutDate = dt2.Rows[0]["SHIFTLOGOUTDATE"].ToString();
                    tda.ShiftLogoutTime = dt2.Rows[0]["SHIFTLOGOUTTIME"].ToString();
                    tda.LogoutDate = dt2.Rows[0]["LOGOUTDATE"].ToString();
                    tda.LogoutTime = dt2.Rows[0]["LOGOUTTIME"].ToString();
                    tda.IntDiff = dt2.Rows[0]["INTDIFF"].ToString();
                    tda.OuttDiff = dt2.Rows[0]["OUTTDIFF"].ToString();
                    tda.TimeDiff = dt2.Rows[0]["TIMEDIFF"].ToString();
                    tda.rmission = dt2.Rows[0]["RMISSION"].ToString();
                    tda.WHrs1 = dt2.Rows[0]["WHRS1"].ToString();
                    tda.WHrs2 = dt2.Rows[0]["WHRS2"].ToString();
                    tda.WorkedHrs = dt2.Rows[0]["WORKEDHOURS"].ToString();
                    tda.HA = dt2.Rows[0]["HA"].ToString();
                    tda.OTHrs = dt2.Rows[0]["OTHRS"].ToString();
                    tda.ensation = dt2.Rows[0]["ENSATION"].ToString();
                    tda.Status = dt2.Rows[0]["STATUS"].ToString();
                    TData.Add(tda);

                }
            }
            L.EmpLoginDetlists = TData;
            return View(L);

        }



    }
}
