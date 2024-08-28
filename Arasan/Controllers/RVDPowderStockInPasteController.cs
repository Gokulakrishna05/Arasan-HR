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
    public class RVDPowderStockInPasteController : Controller
    {
        IRVDPowderStockInPaste RVDPowderStockInPasteService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public RVDPowderStockInPasteController(IRVDPowderStockInPaste _RVDPowderStockInPasteService, IConfiguration _configuratio)
        {
            RVDPowderStockInPasteService = _RVDPowderStockInPasteService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult RVDPowderStockInPaste(string strfrom, string strTo)
        {

            try
            {
                RVDPowderStockInPasteModel objR = new RVDPowderStockInPasteModel();

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
            List<RVDPowderStockInPasteModelItem> Reg = new List<RVDPowderStockInPasteModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)RVDPowderStockInPasteService.GetAllRVDPowderStockInPaste(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new RVDPowderStockInPasteModelItem
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    opqty = dtUsers.Rows[i]["OPQTY"].ToString(),
                    recqty = dtUsers.Rows[i]["RECQTY"].ToString(),
                    issqty = dtUsers.Rows[i]["ISSQTY"].ToString(),
                     


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

            SvSql = "Select x.DocDate , Sum(x.OpQty) OQ , Sum(x.RQty) RQ , Sum(x.IQty) IQ\r\nFrom \r\n(\r\n\r\nSelect '" + dtFrom + "' DocDate , Sum(Qty) OpQty , 0 RQty , 0 IQty\r\nFrom \r\n(\r\nSelect TL.LocationType , Ls.LotNo , I.ItemID , Ls.DrumNo , Sum(Ls.PlusQty)-Sum(Ls.MinusQty) Qty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.DrumNo Is Not Null\r\nAnd Ls.DocDate < '" + dtFrom + "'\r\nGroup By TL.LocationType , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHaving Sum(Ls.PlusQty)-Sum(Ls.MinusQty)  > 0\r\n)\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RecQty , 0 IssQty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I , LocDetails FL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.StockTransType In ('DRUM ISSUE','CURING RECHARGE')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd ( FL.LocationType = 'RVD' Or FL.LocID = 'RVD SHED' ) \r\nGroup By Ls.DocDate , TL.LocationType , FL.LocationType\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RecQty , Sum(Ls.MinusQty) IssQty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.StockTransType In ('BPROD INPUT')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , TL.LocationType\r\n\r\n) x\r\nGroup By x.DocDate\r\nOrder By x.DocDate";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "RVDPowderStockInPasteDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=RVDPowderStockInPasteDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "RVDPowderStockInPasteDetails.xlsx");
                    }
                }

            }

        }
    }
}
