
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
    public class EmployeeAttendanceDetails : Controller
    {

        IEmployeeAttendanceDetails Employee;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public EmployeeAttendanceDetails(IEmployeeAttendanceDetails _Employee, IConfiguration _configuratio)
        {
            Employee = _Employee;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult EmployeeAttendanceDetail(string id)
        {

            EmployeeAtttendanceDetailsModel A = new EmployeeAtttendanceDetailsModel();

            List<AttendanceDetails> TData = new List<AttendanceDetails>();
            AttendanceDetails tda = new AttendanceDetails();


            DataTable dtv = datatrans.GetSequence("MISP");
            if (dtv.Rows.Count > 0)
            {
                A.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new AttendanceDetails();
                    tda.Itemlst = BindEmp();
                    tda.Itemlsts = BindInOut();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
 
                dt = Employee.GetEmployeeAttendanceBasicEdit(id);
                if (dt.Rows.Count > 0)
                {
                    A.DocId = dt.Rows[0]["DOCID"].ToString();
                    A.Docdate = dt.Rows[0]["DOCDATE"].ToString();

                }
                DataTable dtt = new DataTable();
 
                dtt = Employee.GetEmployeeAttendanceDetailEdit(id);
                 

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tda = new AttendanceDetails();
                        tda.Itemlst = BindEmp();
                        tda.Itemlsts = BindInOut();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                        tda.EmpID = dtt.Rows[0]["EMPID"].ToString();
                        tda.EmpName = dtt.Rows[0]["EMPNAME"].ToString();
                        tda.Depart = dtt.Rows[0]["DEPARTMENT"].ToString();
                        tda.InOut = dtt.Rows[0]["INOUT"].ToString();
                        tda.Time = dtt.Rows[0]["TIME"].ToString();

                    }
                   

                }


            }

            A.EmployeeAttendanceDetailslist = TData;
            
            return View(A);
            
        }


         
        public IActionResult EmployeeAttendancelist()
        {
            return View();
        }
      



        public ActionResult MyListEmployeeAttendancegrid(string strStatus)
        {
            List<EmployeeAttendanceList> Reg = new List<EmployeeAttendanceList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Employee.GetAllEmployeeDetail(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=EmployeeAttendanceDetail?id=" + dtUsers.Rows[i]["EMPMISSINGPUNCHBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    ViewRow = "<a href=ViewEmployeeAttendanceDetails?id=" + dtUsers.Rows[i]["EMPMISSINGPUNCHBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["EMPMISSINGPUNCHBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["EMPMISSINGPUNCHBASICID"].ToString() + "";
                }




                Reg.Add(new EmployeeAttendanceList
                {
                    id = dtUsers.Rows[i]["EMPMISSINGPUNCHBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),

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

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = Employee.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("EmployeeAttendancelist");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("EmployeeAttendancelist");
            }

        }

        public IActionResult ViewEmployeeAttendanceDetails(string id)
        {
            EmployeeAtttendanceDetailsModel Emp = new EmployeeAtttendanceDetailsModel();

            List<AttendanceDetails> TData = new List<AttendanceDetails>();
            AttendanceDetails tda = new AttendanceDetails();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select EMPMISSINGPUNCHBASICID,DOCID,DOCDATE from EMPMISSINGPUNCHBASIC WHERE EMPMISSINGPUNCHBASICID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                Emp.ID = dt.Rows[0]["EMPMISSINGPUNCHBASICID"].ToString();
                Emp.DocId = dt.Rows[0]["DOCID"].ToString();
                Emp.Docdate = dt.Rows[0]["DOCDATE"].ToString();



            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("SELECT EMPMAST.EMPID,EMPMISSINGPUNCHDETAIL.EMPNAME,DEPARTMENT,INOUT,TIME from EMPMISSINGPUNCHDETAIL  left outer join EMPMAST ON EMPMASTID=EMPMISSINGPUNCHDETAIL.EMPID  Where EMPMISSINGPUNCHBASICID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {



                    tda = new AttendanceDetails();
                    tda.EmpID = dt2.Rows[0]["EMPID"].ToString();
                    tda.EmpName = dt2.Rows[0]["EMPNAME"].ToString();
                    tda.Depart = dt2.Rows[0]["DEPARTMENT"].ToString();
                    tda.InOut = dt2.Rows[0]["INOUT"].ToString();
                    tda.Time = dt2.Rows[0]["TIME"].ToString();
                    tda.Itemlst = BindEmp();
                    tda.Itemlsts = BindInOut();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            Emp.EmployeeAttendanceDetailslist = TData;
            return View(Emp);

        }


            public JsonResult GetEmpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindEmp());
        }

        public JsonResult GetEmpsJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindInOut());
        }
        public List<SelectListItem> BindEmp()
            {
                try
                {
                DataTable dtDesg = Employee.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                        
                      
                    }
                    return lstdesg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        public List<SelectListItem> BindInOut()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "In", Value = "In" });
                lstdesg.Add(new SelectListItem() { Text = "Out", Value = "Out" });
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

                string emp = "";
                string dep = "";

                dt = datatrans.GetData("SELECT EMPNAME,DDBASIC.DEPTNAME FROM EMPMAST left outer join DDBASIC ON DDBASICID=EMPMAST.EMPDEPT WHERE EMPMASTID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    emp = dt.Rows[0]["EMPNAME"].ToString();
                    dep = dt.Rows[0]["DEPTNAME"].ToString();

                }

                var result = new { emp = emp,dep = dep };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        [HttpPost]
        public ActionResult EmployeeAttendanceDetail(EmployeeAtttendanceDetailsModel E, string id)
        {

          
            try
            {
                E.ID = id;
                string Strout = Employee.GetInsEmp(E);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (E.ID == null)
                    {
                        TempData["notice"] = "Employee Attendance Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Employee Attendance Updated Successfully...!";
                    }
                    return RedirectToAction("EmployeeAttendancelist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit EmployeeAttendanceDetail";

                    return View(E);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(E);

        }



    }
}
