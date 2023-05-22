using Arasan.Interface;
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
        public LedgerService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Ledger> GetAllLedger()
        {
            List<Ledger> cmpList = new List<Ledger>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select MNAME,DISPNAME,CATEGORY,GROUPORACCOUNT,to_char(DOCDT,'dd-MON-yyyy')DOCDT,MASTERID from MASTER";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Ledger cmp = new Ledger
                        {
                            ID = rdr["MASTERID"].ToString(),
                            MName = rdr["MNAME"].ToString(),
                            DispName = rdr["DISPNAME"].ToString(),
                            Date = rdr["DOCDT"].ToString(),
                            GrpAccount = rdr["GROUPORACCOUNT"].ToString(),
                            Category = rdr["CATEGORY"].ToString(),


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

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("LEDGERPROC", objConn);


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

                    objCmd.Parameters.Add("MNAME", OracleDbType.NVarchar2).Value = cy.MName;
                    objCmd.Parameters.Add("DISPNAME", OracleDbType.NVarchar2).Value = cy.DispName;
                    objCmd.Parameters.Add("GROUPORACCOUNT", OracleDbType.NVarchar2).Value = cy.GrpAccount;
                    objCmd.Parameters.Add("CATEGORY", OracleDbType.NVarchar2).Value = cy.Category;
                    objCmd.Parameters.Add("DOCDT", OracleDbType.Date).Value = DateTime.Parse(cy.Date);

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
        public DataTable GetLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select MNAME,DISPNAME,CATEGORY,GROUPORACCOUNT,to_char(DOCDT,'dd-MON-yyyy')DOCDT,MASTERID from MASTER where MASTERID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
