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
    public class PasteStockInMixingController : Controller
    {
        IPasteStockInMixing PasteStockInMixingService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public PasteStockInMixingController(IPasteStockInMixing _PasteStockInMixingService, IConfiguration _configuratio)
        {
            PasteStockInMixingService = _PasteStockInMixingService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PasteStockInMixing(string strfrom, string strTo)
        {

            try
            {
                PasteStockInMixingModel objR = new PasteStockInMixingModel();

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
            List<PasteStockInMixingModelItem> Reg = new List<PasteStockInMixingModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PasteStockInMixingService.GetAllPasteStockInMixing(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new PasteStockInMixingModelItem
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    opqty = dtUsers.Rows[i]["OPQTY"].ToString(),
                    rqty = dtUsers.Rows[i]["RQTY"].ToString(),
                    piqty = dtUsers.Rows[i]["PIQTY"].ToString(),
                    riqty = dtUsers.Rows[i]["RIQTY"].ToString(),
                    rmiqty = dtUsers.Rows[i]["RMIQTY"].ToString(),
                    


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

            SvSql = "SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ\r\nFROM \r\n(\r\nSELECT  '" + dtFrom + "'  DocDate , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) OpQty , 0 RQty , 0 PIQty , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in  ('MIXING','PACKING')\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nAND Ls.DocDate < '" + dtFrom + "'\r\nGROUP BY TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nAND TL.LocationType in ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition')\r\nGROUP BY TL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE')\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND FL.LocationType NOT in ('MIXING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND TL.LocationType  IN ('MIXING','PACKING') \r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE')\r\nGROUP BY TL.LocID , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'PACKING INPUT'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in ('PACKING')\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nGROUP BY TL.LocID , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'DRUM ISSUE'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType NOT IN ('MIXING','PACKING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType  IN ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE')\r\nGROUP BY TL.LocID , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE')\r\nAND TL.LocationType IN ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work')\r\nGROUP BY TL.LocationType , Ls.DocDate\r\n) x\r\nGROUP BY x.DocDate\r\nORDER BY x.DocDate";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "PasteStockInMixingDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PasteStockInMixingDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PasteStockInMixingDetails.xlsx");
                    }
                }

            }

        }
    }
}
