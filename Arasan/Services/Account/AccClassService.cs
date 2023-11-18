using Arasan.Interface;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccClassService : IAccClass
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccClassService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<AccClass> GetAllAccClass(string active)
        {
            if (string.IsNullOrEmpty(active))
            {
                active = "Y";
            }
            List<AccClass> cmpList = new List<AccClass>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select ACCCLASSID,ACCCLASS_CODE,ACCOUNT_CLASS,IS_ACTIVE from ACCCLASS WHERE IS_ACTIVE='" + active + "' order by ACCCLASS.ACCCLASSID DESC  ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccClass cmp = new AccClass
                        {
                            ID = rdr["ACCCLASSID"].ToString(),
                            Accounttype = rdr["ACCCLASS_CODE"].ToString(),
                            Accountclass = rdr["ACCOUNT_CLASS"].ToString(),
                            status = rdr["IS_ACTIVE"].ToString()


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public string AccClassCRUD(AccClass ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(ACCCLASS_CODE) as cnt FROM ACCCLASS WHERE ACCCLASS_CODE = LTRIM(RTRIM('" + ss.Accountclass + "')) and ACCOUNT_CLASS =LTRIM(RTRIM('" + ss.Accounttype + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Account Code Already Existed";
                        return msg;
                    }
                }


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACCCLASSPROC", objConn);


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

                    
                    objCmd.Parameters.Add("ACCOUNT_CLASS", OracleDbType.NVarchar2).Value = ss.Accountclass;
                    objCmd.Parameters.Add("ACCCLASS_CODE", OracleDbType.NVarchar2).Value = ss.Accounttype;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";

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

        public DataTable GetAccClass(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCCLASS_CODE,ACCOUNT_CLASS from ACCCLASS where ACCCLASSID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetType()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT ACCOUNTTYPEID,ACCOUNTTYPE FROM ACCTYPE ";
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
                    svSQL = "UPDATE ACCCLASS SET IS_ACTIVE ='N' WHERE ACCCLASSID='" + id + "'";
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
                    svSQL = "UPDATE ACCCLASS SET IS_ACTIVE ='Y' WHERE ACCCLASSID='" + id + "'";
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

        public DataTable GetAllClass()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCCLASSID,ACCCLASS_CODE,ACCOUNT_CLASS from ACCCLASS  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
