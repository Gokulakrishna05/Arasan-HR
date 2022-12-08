using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Arasan.Interface;

namespace Arasan.Services
{
    public class PurchaseReturnService : IPurchaseReturn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<PurchaseReturn> GetAllPurReturn()
        {
            List<PurchaseReturn> cmpList = new List<PurchaseReturn>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,PRETBASIC.EXCHANGERATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,PRETBASIC.LOCID,PRETBASIC.REASONCODE,PRETBASIC.REJBY,PRETBASIC.TRANSITLOCID,PRETBASIC.TEMPFIELD,CURRENCY.MAINCURR,PARTYRCODE.PARTY,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID from PRETBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PRETBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on PRETBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PRETBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY PRETBASIC.PRETBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseReturn cmp = new PurchaseReturn
                        {
                            ID = rdr["PRETBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            RetNo = rdr["DOCID"].ToString(),
                            RetDate = rdr["DOCDATE"].ToString(),
                            ExRate = rdr["EXCHANGERATE"].ToString(),
                            Currency = rdr["MAINCURR"].ToString(),
                          
                            Reason = rdr["REASONCODE"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetPurchaseReturn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETBASIC.BRANCHID,PRETBASIC.PARTYID,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,PRETBASIC.LOCID,PRETBASIC.MAINCURRENCY,PRETBASIC.EXCHANGERATE,PRETBASIC.REASONCODE,PRETBASIC.REJBY,PRETBASIC.TRANSITLOCID,PRETBASIC.TEMPFIELD,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID  from PRETBASIC where PRETBASIC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string PurReturnCRUD(PurchaseReturn cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURRETURNPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURRETURNPROC";*/

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
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.RetNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RetDate);
                    objCmd.Parameters.Add("EXCHANGERATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.ReqDate);
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = cy.Reason;
                    objCmd.Parameters.Add("REJBY", OracleDbType.NVarchar2).Value = cy.Rej;
                    objCmd.Parameters.Add("TRANSITLOCID", OracleDbType.NVarchar2).Value = cy.Trans;
                    objCmd.Parameters.Add("TEMPFIELD", OracleDbType.NVarchar2).Value = cy.Temp;
                    objCmd.Parameters.Add("RGRNNO", OracleDbType.NVarchar2).Value = cy.Grn;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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
        public DataTable GetPO()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,POBASICID from POBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPODetails(string POID)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UNITMAST.UNITID,CF,QTY,RATE,AMOUNT,ITEMMASTER.ITEMID,DISCAMT,FREIGHTCHGS,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT FROM PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE POBASICID='" + POID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "Select STATE,STATEMASTID from STATEMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
