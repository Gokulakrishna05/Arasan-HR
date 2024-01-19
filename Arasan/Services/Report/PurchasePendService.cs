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

namespace Arasan.Services
{
    public class PurchasePendService : IPurchasePend
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchasePendService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllReport(string Branch, string Sdate, string Edate)
        {
            string SvSql = string.Empty;
            SvSql = "select ((QTY+RETQTY)-(nvl(GRNQTY,0)+SHCLQTY+PINDDETAIL.POQTY)) PEND_QTY,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy')DUEDATE ,PINDDETAIL.GRNQTY ,PINDBASIC.PINDBASICID,PINDBASIC.DOCID,to_char(PINDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,ITEMMASTER.ITEMID,PINDDETAIL.QTY  from PINDBASIC INNER JOIN PINDDETAIL ON PINDDETAIL.PINDDETAILID = PINDBASIC.PINDBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=PINDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = PINDBASIC.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = PINDDETAIL.ITEMID Where PINDBASIC.BRANCHID ='" + Branch + "' AND PINDBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'  ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
