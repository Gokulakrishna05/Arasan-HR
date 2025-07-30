using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;



namespace Arasan.Services
{
    public class IncentiveService : IIncentive
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public IncentiveService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetIncentiveEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select EMPLOYEE_ID,DESIGNATION_ID,DEPARTMENT_ID,INCENTIVE_NAME,INCENTIVE_TYPE,AMOUNT,REASON from HR_INCENTIVE_MASTER WHERE ID='" + id + "' ";

            //SvSql = "Select EMPMAST.EMPNAME,DESIGNATION_ID,DEPARTMENT_ID,INCENTIVE_NAME,INCENTIVE_TYPE,AMOUNT,REASON from HR_INCENTIVE_MASTER LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_INCENTIVE_MASTER.EMPLOYEE_ID WHERE ID='" + id + "'  ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpId()
        {
            string SvSql = string.Empty;
            //SvSql = "Select EMPNAME,EMPMASTID from EMPMAST  WHERE IS_ACTIVE='Y'";
            SvSql = "Select EMPNAME,EMPMASTID from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string IncentiveCRUD(Incentive ic)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();

                    if (ic.ID == null)
                    {

                        svSQL = "Insert into HR_INCENTIVE_MASTER (EMPLOYEE_ID,DESIGNATION_ID,DEPARTMENT_ID,INCENTIVE_NAME,INCENTIVE_TYPE,AMOUNT,REASON) VALUES ('" + ic.Emp + "','" + ic.Des + "','" + ic.Dpt + "','" + ic.Icem + "','" + ic.Ictpe + "','" + ic.Amt + "','" + ic.Rean + "')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = " UPDATE HR_INCENTIVE_MASTER SET EMPLOYEE_ID ='" + ic.Emp + "',DESIGNATION_ID='" + ic.Des + "',DEPARTMENT_ID='" + ic.Dpt + "',INCENTIVE_NAME='" + ic.Icem + "',INCENTIVE_TYPE='" + ic.Ictpe + "',AMOUNT='" + ic.Amt + "',REASON='" + ic.Rean + "'   Where ID = '" + ic.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
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

        public DataTable GetAllIncentive(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select ID,EMPMAST.EMPNAME,DESIGNATION_ID,DEPARTMENT_ID,HR_INCENTIVE_MASTER.IS_ACTIVE from HR_INCENTIVE_MASTER LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_INCENTIVE_MASTER.EMPLOYEE_ID WHERE HR_INCENTIVE_MASTER.IS_ACTIVE='Y' ORDER BY HR_INCENTIVE_MASTER.ID DESC";

            }
            else
            {
                SvSql = "Select ID,EMPMAST.EMPNAME,DESIGNATION_ID,DEPARTMENT_ID,HR_INCENTIVE_MASTER.IS_ACTIVE from HR_INCENTIVE_MASTER LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_INCENTIVE_MASTER.EMPLOYEE_ID WHERE HR_INCENTIVE_MASTER.IS_ACTIVE='N' ORDER BY HR_INCENTIVE_MASTER.ID DESC";

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

                    if (tag == "Del")
                    {
                        svSQL = "UPDATE HR_INCENTIVE_MASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE HR_INCENTIVE_MASTER SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
