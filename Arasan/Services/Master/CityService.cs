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
    public class CityService : ICityService
    {
        private readonly string _connectionString;
        public CityService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<City> GetAllCity()
        {
            List<City> staList = new List<City>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CITYNAME,STATEID,CITYID,COUNTRYID from CITYMASTER";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        City sta = new City
                        {
                            ID = rdr["CITYID"].ToString(),
                            Cit = rdr["CITYNAME"].ToString(),
                            State = rdr["STATEID"].ToString(),
                            countryid = rdr["COUNTRYID"].ToString()
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID from STATEMAST order by STATEMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public City GetCityById(string eid)
        {
            City City = new City();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CITYNAME,STATEID,CITYID,COUNTRYID from CITYMASTER where CITYID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        City sta = new City
                        {
                            ID = rdr["CITYID"].ToString(),
                            Cit = rdr["CITYNAME"].ToString(),
                            State = rdr["STATEID"].ToString(),
                            countryid = rdr["COUNTRYID"].ToString()
                        };
                        City = sta;
                    }
                }
            }
            return City;
        }
         
        public string CityCRUD(City ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; 
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CITYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "CITYPROC";*/

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

                    objCmd.Parameters.Add("CITYNAME", OracleDbType.NVarchar2).Value = ss.Cit;
                    objCmd.Parameters.Add("STATEID", OracleDbType.NVarchar2).Value = ss.State;
                    objCmd.Parameters.Add("COUNTRYID", OracleDbType.NVarchar2).Value = ss.countryid;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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

