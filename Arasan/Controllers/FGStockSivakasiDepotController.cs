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
    public class FGStockSivakasiDepotController : Controller
    {
        IFGStockSivakasiDepot FGStockSivakasiDepotService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public FGStockSivakasiDepotController(IFGStockSivakasiDepot _FGStockSivakasiDepotService, IConfiguration _configuratio)
        {
            FGStockSivakasiDepotService = _FGStockSivakasiDepotService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult FGStockSivakasiDepot(string strfrom)
        {
            try
            {
                FGStockSivakasiDepotModel objR = new FGStockSivakasiDepotModel();

                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom)
        {
            List<FGStockSivakasiDepotModelItems> Reg = new List<FGStockSivakasiDepotModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)FGStockSivakasiDepotService.GetAllFGStockSivakasiDepot(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new FGStockSivakasiDepotModelItems
                {

                    itemcat = dtUsers.Rows[i]["ITEMCAT"].ToString(),
                    sncategory = dtUsers.Rows[i]["SNCATEGORY"].ToString(),
                    grade = dtUsers.Rows[i]["GRADE"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    noofbags = dtUsers.Rows[i]["NOOFBAGS"].ToString(),
                    avgper = dtUsers.Rows[i]["AVGPER"].ToString(),
                    totqty = dtUsers.Rows[i]["TOTQTY"].ToString(),
                    laydays = dtUsers.Rows[i]["LAYDAYS"].ToString(),
                    subcategory = dtUsers.Rows[i]["SUBCATEGORY"].ToString(),
                    

                });
            }
            return Json(new
            {
                Reg
            });

        }





        public IActionResult ExportToExcel(string dtFrom)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            SvSql = "Select itemcat,sncategory,Itemid Grade,Unitid Unit,Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid)*Qty) Totqty,To_date(sysdate,'DD/MM/YYYY')-To_Date(min(dt1),'DD/MM/YYYY') Laydays,subcategory From \r\n (\r\nSelect I.itemcat,i.SNCATEGORY,I.Itemid,U.Unitid,M.Lotno,Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1,i.SUBCATEGORY\r\n From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,LotmastA M\r\nWhere S.Docdate <= '" + dtFrom + "'\r\nAnd I.Itemmasterid = S.Itemid\r\nAnd I.Priunit = U.Unitmastid\r\nAnd S.Cancel='F' and S.DRUMNO not like '%E%'\r\nAnd L.Locdetailsid = S.Locid\r\nAnd S.Lotno=M.Lotno(+)\r\nAnd L.LocID = 'FG GODOWN-SVKS DEPOT'\r\nGroup By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate\r\nHaving Sum(Plusqty-Minusqty) > 0\r\nUnion\r\nSelect I.itemcat,i.SNCATEGORY,I.Itemid||'-Export',U.Unitid,M.Lotno,Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1,i.SUBCATEGORY\r\n From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,LotmastA M\r\nWhere S.Docdate <= '" + dtFrom + "'\r\nAnd I.Itemmasterid = S.Itemid\r\nAnd I.Priunit = U.Unitmastid\r\nAnd S.Cancel='F' and S.DRUMNO like '%E%'\r\nAnd L.Locdetailsid = S.Locid\r\nAnd S.Lotno=M.Lotno(+)\r\nAnd L.LocID = 'FG GODOWN-SVKS DEPOT'\r\nGroup By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate\r\nHaving Sum(Plusqty-Minusqty) > 0\r\n)\r\nGroup by itemcat,sncategory,Itemid,Unitid,Qty,subcategory\r\nOrder by 2,9 ,1,Itemid";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "FGStockSivakasiDepotDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=FGStockSivakasiDepotDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "FGStockSivakasiDepotDetails.xlsx");
                    }
                }

            }

        }
    }
}
