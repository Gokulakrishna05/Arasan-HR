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


namespace Arasan.Controllers.Master
{
    public class EmpShiftScheduleController : Controller
    {
        IEmpShiftSchedule Shift;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public EmpShiftScheduleController(IEmpShiftSchedule _Shift, IConfiguration _configuratio)
        {
            Shift = _Shift;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult EmpShiftSchedule(String id)
        {
            EmpShiftScheduleModel A = new EmpShiftScheduleModel();

            A.MonthLst = BindMonth();
            A.EmpCategoryLst = BindCategory();
            A.DepLst = BindDep();
            A.Branch = Request.Cookies["BranchId"];


            List<EmployeeShift> TData = new List<EmployeeShift>();
            EmployeeShift tda = new EmployeeShift();


            DataTable dtv = datatrans.GetSequence("SEntr");
            if (dtv.Rows.Count > 0)
            {
                A.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new EmployeeShift();
                    //tda.EmplIdlst = BindEmplId();
                    tda.Shiftlst = BindShift();
                    tda.WOFFlst = BindWOFF();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = Shift.GetShiftScheduleBasicEdit(id);
                if (dt.Rows.Count > 0)
                {
                    A.DocId = dt.Rows[0]["DOCID"].ToString();
                    A.Month = dt.Rows[0]["MONTH"].ToString();
                    A.EmpCategory = dt.Rows[0]["PAYCATEGORY"].ToString();
                    A.Dep = dt.Rows[0]["DEPARTMENT"].ToString();

                }
                DataTable dt2 = new DataTable();

                dt2 = Shift.GetShiftScheduleDetailEdit(id);


                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new EmployeeShift();
                        //tda.EmplIdlst = BindEmplId();
                        tda.Shiftlst = BindShift();
                        tda.WOFFlst = BindWOFF();
                        tda.Isvalid = "Y";
                      
                        //tda.EmplID = dt2.Rows[0]["EMPLID"].ToString();
                        tda.empid = dt2.Rows[0]["EMPID"].ToString();
                        tda.empname = dt2.Rows[0]["EMPNAME"].ToString();
                        tda.StartDate = dt2.Rows[0]["STARTDATE"].ToString();
                        tda.EndDate = dt2.Rows[0]["ENDDATE"].ToString();
                        tda.ShVal = dt2.Rows[0]["SHVALL"].ToString();
                        tda.Shift = dt2.Rows[0]["SHIFT"].ToString();
                        tda.StTime = dt2.Rows[0]["STTIME"].ToString();
                        tda.EndTime = dt2.Rows[0]["ENDTIME"].ToString();
                        tda.WOFF = dt2.Rows[0]["WOFF"].ToString();
                        TData.Add(tda);

                    }


                }


            }
            A.EmpShiftSchedulelist = TData;

          
            return View(A);
        }



        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = Shift.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("EmpShiftSchedulelist");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("EmpShiftSchedulelist");
            }

        }

        public IActionResult ViewEmployeeShift(string id)
        {
            EmpShiftScheduleModel A = new EmpShiftScheduleModel();

            List<EmployeeShift> TData = new List<EmployeeShift>();
            EmployeeShift tda = new EmployeeShift();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select EMPSHIFTBASICID,EMPSHIFTBASIC.DOCID,MONTH,PCBASIC.PAYCATEGORY,DDBASIC.DEPTCODE from EMPSHIFTBASIC left outer join PCBASIC ON PCBASICID=EMPSHIFTBASIC.PAYCATEGORY left outer join DDBASIC ON DDBASICID=EMPSHIFTBASIC.DEPARTMENT  WHERE EMPSHIFTBASICID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                A.ID = dt.Rows[0]["EMPSHIFTBASICID"].ToString();
                A.DocId = dt.Rows[0]["DOCID"].ToString();
                A.Month = dt.Rows[0]["MONTH"].ToString();
                A.EmpCategory = dt.Rows[0]["PAYCATEGORY"].ToString();
                A.Dep = dt.Rows[0]["DEPTCODE"].ToString();


            }
            DataTable dtt = new DataTable();

            dtt = datatrans.GetData("Select EMPSHIFTDETAILID,EMPMAST.EMPID EMP,EMPLID,EMPSHIFTDETAIL.EMPID,EMPSHIFTDETAIL.EMPNAME,to_char(STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(ENDDATE,'dd-MON-yyyy')ENDDATE,SHVALL,SHIFTMAST.SHIFTNO,STTIME,ENDTIME,EMPSHIFTDETAIL.WOFF from  EMPSHIFTDETAIL left outer join EMPMAST ON EMPMASTID=EMPSHIFTDETAIL.EMPLID left outer join SHIFTMAST ON SHIFTMASTID=EMPSHIFTDETAIL.SHIFT  Where EMPSHIFTBASICID='" + id + "'");

            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {



                    tda = new EmployeeShift();
                    //tda.EmplID = dtt.Rows[0]["EMP"].ToString();
                    tda.empid = dtt.Rows[0]["EMPID"].ToString();
                    tda.empname = dtt.Rows[0]["EMPNAME"].ToString();
                    tda.StartDate = dtt.Rows[0]["STARTDATE"].ToString();
                    tda.EndDate = dtt.Rows[0]["ENDDATE"].ToString();
                    tda.ShVal = dtt.Rows[0]["SHVALL"].ToString();
                    tda.Shift = dtt.Rows[0]["SHIFTNO"].ToString();
                    tda.StTime = dtt.Rows[0]["STTIME"].ToString();
                    tda.EndTime = dtt.Rows[0]["ENDTIME"].ToString();
                    tda.WOFF = dtt.Rows[0]["WOFF"].ToString();
                    //tda.EmplIdlst = BindEmplId();
                    tda.Shiftlst = BindShift();
                    tda.WOFFlst = BindWOFF();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            A.EmpShiftSchedulelist = TData;
            return View(A);

        }

        public ActionResult GetEmployeeDetails(string suppid)
        {
            try
            {
                EmpShiftScheduleModel ca = new EmpShiftScheduleModel();
                List<EmployeeShift> TData = new List<EmployeeShift>();
                EmployeeShift dta = new EmployeeShift();
                //string grnno = "";
                //string grnamt = "";
                DataTable dtPR = new DataTable();

                dtPR = Shift.GetEmployeeDetail(suppid);

                if (dtPR.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPR.Rows.Count; i++)
                    {
                        dta = new EmployeeShift();
                        dta.empid = dtPR.Rows[i]["EMPID"].ToString();
                        dta.empname = dtPR.Rows[i]["EMPNAME"].ToString();
                        dta.Isvalid = "Y";
                        TData.Add(dta);
                    }
                }
                ca.EmpShiftSchedulelist = TData;
                return Json(ca.EmpShiftSchedulelist);
                //var result = new { grnno = grnno, grnamt = grnamt };
                //return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
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
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = Shift.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PAYCATEGORY"].ToString(), Value = dtDesg.Rows[i]["PCBASICID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SelectListItem> BindDep()
        {
            try
            {
                DataTable dtDesg = Shift.GetDep();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPTNAME"].ToString(), Value = dtDesg.Rows[i]["DDBASICID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public List<SelectListItem> BindEmplId()
        //{
        //    try
        //    {
        //        DataTable dtDesg = Shift.GetEmplId();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["empid"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = Shift.GetShift();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SelectListItem> BindWOFF()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "SUNDAY", Value = "SUNDAY" });
                lstdesg.Add(new SelectListItem() { Text = "MONDAY", Value = "MONDAY" });
                lstdesg.Add(new SelectListItem() { Text = "TUESDAY", Value = "TUESDAY" });
                lstdesg.Add(new SelectListItem() { Text = "WEDNESDAY", Value = "WEDNESDAY" });
                lstdesg.Add(new SelectListItem() { Text = "THURSDAY", Value = "THURSDAY" });
                lstdesg.Add(new SelectListItem() { Text = "FRIDAY", Value = "FRIDAY" });
                lstdesg.Add(new SelectListItem() { Text = "SATUREDAY", Value = "SATUREDAY" });
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public JsonResult GetEmpShiftJSON()
        //{
          
        //    return Json(BindEmplId());
        //}
        public JsonResult GetEmpShift2JSON()
        {

            return Json(BindShift());
        }
        public JsonResult GetEmpShift3JSON()
        {

            return Json(BindWOFF());
        }
        //public ActionResult GetEmpDetails(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        string emp = "";
        //        string dep = "";

        //        dt = datatrans.GetData("SELECT EMPNAME,EMPID FROM EMPMAST WHERE EMPMASTID='" + ItemId + "'");

        //        if (dt.Rows.Count > 0)
        //        {

        //            emp = dt.Rows[0]["EMPNAME"].ToString();
        //            dep = dt.Rows[0]["EMPID"].ToString();

        //        }

        //        var result = new { emp = emp, dep = dep };
        //        return Json(result);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
       
        public IActionResult EmpShiftSchedulelist()
        {
            return View();
        }

        public ActionResult MyListEmpShiftSchedulegrid(string strStatus)
        {
            List<EmployeeShifScheduletList> Reg = new List<EmployeeShifScheduletList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Shift.GetAllEmpShift(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=EmpShiftSchedule?id=" + dtUsers.Rows[i]["EMPSHIFTBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewEmployeeShift?id=" + dtUsers.Rows[i]["EMPSHIFTBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["EMPSHIFTBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["EMPSHIFTBASICID"].ToString() + "";
                }

                Reg.Add(new EmployeeShifScheduletList
                {
                    id = dtUsers.Rows[i]["EMPSHIFTBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    month = dtUsers.Rows[i]["Month"].ToString(),
                    category = dtUsers.Rows[i]["PAYCATEGORY"].ToString(),
                    department = dtUsers.Rows[i]["DEPTCODE"].ToString(),
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
        [HttpPost]
        public ActionResult EmpShiftSchedule(EmpShiftScheduleModel E, string id)
        {
            try
            {
                E.ID = id;
                string Strout = Shift.GetInsEmp(E);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (E.ID == null)
                    {
                        TempData["notice"] = "Employee Shift Schedule Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Employee Shift Schedule Updated Successfully...!";
                    }
                    return RedirectToAction("EmpShiftSchedulelist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit EmpShiftSchedulelist";

                    return View(E);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
