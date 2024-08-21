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
using Microsoft.CodeAnalysis.Differencing;
using static Nest.JoinField;
using System.Diagnostics;
namespace Arasan.Controllers
{
    public class BalanceSheetController : Controller
    {
        IBalanceSheet BalanceSheetService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public BalanceSheetController(IBalanceSheet _BalanceSheetService, IConfiguration _configuratio)
        {
            BalanceSheetService = _BalanceSheetService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BalanceSheet(string strfrom)
        {
            try
            {
                BalanceSheetModel objR = new BalanceSheetModel();
                objR.Brlst = BindBranch();
 
                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult BalanceSheet1(string strfrom)
        {
            try
            {
                BalanceSheetModel objR = new BalanceSheetModel();
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
            List<BalanceSheetItems> Reg = new List<BalanceSheetItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)BalanceSheetService.GetAllBalanceSheet(dtFrom, Branch);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new BalanceSheetItems
                {
                    groupname = dtUsers.Rows[i]["GROUPNAME"].ToString(),
                    db = dtUsers.Rows[i]["DB"].ToString(),
                    cr = dtUsers.Rows[i]["CR"].ToString(),
                    //malie = dtUsers.Rows[i]["MALIE"].ToString(),
                    //mid = dtUsers.Rows[i]["MID"].ToString(),
 
                });
            }
            return Json(new
            {
                Reg
            });

        }
        public ActionResult MyListPurchaseRepItemReportGrid1(string dtFrom, string Branchs)
        {
            List<BalanceSheetItems> Reg = new List<BalanceSheetItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)BalanceSheetService.GetAllBalanceSheet1(dtFrom, Branchs);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new BalanceSheetItems
                {
                    groupname = dtUsers.Rows[i]["GROUPNAME"].ToString(),
                    db = dtUsers.Rows[i]["DB"].ToString(),
                    cr = dtUsers.Rows[i]["CR"].ToString(),
                    //malie = dtUsers.Rows[i]["MALIE"].ToString(),
                    //mid = dtUsers.Rows[i]["MID"].ToString(),

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

        


        public IActionResult ExportToExcel(string dtFrom, string Branch)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " SELECT GROUPNAME,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR,MALIE,MID FROM(SELECT M.MNAME GROUPNAME, M1.MNAME, DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID AND b.companyID = c.companymastid AND c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND M1.MPARENT1 IN(1,51)AND(B.BRANCHID = '"+Branch+"' OR '$branch' = 'ALL')AND D.T2VCHDT <= TO_DATE('"+dtFrom+"', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE,M.MASTERID HAVING(SUM(DEBIT)-SUM(CREDIT)) <> 0)GROUP BY GROUPNAME,MALIE,MID ORDER BY DECODE(MALIE, 'l',1,'a',2,'i',3,'e', 4),GROUPNAME";

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
                    wb.Worksheets.Add(dtNew1, "BalanceSheetDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=BalanceSheetDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "BalanceSheetDetails.xlsx");
                    }
                }

            }

        }
        public IActionResult ExportToExcel1(string dtFrom, string Branchs)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " SELECT GROUPNAME,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR,MALIE,MID FROM( SELECT M.MNAME GROUPNAME, M1.MNAME, DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID AND b.companyID = c.companymastid AND c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND M1.MPARENT1 IN(101,151)AND(B.BRANCHID = '"+Branchs+"' OR '"+Branchs+"' = 'ALL')AND D.T2VCHDT <= TO_DATE('"+dtFrom+"', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE,M.MASTERID HAVING(SUM(DEBIT)-SUM(CREDIT)) <> 0)GROUP BY GROUPNAME,MALIE,MID ORDER BY DECODE(MALIE, 'l',1,'a',2,'i',3,'e', 4),GROUPNAME";

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
                    wb.Worksheets.Add(dtNew1, "BalanceSheetDetails1");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=BalanceSheetDetails1.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "BalanceSheetDetails1.xlsx");
                    }
                }

            }

        }
    }
}
