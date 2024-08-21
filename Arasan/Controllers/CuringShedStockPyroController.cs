//using Microsoft.AspNetCore.Mvc;

//namespace Arasan.Controllers
//{
//    public class CuringShedStockPyroController : Controller
//    {
//        public IActionResult CuringShedStockPyro()
//        {
//            return View();
//        }
//    }
//}
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
    public class CuringShedStockPyroController : Controller
    {
        ICuringShedStockPyro CuringShedStockPyroService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public CuringShedStockPyroController(ICuringShedStockPyro _CuringShedStockPyroService, IConfiguration _configuratio)
        {
            CuringShedStockPyroService = _CuringShedStockPyroService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CuringShedStockPyro(string strfrom, string strto)
        {
            try
            {
                CuringShedStockPyroModel objR = new CuringShedStockPyroModel();
 
                objR.dtFrom = strfrom;
                objR.dtTo = strto;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo)
        {
            List<CuringShedStockPyroModelItems> Reg = new List<CuringShedStockPyroModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)CuringShedStockPyroService.GetAllCuringShedStockPyro(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new CuringShedStockPyroModelItems
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    opqty = dtUsers.Rows[i]["OQ"].ToString(),
                    rqty = dtUsers.Rows[i]["RQ"].ToString(),
                    piqty = dtUsers.Rows[i]["IQ"].ToString(),
                    riqty = dtUsers.Rows[i]["RIQ"].ToString(),
                    rmiqty = dtUsers.Rows[i]["RMIQ"].ToString(),
                    

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
            //SvSql = " SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ FROM  ( SELECT  '"+dtFrom+"'  DocDate , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) OpQty , 0 RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in  ('MIXING','PACKING') AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND Ls.DocDate < '"+dtFrom+"' GROUP BY TL.LocID UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND TL.LocationType in ('MIXING','PACKING') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition') GROUP BY TL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE') AND Ls.LocID = TL.LocDetailsID AND FL.LocationType NOT in ('MIXING') AND Ls.FromLocID = FL.LocDetailsID AND TL.LocationType  IN ('MIXING','PACKING')  AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'PACKING INPUT' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in ('PACKING') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') GROUP BY TL.LocID , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'DRUM ISSUE' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType NOT IN ('MIXING','PACKING') AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType  IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') AND TL.LocationType IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work') GROUP BY TL.LocationType , Ls.DocDate ) x GROUP BY x.DocDate ORDER BY x.DocDate";
            SvSql = " SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ FROM  (SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND TL.LocationType in ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition') GROUP BY TL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE') AND Ls.LocID = TL.LocDetailsID AND FL.LocationType NOT in ('MIXING') AND Ls.FromLocID = FL.LocDetailsID AND TL.LocationType  IN ('MIXING','PACKING')  AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'PACKING INPUT' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in ('PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') GROUP BY TL.LocID , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'DRUM ISSUE' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType NOT IN ('MIXING','PACKING') AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType  IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') AND TL.LocationType IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work') GROUP BY TL.LocationType , Ls.DocDate ) x GROUP BY x.DocDate ORDER BY x.DocDate";

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
                    wb.Worksheets.Add(dtNew1, "CuringShedStockPyro");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=CuringShedStockPyroDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "CuringShedStockPyroDetails.xlsx");
                    }
                }

            }

        }
    }
}
