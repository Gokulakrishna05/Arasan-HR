using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class AccTreeviewService : IAccTreeView
    {
        private readonly string _connectionString;
        public AccTreeviewService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetAccType()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPEID,ACCOUNTTYPE from ACCTYPE where STATUS='Active'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP where STATUS='Active' AND ACCTYPE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from LEDGER Where STATUS='Active' AND ACCGROUP='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        

    }
}
