using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccountGroupService : IAccountGroup
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccountGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<AccountGroup> GetAllAccountGroup()
        {
            List<AccountGroup> cmpList = new List<AccountGroup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    //cmd.CommandText = "Select BRANCHMAST.BRANCHID,ACCOUNTGROUP,ACCOUNTTYPE,GROUPCODE,DISPLAY_NAME,ACCGROUP.STATUS,ACCGROUPID from ACCGROUP LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ACCGROUP.BRANCHID where ACCGROUP.STATUS='Active'";
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,ACCOUNTGROUP,ACCTYPE.ACCOUNTTYPE,GROUPCODE,DISPLAY_NAME,ACCGROUP.STATUS,ACCGROUPID from ACCGROUP LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ACCGROUP.BRANCHID LEFT OUTER JOIN ACCTYPE ON ACCOUNTTYPEID =ACCGROUP.ACCOUNTTYPE where ACCGROUP.STATUS='Active'\r\n";
                   
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccountGroup cmp = new AccountGroup
                        {
                            ID = rdr["ACCGROUPID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            AccGroup = rdr["ACCOUNTGROUP"].ToString(),
                            AType= rdr["ACCOUNTTYPE"].ToString(),
                            GCode = rdr["GROUPCODE"].ToString(),
                            Display = rdr["DISPLAY_NAME"].ToString(),
                            Status = rdr["STATUS"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string AccountGroupCRUD(AccountGroup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM ACCGROUP WHERE ACCOUNTGROUP =LTRIM(RTRIM('" + cy.AccGroup + "')) and GROUPCODE =LTRIM(RTRIM('" + cy.GCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Account Group Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACCGROPROC", objConn);
                   
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

                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ACCOUNTGROUP", OracleDbType.NVarchar2).Value = cy.AccGroup;
                    objCmd.Parameters.Add("ACCOUNTTYPE", OracleDbType.NVarchar2).Value = cy.AType;
                    objCmd.Parameters.Add("GROUPCODE", OracleDbType.NVarchar2).Value = cy.GCode;
                    objCmd.Parameters.Add("DISPLAY_NAME", OracleDbType.NVarchar2).Value = cy.Display;

                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Active";
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
                    svSQL = "UPDATE ACCGROUP SET STATUS ='InActive' WHERE ACCGROUPID='" + id + "'";
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
        public DataTable GetAccountGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ACCGROUPID,BRANCHID,ACCOUNTGROUP,ACCOUNTTYPE,GROUPCODE,DISPLAY_NAME FROM ACCGROUP where ACCGROUPID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccType()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ACCOUNTTYPEID,ACCOUNTTYPE FROM ACCTYPE order by ACCOUNTTYPEID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
