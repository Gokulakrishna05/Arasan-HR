using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Elasticsearch.Net;
namespace Arasan.Controllers
{
    public class StockStatementController : Controller
    {
        IStockStatement stockStatement;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public StockStatementController(IStockStatement _stockStatement, IConfiguration _configuratio)
        {
            stockStatement = _stockStatement;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StockStatement()
        {
            try
            {
                StockStatement objR = new StockStatement();

                objR.LocLst = BindLocation();
                objR.Brchlst = BindBranch();
                objR.TypLst = BindType();
                objR.Siwlst = BindSiwise("");

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
                DataTable dtDesg = stockStatement.GetBranch();
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

        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = stockStatement.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Wcid"].ToString(), Value = dtDesg.Rows[i]["Wcid"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindType()
        {
            try
            {
                DataTable dtDesg = stockStatement.GetType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Shed_Item"].ToString(), Value = dtDesg.Rows[i]["Shed_Item"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindSiwise(string type)
        {
            try
            {
                DataTable dtDesg = stockStatement.Getswis(type);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LIST"].ToString(), Value = dtDesg.Rows[i]["LIST"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListStockStatementGrid(string type, string loc, string branch, string asdate)
        {
            List<StockStatementItems> Reg = new List<StockStatementItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)stockStatement.GetAllStockStatementReport(type, loc, branch, asdate);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new StockStatementItems
                {
                    dno = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    iid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    lno = dtUsers.Rows[i]["LOTNO"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    bid = dtUsers.Rows[i]["BINID"].ToString(),

                });
            }
            return Json(new
            {
                Reg
            });
        }

        public IActionResult ExportToExcel(string type, string loc, string branch, string asdate)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";

            SvSql = @"Select C.Drumno,I.Itemid,C.Lotno, C.Qty,b.Binid 
                            From Curinpbasic Cp,curinpdetail Cd, binbasic B,Itemmaster I,
                            (
                            Select Drumno, Ls.Itemid, ls.Lotno, round(Sum(Plusqty) - sum(Minusqty), 0) Qty
                            From Lstockvalue Ls, locdetails L, branchmast Bm
                            Where Bm.Branchmastid = L.Branchid
                            And L.Locdetailsid = Ls.Locid
                            And(L.Locid = '" + loc + "' Or  'ALL' = '" + loc + "')";
            SvSql += "And Ls.Docdate <= '" + asdate + "' And(Bm.Branchid = '" + branch + "' Or 'ALL' = '" + branch + "')";

            SvSql += @"Group By Drumno,ls.Lotno,Ls.Itemid
                            Having Sum(Plusqty) - sum(Minusqty) > 0
                            ) C
                            Where Cp.Curinpbasicid = Cd.Curinpbasicid
                            And Cd.Batchno = C.Lotno(+)
                            And B.Binbasicid = Cd.Binmasterid
                            And I.Itemmasterid = C.Itemid
                            And(('Shed wise' = '" + type + "') Or('Item wise' = '" + type + "') )";
            SvSql += "Order By B.Binid,I.Itemid,C.Drumno";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "StockStatement");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=StockStatement.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "StockStatement.xlsx");
                    }
                }

            }

        }
        public JsonResult GetPSchedJSON(string schid)
        {
            return Json(BindSiwise(schid));

        }
    }
}
