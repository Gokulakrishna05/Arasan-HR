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
    public class LedgerService : ILedger
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LedgerService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<Ledger> GetAllLedger()
        {
           
            List<Ledger> cmpList = new List<Ledger>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())


                {
                    con.Open();
                    //cmd.CommandText = "Select ACCTYPE.ACCOUNTTYPE,ACCGROUP.ACCOUNTGROUP,LEDNAME,to_char(LEDGER.DOCDATE,'dd-MON-yyyy')DOCDATE,DISPLAY_NAME,OPSTOCK,CLSTOCK,CATEGORY,ACCGROUP ,LEDGERID from LEDGER LEFT OUTER JOIN ACCTYPE ON ACCTYPE.ACCOUNTTYPEID=LEDGER.ACCOUNTTYPE LEFT OUTER JOIN ACCGROUP ON ACCGROUP.ACCGROUPID = LEDGER.ACCGROUP where LEDGER.STATUS= 'Active' ";
                    cmd.CommandText = "Select ACCTYPE.ACCOUNTTYPE,ACCGROUP.ACCOUNTGROUP,LEDGER.LEDNAME,to_char(LEDGER.DOCDATE,'dd-MON-yyyy')DOCDATE,LEDGER.DISPLAY_NAME,LEDGER.OPSTOCK,LEDGER.CLSTOCK,LEDGER.CATEGORY ,LEDGER.LEDGERID from LEDGER LEFT OUTER JOIN ACCTYPE ON ACCTYPE.ACCOUNTTYPEID=LEDGER.ACCOUNTTYPE LEFT OUTER JOIN ACCGROUP ON ACCGROUP.ACCGROUPID = LEDGER.ACCGROUP where LEDGER.STATUS= 'Active'  ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Ledger cmp = new Ledger
                        {
                            ID = rdr["LEDGERID"].ToString(),
                            AType = rdr["ACCOUNTTYPE"].ToString(),
                            AccGroup = rdr["ACCOUNTGROUP"].ToString(),
                            LedName = rdr["LEDNAME"].ToString(),
                            DisplayName = rdr["DISPLAY_NAME"].ToString(),
                            OpStock = rdr["OPSTOCK"].ToString(),
                            ClStock = rdr["CLSTOCK"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Category = rdr["CATEGORY"].ToString() 


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string LedgerCRUD(Ledger cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM LEDGER WHERE ACCOUNTTYPE =LTRIM(RTRIM('" + cy.AType + "')) and LEDNAME =LTRIM(RTRIM('" + cy.LedName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "LEDGER Already Existed";
                        return msg;
                    }
                }
                string AccGroupID = datatrans.GetDataString("Select ACCGROUPID from ACCGROUP where ACCOUNTGROUP='" + cy.AccGroup + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ALEDGERPROC", objConn);


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

                    objCmd.Parameters.Add("ACCOUNTTYPE", OracleDbType.NVarchar2).Value = cy.AType;
                    objCmd.Parameters.Add("ACCOUNTGROUP", OracleDbType.NVarchar2).Value = cy.AccGroup;
                    objCmd.Parameters.Add("LEDNAME", OracleDbType.NVarchar2).Value = cy.LedName;
                    objCmd.Parameters.Add("DISPLAY_NAME", OracleDbType.NVarchar2).Value = cy.DisplayName;
                    objCmd.Parameters.Add("OPSTOCK", OracleDbType.NVarchar2).Value = cy.OpStock;
                    objCmd.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cy.ClStock;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("CATEGORY", OracleDbType.NVarchar2).Value = cy.Category;

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
        public DataTable GetAccType()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT  ACCOUNTTYPEID,ACCOUNTTYPE FROM ACCTYPE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetGroupDetails(string id)
        ////{
        //    string SvSql = string.Empty;
        //    SvSql = "select ACCOUNTGROUP,ACCGROUPID from ACCGROUP where ACCTYPE= '" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public string StatusChange(string tag, int id)
        {

            try
            {

                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE LEDGER SET STATUS ='InActive' WHERE LEDGERID='" + id + "'";
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
       
        public DataTable GetLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ACCOUNTTYPE,ACCGROUP,LEDNAME,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,OPSTOCK,CLSTOCK,DISPLAY_NAME,CATEGORY,LEDGERID from LEDGER where LEDGERID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "select ACCOUNTGROUP,ACCGROUPID from ACCGROUP where ACCTYPE= '" + id + "'";
            SvSql = "select ACCOUNTGROUP,ACCGROUPID from ACCGROUP ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
