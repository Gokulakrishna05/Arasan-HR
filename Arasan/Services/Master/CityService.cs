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
        DataTransactions datatrans;
        public CityService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //public IEnumerable<City> GetAllCity(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "Y";
        //    }
        //    List<City> staList = new List<City>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select CITYNAME,STATEMAST.STATE,CITYID,CONMAST.COUNTRY,CITYMASTER.IS_ACTIVE from CITYMASTER left outer join CONMAST on COUNTRYMASTID=CITYMASTER.COUNTRYID left outer join STATEMAST on STATEMASTID=CITYMASTER.STATEID WHERE CITYMASTER.IS_ACTIVE='" + status + "' order by STATEMAST.STATEMASTID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                City sta = new City
        //                {
        //                    ID = rdr["CITYID"].ToString(),
        //                    Cit = rdr["CITYNAME"].ToString(),
        //                    State = rdr["STATE"].ToString(),
        //                    countryid = rdr["COUNTRY"].ToString(),
        //                    status = rdr["IS_ACTIVE"].ToString()
        //                };
        //                staList.Add(sta);
        //            }
        //        }
        //    }
        //    return staList;
        //}
        public DataTable GetState(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID ,STATEMAST.IS_ACTIVE from  STATEMAST  where STATEMAST.IS_ACTIVE='Y' order by COUNTRYMASTID ";    
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRY,CONMASTID,CONMAST.IS_ACTIVE from CONMAST  WHERE CONMAST.IS_ACTIVE='Y' order by COUNTRY ASC";
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
                    cmd.CommandText = "Select COUNTRYID,STATEID,CITYID,CITYNAME from CITYMASTER where CITYID=" + eid + "";
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
                string svSQL = "";

                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(CITYNAME) as cnt FROM CITYMASTER WHERE CITYNAME = LTRIM(RTRIM('" + ss.Cit + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "City Already Existed";
                        return msg;
                    }
                }
               
                string StaName = datatrans.GetDataString("Select STATE from STATEMAST where STATEMASTID='" + ss.State + "' ");
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

                    objCmd.Parameters.Add("COUNTRY", OracleDbType.NVarchar2).Value = ss.countryid;
                    objCmd.Parameters.Add("STATEID", OracleDbType.NVarchar2).Value = ss.State;
                    objCmd.Parameters.Add("CITYNAME", OracleDbType.NVarchar2).Value = ss.Cit;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
                    if (ss.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = ss.createdby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = ss.createdby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = DateTime.Now;
                    }
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
        public DataTable GetCity(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select CITYNAME,STATEID,CITYID,COUNTRYID from CITYMASTER where CITYID='" + id + "' ";
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
                    svSQL = "UPDATE CITYMASTER SET IS_ACTIVE ='N' WHERE CITYID='" + id + "'";
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

        } public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CITYMASTER SET IS_ACTIVE ='Y' WHERE CITYID='" + id + "'";
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

        public DataTable GetAllCitys(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select CITYMASTER.IS_ACTIVE,CITYNAME,STATEMAST.STATE,CONMAST.COUNTRY,CITYMASTER.CITYID from CITYMASTER left outer join STATEMAST on STATEMAST.STATEMASTID=CITYMASTER.STATEID left outer join CONMAST on CONMAST.CONMASTID=CITYMASTER.COUNTRYID  WHERE CITYMASTER.IS_ACTIVE = 'Y' ORDER BY CITYID DESC ";
            }
            else
            {
                SvSql = "select CITYMASTER.IS_ACTIVE,CITYNAME,STATEMAST.STATE,CONMAST.COUNTRY,CITYMASTER.CITYID from CITYMASTER left outer join STATEMAST on STATEMAST.STATEMASTID=CITYMASTER.STATEID left outer join CONMAST on CONMAST.CONMASTID=CITYMASTER.COUNTRYID  WHERE CITYMASTER.IS_ACTIVE = 'N' ORDER BY CITYID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }  
        
        
       
    }

}

