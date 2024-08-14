using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class BranchSelectionController : Controller
    {
        IBranchSelectionService BranchSelectionService;
        DataTransactions datatrans;
        private string? _connectionString;

        public BranchSelectionController(IBranchSelectionService _BranchSelectionService, IConfiguration _configuratio)
        {
            BranchSelectionService = _BranchSelectionService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");

        }
        public IActionResult BranchSelection(string id)
        {
            BranchSelection br = new BranchSelection();
			var userId = Request.Cookies["UserId"];
			br.Loclst = BindLocation(userId);
            br.Brlst = BindBranch();
            //br.User = Request.Cookies["UserId"];

            return View(br);
        }
        [HttpPost]
        public IActionResult BranchSelection(BranchSelection model)

        {
            datatrans = new DataTransactions(_connectionString);
            var userId = Request.Cookies["UserId"];
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(3);
            Response.Cookies.Append("LocationId", model.Location, option);
            Response.Cookies.Append("BranchId", model.Branch, option);
            var brname = datatrans.GetDataString("select BRANCHID from BRANCHMAST Where BRANCHMASTID='"+ model.Branch + "'");
            var locname =  datatrans.GetDataString("Select LOCID from LOCDETAILS Where LOCDETAILSID='" + model.Location + "'");
            Response.Cookies.Append("LocationName", locname, option);
            Response.Cookies.Append("BranchName", brname, option);
            //ViewBag.UserId = userId;
            return RedirectToAction(actionName: "EmployeeAttendancelist", controllerName: "EmployeeAttendanceDetails");
        }
        public JsonResult GetLocDetail(string branch)
        {
            return Json(BindLocation(branch));

        }
        public List<SelectListItem> BindBranch( )
        {
            try
            {
                DataTable dtDesg = BranchSelectionService.GetBranch( );
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation(string branch)
        {
            try
            {
                DataTable dtDesg = BranchSelectionService.GetLocation(branch);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["loc"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
