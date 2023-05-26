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
                    cmd.CommandText = "Select HSNCODEID,HSNCODE,DESCRIPTION,CGST,SGST,IGST,STATUS from HSNCODE WHERE STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        HSNcode sta = new HSNcode
                        {
                            ID = rdr["HSNCODEID"].ToString(),
                            HCode = rdr["HSNCODE"].ToString(),
                            Dec = rdr["DESCRIPTION"].ToString(),
                            CGst = rdr["CGST"].ToString(),
                            SGst = rdr["SGST"].ToString(),
                            IGst = rdr["IGST"].ToString() 
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
                    cmd.CommandText = "Select HSNCODEID,HSNCODE,DESCRIPTION,CGST,SGST,IGST from HSNCODE where HSNCODEID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        HSNcode sta = new HSNcode
                        {
                            ID = rdr["HSNCODEID"].ToString(),
                            HCode = rdr["HSNCODE"].ToString(),
                            Dec = rdr["DESCRIPTION"].ToString(),
                            CGst = rdr["CGST"].ToString(),
                            SGst = rdr["SGST"].ToString(),
                            IGst = rdr["IGST"].ToString()
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
                string StatementType = string.Empty;
                //string svSQL = "";

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
                    objCmd.Parameters.Add("CGST", OracleDbType.NVarchar2).Value = ss.CGst;
                    objCmd.Parameters.Add("SGST", OracleDbType.NVarchar2).Value = ss.SGst;
                    objCmd.Parameters.Add("IGST", OracleDbType.NVarchar2).Value = ss.IGst;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public DataTable GetHSNcode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION,CGST,SGST,IGST from HSNCODE where HSNCODEID = '" + id + "' ";
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
                    svSQL = "UPDATE HSNCODE SET STATUS ='INACTIVE' WHERE HSNCODEID='" + id + "'";
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