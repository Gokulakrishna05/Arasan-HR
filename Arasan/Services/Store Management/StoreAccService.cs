using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Interface.Master
{
    public class StoreAccService : IStoreAccService
    {
        private readonly string _connectionString;
        public StoreAccService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<StoreAcc> GetAllStoreAcc()
        {
            List<StoreAcc> staList = new List<StoreAcc>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DOCID,DOCDATE,REFNO,REFDATE,RETNO,RETDATE,NARRATION,STORESACCBASICID from STORESACCBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreAcc sta = new StoreAcc
                        {
                            ID = rdr["STORESACCBASICID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Refno = rdr["REFNO"].ToString(),
                            Refdate = rdr["REFDATE"].ToString(),
                            Retno = rdr["RETNO"].ToString(),
                            Retdate = rdr["RETDATE"].ToString(),
                            Narr = rdr["NARRATION"].ToString()

                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        public StoreAcc GetStoreAccById(string eid)
        {
            StoreAcc StoreAcc = new StoreAcc();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DOCID,DOCDATE,REFNO,REFDATE,RETNO,RETDATE,NARRATION,STORESACCBASICID  from STORESACCBASIC where STORESACCBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreAcc sta = new StoreAcc
                        {

                            ID = rdr["STORESACCBASICID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Refno = rdr["REFNO"].ToString(),
                            Refdate = rdr["REFDATE"].ToString(),
                            Retno = rdr["RETNO"].ToString(),
                            Retdate = rdr["RETDATE"].ToString(),
                            Narr = rdr["NARRATION"].ToString()
                        };
                        StoreAcc = sta;
                    }
                }
            }
            return StoreAcc;
        }

        public string StoreAccCRUD(StoreAcc ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("STORESACCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STORESACCBASICPROC";*/

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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = ss.Refno;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = ss.Refdate;
                    objCmd.Parameters.Add("RETNO", OracleDbType.NVarchar2).Value = ss.Retno;
                    objCmd.Parameters.Add("RETDATE", OracleDbType.NVarchar2).Value = ss.Retdate;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
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


        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

       
    }
}
