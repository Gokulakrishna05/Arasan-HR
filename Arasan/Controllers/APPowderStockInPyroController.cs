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
    public class APPowderStockInPyroController : Controller
    {
        IAPPowderStockInPyro APPowderStockInPyroService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public APPowderStockInPyroController(IAPPowderStockInPyro _APPowderStockInPyroService, IConfiguration _configuratio)
        {
            APPowderStockInPyroService = _APPowderStockInPyroService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult APPowderStockInPyro(string strfrom, string strTo)
        {

            try
            {
                APPowderStockInPyroModel objR = new APPowderStockInPyroModel();
               
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
            List<APPowderStockInPyroModelItem> Reg = new List<APPowderStockInPyroModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)APPowderStockInPyroService.GetAllAPPowderStockInPyro(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new APPowderStockInPyroModelItem
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
            //if (dtFrom == null && dtTo == null)
            //{
            // SvSql = " Select I.ItemID , U.UnitID , Br.BranchID , L.LocID , Sum(x.OQ) OQ , Sum(x.OV) OV , Sum(x.RQ) RQ , Sum(x.RV) RV , Sum(x.IQ) IQ , Sum(x.IV) IV From(Select Sv.ItemID , Sv.LocID , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,-Sv.Qty)) OQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, -Sv.StockValue)) OV , 0 RQ , 0 RV , 0 IQ , 0 IV From AsStockValue Sv Where Sv.DocDate < '" + dtFrom + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID Union All Select Sv.ItemID , Sv.LocID , 0 OQ , 0 OV , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.Qty, 0)) RQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, 0)) RV   , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.Qty, 0)) IQ , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.StockValue, 0)) IV From AsStockValue Sv Where Sv.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID) x,  ItemMaster I, LocDetails L , UnitMast U, BranchMast Br Where x.ItemID = I.ItemMasterID And x.LocID = L.LocDetailsID And L.BranchID = Br.BranchMastID And U.UnitMastID = I.PriUnit And Br.BranchID = '" + Branch + "' And(L.LocID = '" + Location + "' ) Group By Br.BranchID , L.LocID , I.ItemID , U.UnitID Order By Br.BranchID , L.LocID , I.ItemID , U.UnitID";
            //"SELECT z.Dt , SUM(z.OpQty) Op , SUM(z.RecQty) ApRec , SUM(z.IssQty) ApIss\r\nFROM \r\n(\r\nSELECT :SD Dt , SUM(Qty) OpQty, 0 RecQty, 0 IssQty\r\nFROM \r\n(\r\nSELECT SUM(Ls.PlusQty-Ls.MinusQty) Qty\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'BALL MILL'\r\nAND I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG')\r\nAND Ls.DocDate < :SD\r\nGROUP BY TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHAVING SUM(Ls.PlusQty-Ls.MinusQty)  > 0\r\n)\r\nUNION ALL";
            //) z GROUP BY z.Dt ORDER BY  z.Dt
            SvSql = "SELECT Ls.DocDate, 0 OpQty, SUM(Ls.PlusQty) RecQty, 0 IssQty FROM LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I WHERE Ls.StockTransType = 'DRUM ISSUE' AND Ls.ItemID = I.ItemMasterID AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType IN ('AP MILL','SIEVE & BLEND')  AND Ls.LocID = TL.LocDetailsID AND TL.LocationType = 'BALL MILL' AND (I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG') OR I.ITEMID='ATOMIS.ALU.GRADE A-150 MICRON') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' GROUP BY Ls.DocDate , FL.LocationType , TL.LocationType UNION ALL SELECT Ls.DocDate, 0 OpQty, 0 RecQty, SUM(Ls.MinusQty) IssQty FROM LStockValue Ls , LocDetails TL , ItemMaster I WHERE Ls.StockTransType in  ('PROD INPUT','Stock Reconcilation') AND Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND TL.LocationType = 'BALL MILL' AND (I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG') OR I.ITEMID='ATOMIS.ALU.GRADE A-150 MICRON') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' GROUP BY Ls.DocDate , TL.LocationType";
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
