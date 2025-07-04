using System.Data;
using System.Reflection;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class AttendanceReportController : Controller
    {
        IAttendanceReport AttendanceReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public AttendanceReportController(IAttendanceReport _AttendanceReportService, IConfiguration _configuratio)
        {
            AttendanceReportService = _AttendanceReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult ListAttendanceReport()
        {
            return View();
        }
        public ActionResult MyListAttendanceReportgrid()
        {
            List<AttendanceReportgrid> Reg = new List<AttendanceReportgrid>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = AttendanceReportService.GetAllAttendanceReportGrid();

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Attendance = string.Empty;
                string InStatus = string.Empty;
                string OutStatus = string.Empty;

                if (dtUsers.Rows[i]["MISSING_IN"].ToString() != "" || dtUsers.Rows[i]["MISSING_OUT"].ToString() != "")
                {
                    Attendance = "Present";

                    if (DateTime.TryParse(dtUsers.Rows[i]["SHIFT_START"].ToString(), out DateTime shiftStart) &&
                        DateTime.TryParse(dtUsers.Rows[i]["MISSING_IN"].ToString(), out DateTime missingIn) &&
                        shiftStart >= missingIn)
                    {
                        InStatus = "Early";
                    }
                    else
                    {
                        InStatus = "Late";
                    }

                    if (DateTime.TryParse(dtUsers.Rows[i]["SHIFT_END"].ToString(), out DateTime shiftEnd) &&
                        DateTime.TryParse(dtUsers.Rows[i]["MISSING_OUT"].ToString(), out DateTime missingOut) &&
                        shiftEnd >= missingOut)
                    {
                        OutStatus = "Early";
                    }
                    else
                    {
                        OutStatus = "";
                    }

                }
                else
                {
                    Attendance = "Absent";
                }
                Reg.Add(new AttendanceReportgrid
                {
                    empid = dtUsers.Rows[i]["EMPID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    attdate = dtUsers.Rows[i]["ATTENDANCE_DATE"].ToString(),
                    missin = dtUsers.Rows[i]["MISSING_IN"].ToString(),
                    missout = dtUsers.Rows[i]["MISSING_OUT"].ToString(),
                    shiftno = dtUsers.Rows[i]["SHIFTNO"].ToString(),
                    shiftstart = dtUsers.Rows[i]["SHIFT_START"].ToString(),
                    shiftend = dtUsers.Rows[i]["SHIFT_END"].ToString(),
                    weekoff = dtUsers.Rows[i]["WEEK_OFF"].ToString(),
                    attendance = Attendance,
                    instatus = InStatus,
                    outstatus = OutStatus,
                });
            }

            return Json(new
            {
                Reg
            });
        }
    }
}
