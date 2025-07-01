using Arasan.Interface;
using Arasan.Interface.Transaction;
using Arasan.Models;
using Arasan.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System.Data;
using System.Globalization;

namespace Arasan.Controllers.Transaction
{
    public class MissingPunchEntryController : Controller
    {
        IMissingPunchEntry missingPunchEntry;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public MissingPunchEntryController(IMissingPunchEntry _missingPunchEntry, IConfiguration _configuratio)
        {
            missingPunchEntry = _missingPunchEntry;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult MissingPunchEntry(string id)
        {
            MissingPunchEntry py = new MissingPunchEntry();
            py.createby = Request.Cookies["UserId"];
            py.PunchDate = DateTime.Now.ToString("dd-MMM-yyyy");
            py.EmpNamelst = BindEmpName();
            py.MissingPunch = BindMissingPunch();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = datatrans.GetData("SELECT ID,EMPLOYEE_ID,ATTENDANCE_DATE,MISSING_IN,MISSING_OUT,DEVICE_ID,REASON,STATUS,MISSING_IN_OUT FROM HRM_MISSING_PUNCH WHERE ID='" + id + "'");

                if (dt.Rows.Count > 0)
                {
                    py.EmployeeId = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                    py.PunchDate = dt.Rows[0]["ATTENDANCE_DATE"].ToString();
                    py.Missing = dt.Rows[0]["MISSING_IN_OUT"].ToString();

                    // Format Missing In
                    var rawMissingIn = dt.Rows[0]["MISSING_IN"].ToString();
                    if (!string.IsNullOrEmpty(rawMissingIn))
                    {
                        DateTime parsedTime;
                        if (DateTime.TryParseExact(rawMissingIn, "hh.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime))
                        {
                            py.MissingIn = parsedTime.ToString("hh:mm tt"); // shows as 12:10 PM
                        }
                        else
                        {
                            py.MissingIn = rawMissingIn; // fallback
                        }
                    }

                    // Format Missing Out
                    var rawMissingOut = dt.Rows[0]["MISSING_OUT"].ToString();
                    if (!string.IsNullOrEmpty(rawMissingOut))
                    {
                        DateTime parsedTime;
                        if (DateTime.TryParseExact(rawMissingOut, "hh.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime))
                        {
                            py.MissingOut = parsedTime.ToString("hh:mm tt"); // shows as 12:10 PM
                        }
                        else
                        {
                            py.MissingOut = rawMissingOut; // fallback
                        }
                    }

                    py.Device = dt.Rows[0]["DEVICE_ID"].ToString();
                    py.Reason = dt.Rows[0]["REASON"].ToString();
                    py.ID = id;
                }

            }

            return View(py);
        }
        [HttpPost]
        public ActionResult MissingPunchEntry(MissingPunchEntry by, string id)
        {

            try
            {
                by.ID = id;
                string Strout = missingPunchEntry.MissingPunchEntryCRUD(by);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (by.ID == null)
                    {
                        TempData["notice"] = "Missing PunchEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Missing PunchEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListMissingPunchEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Missing PunchEntry";
                    TempData["notice"] = Strout;
                    return RedirectToAction("MissingPunchEntry");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(by);
        }
        public IActionResult ListMissingPunchEntry()
        {
            return View();
        }
        public ActionResult MyListMissingPunchEntrygrid(string strStatus)
        {
            List<MissingPunchEntrygrid> Reg = new List<MissingPunchEntrygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = missingPunchEntry.GetAlLMissingPunchEntry(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;


                //if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                //{

                EditRow = "<a href=MissingPunchEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                ViewRow = "<a href=ViewMissingPunchEntry?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";

                DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                //}
                //else
                //{

                //    EditRow = "";
                //    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["SHIFTMASTID"].ToString() + "";

                //}

                Reg.Add(new MissingPunchEntrygrid
                {
                    emp = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    attendance = dtUsers.Rows[i]["ATTENDANCE_DATE"].ToString(),
                    missing = dtUsers.Rows[i]["MISSING_IN_OUT"].ToString(),
                    device = dtUsers.Rows[i]["DEVICE_ID"].ToString(),
                    reason = dtUsers.Rows[i]["REASON"].ToString(),
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
        public IActionResult ViewMissingPunchEntry(string id)
        {
            MissingPunchEntry py = new MissingPunchEntry();

            DataTable dt = datatrans.GetData("SELECT ID,EMPMAST.EMPNAME,to_char(ATTENDANCE_DATE,'dd-MON-yyyy')ATTENDANCE_DATE,MISSING_IN,MISSING_OUT,DEVICE_ID,REASON,STATUS,MISSING_IN_OUT FROM HRM_MISSING_PUNCH LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=HRM_MISSING_PUNCH.EMPLOYEE_ID WHERE ID='" + id + "'");

            if (dt.Rows.Count > 0)
            {
                py.EmployeeId = dt.Rows[0]["EMPNAME"].ToString();
                py.PunchDate = dt.Rows[0]["ATTENDANCE_DATE"].ToString();
                py.Missing = dt.Rows[0]["MISSING_IN_OUT"].ToString();

                // Format Missing In
                var rawMissingIn = dt.Rows[0]["MISSING_IN"].ToString();
                if (!string.IsNullOrEmpty(rawMissingIn))
                {
                    DateTime parsedTime;
                    if (DateTime.TryParseExact(rawMissingIn, "hh.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime))
                    {
                        py.MissingIn = parsedTime.ToString("hh:mm tt"); // shows as 12:10 PM
                    }
                    else
                    {
                        py.MissingIn = rawMissingIn; // fallback
                    }
                }

                // Format Missing Out
                var rawMissingOut = dt.Rows[0]["MISSING_OUT"].ToString();
                if (!string.IsNullOrEmpty(rawMissingOut))
                {
                    DateTime parsedTime;
                    if (DateTime.TryParseExact(rawMissingOut, "hh.mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime))
                    {
                        py.MissingOut = parsedTime.ToString("hh:mm tt"); // shows as 12:10 PM
                    }
                    else
                    {
                        py.MissingOut = rawMissingOut; // fallback
                    }
                }

                py.Device = dt.Rows[0]["DEVICE_ID"].ToString();
                py.Reason = dt.Rows[0]["REASON"].ToString();
                py.ID = id;
            }
            return View(py);

        }
        public List<SelectListItem> BindEmpName()
        {
            try
            {
                DataTable dtDesg = missingPunchEntry.GetEmpName();
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
        public List<SelectListItem> BindMissingPunch()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Missing In", Value = "Missing In" });
                lstdesg.Add(new SelectListItem() { Text = "Missing Out", Value = "Missing Out" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
