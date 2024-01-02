using Arasan.Interface;

using Arasan.Interface.Master;
using Arasan.Models;
//using Arasan.Models.Master;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class ETariffService : IETariff
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ETariffService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string ETariffCRUD(ETariff cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM TARIFFMASTER WHERE TARIFFID =LTRIM(RTRIM('" + cy.Tariff + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "ETariff Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ETARIFFPROC", objConn);

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

                    objCmd.Parameters.Add("TARIFFID", OracleDbType.NVarchar2).Value = cy.Tariff;
                    objCmd.Parameters.Add("TARIFFDESC", OracleDbType.NVarchar2).Value = cy.Tariffdes;
                    objCmd.Parameters.Add("PERCENTAGE", OracleDbType.NVarchar2).Value = cy.Per;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                   
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

        public DataTable GetETariff(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TARIFFID,TARIFFDESC,PERCENTAGE FROM TARIFFMASTER WHERE TARIFFMASTERID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllETariff(string strStatus)
        {
            string SvSql = string.Empty;

             

            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT IS_ACTIVE,TARIFFMASTERID,TARIFFID,TARIFFDESC,PERCENTAGE FROM TARIFFMASTER WHERE IS_ACTIVE = 'Y' ORDER BY TARIFFMASTERID DESC";
            }
            else
            {
                SvSql = "SELECT IS_ACTIVE,TARIFFMASTERID,TARIFFID,TARIFFDESC,PERCENTAGE FROM TARIFFMASTER WHERE IS_ACTIVE = 'N' ORDER BY TARIFFMASTERID DESC";

            }
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
                    svSQL = "UPDATE TARIFFMASTER SET IS_ACTIVE ='N' WHERE TARIFFMASTERID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE TARIFFMASTER SET IS_ACTIVE ='Y' WHERE TARIFFMASTERID='" + id + "'";
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
