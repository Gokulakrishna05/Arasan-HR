using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class OnDutyService : IOnDuty
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public OnDutyService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetEmplId()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPID  from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string OnDutyCRUD(OnDuty cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ONDUTYPROC", objConn);


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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("EMPLOYEEID", OracleDbType.NVarchar2).Value = cy.EmplId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.EmpName;
                    objCmd.Parameters.Add("EMPDESIGN", OracleDbType.NVarchar2).Value = cy.EDes;
                    objCmd.Parameters.Add("EGENDER", OracleDbType.NVarchar2).Value = cy.EGen;
               
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        if (cy.OdLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ODLS cp in cy.OdLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.StartDate != "")
                                    {

                                        svSQL = "Insert into ODDETAIL (ODBASICID,DUTYDATE,FROMTIME,TOTIME,TOHRS,DSCN,REASON,STATUS) VALUES ('" + Pid + "','" + cp.StartDate + "','" + cp.FrTime + "','" + cp.ToTime + "','" + cp.ToHR + "','" + cp.DuSit + "','" + cp.Res + "','" + cp.Sts + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete ODDETAIL WHERE ODBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ODLS cp in cy.OdLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.StartDate != "")
                                    {

                                        svSQL = "Insert into ODDETAIL (ODBASICID,DUTYDATE,FROMTIME,TOTIME,TOHRS,DSCN,REASON,STATUS) VALUES ('" + cy.ID + "','" + cp.StartDate + "','" + cp.FrTime + "','" + cp.ToTime + "','" + cp.ToHR + "','" + cp.DuSit + "','" + cp.Res + "','" + cp.Sts + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
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

        public DataTable GetAllOnDuty(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ODBASICID,ODBASIC.DOCID,EMPMAST.EMPID,to_char(ODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,ODBASIC.EMPNAME,ODBASIC.EMPDESIGN,ODBASIC.EGENDER,ODBASIC.IS_ACTIVE FROM ODBASIC left outer join EMPMAST ON EMPMASTID=ODBASIC.EMPLOYEEID WHERE ODBASIC.IS_ACTIVE = 'Y' ";

            }
            else
            {
                SvSql = "SELECT ODBASICID,ODBASIC.DOCID,EMPMAST.EMPID,to_char(ODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,ODBASIC.EMPNAME,ODBASIC.EMPDESIGN,ODBASIC.EGENDER,ODBASIC.IS_ACTIVE FROM ODBASIC left outer join EMPMAST ON EMPMASTID=ODBASIC.EMPLOYEEID WHERE ODBASIC.IS_ACTIVE = 'N' ";

            }

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditOnDuty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(DOCDATE,'dd-MON-yyyy')DOCDATE, DOCID,EMPLOYEEID,EMPNAME,EMPDESIGN,EGENDER from ODBASIC where ODBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditDutDet(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(DUTYDATE,'dd-MON-yyyy')DUTYDATE, FROMTIME,TOTIME,TOHRS,DSCN,REASON,STATUS from ODDETAIL where ODBASICID = '" + id + "' ";
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
                    svSQL = "UPDATE ODBASIC SET IS_ACTIVE ='N' WHERE ODBASICID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ODBASIC SET IS_ACTIVE ='Y' WHERE ODBASICID='" + id + "'";
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
