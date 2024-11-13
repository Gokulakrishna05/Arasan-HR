using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class CustomerComplaintService : ICustomerComplaint
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CustomerComplaintService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        

        public DataTable GetAllListCustomerComplaint(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT CMPLBASICID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,CCIRNO FROM CMPLBASIC WHERE STATUS='Y' ORDER BY  CMPLBASIC.CMPLBASICID DESC";
            }
            else
            {
                SvSql = "SELECT CMPLBASICID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,CCIRNO FROM CMPLBASIC WHERE STATUS='Y' ORDER BY  CMPLBASIC.CMPLBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CMPLBASIC SET CMPLBASIC.STATUS ='N' WHERE CMPLBASICID='" + id + "'";
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
        public string ActStatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CMPLBASIC SET CMPLBASIC.STATUS ='Y' WHERE CMPLBASICID='" + id + "'";
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
        public string CustomerComplaintCRUD(CustomerComplaint cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE = 'T'");
                string DocNo = string.Format("{0}{1}", "Ex-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'Ex-F' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.ComplaintNo = DocNo;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CMPLBASICPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.ComplaintNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.ComplaintDate;
                    objCmd.Parameters.Add("CCIRINITNAME", OracleDbType.NVarchar2).Value = cy.CCIRINITNAME;
                    objCmd.Parameters.Add("INVESTIGATEDBY", OracleDbType.NVarchar2).Value = cy.INVESTIGATEDBY;
                    objCmd.Parameters.Add("CCIRNO", OracleDbType.NVarchar2).Value = cy.CCIRNo;
                    objCmd.Parameters.Add("CCIRNAME", OracleDbType.NVarchar2).Value = cy.CCIRNAME;
                    objCmd.Parameters.Add("RESINVESTIGATION", OracleDbType.NVarchar2).Value = cy.Result;
                    objCmd.Parameters.Add("NATDISPOSITION", OracleDbType.NVarchar2).Value = cy.Nature;
                    objCmd.Parameters.Add("DISPINVESTIGATED", OracleDbType.NVarchar2).Value = cy.DISINVESTIGATEDBY;
                    objCmd.Parameters.Add("REVIEWEDBY", OracleDbType.NVarchar2).Value = cy.REVIEWEDBY;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;

                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = cy.CreatedOn;
                    objCmd.Parameters.Add("UPDATED_BY", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = cy.UpdatedOn;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Y";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                       
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    //objConn.Close();
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
