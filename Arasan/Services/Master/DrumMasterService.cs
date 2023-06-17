using Arasan.Interface.Master;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class DrumMasterService : IDrumMaster
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DrumMasterService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<DrumMaster> GetAllDrumMaster()
        {
            List<DrumMaster> cmpList = new List<DrumMaster>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DRUMMASTID,DRUMMAST.DRUMNO,DRUMMAST.CATEGORY,LOCDETAILS.LOCID,DRUMMAST.DRUMTYPE from DRUMMAST LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUMMAST.LOCATION WHERE DRUMMAST.STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumMaster cmp = new DrumMaster
                        {
                            ID = rdr["DRUMMASTID"].ToString(),
                            DrumNo = rdr["DRUMNO"].ToString(),
                            Category = rdr["CATEGORY"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DrumType = rdr["DRUMTYPE"].ToString(),
                            
                            

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "select COLUMN1 from DRUM_CATEGORY WHERE COLUMN2='Y' order by COLUMN1 asc";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetDrumType()
        {
            string SvSql = string.Empty;
            SvSql = "select COLUMN1 from DRUM_DRUMTYPE  order by COLUMN1 asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string DrumMasterCRUD(DrumMaster ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM DRUMMAST WHERE DRUMNO = LTRIM(RTRIM('" + ss.DrumNo + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "DrumNo Already Existed";
                        return msg;
                    }
                }
                else
                {
                    svSQL = " SELECT Count(*) as cnt FROM DRUMMAST WHERE DRUMNO = LTRIM(RTRIM('" + ss.DrumNo + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "DrumNo Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DRUM_PROCEDURE", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DRUM_PROCEDURE";*/

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

                    objCmd.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ss.DrumNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value =ss.DocDate;
                    objCmd.Parameters.Add("CATEGORY", OracleDbType.NVarchar2).Value = ss.Category;
                    objCmd.Parameters.Add("LOCATION", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DRUMTYPE", OracleDbType.NVarchar2).Value = ss.DrumType;
                    objCmd.Parameters.Add("TAREWT", OracleDbType.NVarchar2).Value = ss.TargetWeight;
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
        public DataTable GetDrumMaster(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,to_char(DRUMMAST.DOCDATE,'dd-MON-yyyy') DOCDATE,CATEGORY,LOCATION ,DRUMTYPE,TAREWT from DRUMMAST where DRUMMASTID = '" + id + "' ";
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
                    svSQL = "UPDATE DRUMMAST SET STATUS ='INACTIVE' WHERE DRUMMASTID='" + id + "'";
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
