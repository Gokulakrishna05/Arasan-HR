
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
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services.Report
{
    public class BatchProductionSummaryService : IBatchProductionSummary
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchProductionSummaryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllBatchProductionSummary(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                SvSql = "SELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 SEQ, \r\n       I.ITEMID, U.UNITID , SUM(D.IPRIQTY) QTY   , SUM(D.IQTY) WIPQTY , DECODE(AVG(BC.MTONO),0,NULL,NULL,NULL,'Cir. No : '||TO_CHAR(AVG(BC.MTONO))) MTONO , NULL\r\nFROM BPRODBASIC B , BPRODINPDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , PSBASIC PS , ITEMMASTER PSI , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.IITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'INPUT'\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND PS.PSBASICID = BC.PSCHNO\r\nAND PS.OPITEMID = PSI.ITEMMASTERID\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID , U.UNITID \r\nUNION\r\nSELECT 'INPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.CONSQTY) QTY    , SUM(D.CONSQTY)  , NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODCONSDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.CITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND B.ETYPE = 'INPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nUNION\r\nSELECT 'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.PRIOQTY) QTY    , SUM(D.OQTY) ,  NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODOUTDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.OITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.BATCH = BC.DOCID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n      W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nUNION\r\nSELECT  'OUTPUT' ETYPE , W.WCID , P.PROCESSID , 1 , \r\n       I.ITEMID, U.UNITID , SUM(D.WQTY) QTY    , SUM(D.WQTY) , NULL MTONO , NULL\r\nFROM BPRODBASIC B , BPRODWASTEDET D , WCBASIC W , PROCESSMAST P , ITEMMASTER I , BCPRODBASIC BC , UNITMAST U , SELECTEDVALUES S\r\nWHERE B.BPRODBASICID = D.BPRODBASICID\r\nAND B.WCID = W.WCBASICID\r\nAND B.PROCESSID = P.PROCESSMASTID\r\nAND D.WITEMID = I.ITEMMASTERID\r\nAND I.PRIUNIT = U.UNITMASTID\r\nAND B.DOCDATE BETWEEN '"+dtFrom+"' AND :ED\r\nAND B.BATCH = BC.DOCID\r\nAND B.ETYPE = 'OUTPUT'\r\nAND BC.BCPRODBASICID = S.SELECTEDID\r\nGROUP BY \r\n       W.WCID , P.PROCESSID , I.ITEMID, U.UNITID\r\nORDER BY 2 , 3 , 10 , 9";

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
