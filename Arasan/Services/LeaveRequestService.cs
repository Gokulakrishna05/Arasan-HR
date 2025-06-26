using System.Data;
using System.Data.SqlClient;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class LeaveRequestService : ILeaveRequest
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LeaveRequestService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllLeaveRequestGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select LEAVE_ID,EMPMAST.EMPID,LEAVETYPEMASTER.LEAVETYPENAME,TOTAL_DAYS,STATUS,LEAVEREQUEST.IS_ACTIVE from LEAVEREQUEST LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = LEAVEREQUEST.EMP_ID LEFT OUTER JOIN LEAVETYPEMASTER ON LEAVETYPEMASTER.ID = LEAVEREQUEST.LEAVE_TYPE WHERE LEAVEREQUEST.IS_ACTIVE='Y' ORDER BY LEAVEREQUEST.LEAVE_ID DESC ";
            }
            else
            {
                SvSql = "Select LEAVE_ID,EMPMAST.EMPID,LEAVETYPEMASTER.LEAVETYPENAME,TOTAL_DAYS,STATUS,LEAVEREQUEST.IS_ACTIVE from LEAVEREQUEST LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = LEAVEREQUEST.EMP_ID LEFT OUTER JOIN LEAVETYPEMASTER ON LEAVETYPEMASTER.ID = LEAVEREQUEST.LEAVE_TYPE WHERE LEAVEREQUEST.IS_ACTIVE='N' ORDER BY LEAVEREQUEST.LEAVE_ID DESC ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditLeaveRequest(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LEAVE_ID,EMP_ID,LEAVE_TYPE,to_char(LEAVEREQUEST.FROM_DATE,'dd-MON-yyyy')FROM_DATE,to_char(LEAVEREQUEST.TO_DATE,'dd-MON-yyyy')TO_DATE,TOTAL_DAYS,REASON from LEAVEREQUEST WHERE LEAVE_ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string LeaveRequestCRUD(LeaveRequest Cy)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Cy.LeaveID == null)
                    {
                        svSQL = "Insert into LEAVEREQUEST (EMP_ID,LEAVE_TYPE,FROM_DATE,TO_DATE,TOTAL_DAYS,REASON) values ('" + Cy.EmpID + "','" + Cy.LeaveType + "','" + Cy.FromDate + "','" + Cy.ToDate + "','" + Cy.TotDays + "','" + Cy.Reason + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE LEAVEREQUEST SET EMP_ID = '" + Cy.EmpID + "',LEAVE_TYPE = '" + Cy.LeaveType + "',FROM_DATE = '" + Cy.FromDate + "',TO_DATE = '" + Cy.ToDate + "',TOTAL_DAYS = '" + Cy.TotDays + "',REASON = '" + Cy.Reason + "'  Where LEAVE_ID = '" + Cy.LeaveID + "' ";
                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return msg;
        }

        public string ViewLeaveRequest(LeaveRequest Cy)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Cy.Status == "Approve")
                    {
                        svSQL = "UPDATE LEAVEREQUEST SET STATUS = 'Approve' Where LEAVE_ID = '" + Cy.LeaveID + "' ";
                    }
                    else
                    {
                        svSQL = "UPDATE LEAVEREQUEST SET STATUS = 'Reject' Where LEAVE_ID = '" + Cy.LeaveID + "' ";
                    }
                    //if (Cy.LeaveID == null)
                    //{
                    //    svSQL = "Insert into LEAVEREQUEST (EMP_ID,LEAVE_TYPE,FROM_DATE,TO_DATE,TOTAL_DAYS,REASON) values ('" + Cy.EmpID + "','" + Cy.LeaveType + "','" + Cy.FromDate + "','" + Cy.ToDate + "','" + Cy.TotDays + "','" + Cy.Reason + "') ";
                    //}
                    //else
                    //{
                    //    svSQL = " UPDATE LEAVEREQUEST SET EMP_ID = '" + Cy.EmpID + "',LEAVE_TYPE = '" + Cy.LeaveType + "',FROM_DATE = '" + Cy.FromDate + "',TO_DATE = '" + Cy.ToDate + "',TOTAL_DAYS = '" + Cy.TotDays + "',REASON = '" + Cy.Reason + "'  Where LEAVE_ID = '" + Cy.LeaveID + "' ";
                    //}
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return msg;
        }

        public DataTable GetEmployee()
        {
            string SvSql = string.Empty;
            SvSql = "select EMPID,EMPMASTID from EMPMAST where EMPMAST.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLeaveType()
        {
            string SvSql = string.Empty;
            SvSql = "select LEAVETYPENAME,ID from LEAVETYPEMASTER where LEAVETYPEMASTER.IS_ACTIVE = 'Y' ";
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
                    if (tag == "Del")
                    {
                        svSQL = "UPDATE LEAVEREQUEST SET IS_ACTIVE ='N' WHERE LEAVE_ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE LEAVEREQUEST SET IS_ACTIVE ='Y' WHERE LEAVE_ID='" + id + "'";
                    }
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

    }
}
