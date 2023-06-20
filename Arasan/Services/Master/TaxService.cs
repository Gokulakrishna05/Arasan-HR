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
    public class TaxService : ITaxService
    {

        private readonly string _connectionString;
        DataTransactions datatrans;
        public TaxService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<Tax> GetAllTax()
        {
            List<Tax> cmpList = new List<Tax>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select TAXMASTID,Tax,PERCENTAGE,STATUS from TAXMAST WHERE STATUS='ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Tax cmp = new Tax
                        {
                            ID = rdr["TAXMASTID"].ToString(),
                            Taxtype = rdr["TAX"].ToString(),
                            Percentage = rdr["PERCENTAGE"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public string TaxCRUD(Tax cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(TAX) as cnt FROM TAXMAST WHERE TAX = LTRIM(RTRIM('" + cy.Taxtype + "')) and PERCENTAGE = LTRIM(RTRIM('" + cy.Percentage + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Tax Already Existed";
                        return msg;
                    }
                   
                }



                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("Tax_PROC", objConn);


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

                    objCmd.Parameters.Add("TAX", OracleDbType.NVarchar2).Value = cy.Taxtype;
                    objCmd.Parameters.Add("PERCENTAGE", OracleDbType.NVarchar2).Value = cy.Percentage;
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

        public DataTable GetTax(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select TAXMASTID,TAX,PERCENTAGE from TAXMAST where TAXMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE TAXMAST SET STATUS ='INACTIVE' WHERE TAXMASTID='" + id + "'";
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
