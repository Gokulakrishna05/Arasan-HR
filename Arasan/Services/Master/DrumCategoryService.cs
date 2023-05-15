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
                    cmd.CommandText = "Select DRUMMASTID,CATEGORY from  DRUMMAST";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumCategory cmp = new DrumCategory
                        {
                            ID = rdr["DRUMMASTID"].ToString(),
                            CategoryType = rdr["CATEGORY"].ToString()

                            //Description = rdr["DESCRIPTION"].ToString(),
                            //Status = rdr["STATUS"].ToString(),
                           

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

                //string svSQL = "";

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
