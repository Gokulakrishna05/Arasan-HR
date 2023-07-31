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
    public class StateService : IStateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<State> GetAllState(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }
            List<State> staList = new List<State>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select STATE,STCODE,CONMAST.COUNTRY,STATEMASTID ,STATEMAST.STATUS from  STATEMAST LEFT OUTER JOIN CONMAST ON CONMAST.COUNTRYMASTID=STATEMAST.COUNTRYMASTID WHERE STATEMAST.STATUS ='" + status + "' order by STATEMAST.STATEMASTID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        State sta = new State
                        {
                            ID = rdr["STATEMASTID"].ToString(),
                            StateName = rdr["STATE"].ToString(),
                            StateCode = rdr["STCODE"].ToString(),
                            countryid = rdr["COUNTRY"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            //SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST  WHERE STATUS='ACTIVE' order by COUNTRYMASTID asc" ;
            SvSql = "select COUNTRY,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditState(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STCODE,COUNTRYMASTID,STATEMASTID from  STATEMAST  where STATEMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public State GetStateById(string eid)
        {
            State State = new State();
            using (OracleConnection con = new OracleConnection(_connectionString)) 
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select STATE,STCODE,COUNTRYMASTID,STATEMASTID from STATEMAST where STATEMASTID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        State sta = new State
                        {
                            ID = rdr["STATEMASTID"].ToString(),
                            StateName = rdr["STATE"].ToString(),
                            StateCode = rdr["STCODE"].ToString(),
                            countryid = rdr["COUNTRYMASTID"].ToString()
                        };
                        State = sta;
                    }
                }
            }
            return State;
        }

        public string StateCRUD(State ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(STATE) as cnt FROM STATEMAST WHERE STATE =LTRIM(RTRIM('" + ss.StateName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "State Already Existed";
                        return msg;
                    }
                }
                

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("STATEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STATEPROC";*/

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

                    objCmd.Parameters.Add("STATE", OracleDbType.NVarchar2).Value = ss.StateName;
                    objCmd.Parameters.Add("STCODE", OracleDbType.NVarchar2).Value = ss.StateCode;
                    objCmd.Parameters.Add("COUNTRYMASTID", OracleDbType.NVarchar2).Value = ss.countryid;
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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE STATEMAST SET STATUS ='INACTIVE' WHERE STATEMASTID='" + id + "' ";
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
                    svSQL = "UPDATE STATEMAST SET STATUS ='ACTIVE' WHERE STATEMASTID='" + id + "'";
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
