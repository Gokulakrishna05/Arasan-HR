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
    public class FGDailyStockReportController : Controller
    {
        IFGDailyStockReport fGDailyStockReport;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public FGDailyStockReportController(IFGDailyStockReport _fGDailyStockReport, IConfiguration _configuratio)
        {
            fGDailyStockReport = _fGDailyStockReport;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult FGDailyStockReport(string strfrom, string strTo)
        {
            try
            {
                FGDailyStockReport objR = new FGDailyStockReport();

                objR.LocLst = BindLocation();

                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
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
                DataTable dtDesg = fGDailyStockReport.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListFGDailyStockGrid(string dtFrom, string loc)
        {
            List<FGDailyStockReportItems> Reg = new List<FGDailyStockReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)fGDailyStockReport.GetAllFGDailyStockReport(dtFrom, loc);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new FGDailyStockReportItems
                {

                    icat = dtUsers.Rows[i]["ITEMCAT"].ToString(),
                    sncat = dtUsers.Rows[i]["SNCATEGORY"].ToString(),
                    gra = dtUsers.Rows[i]["GRADE"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    nob = dtUsers.Rows[i]["NOOFBAGS"].ToString(),
                    avg = dtUsers.Rows[i]["AVGPER"].ToString(),
                    tqty = dtUsers.Rows[i]["TOTQTY"].ToString(),
                    lday = dtUsers.Rows[i]["LAYDAYS"].ToString(),
                    sucat = dtUsers.Rows[i]["SUBCATEGORY"].ToString(),
                    

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public IActionResult ExportToExcel(string dtFrom, string loc)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";

            SvSql = " Select itemcat, sncategory, Itemid Grade,Unitid Unit, Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid) * Qty) Totqty,To_date(sysdate, 'DD/MM/YYYY') - To_Date(min(dt1), 'DD/MM/YYYY') Laydays,subcategory From ( Select I.itemcat, i.SNCATEGORY, I.Itemid, U.Unitid, M.Lotno, Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1, i.SUBCATEGORY From PLstockvalue S, Itemmaster I,Locdetails L, Unitmast U,LotmastA M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And I.Priunit = U.Unitmastid And S.Cancel = 'F' and S.DRUMNO not like '%E%' And L.Locdetailsid = S.Locid And S.Lotno = M.Lotno(+) And(L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "') Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate Having Sum(Plusqty - Minusqty) > 0 Union Select I.itemcat,i.SNCATEGORY,I.Itemid || '-Export',U.Unitid,M.Lotno,Sum(Plusqty - Minusqty) Qty,M.Docdate Dt1, i.SUBCATEGORY From PLstockvalue S, Itemmaster I,Locdetails L, Unitmast U,LotmastA M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And I.Priunit = U.Unitmastid And S.Cancel = 'F' and S.DRUMNO like '%E%' And L.Locdetailsid = S.Locid And S.Lotno = M.Lotno(+) And(L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "') Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate Having Sum(Plusqty - Minusqty) > 0 ) Group by itemcat,sncategory,Itemid,Unitid,Qty,subcategory Order by 2,9 ,1,Itemid";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "FGDailyStockReport");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=FGDailyStockReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "FGDailyStockReport.xlsx");
                    }
                }

            }

        }
    }
}
