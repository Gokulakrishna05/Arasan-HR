using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace Arasan.Controllers
{
    public class PurMonReportController : Controller
    {
        IPurMonReport PurMonReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurMonReportController(IPurMonReport _PurMonReportService, IConfiguration _configuratio)
        {
            PurMonReportService = _PurMonReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurMonReport(string strfrom, string strTo)
        {
            try
            {
                PurMonReport objR = new PurMonReport();
                objR.Brlst = BindBranch();
                objR.Sdate = strfrom;
                objR.Edate = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public ActionResult MyListPurMonReportGrid(string Branch, string Sdate, string Edate)
        {
            List<PurMonReportItem> Reg = new List<PurMonReportItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurMonReportService.GetAllReport(Branch, Sdate, Edate);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new PurMonReportItem 
                {
                    //id = Convert.ToInt64(dtUsers.Rows[i]["SFINYR"].ToString()),
                    part = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    jan = dtUsers.Rows[i]["Jan"].ToString(),
                    //feb = dtUsers.Rows[i]["QTY"].ToString(),
                    //mar = dtUsers.Rows[i]["GRNQTY"].ToString(),
                    //april = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //may = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //june = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //july = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //aug = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //sep = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //act = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //nov = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    //dec = dtUsers.Rows[i]["DUEDATE"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }


    }
}


