using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;



using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Arasan.Controllers
{
    public class OTEntryController : Controller
    {
        IOTEntry Entry;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public OTEntryController(IOTEntry _Entry, IConfiguration _configuratio)
        {
            Entry = _Entry;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult OTEntry(string id)
        {
            OTEntry ic = new OTEntry();
            ic.EmpNamelst = BindEmpName();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = Entry.GetOTEntryEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.PID = dt.Rows[0]["ID"].ToString();
                    ic.EmpNamelst = BindEmpName();
                    ic.EmpName = dt.Rows[0]["EMPLOYEE_NAME"].ToString();
                    ic.Date = dt.Rows[0]["OT_DATE"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                    ic.OTHours = dt.Rows[0]["OT_HOURS"].ToString();
                    ic.OTON = dt.Rows[0]["OT_PERFORMED_ON"].ToString(); 

                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult OTEntry(OTEntry Cy, string id)
        {
            try
            {
                id = Cy.PID;
                string Strout = Entry.OTEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.PID == null)
                    {
                        TempData["notice"] = "OTEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "OTEntry Updated Successfully...!";
                    }
                    return RedirectToAction("OTEntryList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit OTEntry";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(Cy);

        }
        public IActionResult OTEntryList()
        {
            OTEntry ic = new OTEntry();
            return View(ic);
        }

        public ActionResult MyListOTEntrygrid(string strStatus)
        {
            List<OTEntryList> Reg = new List<OTEntryList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Entry.GetAllOTEntry(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                string Approve = string.Empty;
                string Reject = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=OTEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewOTEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                    //Approve = "ApproveItem?tag=Entry&id=" + dtUsers.Rows[i]["ID"].ToString() + "";

                        if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve")
                    //if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve" || dtUsers.Rows[i]["STATUS"].ToString() == "Reject")
                    {
                        ViewRow = "";
                        ViewRow = "<a href=ViewOTEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                        Approve = "<a href=ApproveEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Approved.png' alt='Approved' /></a>";
                        //Reject = "<a href=RejectEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/rejected.png' alt='reject' /></a>";

                    }
                    else if (dtUsers.Rows[i]["STATUS"].ToString() == "Reject")
                    {
                        ViewRow = "";
                        ViewRow = "<a href=ViewOTEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                        Approve = "<a href=RejectEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/cancel.png' alt='Reject' /></a>";
                    }

                }
                else
                {
                    EditRow = "";
                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new OTEntryList
                {
                    pid = dtUsers.Rows[i]["ID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    date = dtUsers.Rows[i]["OT_DATE"].ToString(),
                    othours = dtUsers.Rows[i]["OT_PERFORMED_ON"].ToString(),
                    oton = dtUsers.Rows[i]["OT_HOURS"].ToString(),
                    editrow = EditRow,
                    viewrow = ViewRow,
                    approverow = Approve,
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
                DataTable dtDesg = Entry.GetEmpName();
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


        public IActionResult ViewOTEntry(string id)
        {
            OTEntry ic = new OTEntry();
            //ic.Status = "Approve";
            DataTable dt = new DataTable();
            //dt = datatrans.GetData("Select ID,EMPMAST.EMPNAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,FROMTIME,TOTIME,REASON,STATUS from PERMISSIONS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = PERMISSIONS.EMPLOYEEID WHERE PERMISSIONID='" + id + "'");
            dt = datatrans.GetData("Select ID,EMPMAST.EMPNAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS,STATUS from OTENTRY LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = OTENTRY.EMPLOYEE_NAME WHERE ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                ic.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                ic.Date = dt.Rows[0]["OT_DATE"].ToString();
                ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                ic.OTHours = dt.Rows[0]["OT_HOURS"].ToString();
                ic.OTON = dt.Rows[0]["OT_PERFORMED_ON"].ToString();
                ic.Status = dt.Rows[0]["STATUS"].ToString();
                ic.PID = id;
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult ViewOTEntry(OTEntry Cy, string id)
        {
            try
            {
                id = Cy.PID;
                string Strout = Entry.ViewOTEntry(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.PID == null)
                    {
                        TempData["notice"] = "OTEntry Updated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "OTEntry Updated Successfully...!";
                    }
                    return RedirectToAction("OTEntryList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit OTEntry";

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
            string flag = Entry.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("OTEntryList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("OTEntryList");
            }

        }

    }
}
