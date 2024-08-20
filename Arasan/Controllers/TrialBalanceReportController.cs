using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;
using Arasan.Interface;
using Elasticsearch.Net;
using Arasan.Services.Report;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers
{
    public class TrialBalanceReportController : Controller
    {
        ITrialBalanceReport TrialBalanceReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public TrialBalanceReportController(ITrialBalanceReport _TrialBalanceReportService, IConfiguration _configuratio)
        {
            TrialBalanceReportService = _TrialBalanceReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult TrialBalanceReport(string strfrom, string strTo)
        {
            try
            {
                TrialBalanceReportModel objR = new TrialBalanceReportModel();
                objR.Brlst = BindBranch();
                 objR.Masterlst = BindMaster("");

                objR.dtFrom = strfrom;
                 return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetBranchJSON(string ItemId)
        {
            
            return Json(BindMaster(ItemId));

        }


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom,string Branch, string Master)
        {
            List<TrialBalanceReportModelItems> Reg = new List<TrialBalanceReportModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)TrialBalanceReportService.GetAllTrialBalanceReport(dtFrom,Branch,Master);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new TrialBalanceReportModelItems
                {
                    groupname = dtUsers.Rows[i]["GROUPNAME"].ToString(),
                    mname = dtUsers.Rows[i]["MNAME"].ToString(),
                    debit = dtUsers.Rows[i]["DEBIT"].ToString(),
                    credit = dtUsers.Rows[i]["CREDIT"].ToString(),
                    masterid = dtUsers.Rows[i]["MASTERID"].ToString(),
                    mstatus = dtUsers.Rows[i]["MSTATUS"].ToString(),
                    malie = dtUsers.Rows[i]["MALIE"].ToString(),
                  
                });
            }
            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public List<SelectListItem> BindMaster(string id)
        {
            try
            {
                DataTable dtDesg = TrialBalanceReportService.GetMaster(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult ExportToExcel(string dtFrom,string Branch, string Master)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " SELECT M.MNAME GROUPNAME, M1.MNAME, Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DEBIT , Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B , companymast c WHERE D.MID = M1.MASTERID AND M.MASTERID='" + Master + "' And b.companyID = c.companymastid And c.companyid ='TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND (B.BRANCHID = '" + Branch + "' OR '" + Branch + "' = 'ALL' ) AND D.T2VCHDT <= to_date('" + dtFrom + "','dd/mm/yyyy') GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE HAVING (SUM(DEBIT)-SUM(CREDIT)) <> 0 ORDER BY DECODE(M.MALIE, 'l',1,'a',2,'i',3,'e', 4),  M.MTREEID ,M1.MNAME";

            //}

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "TrialBalanceReportDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=TrialBalanceReportDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "TrialBalanceReportDetails.xlsx");
                    }
                }

            }

        }
    }
}
