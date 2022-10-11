using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class HSNcodeService : IHSNcodeService
    {
        private readonly string _connectionString;
        public HSNcodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<HSNcode> GetAllHSNcode()
        {
            List<HSNcode> staList = new List<HSNcode>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select HSNCODE,DESCRIPTION,GST,HSNCODEID from HSNCODE";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        HSNcode sta = new HSNcode
                        {
                            ID = rdr["HSNCODEID"].ToString(),
                            HCode = rdr["HSNCODE"].ToString(),
                            Dec = rdr["DESCRIPTION"].ToString(),
                            Gt = rdr["GST"].ToString()
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        
        public HSNcode GetHSNcodeById(string eid)
        {
            HSNcode HSNcode = new HSNcode();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select HSNCODE,DESCRIPTION,GST,HSNCODEID from HSNCODE where HSNCODEID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        HSNcode sta = new HSNcode
                        {
                            ID = rdr["HSNCODEID"].ToString(),
                            HCode = rdr["HSNCODE"].ToString(),
                            Dec = rdr["DESCRIPTION"].ToString(),
                            Gt = rdr["GST"].ToString()
                        };
                        HSNcode = sta;
                    }
                }
            }
            return HSNcode;
        }

        public string HSNcodeCRUD(HSNcode ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("HSNPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "HSNPROC";*/

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

                    objCmd.Parameters.Add("HSNCODE", OracleDbType.NVarchar2).Value = ss.HCode;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = ss.Dec;
                    objCmd.Parameters.Add("GST", OracleDbType.NVarchar2).Value = ss.Gt;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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

       
    }

}
