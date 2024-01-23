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
    public class ReceiptReportService : IReceiptReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ReceiptReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString); 
        }

        public DataTable GetAllReport(string Branch, string Sdate, string Edate)
        {
            string SvSql = string.Empty;
            //SvSql = "select RECDCBASIC.RECDCBASICID,LOCDETAILS.LOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,BRANCHMAST.BRANCHID,RDELBASIC.DOCID,RECDCDETAIL.ITEMID,UNITMAST.UNITID,RECDCDETAIL.QTY,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RECDCBASIC.DOCID,RECDCDETAIL.REJQTY,RECDCDETAIL.ACCQTY,RECDCDETAIL.PENDQTY from RECDCBASIC INNER JOIN RECDCDETAIL ON RECDCDETAIL.RECDCDETAILID = RECDCBASIC.RECDCBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=RECDCBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID Where RECDCBASIC.BRANCHID='" + Branch + "' AND RECDCBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";
            SvSql = "select RECDCBASIC.RECDCBASICID,LOCDETAILS.LOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,BRANCHMAST.BRANCHID,RDELBASIC.DOCID,RECDCDETAIL.ITEMID,UNITMAST.UNITID,RECDCDETAIL.QTY,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RECDCBASIC.DOCID,RECDCDETAIL.REJQTY,RECDCDETAIL.ACCQTY,RECDCDETAIL.PENDQTY from RECDCBASIC INNER JOIN RECDCDETAIL ON RECDCDETAIL.RECDCDETAILID = RECDCBASIC.RECDCBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=RECDCBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID Where RECDCBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}
