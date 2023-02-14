using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class SalesEnqService : ISalesEnq
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesEnqService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<SalesEnquiry> GetAllSalesEnq()
        {
            List<SalesEnquiry> cmpList = new List<SalesEnquiry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  PARTYRCODE.PARTY,ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE, ENQ_TYPE,CUSTOMER_TYPE,CUSTOMER_NAME,SALES_ENQUIRY.ID from SALES_ENQUIRY LEFT OUTER JOIN  PARTYMAST on SALES_ENQUIRY.CUSTOMER_NAME=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH')\r\n";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesEnquiry cmp = new SalesEnquiry
                        {

                            ID = rdr["ID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Customer = rdr["PARTY"].ToString(),
                            EnqNo = rdr["ENQ_NO"].ToString(),
                            EnqDate = rdr["ENQ_DATE"].ToString(),
                            CustomerType = rdr["CUSTOMER_TYPE"].ToString(),
                            EnqType = rdr["ENQ_TYPE"].ToString(),
                         
                            



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string SalesEnqCRUD(SalesEnquiry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALESENQPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

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
                    objCmd.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("CUSTOMER_NAME", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("ENQ_NO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQ_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.EnqDate);
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
              
                    objCmd.Parameters.Add("CUSTOMER_TYPE", OracleDbType.NVarchar2).Value = cy.CustomerType;
                    objCmd.Parameters.Add("ENQ_TYPE", OracleDbType.NVarchar2).Value = cy.EnqType;
                    objCmd.Parameters.Add("CURRENCY_TYPE", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("ADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("CONTACT_PERSON", OracleDbType.NVarchar2).Value = cy.ContactPersion;
                    objCmd.Parameters.Add("CONTACT_PERSON_MOBILE", OracleDbType.NVarchar2).Value = cy.Mobile;
                    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = cy.Priority;
                    objCmd.Parameters.Add("LEADBY", OracleDbType.NVarchar2).Value = cy.Recieved;
                    objCmd.Parameters.Add("ASSIGNED_TO", OracleDbType.NVarchar2).Value = cy.Assign;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    //objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();


                        objConn.Close();
                              


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
        public DataTable GetSalesEnquiry(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCH_ID,ENQ_NO,to_char(ENQ_DATE,'dd-MON-yyyy')ENQ_DATE, ENQ_TYPE,CUSTOMER_TYPE,CUSTOMER_NAME,ADDRESS,CITY,PINCODE,CONTACT_PERSON,CONTACT_PERSON_MOBILE,PRIORITY,LEADBY,ASSIGNED_TO,CURRENCY_TYPE,SALES_ENQUIRY.ID from SALES_ENQUIRY  where SALES_ENQUIRY.ID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}

