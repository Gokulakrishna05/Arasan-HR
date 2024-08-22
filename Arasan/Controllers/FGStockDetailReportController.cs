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
    public class FGStockDetailReportController : Controller
    {
        IFGStockDetailReport FGStockDetailReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public FGStockDetailReportController(IFGStockDetailReport _FGStockDetailReportService, IConfiguration _configuratio)
        {
            FGStockDetailReportService = _FGStockDetailReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult FGStockDetailReport(string strfrom)
        {
            try
            {
                FGStockDetailReportModel objR = new FGStockDetailReportModel();

                objR.LocLst = BindLocation();

                objR.dtFrom = strfrom;
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
                DataTable dtDesg = FGStockDetailReportService.GetLocation();
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
        public ActionResult MyListFGStockDetailReportGrid(string dtFrom, string loc)
        {
            List<FGStockDetailReportModelItems> Reg = new List<FGStockDetailReportModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)FGStockDetailReportService.GetAllFGStockDetailReport(dtFrom, loc);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new FGStockDetailReportModelItems
                {

                    icat = dtUsers.Rows[i]["ITEMCAT"].ToString(),
                    sncat = dtUsers.Rows[i]["SNCATEGORY"].ToString(),
                    gra = dtUsers.Rows[i]["GRADE"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    nob = dtUsers.Rows[i]["NOOFBAGS"].ToString(),
                    avg = dtUsers.Rows[i]["AVGPER"].ToString(),
                    tqty = dtUsers.Rows[i]["TOTQTY"].ToString(),
                    sucat = dtUsers.Rows[i]["SUBCATEGORY"].ToString(),
                    asdate = dtUsers.Rows[i]["ASDATE"].ToString(),


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

            SvSql = "Select itemcat,sncategory,Itemid Grade,Unitid Unit,Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid)*Qty) Totqty,subcategory,dt1 as asdate From ( Select I.itemcat,i.SNCATEGORY,I.Itemid,U.Unitid,S.Lotno,Sum(Plusqty-Minusqty) Qty,(to_date(SysDate,'dd-mon-yy')-Min(M.Docdate)) Dt1,i.SUBCATEGORY  From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,PLotmast M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And S.Cancel='F' And I.Priunit = U.Unitmastid And S.LOTNO=M.LOTNO And L.Locdetailsid = S.Locid And ( L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "' )  Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,S.Lotno,i.SUBCATEGORY,M.DOCDATE Having Sum(Plusqty-Minusqty) > 0 Order by 2 ) Group by itemcat,sncategory,Itemid,Unitid,Qty,subcategory,dt1  Order by 2,1,Itemid asc, 9 desc";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "FGStockSivakasiDepot");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=FGStockSivakasiDepot.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "FGStockSivakasiDepot.xlsx");
                    }
                }

            }

        }
    }
}
