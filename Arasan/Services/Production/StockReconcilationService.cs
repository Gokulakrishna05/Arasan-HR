using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
namespace Arasan.Services 
{
    public class StockReconcilationService : IStockReconcilation
    {

        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockReconcilationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select I.ITEMID,L.ITEMID as item from LSTOCKVALUE L,ITEMMASTER I WHERE L.ITEMID=I.ITEMMASTERID AND L.LOCID='" + id + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  I.ITEMID,L.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum(string id, string loc)
        {
            string SvSql = string.Empty;
            SvSql = "Select L.DRUMNO from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.DRUMNO=LT.DRUMNO AND L.ITEMID='" + id + "' AND L.LOCID='" + loc + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.DRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllStockReconcilationDeatils(string strStatus, string strfrom, string strTo)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  DOCID, to_char(DSADDBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,DSADDBASICID,REASON,LOCDETAILS.LOCID   from DSADDBASIC   LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DSADDBASIC.LOCID   WHERE DSADDBASIC.IS_ACTIVE='Y' ORDER BY  DSADDBASICID DESC";
            }
            else
            {
                SvSql = "Select  DOCID, to_char(DSADDBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,DSADDBASICID,REASON,LOCDETAILS.LOCID   from DSADDBASIC   LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DSADDBASIC.LOCID   WHERE DSADDBASIC.IS_ACTIVE='N' ORDER BY  DSADDBASICID DESC";

            }
            if (strfrom == null && strTo == null)
            {
                SvSql = "Select  DOCID, to_char(DSADDBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,DSADDBASICID,REASON,LOCDETAILS.LOCID   from DSADDBASIC   LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DSADDBASIC.LOCID   WHERE DSADDBASIC.IS_ACTIVE='Y' ORDER BY  DSADDBASICID DESC";
            }
            else
            {
                SvSql = "Select  DOCID, to_char(DSADDBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,DSADDBASICID,REASON,LOCDETAILS.LOCID   from DSADDBASIC   LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DSADDBASIC.LOCID   WHERE DSADDBASIC.IS_ACTIVE='Y' ORDER BY  DSADDBASICID DESC";
                if (strfrom != null && strTo != null)
                {
                    SvSql += " WHERE DSADDBASIC.DOCDATE BETWEEN '" + strfrom + "' AND '" + strTo + "'";
                }
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
