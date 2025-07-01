using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


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
            ic.EmpNamelst = BindEmpName();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = Person.GetPermissionsEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.PID = dt.Rows[0]["PERMISSIONID"].ToString();
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMPLOYEEID"].ToString();
                    ic.PerDate = dt.Rows[0]["PERMISSIONDATE"].ToString();
                    ic.FromTime = dt.Rows[0]["FROMTIME"].ToString();
                    ic.ToTime = dt.Rows[0]["TOTIME"].ToString();
                    ic.Reason = dt.Rows[0]["REASON"].ToString();
                    
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Permissions(Permissions Cy, string id)
        {
            try
            {
                id = Cy.PID;
                string Strout = Person.PermissionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.PID == null)
                    {
                        TempData["notice"] = "Permission Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Permission Updated Successfully...!";
                    }
                    return RedirectToAction("PermissionsList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Permissions";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(Cy);

        }

        public IActionResult PermissionsList()
        {
            Permissions ic = new Permissions();
            return View(ic);
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
                    ViewRow = "<a href=ViewPermissions?id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "";

                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve" || dtUsers.Rows[i]["STATUS"].ToString() == "Reject")
                    {
                        EditRow = "";
                    }

                }
                else
                {
                    EditRow = "";
                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["PERMISSIONID"].ToString() + "";
                }
                Reg.Add(new PermissionsList
                {
                    pid = dtUsers.Rows[i]["PERMISSIONID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    perdate = dtUsers.Rows[i]["PERMISSIONDATE"].ToString(),
                    fromtime = dtUsers.Rows[i]["FROMTIME"].ToString(),
                    totime = dtUsers.Rows[i]["TOTIME"].ToString(),
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

        private List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = Person.GetEmpName();
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

        public IActionResult ViewPermissions(string id)
        {
            Permissions ic = new Permissions();
            ic.Status = "Approve";
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select PERMISSIONID,EMPMAST.EMPNAME,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,REASON,STATUS from PERMISSIONS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = PERMISSIONS.EMPLOYEEID WHERE PERMISSIONID='" + id + "'");
            
            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.PerDate = dt.Rows[0]["PERMISSIONDATE"].ToString();
                ic.FromTime = dt.Rows[0]["FROMTIME"].ToString();
                ic.ToTime = dt.Rows[0]["TOTIME"].ToString();
                ic.Reason = dt.Rows[0]["REASON"].ToString();
                ic.Status = dt.Rows[0]["STATUS"].ToString();
                ic.PID = id;
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult ViewPermissions(Permissions Cy, string id)
        {
            try
            {
                id = Cy.PID;
                string Strout = Person.ViewPermission(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.PID == null)
                    {
                        TempData["notice"] = "Permission Updated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Permission Updated Successfully...!";
                    }
                    return RedirectToAction("PermissionsList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Permission";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(Cy);
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


    }
}
