using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Arasan.Interface.Master;

namespace Arasan.Services.Master
{
    public class EmpMultipleAllocationService : IEmpMultipleAllocationService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EmpMultipleAllocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }
        public string EmpMultipleAllocationCRUD(EmpMultipleAllocation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();

                    OracleCommand objCmd = new OracleCommand("EMPALLOCATIONPROC", objConn);
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

                    objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = cy.Emp;
                    objCmd.Parameters.Add("CREATEDDATE", OracleDbType.NVarchar2).Value = cy.EDate;
                    //objCmd.Parameters.Add("CREATEDDATE", OracleDbType.NVarchar2).Value = cy.Location[i];
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    objCmd.ExecuteNonQuery();
                    Object Pid = objCmd.Parameters["OUTID"].Value;

                    for (int i = 0; i < cy.Location.Length; i++)
                    {
                        svSQL = "Insert into EMPALLOCATIONDETAILS (EMPALLOCATIONID,LOCATIONID) VALUES ('"+ Pid + "','"+ cy.Location[i] + "')";
                        OracleCommand objCmddts = new OracleCommand(svSQL, objConn);
                        objCmddts.ExecuteNonQuery();

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

       

        public DataTable GetEmp(string action)
        {
            string SvSql = string.Empty;
            if (action == "insert")
            {
                SvSql = "SELECT EMPMASTID,EMPNAME FROM EMPMAST WHERE EMPMASTID NOT IN (SELECT EMPID from EMPALLOCATION WHERE IS_ACTIVE = 'Y') order by EMPNAME";
            }
            if (action == "update")
            {
                SvSql = "SELECT EMPMASTID,EMPNAME FROM EMPMAST order by EMPNAME";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpAllocation()
        {
            string SvSql = string.Empty;
            SvSql = "select EMPMAST.EMPNAME,to_char(CREATEDDATE,'dd-MON-yyyy') EMPDATE,EMPALLOCATIONID from EMPALLOCATION LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EMPALLOCATION.EMPID Order by EMPALLOCATIONID DESC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMlocation()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LOCDETAILSID,LOCID FROM LOCDETAILS WHERE LOCATIONTYPE='BALL MILL'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public long GetMregion(string regionid, string id)
        {
            string SvSql = "SELECT LOCATIONID LOCID from EMPALLOCATIONDETAILS where LOCATIONID=" + regionid + " and EMPALLOCATIONID=" + id + "";
            DataTable dtCity = new DataTable();
            long user_id = datatrans.GetDataIdlong(SvSql);
            return user_id;
        }

        public DataTable GetEmpMultipleItem(string PRID)
        {
            string SvSql = string.Empty;
            SvSql = "select LOCDETAILS.LOCID,EMPALLOCATIONID from EMPALLOCATIONDETAILS  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EMPALLOCATIONDETAILS.LOCATIONID Order by EMPALLOCATIONDETAILSID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpMultipleAllocationServiceName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMPMAST.EMPNAME,to_char(EMPDATE,'dd-MON-yyyy') EMPDATE,LOCDETAILS.LOCID,EMPALLOCATIONID from EMPALLOCATION LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EMPALLOCATION.EMPNAME LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EMPALLOCATION.LOCATIONNAME Order by EMPALLOCATIONID DESC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmpMultipleAllocationReassign(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(EMPDATE,'dd-MON-yyyy')EMPDATE,LOCDETAILS.LOCID,EMPALLOCATIONID from EMPALLOCATION LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EMPALLOCATION.LOCATIONNAME  Where EMPALLOCATION.EMPALLOCATIONID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ReassignEmpMultipleAllocation(EmpReasign cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EMPALLOCATIONPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "CITYPROC";*/

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

                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.Emp;
                    objCmd.Parameters.Add("EMPDATE", OracleDbType.NVarchar2).Value = cy.EDate;
                    objCmd.Parameters.Add("LOCATIONNAME", OracleDbType.NVarchar2).Value = cy.Location;
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
