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
            SvSql = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID Where BALANCE_QTY !=0 GROUP BY ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetBinid()
        {
            string SvSql = string.Empty;

            SvSql = "SELECT CONCAT(BINBASIC.BINID,ITEMMASTER.ITEMID) AS full_name FROM ITEMMASTER LEFT OUTER JOIN BINBASIC ON BINBASICID = ITEMMASTER.BINNO  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDeatils() 
        {
            
            string SvSql = string.Empty;
            SvSql = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,BINBASIC.BINID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN BINBASIC ON BINBASICID = ITEMMASTER.BINNO  LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID Where BALANCE_QTY !=0 AND  LOCDETAILS.LOCID = 'STORES' GROUP BY ITEMMASTER.ITEMID,BINBASIC.BINID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
