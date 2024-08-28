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
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;

namespace Arasan.Controllers
{
    public class BatchProductionSummaryController : Controller
    {
        IBatchProductionSummary BatchProductionSummaryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public BatchProductionSummaryController(IBatchProductionSummary _BatchProductionSummaryService, IConfiguration _configuratio)
        {
            BatchProductionSummaryService = _BatchProductionSummaryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchProductionSummary(string strfrom, string strTo)
        {

            try
            {
                BatchProductionSummaryModel objR = new BatchProductionSummaryModel();

                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo)
        {
            List<BatchProductionSummaryModelItem> Reg = new List<BatchProductionSummaryModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)BatchProductionSummaryService.GetAllBatchProductionSummary(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new BatchProductionSummaryModelItem
                {
                    etype = dtUsers.Rows[i]["ETYPE"].ToString(),
                    wcid = dtUsers.Rows[i]["WCID"].ToString(),
                    processid = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    seq = dtUsers.Rows[i]["SEQ"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unitid = dtUsers.Rows[i]["UNITID"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    wipqty = dtUsers.Rows[i]["WIPQTY"].ToString(),
                    mtono = dtUsers.Rows[i]["MTONO"].ToString(),
                   


                });
            }

            return Json(new
            {
                Reg
            });

        }




        public IActionResult ExportToExcel(string dtFrom, string dtTo)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";

            SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, \r\n       I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL\r\nFROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.IITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'INPUT'\r\nAND B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND PS.PSBASICID = BC.PSCHNO\r\nAND PS.OPITEMID = PSI.ITEMMASTERID\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID , U.UNITID \r\nUNION\r\nSELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.CITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND B.ETYPE = 'INPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nUNION\r\nSELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.OITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nUNION\r\nSELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.WITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.DOCDATE BETWEEN '" + dtFrom + "' AND :ED\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n       W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nORDER BY 2 , 3 , 10 , 9";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "APPowderStockInPyroDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=APPowderStockInPyroDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "APPowderStockInPyroDetails.xlsx");
                    }
                }

            }

        }
    }
}
