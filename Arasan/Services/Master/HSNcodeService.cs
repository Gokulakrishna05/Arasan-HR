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
        DataTransactions datatrans;
        public HSNcodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<HSNcode> GetAllHSNcode(string status)
        {
            List<HSNcode> staList = new List<HSNcode>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select HSNCODEID,HSNCODE,DESCRIPTION,CGST,SGST,IGST,STATUS from HSNCODE WHERE STATUS= 'ACTIVE' order by HSNCODE.HSNCODEID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        HSNcode sta = new HSNcode
                        {
                            ID = rdr["HSNCODEID"].ToString(),
                            HCode = rdr["HSNCODE"].ToString(),
                            Dec = rdr["DESCRIPTION"].ToString(),
                           
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

         
        public string HSNcodeCRUD(HSNcode ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                string sv = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(HSNCODE) as cnt FROM HSNCODE WHERE HSNCODE = LTRIM(RTRIM('" + ss.HCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "HsnCode Already Existed";
                        return msg;
                    }
                }

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

                    objCmd.Parameters.Add("ISACTIVE", OracleDbType.NVarchar2).Value = "Y";
                    if (ss.ID == null)
                    {
                       
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                    }
                   
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;

                        if (ss.ID != null)
                        {
                            Pid = ss.ID;

                            sv = "DELETE HSNROW WHERE HSNCODEID = '" + Pid + "' ";
                            OracleCommand objCmdd = new OracleCommand(sv, objConn);
                            objCmdd.ExecuteNonQuery();
                        }
                        foreach (HSNItem cp in ss.hsnlst)
                        {
                            if (cp.Isvalid == "Y" && cp.tariff != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("HSNROWPROC", objConns);
                                   
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                   
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("HSNCODEID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("TARIFFID", OracleDbType.NVarchar2).Value = cp.tariff;
                                   

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }

                        }
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

        public DataTable GettariffItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID from HSNROW where HSNROW.HSNCODEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Gettariff()
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID,TARIFFMASTERID from TARIFFMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetHSNcode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION from HSNCODE where HSNCODEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetAllhsncode(string strStatus)
        //{
        //    string SvSql = string.Empty;
        //    if (strStatus == "Y" || strStatus == null)
        //    {
        //        SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION from HSNCODE WHERE ISACTIVE='Y' Order by HSNCODEID DESC  ";
        //    }
        //    else
        //    {
        //        SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION from HSNCODE WHERE ISACTIVE='N' Order by HSNCODEID DESC  ";

        //    }
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public DataTable GetAllhsncode()
        {
            string SvSql = string.Empty;
           
          
                SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION from HSNCODE WHERE ISACTIVE='Y' Order by HSNCODEID DESC  ";
          
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Gethsnitem(string PRID)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFMASTER.TARIFFID,HSNCODEID from HSNROW  LEFT OUTER JOIN TARIFFMASTER ON TARIFFMASTER.TARIFFMASTERID = HSNROW.TARIFFID Order by HSNCODEID DESC";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetCGst()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select * from TAXMAST where TAX='CGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetSGst()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select * from TAXMAST where TAX='SGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetIGst()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select * from TAXMAST where TAX='IGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE HSNCODE SET ISACTIVE ='N' WHERE HSNCODEID='" + id + "'";
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
        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE HSNCODE SET ISACTIVE ='Y' WHERE HSNCODEID='" + id + "'";
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