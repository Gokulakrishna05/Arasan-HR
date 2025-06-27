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
    public class PermissionsController : Controller
    {

        IPermissions Person;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PermissionsController(IPermissions _Person, IConfiguration _configuratio)
        {
            Person = _Person;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult Permissions(string id)
        {
            Permissions ic = new Permissions();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = Person.GetPermissionsEdit(id);
                if (dt.Rows.Count > 0)
                {
                    //ic.PID = dt.Rows[0]["PERMISSIONID"].ToString();
                    ic.EmpID = dt.Rows[0]["EMPLOYEEID"].ToString();
                    ic.PerDate = dt.Rows[0]["PERMISSIONDATE"].ToString();
                    ic.FTDate = dt.Rows[0]["FROMTIME"].ToString();
                    ic.TTDate = dt.Rows[0]["TOTIME"].ToString();
                    ic.Reason = dt.Rows[0]["REASON"].ToString();
                    ic.Remarks = dt.Rows[0]["REMARKS"].ToString();
                   // ic.ADDGDate = dt.Rows[0]["CREATEDBY"].ToString();
                    //ic.Mdate = dt.Rows[0]["MODIFIEDDATE"].ToString();
                    //ic.Mby = dt.Rows[0]["MODIFIEDBY"].ToString();

                }
            }
            return View(ic);
          
        }

        [HttpPost]
        public ActionResult Permissions(Permissions Em, string id)
        {


            try
            {
                id = Em.ID;
                string Strout = Person.GetPermi(Em);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Em.ID == null)
                    {
                        TempData["notice"] = "Permissions  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Permissions  Updated Successfully...!";
                    }
                    //return RedirectToAction("LeaveTypeMasterlist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Permissions";

                    return View(Em);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Em);

        }
        public IActionResult PermissionsList()
        {
            return View();
        }

        public ActionResult MyListPermissionsgrid(string strStatus)
        {
            List<PermissionsList> Reg = new List<PermissionsList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Person.GetAllPermissions(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Permissions?id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    ViewRow = "<a href=ViewPermissions?id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "";
                }


                Reg.Add(new PermissionsList
                {
                    id = dtUsers.Rows[i]["PERMISSIONID"].ToString(),
                    perdate = dtUsers.Rows[i]["PERMISSIONDATE"].ToString(),
                    ftdate = dtUsers.Rows[i]["FROMTIME"].ToString(),
                    ttdate = dtUsers.Rows[i]["TOTIME"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    //rrow = Regenerate
                });
            }

            return Json(new
            {
                Reg
            });



        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = Person.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("PermissionsList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("PermissionsList");
            }

        }

        public IActionResult ViewPermissions(string id)
        {
            Permissions Emp = new Permissions();

            //List<LeaveTypeMasterList> TData = new List<LeaveTypeMasterList>();
            // AttendanceDetails tda = new AttendanceDetails();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select PERMISSIONID,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME from PERMISSIONS WHERE PERMISSIONID='" + id + "'");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                Emp.ID = dt.Rows[0]["PERMISSIONID"].ToString();
                Emp.PerDate = dt.Rows[0]["PERMISSIONDATE"].ToString();
                Emp.FTDate = dt.Rows[0]["FROMTIME"].ToString();
                Emp.TTDate = dt.Rows[0]["TOTIME"].ToString();
                //Emp.ID = id;
            }
            return View(Emp);

        }

    }
}
