using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services 
{
    public class AccConfigService : IAccConfig
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccConfigService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = " select LEDNAME,LEDGERID from LEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetConfig()
        {
            string SvSql = string.Empty;
            SvSql = " select TYPE,LEDGERID,ACCOUNTCONFIGID from ACCOUNTCONFIG";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ConfigCRUD(AccConfig cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                foreach (ConfigItem cp in cy.ConfigLst)
                {
                    using (OracleConnection objConn = new OracleConnection(_connectionString))
                    {
                        OracleCommand objCmd = new OracleCommand("ACCCONFIGPROC", objConn);


                        objCmd.CommandType = CommandType.StoredProcedure;
                        
                            StatementType = "Update";
                            objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
               
                        objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cp.Type;
                        objCmd.Parameters.Add("LEDGERID", OracleDbType.NVarchar2).Value = cp.Ledger;

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
