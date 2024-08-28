using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services
{
    public class BatchProductionSummaryReportBatchwiseService : IBatchProductionSummaryReportBatchwise
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchProductionSummaryReportBatchwiseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllBatchProductionSummaryReportBatchwise(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                SvSql = "SELECT B.BATCH , 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, \r\n       I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , BC.DOCDATE , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , PSI.ITEMID BITEM\r\nFROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.IITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nAND B.ETYPE = 'INPUT'\r\nAND PS.PSBASICID = BC.PSCHNO\r\nAND PS.OPITEMID = PSI.ITEMMASTERID\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      B.BATCH , W.WCID , P.PROCESSID , I.ITEMID , U.UNITID, BC.DOCDATE , PSI.ITEMID \r\nUNION\r\nSELECT B.BATCH , 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , BC.DOCDATE,NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.CITEMID = I.ITEMMASTERID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'INPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      B.BATCH , W.WCID , P.PROCESSID , I.ITEMID, U.UNITID , BC.DOCDATE \r\nUNION\r\nSELECT B.BATCH , 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) , BC.DOCDATE , NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.OITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      B.BATCH , W.WCID , P.PROCESSID , I.ITEMID, U.UNITID , BC.DOCDATE \r\nUNION\r\nSELECT B.BATCH , 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , BC.DOCDATE ,NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.WITEMID = I.ITEMMASTERID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      B.BATCH , W.WCID , P.PROCESSID , I.ITEMID, U.UNITID , BC.DOCDATE \r\nUNION\r\nSELECT B.DOCID , 'OUTPUT RATIO' , W.WCID , P.PROCESSID , 1 , 'OUTPUT RATIO   ' || TO_CHAR(ROUND(((O.OQTY*100)/I.IQTY),2) , '990.00') || ' %' , ' ' , 0  ,0 , B.DOCDATE, NULL MTONO , NULL\r\nFROM BCPRODBASIC B , WCBASIC W , PROCESSMAST P , SELECTEDVALUES S , \r\n(\r\nSELECT B.BATCH , SUM(D.OQTY) OQTY , SUM(D.OXQTY) EXQTY\r\nFROM BPRODBASIC B , BPRODOUTDET D\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nGROUP BY\r\n      B.BATCH\r\n) O ,\r\n(\r\nSELECT B.BATCH , SUM(D.IQTY) IQTY \r\nFROM BPRODBASIC B , BPRODINPDET D\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' And '"+dtTo+"'\r\nGROUP BY\r\n      B.BATCH\r\n) I\r\nWHERE B.WCID = W.WCBASICID\r\nAND B.WPROCESSID = P.PROCESSMASTID\r\nAND B.DOCID = O.BATCH \r\nAND B.DOCID = I.BATCH\r\nAND B.BCPRODBASICID = S.SELECTEDID\r\nORDER BY 1 , 2 , 3 , 12 , 11";
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
