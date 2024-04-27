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

        public DataTable GetAccClass()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ACCOUNT_CLASS,ACCCLASSID FROM ACCCLASS WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParent()
        {
            string SvSql = string.Empty;
            SvSql = "select MASTERID,MNAME,MPARENT from master where MPARENT=0";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getchild(string parentid)
        {
            string SvSql = string.Empty;
            SvSql = "select MASTERID,MNAME,MPARENT from master where MPARENT='"+ parentid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPEID,ACCOUNTTYPE from ACCTYPE where IS_ACTIVE='Y' AND ACCCLASSID='"+ id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP where IS_ACTIVE='Y' AND ACCOUNTTYPE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER Where IS_ACTIVE='Y' AND ACCGROUP='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        

    }
}
