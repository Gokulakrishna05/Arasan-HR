using Arasan.Interface;

using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Account
{
    public class AccountTypeService : IAccDescrip
    {
        //    private readonly string _connectionString;
        //    DataTransactions datatrans;
        //    public AccountTypeService(IConfiguration _configuration)
        //    {
        //        _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        //        datatrans = new DataTransactions(_connectionString);
        //    }

        //    public IEnumerable<AccountType> GetAllAccDescrip(string status)
        //    {
        //        if (string.IsNullOrEmpty(status))
        //        {
        //            status = "ACTIVE";
        //        }
        //        List<AccDescrip> cmpList = new List<AccDescrip>();
        //        using (OracleConnection con = new OracleConnection(_connectionString))
        //        {

        //            using (OracleCommand cmd = con.CreateCommand())
        //            {
        //                con.Open();
        //                cmd.CommandText = "Select BRANCHMAST.BRANCHID,ADTRANSID,ADTRANSDESC,ADSCHEME,ADSCHEMEDESC from ADCOMPH LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ADCOMPH.BRANCHID ";
        //                OracleDataReader rdr = cmd.ExecuteReader();
        //                while (rdr.Read())
        //                {
        //                    AccountType cmp = new AccountType
        //                    {
        //                        ID = rdr["ADCOMPHID"].ToString(),

        //                        TransName = rdr["ADTRANSDESC"].ToString();
        //                        TransID = rdr["ADTRANSID"].ToString();
        //                        Scheme = rdr["ADSCHEME"].ToString();
        //                        Descrip = rdr["ADSCHEMEDESC"].ToString();

        //                };
        //                    cmpList.Add(cmp);
        //                }
        //            }
        //        }
        //        return cmpList;
        //}

        //    public DataTable GetAccDescrip(string id)
        //    {
        //        string SvSql = string.Empty;
        //        SvSql = "select ACCOUNTTYPEID, ACCOUNTCODE,ACCOUNTTYPE,STATUS from ACCTYPE where ACCOUNTTYPEID = '" + id + "' ";
        //        DataTable dtt = new DataTable();
        //        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //        adapter.Fill(dtt);
        //        return dtt;
        //    }
    }
}