using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace Arasan.Controllers.Report
{
    public class PurchaseReportController : Controller
    {
        IPurchaseReportService PurchaseReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurchaseReportController(IPurchaseReportService _PurchaseReportService, IConfiguration _configuratio)
        {
            PurchaseReportService = _PurchaseReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseReport(string id)
        {
            PurchaseReport ca = new PurchaseReport();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            //ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
           
            if (id == null)
            {

            }
            else
            {

            }

            return View(ca);
        }
        [HttpPost]
        public IActionResult PurchaseReport(string st, string ed)
        {
            return View();
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
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
    }
}
