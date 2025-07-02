using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class LeaveRequestController : Controller
    {
        ILeaveRequest LeaveRequestService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public LeaveRequestController(ILeaveRequest _LeaveRequestService, IConfiguration _configuratio)
        {
            LeaveRequestService = _LeaveRequestService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult LeaveRequest(string id)
        {
            LeaveRequest ic = new LeaveRequest();
            ic.EmpLst = BindEmp();
            ic.LeaveTypeLst = BindLeaveType();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = LeaveRequestService.GetEditLeaveRequest(id);
                if (dt.Rows.Count > 0)
                {
                    ic.LeaveID = dt.Rows[0]["LEAVE_ID"].ToString();
                    ic.EmpLst = BindEmp();
                    ic.EmpID = dt.Rows[0]["EMP_ID"].ToString();
                    ic.LeaveTypeLst = BindLeaveType();
                    ic.LeaveType = dt.Rows[0]["LEAVE_TYPE"].ToString();
                    ic.FromDate = dt.Rows[0]["FROM_DATE"].ToString();
                    ic.ToDate = dt.Rows[0]["TO_DATE"].ToString();
                    ic.TotDays = dt.Rows[0]["TOTAL_DAYS"].ToString();
                    ic.Reason = dt.Rows[0]["REASON"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult LeaveRequest(LeaveRequest Cy, string id)
        {
            try
            {
                id = Cy.LeaveID;
                string Strout = LeaveRequestService.LeaveRequestCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.LeaveID == null)
                    {
                        TempData["notice"] = "Leave Request Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Leave Request Updated Successfully...!";
                    }
                    return RedirectToAction("ListLeaveRequest");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Leave Request";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ListLeaveRequest()
        {
            //LeaveRequest ic = new LeaveRequest();
            return View();
        }
        
        public ActionResult MyListLeaveRequestgrid(string strStatus)
        {
            List<ListLeaveRequest> Reg = new List<ListLeaveRequest>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = LeaveRequestService.GetAllLeaveRequestGrid(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=LeaveRequest?id=" + dtUsers.Rows[i]["LEAVE_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewLeaveRequest?id=" + dtUsers.Rows[i]["LEAVE_ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["LEAVE_ID"].ToString() + "";

                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Approve" || dtUsers.Rows[i]["STATUS"].ToString() == "Reject")
                    {
                        EditRow = "";
                        ViewRow = "";
                    }
                    
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["LEAVE_ID"].ToString() + "";
                }
                Reg.Add(new ListLeaveRequest
                {
                    leaveid = dtUsers.Rows[i]["LEAVE_ID"].ToString(),
                    empid = dtUsers.Rows[i]["EMPID"].ToString(),
                    leavetype = dtUsers.Rows[i]["LEAVETYPENAME"].ToString(),
                    totdays = dtUsers.Rows[i]["TOTAL_DAYS"].ToString(),
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
        public IActionResult ViewLeaveRequest(string id)
        {
            LeaveRequest ic = new LeaveRequest();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("Select LEAVE_ID,EMPMAST.EMPID,LEAVETYPEMASTER.LEAVETYPENAME,to_char(LEAVEREQUEST.FROM_DATE,'dd-MON-yyyy')FROM_DATE,to_char(LEAVEREQUEST.TO_DATE,'dd-MON-yyyy')TO_DATE,TOTAL_DAYS,REASON,STATUS from LEAVEREQUEST LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = LEAVEREQUEST.EMP_ID LEFT OUTER JOIN LEAVETYPEMASTER ON LEAVETYPEMASTER.ID = LEAVEREQUEST.LEAVE_TYPE WHERE LEAVE_ID='" + id + "'");
            
            if (dt.Rows.Count > 0)
            {
                //ic.EmpLst = BindEmp();
                ic.EmpID = dt.Rows[0]["EMPID"].ToString();
                //ic.LeaveTypeLst = BindLeaveType();
                ic.LeaveType = dt.Rows[0]["LEAVETYPENAME"].ToString();
                ic.FromDate = dt.Rows[0]["FROM_DATE"].ToString();
                ic.ToDate = dt.Rows[0]["TO_DATE"].ToString();
                ic.TotDays = dt.Rows[0]["TOTAL_DAYS"].ToString();
                ic.Reason = dt.Rows[0]["REASON"].ToString();
                ic.Status = dt.Rows[0]["STATUS"].ToString();
                ic.LeaveID = id;
            }
            return View(ic);

        }
        [HttpPost]
        public ActionResult ViewLeaveRequest(LeaveRequest Cy, string id)
        {
            try
            {
                id = Cy.LeaveID;
                string Strout = LeaveRequestService.ViewLeaveRequest(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.LeaveID == null)
                    {
                        TempData["notice"] = "Leave Request Updated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Leave Request Updated Successfully...!";
                    }
                    return RedirectToAction("ListLeaveRequest");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit Leave Request";

                    return View(Cy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SelectListItem> BindLeaveType()
        {
            try
            {
                DataTable dtDesg = LeaveRequestService.GetLeaveType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEAVETYPENAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = LeaveRequestService.GetEmployee();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
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
            string flag = LeaveRequestService.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLeaveRequest");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLeaveRequest");
            }

        }

    }
}
