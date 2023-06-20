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
    public class CuringService : ICuringService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CuringService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetCuring()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCDETAILSID,LOCID from LOCDETAILS where LOCID = 'CURING'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSubgroup()
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBGROUP from CURINGSUBGROUPMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<Curing> GetAllCuring(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<Curing> cmpList = new List<Curing>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();

                    cmd.CommandText = "Select LOCDETAILS.LOCID,SUBGROUP,SHEDNUMBER,CAPACITY,CURINGMASTERID  from CURINGMASTER LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID = CURINGMASTER.LOCATIONID WHERE CURINGMASTER.STATUS='" + status + "' order by CURINGMASTER.CURINGMASTERID DESC ";

                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Curing cmp = new Curing
                        {
                            ID = rdr["CURINGMASTERID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Sub = rdr["SUBGROUP"].ToString(),
                            Shed = rdr["SHEDNUMBER"].ToString(),
                            Cap = rdr["CAPACITY"].ToString(),
                            status = rdr["STATUS"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public Curing GetCuringById(string eid)
        {
            Curing Curing = new Curing();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select LOCATIONID,SUBGROUP,SHEDNUMBER,CAPACITY,STATUS,CURINGMASTERID from CURINGMASTER where CURINGMASTERID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Curing cmp = new Curing
                        {
                            ID = rdr["CURINGMASTERID"].ToString(),
                            Location = rdr["LOCATIONID"].ToString(),
                            Sub = rdr["SUBGROUP"].ToString(),
                            Shed = rdr["SHEDNUMBER"].ToString(),
                            Cap = rdr["CAPACITY"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        Curing = cmp;
                    }
                }
            }
            return Curing;
        }
        public string CuringCRUD(Curing cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;

                string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM CURINGMASTER WHERE SHEDNUMBER = LTRIM(RTRIM('" + cy.Shed + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Curing Already Existed";
                        return msg;
                    }
                }
                else
                {
                    svSQL = " SELECT Count(*) as cnt FROM CURINGMASTER WHERE SHEDNUMBER = LTRIM(RTRIM('" + cy.Shed + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Curing Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CURINGMASTERPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "COUNTRYPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    }

                    objCmd.Parameters.Add("LOCATIONID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("SUBGROUP", OracleDbType.NVarchar2).Value = cy.Sub;
                    objCmd.Parameters.Add("SHEDNUMBER", OracleDbType.NVarchar2).Value = cy.Shed;
                    objCmd.Parameters.Add("CAPACITY", OracleDbType.NVarchar2).Value = cy.Cap;
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

        public DataTable GetCuringDeatil(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CURINGMASTERID,LOCDETAILS.LOCID,SUBGROUP,SHEDNUMBER,CAPACITY from CURINGMASTER LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID = CURINGMASTER.LOCATIONID where CURINGMASTERID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCuringDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CURINGMASTERID,LOCATIONID,SUBGROUP,SHEDNUMBER,CAPACITY from CURINGMASTER where CURINGMASTERID='" + id + "' ";
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
                    svSQL = "UPDATE CURINGMASTER SET STATUS ='INACTIVE' WHERE CURINGMASTERID='" + id + "'";
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
