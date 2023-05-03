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
        public CuringService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        //public DataTable GetSubgroup()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select SUBGROUP from CURINGSUBGROUPMAST";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public IEnumerable<Curing> GetAllCuring()
        {
            List<Curing> cmpList = new List<Curing>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select LOCDETAILS.LOCID,SUBGROUP,SHEDNUMBER,NOFDAYS,CURINGMASTERID  from CURINGMASTER LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID = CURINGMASTER.LOCATIONID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Curing cmp = new Curing
                        {
                            ID = rdr["CURINGMASTERID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Sub = rdr["SUBGROUP"].ToString(),
                            Shed = rdr["SHEDNUMBER"].ToString(),
                            Days = rdr["NOFDAYS"].ToString(),
                            //Status = rdr["STATUS"].ToString()
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
                    cmd.CommandText = "Select LOCATIONID,SUBGROUP,SHEDNUMBER,NOFDAYS,CURINGMASTERID from CURINGMASTER where CURINGMASTERID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Curing cmp = new Curing
                        {
                            ID = rdr["CURINGMASTERID"].ToString(),
                            Location = rdr["LOCATIONID"].ToString(),
                            Sub = rdr["SUBGROUP"].ToString(),
                            Shed = rdr["SHEDNUMBER"].ToString(),
                            Days = rdr["NOFDAYS"].ToString()
                            //Status = rdr["STATUS"].ToString()
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

                //string svSQL = "";

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
                    objCmd.Parameters.Add("NOFDAYS", OracleDbType.NVarchar2).Value = cy.Days;
                    //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = cy.Status;
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
    }
}
