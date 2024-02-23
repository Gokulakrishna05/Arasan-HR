using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;

using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Diagnostics;

namespace Arasan.Services.Report
{
    public class BatchReportService : IBatchReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //public DataTable GetWorkCenter()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select WCID,WCBASICID from WCBASIC ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetProcess()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select PROCESSID ,PROCESSMASTID from PROCESSMAST";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcess()
        {
            string SvSql = string.Empty;
            SvSql = "Select distinct P.ProcessMastID , P.ProcessID From ProcessMast P ,bprodbasic b Where Upper(P.BatchYN) = 'Y' and b.PROCESSID = p.PROCESSMASTID Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPschno()
        {
            string SvSql = string.Empty;
            SvSql = "Select unique B.PsBasicID , B.DocID \"Prod_Sch_No\" From PsBasic B , ProcessMast P , WcBasic W,bprodbasic bp Where B.ProcessID = P.ProcessMastID And B.WcID = W.WcBasicID and b.PsBasicID = bp.pschno";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllSchReportItems(string dtFrom, string dtTo, string WorkCenter, string Process, string Pschno)
        {

            try
            {
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
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllBatchReport(string dtFrom, string dtTo)
        {

            try
            {
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
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
