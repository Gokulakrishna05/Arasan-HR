using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services 
{
    public class PackDrumAllocationService :IPackDrumAllocation
    {
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;

        public PackDrumAllocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetLoc()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCID,LOCDETAILSID from LOCDETAILS where LOCID IN ('APS PACKING','DG PASTE PACKING','PACKING','PASTE PACKING','POLISH PACKING','PYRO PACKING') ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public DataTable GetDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select SPREFIX,STARTNO,LASTNO,PDABASICID from PDABASIC where PACKLOCID ='"+id+"' ORDER BY LASTNO DESC fetch  first rows only ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
