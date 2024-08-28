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
    public class RVDPowderStockInPolishController : Controller
    {
        IRVDPowderStockInPolish RVDPowderStockInPolishService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public RVDPowderStockInPolishController(IRVDPowderStockInPolish _RVDPowderStockInPolishService, IConfiguration _configuratio)
        {
            RVDPowderStockInPolishService = _RVDPowderStockInPolishService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult RVDPowderStockInPolish(string strfrom, string strTo)
        {

            try
            {
                RVDPowderStockInPolishModel objR = new RVDPowderStockInPolishModel();

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
            List<RVDPowderStockInPolishModelItem> Reg = new List<RVDPowderStockInPolishModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)RVDPowderStockInPolishService.GetAllRVDPowderStockInPolish(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new RVDPowderStockInPolishModelItem
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    opqty = dtUsers.Rows[i]["OPQTY"].ToString(),
                    rqty = dtUsers.Rows[i]["RQTY"].ToString(),
                    pqty = dtUsers.Rows[i]["PQTY"].ToString(),
                    iqty = dtUsers.Rows[i]["IQTY"].ToString(),
                    


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

            // SvSql = "Select x.DocDate , Sum(x.OpQty) OQ , Sum(x.RQty) RQ ,Sum(x.PQty) PQ, Sum(x.IQty) IQ\r\nFrom \r\n(\r\nSelect :SD DocDate , Sum(Qty) OpQty , 0 RQty ,0 PQty, 0 IQty\r\nFrom\r\n(\r\nSelect TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo , Sum(Ls.PlusQty)-Sum(Ls.MinusQty) Qty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.DrumNo Is Not Null\r\nAnd Ls.DocDate < :SD\r\nGroup By TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHaving Sum(Ls.PlusQty)-Sum(Ls.MinusQty)  > 0\r\n)\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RQty ,0 PQty, 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType <> 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty ,0 RQty, Sum(Ls.MinusQty) PQty , 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\nUnion All\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty ,0 PQty, Sum(Ls.PlusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType <> 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty , 0 PQty, Sum(Ls.MinusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('RVD POWDER','POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\n) x\r\nGroup By x.DocDate\r\nOrder By x.DocDate";
            SvSql = "Select Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RQty ,0 PQty, 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType <> 'POLISH'\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty ,0 RQty, Sum(Ls.MinusQty) PQty , 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\nUnion All\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty ,0 PQty, Sum(Ls.PlusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType <> 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty , 0 PQty, Sum(Ls.MinusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('RVD POWDER','POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "RVDPowderStockInPolishDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=RVDPowderStockInPolishDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "RVDPowderStockInPolishDetails.xlsx");
                    }
                }

            }

        }
    }
}
