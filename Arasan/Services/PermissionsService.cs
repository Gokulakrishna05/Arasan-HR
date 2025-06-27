using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Arasan.Services
{
    public class PermissionsService : IPermissions
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PermissionsService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetPermissionsEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PERMISSIONID,EMPLOYEEID,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,REASON from PERMISSIONS WHERE PERMISSIONID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string PermissionCRUD(Permissions Cy)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Cy.PID == null)
                    {
                        svSQL = "Insert into PERMISSIONS (EMPLOYEEID,PERMISSIONDATE,FROMTIME,TOTIME,REASON) values ('" + Cy.EmpName + "','" + Cy.PerDate + "','" + Cy.FromTime + "','" + Cy.ToTime + "','" + Cy.Reason + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE PERMISSIONS SET EMPLOYEEID = '" + Cy.EmpName + "',PERMISSIONDATE = '" + Cy.PerDate + "',FROMTIME = '" + Cy.FromTime + "',TOTIME='" + Cy.ToTime + "',REASON='" + Cy.Reason + "' Where PERMISSIONID = '" + Cy.PID + "' ";
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

        public DataTable GetAllPermissions(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select PERMISSIONID,EMPMAST.EMPNAME,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,STATUS,PERMISSIONS.IS_ACTIVE from PERMISSIONS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = PERMISSIONS.EMPLOYEEID WHERE PERMISSIONS.IS_ACTIVE='Y' ORDER BY PERMISSIONS.PERMISSIONID DESC ";
            }
            else
            {
                SvSql = "Select PERMISSIONID,EMPMAST.EMPNAME,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,STATUS,PERMISSIONS.IS_ACTIVE from PERMISSIONS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = PERMISSIONS.EMPLOYEEID WHERE PERMISSIONS.IS_ACTIVE='N' ORDER BY PERMISSIONS.PERMISSIONID DESC ";
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
            SvSql = "select EMPMASTID,EMPNAME from EMPMAST where EMPMAST.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ViewPermission(Permissions Cy)
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
                        svSQL = "UPDATE PERMISSIONS SET STATUS = 'Approve' Where PERMISSIONID = '" + Cy.PID + "' ";
                    }
                    else
                    {
                        svSQL = "UPDATE PERMISSIONS SET STATUS = 'Reject' Where PERMISSIONID = '" + Cy.PID + "' ";
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

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {

                    if (tag == "Del")
                    {
                        svSQL = "UPDATE PERMISSIONS SET IS_ACTIVE ='N' WHERE PERMISSIONID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE PERMISSIONS SET IS_ACTIVE ='Y' WHERE PERMISSIONID='" + id + "'";
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
