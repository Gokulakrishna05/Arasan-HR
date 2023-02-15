using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services;

public class ProductionEntryService : IProductionEntry
{
    private readonly string _connectionString;
    DataTransactions datatrans;
    public ProductionEntryService(IConfiguration _configuratio)
    {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
    }
    public DataTable ShiftDeatils()
    {
        string SvSql = string.Empty;
        SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
}
