using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class DrumCategoryService : IDrumCategory
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DrumCategoryService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        // public IEnumerable<DrumCategory> GetAllDrumCategory(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "ACTIVE";
        //    }
        //    List<DrumCategory> cmpList = new List<DrumCategory>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select CATEGORYID,CATEGORYTYPE,DESCRIPTION,STATUS from DRUMMASTER_CATEGORY WHERE STATUS='" + status + "' order by DRUMMASTER_CATEGORY.CATEGORYID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                DrumCategory cmp = new DrumCategory
        //                {
        //                    ID = rdr["CATEGORYID"].ToString(),
        //                    CategoryType = rdr["CATEGORYTYPE"].ToString(),
        //                    Description = rdr["DESCRIPTION"].ToString()

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
          
        public string DrumCategoryCRUD(DrumCategory ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(CATEGORYTYPE) as cnt FROM DRUMMASTER_CATEGORY WHERE CATEGORYTYPE = LTRIM(RTRIM('" + ss.CateType + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Category Type Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DRUMCATE_PRO", objConn);

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
                    objCmd.Parameters.Add("CATEGORYTYPE", OracleDbType.NVarchar2).Value = ss.CateType;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = ss.Description;
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

        public DataTable GetDrumCategory(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CATEGORYID,CATEGORYTYPE,DESCRIPTION from DRUMMASTER_CATEGORY where CATEGORYID = '" + id + "' ";
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
                    svSQL = "UPDATE DRUMMASTER_CATEGORY SET IS_ACTIVE ='N' WHERE CATEGORYID='" + id + "'";
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
                    svSQL = "UPDATE DRUMMASTER_CATEGORY SET IS_ACTIVE ='Y' WHERE CATEGORYID='" + id + "'";
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

        public DataTable GetAllCategory(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select CATEGORYID,CATEGORYTYPE,DESCRIPTION from DRUMMASTER_CATEGORY  WHERE DRUMMASTER_CATEGORY.IS_ACTIVE = 'Y' ORDER BY CATEGORYID DESC";

            }
            else
            {
                SvSql = "Select CATEGORYID,CATEGORYTYPE,DESCRIPTION from DRUMMASTER_CATEGORY  WHERE DRUMMASTER_CATEGORY.IS_ACTIVE = 'N' ORDER BY CATEGORYID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
