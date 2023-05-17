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
        public DrumCategoryService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
        }

         public IEnumerable<DrumCategory> GetAllDrumCategory()
        {
            List<DrumCategory> cmpList = new List<DrumCategory>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CATEGORYID,CATEGORYTYPE from DRUMMASTER_CATEGORY";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumCategory cmp = new DrumCategory
                        {
                            ID = rdr["CATEGORYID"].ToString(),
                            CategoryType = rdr["CATEGORYTYPE"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
          
        public string DrumCategoryCRUD(DrumCategory ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;

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
                    objCmd.Parameters.Add("CATEGORYTYPE", OracleDbType.NVarchar2).Value = ss.CategoryType;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = ss.Description;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = ss.Status;
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
            SvSql = "Select CATEGORYID,CATEGORYTYPE from DRUMMASTER_CATEGORY where CATEGORYID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
