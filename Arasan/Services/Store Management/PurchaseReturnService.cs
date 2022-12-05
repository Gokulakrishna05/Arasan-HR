using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Arasan.Interface;

namespace Arasan.Services
{
    public class PurchaseReturnService : IPurchaseReturn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        //public DataTable GetItem()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBGROUPCODE";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetPO()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,POBASICID from POBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPODetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UNITMAST.UNITID,CF,QTY,RATE,AMOUNT,ITEMMASTER.ITEMID,DISCPER,DISCAMT,FREIGHTCHGS,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT FROM PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE POBASICID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
