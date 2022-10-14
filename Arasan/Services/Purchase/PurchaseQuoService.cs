using Arasan.Interface;

using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PurchaseQuoService : IPurchaseQuo
    {
        private readonly string _connectionString;

        public PurchaseQuoService(IConfiguration _configuratio)
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

        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCurency()
        {
            string SvSql = string.Empty;
            SvSql = "Select MAINCURR || ' - ' || SYMBOL  as Cur,CURRENCYID from CURRENCY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<PurchaseQuo> GetAllPurQuotation()
        {
            List<PurchaseQuo> cmpList = new List<PurchaseQuo>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID, DOCID,DOCDATE,PARTYID,ENQNO,ENQDATE,PURQUOTBASICID from PURQUOTBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseQuo cmp = new PurchaseQuo
                        {
                            ID = rdr["PURQUOTBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoId = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTYID"].ToString(),

                            EnqNo = rdr["ENQNO"].ToString(),
                            EnqDate = rdr["ENQDATE"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public PurchaseQuo GetPurQuotationById(string eid)
        {
            PurchaseQuo Quotation = new PurchaseQuo();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID, DOCID,DOCDATE,PARTYID,ENQNO,ENQDATE,PURQUOTBASICID from PURQUOTBASIC where PURQUOTBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseQuo cmp = new PurchaseQuo
                        {
                            ID = rdr["PURQUOTBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoId = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTYID"].ToString(),

                            EnqNo = rdr["ENQNO"].ToString(),
                            EnqDate = rdr["ENQDATE"].ToString()

                        };

                        Quotation = cmp;
                    }
                }
            }
            return Quotation;
        }

        public string PurQuotationCRUD(PurchaseQuo cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURQUOPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.QuoId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                 
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.EnqDate;
                  
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
