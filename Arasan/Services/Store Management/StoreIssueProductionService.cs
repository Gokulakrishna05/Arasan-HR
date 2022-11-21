using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class StoreIssueProductionService :IStoreIssueProduction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreIssueProductionService(IConfiguration _configuratio)
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
        //public DataTable GetEmp()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
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
        public IEnumerable<StoreIssueProduction> GetAllStoreIssuePro()
        {
            List<StoreIssueProduction> cmpList = new List<StoreIssueProduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REQNO,to_char(REQDATE,'dd-MON-yyyy')REQDATE,TOLOCID,LOCIDCONS,PROCESSID,NARRATION,PSCHNO,WCID,STORESISSBASICID from STORESISSBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESISSBASIC.BRANCHID  ORDER BY STORESISSBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreIssueProduction cmp = new StoreIssueProduction
                        {

                            SIId = rdr["STORESISSBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),

                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            ReqNo = rdr["REQNO"].ToString(),
                            ReqDate = rdr["REQDATE"].ToString(),
                            Location = rdr["TOLOCID"].ToString(),
                             
                            LocCon = rdr["LOCIDCONS"].ToString(),
                            // net = rdr["NET"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            //MCNo = rdr["MCID"].ToString(),
                            //MCNa = rdr["MCNAME"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            SchNo = rdr["PSCHNO"].ToString(),
                            Work = rdr["WCID"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable EditSIPbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  STORESISSBASIC.BRANCHID,STORESISSBASIC.DOCID,to_char(STORESISSBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STORESISSBASIC.REQNO,to_char(STORESISSBASIC.REQDATE,'dd-MON-yyyy')REQDATE,STORESISSBASIC.TOLOCID,STORESISSBASIC.LOCIDCONS,STORESISSBASIC.PROCESSID,STORESISSBASIC.NARRATION,STORESISSBASIC.PSCHNO,STORESISSBASIC.WCID,STORESISSBASIC.STORESISSBASICID from STORESISSBASIC Where  STORESISSBASIC.STORESISSBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
        public string StoreIssueProCRUD(StoreIssueProduction cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SIPROPROC", objConn);


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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("REQNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("ReqDate", OracleDbType.Date).Value = DateTime.Parse(cy.ReqDate);
                    objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("LOCIDCONS", OracleDbType.NVarchar2).Value = cy.LocCon;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    //objCmd.Parameters.Add("MCID", OracleDbType.NVarchar2).Value = cy.MCNo;
                    //objCmd.Parameters.Add("MCNAME", OracleDbType.NVarchar2).Value = cy.MCNa;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.SchNo;
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
