using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Controllers.Report
{
    public class WorkCenterReportController : Controller
    {
        IWorkCenterReportService WorkCenterReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public WorkCenterReportController(IWorkCenterReportService _WorkCenterReportService, IConfiguration _configuratio)
        {
            WorkCenterReportService = _WorkCenterReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WorkCenterReport(string strfrom, string strTo)
        {
            try
            {
                WorkCenterReport objR = new WorkCenterReport();
                objR.Worklst = BindWorkCenter();
                objR.Processlst = BindProcess();
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListWorkCenterReport()
        {
            return View();
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = WorkCenterReportService.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = WorkCenterReportService.GetProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListWorkCenterReportGrid(string dtFrom, string dtTo, string WorkCenter, string Process)
        {
            List<WorkCenterReportItems> Reg = new List<WorkCenterReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)WorkCenterReportService.GetAllWorkCenterReport(dtFrom, dtTo, WorkCenter,Process);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new WorkCenterReportItems
                {

                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    wc = dtUsers.Rows[i]["WCID"].ToString(),
                    process = dtUsers.Rows[i]["PROCESSID"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ExportToExcel(string dtFrom, string dtTo, string WorkCenter, string Process)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            if (dtFrom == null && WorkCenter == null && Process == null)
            {

                SvSql = "SELECT B.DOCID,to_char(B.DOCDATE,'dd-MON-yyyy')DOCDATE,W.WCID,P.PROCESSID FROM BCPRODBASIC B , WCBASIC W , PROCESSMAST P WHERE B.WCID = W.WCBASICID AND B.WPROCESSID = P.PROCESSMASTID";

            }
            else
            {

                SvSql = "SELECT B.DOCID,to_char(B.DOCDATE,'dd-MON-yyyy')DOCDATE,W.WCID,P.PROCESSID FROM BCPRODBASIC B , WCBASIC W , PROCESSMAST P WHERE B.WCID = W.WCBASICID AND B.WPROCESSID = P.PROCESSMASTID";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }


                if (WorkCenter != null)
                {
                    SvSql += " and W.WCID='" + WorkCenter + "'";
                }


                if (Process != null)
                {
                    SvSql += " and P.PROCESSID='" + Process + "'";
                }
            }
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "WorkCenterDeatils");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=WorkCenterDeatils.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "WorkCenterDeatils.xlsx");
                    }
                }

            }

        }
        private IActionResult File(object excelData, string v)
        {
            throw new NotImplementedException();
        }
    }
}
