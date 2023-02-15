using Arasan.Interface;

using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class CustomerTypeService : ICustomerType
    {
        private readonly string _connectionString;
        public CustomerTypeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<CustomerType> GetAllCustomerType()
        {
            List<CustomerType> staList = new List<CustomerType>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CUSTOMER_TYPE,DESCRIPTION,ID from CUSTOMERTYPE";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CustomerType sta = new CustomerType
                        {
                            ID = rdr["ID"].ToString(),
                            Type = rdr["CUSTOMER_TYPE"].ToString(),
                            Des = rdr["DESCRIPTION"].ToString(),
                           
                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        public string CustomerCRUD(CustomerType cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CUSTOMERTYPEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "COMPANYPROC";*/

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

                    objCmd.Parameters.Add("CUSTOMER_TYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = cy.Des;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
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
        public DataTable GetCustomerType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CUSTOMER_TYPE,DESCRIPTION,CUSTOMERTYPE.ID  from CUSTOMERTYPE where CUSTOMERTYPE.ID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
