using Arasan.Interface.Transaction;
using Arasan.Models;
using Arasan.Models.Transaction;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services.Transaction
{
    public class MissingPunchEntryService : IMissingPunchEntry
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public MissingPunchEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAlLMissingPunchEntry(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ID,EMPMAST.EMPNAME,to_char(ATTENDANCE_DATE,'dd-MON-yyyy')ATTENDANCE_DATE,REASON,DEVICE_ID,MISSING_IN_OUT FROM HRM_MISSING_PUNCH LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=HRM_MISSING_PUNCH.EMPLOYEE_ID\r\n";

            }
            else
            {
                SvSql = "SELECT ID,EMPMAST.EMPNAME,to_char(ATTENDANCE_DATE,'dd-MON-yyyy')ATTENDANCE_DATE,REASON,DEVICE_ID,MISSING_IN_OUT FROM HRM_MISSING_PUNCH LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=HRM_MISSING_PUNCH.EMPLOYEE_ID\r\n";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpName()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPNAME from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string MissingPunchEntryCRUD(MissingPunchEntry pp)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (pp.ID == null)
                    {
                        svSQL = "Insert into HRM_MISSING_PUNCH  (EMPLOYEE_ID, ATTENDANCE_DATE,MISSING_IN_OUT, MISSING_IN, MISSING_OUT, DEVICE_ID, REASON, STATUS, CREATED_BY, CREATED_DATE) values ('" + pp.EmployeeId + "','" + pp.PunchDate + "','" + pp.Missing + "','" + pp.MissingIn + "','" + pp.MissingOut + "','" + pp.Device + "','" + pp.Reason + "','Pending','" + pp.createby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";
                    }

                    else
                    {
                        svSQL = " UPDATE HRM_MISSING_PUNCH SET  EMPLOYEE_ID = '" + pp.EmployeeId + "',  ATTENDANCE_DATE =  '" + pp.PunchDate + "',MISSING_IN_OUT = '" + pp.Missing + "',MISSING_IN = '" + pp.MissingIn + "',MISSING_OUT = '" + pp.MissingOut + "',DEVICE_ID = '" + pp.Device + "',REASON = '" + pp.Reason + "',UPDATED_BY = '" + pp.createby + "',UPDATED_DATE = '" + DateTime.Now.ToString("dd-MMM-yyyy") + "'  Where HRM_MISSING_PUNCH.ID = '" + pp.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    //oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    objconn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }
    }
}
