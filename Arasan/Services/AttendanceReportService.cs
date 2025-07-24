using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class AttendanceReportService : IAttendanceReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AttendanceReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllAttendanceReportGrid()
        {
            string SvSql = string.Empty;
            //if (strStatus == "Y" || strStatus == null)
            //{
                SvSql = "SELECT emp.EMPID,emp.EMPNAME,to_char(hmp.ATTENDANCE_DATE,'dd-MON-yyyy')ATTENDANCE_DATE," +
                "MAX(CASE WHEN hmp.MISSING_IN_OUT = 'IN' THEN hmp.MISSING_IN END) AS MISSING_IN," +
                "MAX(CASE WHEN hmp.MISSING_IN_OUT = 'OUT' THEN hmp.MISSING_OUT END) AS MISSING_OUT,SHIFTMAST.SHIFTNO," +
                "SHIFTMAST.FROMTIME AS SHIFT_START,SHIFTMAST.TOTIME AS SHIFT_END,esd.WOFF AS WEEK_OFF FROM HRM_MISSING_PUNCH hmp " +
                "JOIN EMPMAST emp ON emp.EMPMASTID = hmp.EMPLOYEE_ID,EMPSHIFTDETAIL esd " +
                "LEFT OUTER JOIN SHIFTMAST ON SHIFTMAST.SHIFTMASTID=esd.SHIFT WHERE hmp.ATTENDANCE_DATE >= TRUNC(SYSDATE) - 30 AND NVL(esd.IS_ACTIVE, 'Y') = 'Y' " +
                "and hmp.STATUS='Approved' and esd.EMPLID = hmp.EMPLOYEE_ID GROUP BY emp.EMPID,emp.EMPNAME,hmp.ATTENDANCE_DATE,SHIFTMAST.SHIFTNO," +
                "SHIFTMAST.FROMTIME,SHIFTMAST.TOTIME,esd.WOFF ORDER BY hmp.ATTENDANCE_DATE DESC, emp.EMPNAME";
            //}
            //else
            //{
            //    SvSql = "Select ASSIGN_ALLOWANCE.ID,EMPMAST.EMPNAME,ALLOWANCE_MASTER.ALLOWANCE_NAME,ALLOWANCE_MASTER.ALLOWANCE_TYPE,ASSIGN_ALLOWANCE.IS_ACTIVE from ASSIGN_ALLOWANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = ASSIGN_ALLOWANCE.EMP_NAME LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_NAME_ID LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_TYPE_ID WHERE ASSIGN_ALLOWANCE.IS_ACTIVE='N' ORDER BY ASSIGN_ALLOWANCE.ID DESC ";
            //}
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
