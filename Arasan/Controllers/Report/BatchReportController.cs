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
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers.Report
{
    public class BatchReportController : Controller
    {
        IBatchReportService BatchReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public BatchReportController(IBatchReportService _BatchReportService, IConfiguration _configuratio)
        {
            BatchReportService = _BatchReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchReport(string strfrom, string strTo)
        {
            try
            {
                BatchReport objR = new BatchReport();
                //objR.Worklst = BindWorkCenter();
                //objR.Processlst = BindProcess();
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListWorkCenterReport()
        {
            return View();
        }
        public IActionResult SchReport(string strfrom, string strTo)
        {
            try
            {
                BatchReport br = new BatchReport();
                br.Worklst = BindWorkCenter();
                br.Processlst = BindProcess();
                br.Pschlst = BindPschno();
                br.dtFrom = strfrom;
                br.dtTo = strTo;
                return View(br);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListSchReport()
        {
            return View();
        }
       
        public ActionResult MyListSchReportGrid(string dtFrom, string dtTo ,string WorkCenter, string Process, string Pschno)
        {
            List<SchReportItems> Reg = new List<SchReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)BatchReportService.GetAllSchReportItems(dtFrom, dtTo,WorkCenter, Process,Pschno);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new SchReportItems
                {

                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
                    processid = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    shiftid = dtUsers.Rows[i]["SHIFT"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    drumid = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    ibatchid = dtUsers.Rows[i]["IBATCHNO"].ToString(),
                    qtyid = dtUsers.Rows[i]["IQTY"].ToString(),
                    rate = dtUsers.Rows[i]["IRATE"].ToString(),
                    amount = dtUsers.Rows[i]["IAMOUNT"].ToString(),
                    schno = dtUsers.Rows[i]["PSCHNO"].ToString(),
                    batch = dtUsers.Rows[i]["BATCH"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }
         public IActionResult ExportToExcelSchReport(string dtFrom, string dtTo, string WorkCenter, string Process,string Pschno)
         {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            if (dtFrom == null && dtTo == null && WorkCenter == null && Process == null && Pschno == null)
            {
                SvSql = "Select to_char(B.DocDate,'dd-MON-yyyy')DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo DrumNo , D.IBatchNo , Sum(D.IQty) IQty , Avg(D.IRate) IRate, Sum(D.IAmount) IAmount , Sb.DocID PSchNo , B.Batch From bProdBasic B , bProdInpDet D , ItemMaster I , WcBasic W , ProcessMast P , PSBasic Sb , DrumMast Dm  Where B.bProdBasicID = D.bProdBasicID And Sb.PSBasicID = B.PSchNo And D.IItemID = I.ItemMasterID And Dm.DrumMastID (+) = D.IDrumNo And B.WcID = W.WcBasicID And B.ProcessID = P.ProcessMastID Group By B.DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo, Sb.DocID , B.Batch Order By P.ProcessID , W.WcID , B.DocDate , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo , B.Batch";

            }
            else
            {
                SvSql = "Select to_char(B.DocDate,'dd-MON-yyyy')DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo DrumNo , D.IBatchNo , Sum(D.IQty) IQty , Avg(D.IRate) IRate, Sum(D.IAmount) IAmount , Sb.DocID PSchNo , B.Batch From bProdBasic B , bProdInpDet D , ItemMaster I , WcBasic W , ProcessMast P , PSBasic Sb , DrumMast Dm  Where B.bProdBasicID = D.bProdBasicID And Sb.PSBasicID = B.PSchNo And D.IItemID = I.ItemMasterID And Dm.DrumMastID (+) = D.IDrumNo And B.WcID = W.WcBasicID";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' Group By B.DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo, Sb.DocID , B.Batch Order By P.ProcessID , W.WcID , B.DocDate , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo , B.Batch ";
                }


                if (WorkCenter != null)
                {
                    SvSql += " and W.WcID='" + WorkCenter + "' Group By B.DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo, Sb.DocID , B.Batch Order By P.ProcessID , W.WcID , B.DocDate , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo , B.Batch ";
                }


                if (Process != null)
                {
                    SvSql += " and P.ProcessID='" + Process + "' Group By B.DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo, Sb.DocID , B.Batch Order By P.ProcessID , W.WcID , B.DocDate , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo , B.Batch ";
                }

                if (Pschno != null)
                {
                    SvSql += " and Sb.DocID='" + Pschno + "' Group By B.DocDate , W.WcID , P.ProcessID , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo, Sb.DocID , B.Batch Order By P.ProcessID , W.WcID , B.DocDate , B.Shift , I.ItemID , Dm.DrumNo , D.IBatchNo , B.Batch ";
                }

            }
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "BatchProductionRepSchedulewise");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=BatchProductionRepSchedulewise.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "BatchProductionRepSchedulewise.xlsx");
                    }
                }

            }

         }
        private IActionResult File(object excelData, string v)
        {
            throw new NotImplementedException();
        }
        public List<SelectListItem> BindPschno()
        {
            try
            {
                DataTable dtDesg = BatchReportService.GetPschno();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Prod_Sch_No"].ToString(), Value = dtDesg.Rows[i]["Prod_Sch_No"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = BatchReportService.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = BatchReportService.GetProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListBatchReportGrid(string dtFrom, string dtTo)
        {
            List<BatchReportItems> Reg = new List<BatchReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)BatchReportService.GetAllBatchReport(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new BatchReportItems
                {

                    type = dtUsers.Rows[i]["ETYPE"].ToString(),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
                    process = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    seq = dtUsers.Rows[i]["SEQ"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
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
            if (dtFrom == null && dtTo == null)
            {
                SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL FROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.IITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' AND PS.PSBASICID = BC.PSCHNO AND PS.OPITEMID = PSI.ITEMMASTERID GROUP BY  W.WCID , P.PROCESSID , I.ITEMID , U.UNITID UNION SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL FROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.CITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' GROUP BY W.WCID , P.PROCESSID , I.ITEMID, U.UNITID UNION SELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL FROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.OITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID UNION SELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL FROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.WITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ORDER BY 2 , 3 , 10 , 9";

            }
            else
            {
                SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL FROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.IITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' AND PS.PSBASICID = BC.PSCHNO AND PS.OPITEMID = PSI.ITEMMASTERID  ";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'GROUP BY  W.WCID , P.PROCESSID , I.ITEMID , U.UNITID ";
                }


                //if (WorkCenter != null)
                //{
                //    SvSql += " and W.WCID='" + WorkCenter + "'";
                //}


                //if (Process != null)
                //{
                //    SvSql += " and P.PROCESSID='" + Process + "'";
                //}

                SvSql += " UNION SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL FROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.CITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'INPUT' ";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ";
                }


                //if (WorkCenter != null)
                //{
                //    SvSql += " and W.WCID='" + WorkCenter + "'";
                //}


                //if (Process != null)
                //{
                //    SvSql += " and P.PROCESSID='" + Process + "'";
                //}

                SvSql += " UNION SELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL FROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.OITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' ";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ";
                }


                //if (WorkCenter != null)
                //{
                //    SvSql += " and W.WCID='" + WorkCenter + "'";
                //}


                //if (Process != null)
                //{
                //    SvSql += " and P.PROCESSID='" + Process + "'";
                //}

                SvSql += " UNION SELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL FROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U  WHERE B.BPRODBASICID = D.BPRODBASICID AND B.WCID = W.WCBASICID AND B.PROCESSID = P.PROCESSMASTID AND D.WITEMID = I.ITEMMASTERID AND I.PRIUNIT = U.UNITMASTID AND B.BATCH = BC.DOCID AND B.ETYPE = 'OUTPUT' ";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY  W.WCID , P.PROCESSID , I.ITEMID, U.UNITID ORDER BY 2 , 3 , 10 , 9 ";
                }


                //if (WorkCenter != null)
                //{
                //    SvSql += " and W.WCID='" + WorkCenter + "'";
                //}


                //if (Process != null)
                //{
                //    SvSql += " and P.PROCESSID='" + Process + "'";
                //}
            }
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "BatchProductionDeatils");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=BatchProductionDeatils.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "BatchProductionDeatils.xlsx");
                    }
                }

            }

        }
        //private IActionResult File(object excelData, string v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
