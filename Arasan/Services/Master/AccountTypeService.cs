﻿using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccountTypeService : IAccountType
    {
        private readonly string _connectionString;
        public AccountTypeService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<AccountType> GetAllAccountType()
        {
            List<AccountType> cmpList = new List<AccountType>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ACCOUNTTYPEID,ACCOUNTCODE,ACCOUNTTYPE,STATUS from ACCTYPE WHERE STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccountType cmp = new AccountType
                        {
                            ID = rdr["ACCOUNTTYPEID"].ToString(),
                            AccountCode = rdr["ACCOUNTCODE"].ToString(),
                            Accounttype = rdr["ACCOUNTTYPE"].ToString(),
                            Status = rdr["STATUS"].ToString()
                            

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public string AccountTypeCRUD(AccountType ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACCTYPE_PROC", objConn);
                   

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                    }

                    objCmd.Parameters.Add("ACCOUNTCODE", OracleDbType.NVarchar2).Value = ss.AccountCode;
                    objCmd.Parameters.Add("ACCOUNTTYPE", OracleDbType.NVarchar2).Value = ss.Accounttype;
                    objCmd.Parameters.Add("CREATEDON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATEDBY", OracleDbType.NVarchar2).Value = ss.CreatedBy;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE"; 
                    
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        //for edit & del
        public DataTable GetAccountType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPEID, ACCOUNTCODE,ACCOUNTTYPE,STATUS from ACCTYPE where ACCOUNTTYPEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ACCTYPE SET STATUS ='INACTIVE' WHERE ACCOUNTTYPEID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }

    }
}