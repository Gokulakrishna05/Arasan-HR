using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PrivilegesService : IPrivilegesService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PrivilegesService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetParent()
        {
            string SvSql = string.Empty;
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT=0";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getchild(string parentid)
        {
            string SvSql = string.Empty;
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT='" + parentid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
