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
    public class OpeningSqlController : Controller
    {
        IOpeningSql OpeningSqlService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public OpeningSqlController(IOpeningSql _OpeningSqlService, IConfiguration _configuratio)
        {
            OpeningSqlService = _OpeningSqlService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult OpeningSql(string strfrom)
        {
            try
            {
                OpeningSqlModel objR = new OpeningSqlModel();
                objR.Brlst = BindBranch();
 
                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetBranchJSON(string ItemId)
        //{

        //    return Json(BindMaster(ItemId));

        //}


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string Branch)
        {
            List<OpeningSqlModelItems> Reg = new List<OpeningSqlModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)OpeningSqlService.GetAllOpeningSql(dtFrom, Branch);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new OpeningSqlModelItems
                {
                  
                    debit = dtUsers.Rows[i]["DB"].ToString(),
                    credit = dtUsers.Rows[i]["CR"].ToString(),
                   
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

        


        public IActionResult ExportToExcel(string dtFrom, string Branch )
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            //SvSql = " select Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR from( SELECT M.MNAME GROUPNAME, M1.MNAME, Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID And b.companyID = c.companymastid And c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHIDAND(B.BRANCHID = '" + Branch + "' OR '" + Branch + "' = 'ALL')AND D.T2VCHDT < to_date('" + dtFrom + "', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MASTERID)";
            SvSql = "SELECT Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DB, Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CR FROM ( SELECT  M.MNAME GROUPNAME, M1.MNAME,  Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT,  Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,  M1.MASTERID,  M1.mstatus,  M.MALIE,  M.MASTERID MID  FROM DAILYTRANS D,  MASTER M,  MASTER M1,   BRANCHMAST B,   companymast c   WHERE    D.MID = M1.MASTERID  AND B.companyID = c.companymastid   AND c.companyid = 'TAAI'   AND M.MASTERID = M1.MPARENT    AND B.BRANCHMASTID = D.BRANCHID  AND D.T2VCHDT < TO_DATE('01/01/2024', 'dd/mm/yyyy')  GROUP BY  M.MNAME,   M1.MNAME,   M.MALIE,   M.MTREEID,   M1.MASTERID,   M1.mstatus,  M.MASTERID ) ";
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
                    wb.Worksheets.Add(dtNew1, "OpeningSqlDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=OpeningSqlDetailsDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "OpeningSqlDetailsDetails.xlsx");
                    }
                }

            }

        }
    }
}
