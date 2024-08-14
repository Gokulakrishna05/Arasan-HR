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
    public class AssetStockController : Controller
    {
        IAssetStock AssetStockService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public AssetStockController(IAssetStock _AssetStockService, IConfiguration _configuratio)
        {
            AssetStockService = _AssetStockService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AssetStock(string strfrom, string strTo)
        {

            try
            {
                AssetStockModel objR = new AssetStockModel();
                objR.Brlst = BindBranch();
                objR.Loclst = BindLocation("");
                
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo, string Branch, string Location)
        {
            List<AssetStockItems> Reg = new List<AssetStockItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)AssetStockService.GetAllAssetStock(dtFrom, dtTo, Branch, Location);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new AssetStockItems
                {
                    itemid = dtUsers.Rows[i]["ItemID"].ToString(),
                    unitid = dtUsers.Rows[i]["UnitID"].ToString(),
                    branchid = dtUsers.Rows[i]["BranchID"].ToString(),
                    locid = dtUsers.Rows[i]["LocID"].ToString(),
                    oq = dtUsers.Rows[i]["OQ"].ToString(),
                    ov = dtUsers.Rows[i]["OV"].ToString(),
                    rq = dtUsers.Rows[i]["RQ"].ToString(),
                    rv = dtUsers.Rows[i]["RV"].ToString(),
                    iq = dtUsers.Rows[i]["IQ"].ToString(),
                    iv = dtUsers.Rows[i]["IV"].ToString(),





                });
            }

            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
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
        public List<SelectListItem> BindLocation(string Id)
        {
            try
            {
                DataTable dtDesg = AssetStockService.GetLocation(Id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Location"].ToString(), Value = dtDesg.Rows[i]["Location"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetBranchJSON(string ItemId)
        {
            //SubContractingItem model = new SubContractingItem();
            //model.Itemlst = BindItemlst(ItemId);
            return Json(BindLocation(ItemId));

        }

        //public IActionResult ExportToExcel(string dtFrom, string dtTo)
        //{
        //    DataTransactions _datatransactions;
        //    DataTable dtNew1 = new DataTable();

        //    string SvSql = "";
        //    if (dtFrom == null && dtTo == null)
        //    {
        //        SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL FROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.IITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' AND PS.PSBASICID = BC.PSCHNO AND PS.OPITEMID = PSI.ITEMMASTERID GROUP BY  W.WCID , P.PROCESSID , I.ITEMID , U.UNITID UNION SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL FROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.CITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' GROUP BY W.WCID , P.PROCESSID , I.ITEMID, U.UNITID UNION SELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL FROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.OITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID UNION SELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL FROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.WITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ORDER BY 2 , 3 , 10 , 9";

        //    }
        //    else
        //    {
        //        SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL FROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.IITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' AND PS.PSBASICID = BC.PSCHNO AND PS.OPITEMID = PSI.ITEMMASTERID  ";
        //        if (dtFrom != null && dtTo != null)
        //        {
        //            SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'GROUP BY  W.WCID , P.PROCESSID , I.ITEMID , U.UNITID ";
        //        }


        //        //if (WorkCenter != null)
        //        //{
        //        //    SvSql += " and W.WCID='" + WorkCenter + "'";
        //        //}


        //        //if (Process != null)
        //        //{
        //        //    SvSql += " and P.PROCESSID='" + Process + "'";
        //        //}

        //        SvSql += " UNION SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL FROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.CITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' ";
        //        if (dtFrom != null && dtTo != null)
        //        {
        //            SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ";
        //        }


        //        //if (WorkCenter != null)
        //        //{
        //        //    SvSql += " and W.WCID='" + WorkCenter + "'";
        //        //}


        //        //if (Process != null)
        //        //{
        //        //    SvSql += " and P.PROCESSID='" + Process + "'";
        //        //}

        //        SvSql += " UNION SELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL FROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.OITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' ";
        //        if (dtFrom != null && dtTo != null)
        //        {
        //            SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ";
        //        }


        //        //if (WorkCenter != null)
        //        //{
        //        //    SvSql += " and W.WCID='" + WorkCenter + "'";
        //        //}


        //        //if (Process != null)
        //        //{
        //        //    SvSql += " and P.PROCESSID='" + Process + "'";
        //        //}

        //        SvSql += " UNION SELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL FROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.WITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' ";
        //        if (dtFrom != null && dtTo != null)
        //        {
        //            SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ORDER BY 2 , 3 , 10 , 9 ";
        //        }


        //        //if (WorkCenter != null)
        //        //{
        //        //    SvSql += " and W.WCID='" + WorkCenter + "'";
        //        //}


        //        //if (Process != null)
        //        //{
        //        //    SvSql += " and P.PROCESSID='" + Process + "'";
        //        //}
        //    }
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    using (DataTable dtReport = new DataTable())
        //    {
        //        adapter.Fill(dtReport);

        //        DataView dv1 = dtReport.DefaultView;
        //        // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

        //        dtNew1 = dv1.ToTable();
        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dtNew1, "BatchProductionDeatils");


        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                Response.Headers.Add("content-disposition", "attachment;  filename=BatchProductionDeatils.xlsx");
        //                wb.SaveAs(MyMemoryStream);
        //                //MyMemoryStream.WriteTo(Response.OutputStream);
        //                //Response.Flush();
        //                //Response.End();
        //                //wb.SaveAs(MyMemoryStream);
        //                return File(MyMemoryStream.ToArray(), "application/ms-excel", "BatchProductionDeatils.xlsx");
        //            }
        //        }

        //    }

        //}

        public IActionResult ExportToExcel(string dtFrom, string dtTo, string Branch, string Location)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " Select I.ItemID , U.UnitID , Br.BranchID , L.LocID , Sum(x.OQ) OQ , Sum(x.OV) OV , Sum(x.RQ) RQ , Sum(x.RV) RV , Sum(x.IQ) IQ , Sum(x.IV) IV From(Select Sv.ItemID , Sv.LocID , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,-Sv.Qty)) OQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, -Sv.StockValue)) OV , 0 RQ , 0 RV , 0 IQ , 0 IV From AsStockValue Sv Where Sv.DocDate < '" + dtFrom + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID Union All Select Sv.ItemID , Sv.LocID , 0 OQ , 0 OV , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.Qty, 0)) RQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, 0)) RV   , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.Qty, 0)) IQ , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.StockValue, 0)) IV From AsStockValue Sv Where Sv.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID) x,  ItemMaster I, LocDetails L , UnitMast U, BranchMast Br Where x.ItemID = I.ItemMasterID And x.LocID = L.LocDetailsID And L.BranchID = Br.BranchMastID And U.UnitMastID = I.PriUnit And Br.BranchID = '" + Branch + "' And(L.LocID = '" + Location + "' ) Group By Br.BranchID , L.LocID , I.ItemID , U.UnitID Order By Br.BranchID , L.LocID , I.ItemID , U.UnitID";

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
                    wb.Worksheets.Add(dtNew1, "AssetStockDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=AssetStockDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "AssetStockDetails.xlsx");
                    }
                }

            }

        }
    }
}
