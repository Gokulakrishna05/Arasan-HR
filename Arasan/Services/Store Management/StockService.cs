using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class StockService : IStockService
    {
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;

        public StockService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetStockInHand()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,PINDDETAIL.QTY as QTY,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,pindbasic.DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,PINDDETAILID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,PINDDETAIL.ISSUED_QTY,PINDDETAIL.BAL_QTY,PINDBASIC.BRANCHID AS BRANCH_ID from PINDDETAIL LEFT OUTER JOIN pindbasic on pindbasic.PINDBASICID=PINDDETAIL.PINDBASICID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PINDBASIC.BRANCHID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT  WHERE PINDDETAIL.ISISSUED='N'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
