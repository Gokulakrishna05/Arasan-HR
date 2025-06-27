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

            SvSql = "Select PERMISSIONID,EMPLOYEEID,to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE ,FROMTIME,TOTIME,REASON,REMARKS,APPLIEDON from PERMISSIONS WHERE PERMISSIONID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetAllPermissions(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,PERMISSIONS.IS_ACTIVE from PERMISSIONS WHERE PERMISSIONS.IS_ACTIVE='Y' ORDER BY PERMISSIONS.PERMISSIONID DESC ";

            }
            else
            {
                SvSql = "Select to_char(PERMISSIONDATE,'dd-MON-yyyy')PERMISSIONDATE,FROMTIME,TOTIME,PERMISSIONS.IS_ACTIVE from PERMISSIONS WHERE PERMISSIONS.IS_ACTIVE='N' ORDER BY PERMISSIONS.PERMISSIONID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string GetPermi(Permissions Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                //var userid = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                using (OracleConnection objconn = new OracleConnection(_connectionString))


                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into PERMISSIONS (EMPLOYEEID,PERMISSIONDATE,FROMTIME,TOTIME,REASON,REMARKS) values ('" + Em.EmpID + "','" + Em.PerDate + "','" + Em.FTDate + "','" + Em.TTDate + "','" + Em.Reason + "','" + Em.Remarks + "') ";
                    }

                    else
                    {
                        svSQL = " UPDATE PERMISSIONS SET EMPLOYEEID = '" + Em.EmpID + "',PERMISSIONDATE = '" + Em.PerDate + "',FROMTIME = '" + Em.FTDate + "',TOTIME='" + Em.TTDate + "',REASON='" + Em.Reason + "',REMARKS='" + Em.Remarks + "',  Where PERMISSIONID = '" + Em.PID + "' ";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

    }
}
