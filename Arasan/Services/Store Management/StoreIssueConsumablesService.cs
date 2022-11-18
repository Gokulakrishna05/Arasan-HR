using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace Arasan.Services
{
    public class StoreIssueConsumablesService: IStoreIssueConsumables
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreIssueConsumablesService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE ITEMGROUP='" + value + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMGROUPID,GROUPCODE from itemgroup";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<StoreIssueConsumables> GetAllStoreIssue()
        {
            List<StoreIssueConsumables> cmpList = new List<StoreIssueConsumables>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,DOCDATE,REQNO,REQDATE,TOLOCID,LOCIDCONS,PROCESSID,MCID,MCNAME,NARRATION,USERID,WCID,SCISSBASICID from SCISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SCISSBASIC.BRANCHID  ORDER BY SCISSBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreIssueConsumables cmp = new StoreIssueConsumables
                        {

                            SIId = rdr["SCISSBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                           
                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            ReqNo = rdr["REQNO"].ToString(),
                            ReqDate = rdr["REQDATE"].ToString(),
                            Location = rdr["TOLOCID"].ToString(),
                           
                            LocCon = rdr["LOCIDCONS"].ToString(),
                            // net = rdr["NET"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            MCNo = rdr["MCID"].ToString(),
                            MCNa = rdr["MCNAME"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            User = rdr["USERID"].ToString(),
                            Work = rdr["WCID"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditSICbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  SCISSBASIC.BRANCHID,SCISSBASIC.DOCID,SCISSBASIC.DOCDATE,SCISSBASIC.REQNO,SCISSBASIC.REQDATE,SCISSBASIC.TOLOCID,SCISSBASIC.LOCIDCONS,SCISSBASIC.PROCESSID,SCISSBASIC.MCID,SCISSBASIC.MCNAME,SCISSBASIC.NARRATION,SCISSBASIC.USERID,SCISSBASIC.WCID,SCISSBASICID from SCISSBASIC Where  SCISSBASIC.SCISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StoreIssueCRUD(StoreIssueConsumables cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SICOSUPROC", objConn);
                    

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.SIId == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.SIId;

                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("REQNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("ReqDate", OracleDbType.NVarchar2).Value = cy.ReqDate;
                    objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("LOCIDCONS", OracleDbType.NVarchar2).Value = cy.LocCon;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("MCID", OracleDbType.NVarchar2).Value = cy.MCNo;
                    objCmd.Parameters.Add("MCNAME", OracleDbType.NVarchar2).Value = cy.MCNa;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = cy.User;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Work;
                   
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
               
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
