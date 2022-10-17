using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class MaterialRequisitionService : IMaterialRequisition
    {
        private readonly string _connectionString;

        public MaterialRequisitionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetWorkCenter(string LocationId)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCBASIC.ILOCATION,WCID LOCDETAILS from WCBASIC LEFT OUTER JOIN LOCDETAILS on LOCDETAILS.LOCDETAILSID = WCBASIC.ILOCATION where LOCDETAILS.LOCDETAILSID = '" + LocationId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}
