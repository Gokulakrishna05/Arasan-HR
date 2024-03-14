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
    public class MaterialSplitService :IMaterialSplit
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public MaterialSplitService(IConfiguration _configuratio)
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
    }
}
