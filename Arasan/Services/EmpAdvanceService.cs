using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Arasan.Services
{
    public class EmpAdvanceService : IEmpAdvance    
    {
       
            private readonly string _connectionString;
            DataTransactions datatrans;

            public EmpAdvanceService(IConfiguration _configuratio)


            {
                _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
                datatrans = new DataTransactions(_connectionString);
            }


        public DataTable GetEmpAdvanceEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select EMPLOYEE_ID,ADVANCE_TYPE,ADVANCE_AMOUNT,EMI_AMOUNT,to_char(START_MONTH,'dd-MON-yyyy')START_MONTH,PAID_EMI_COUNT,REMARKS from HR_EMPLOYEE_ADVANCE WHERE ADVANCE_ID='" + id + "' ";

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
        public DataTable GetAdvId()
        {
            string SvSql = string.Empty;
            //SvSql = "Select EMPNAME,EMPMASTID from EMPMAST  WHERE IS_ACTIVE='Y'";
            SvSql = "Select ATYPE,ID from ADVTYPEMASTER  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string EmpAdvanceCRUD(EmpAdvance ic)
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

                        svSQL = "Insert into HR_EMPLOYEE_ADVANCE (EMPLOYEE_ID,ADVANCE_TYPE,ADVANCE_AMOUNT,EMI_AMOUNT,START_MONTH,PAID_EMI_COUNT,REMARKS) VALUES ('" + ic.Empe + "','" + ic.AdvTp + "','" + ic.Advamt + "','" + ic.Emi + "','" + ic.SMn + "','" + ic.Emid + "','"+ic.Rmks+"')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = " UPDATE HR_EMPLOYEE_ADVANCE SET EMPLOYEE_ID='" + ic.Empe + "',ADVANCE_TYPE='" + ic.AdvTp + "',ADVANCE_AMOUNT='" + ic.Advamt + "',EMI_AMOUNT='" + ic.Emi + "',START_MONTH='" + ic.SMn + "',PAID_EMI_COUNT='" + ic.Emid + "',REMARKS='"+ic.Rmks + "'   Where ADVANCE_ID = '" + ic.ID + "' ";
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


        public DataTable GetAllEmpAdvance(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                //SvSql = "Select ID,EMPMAST.EMPNAME,ADVANCE_ID,ADVANCE_TYPE,HR_EMPLOYEE_ADVANCE.IS_ACTIVE from HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID WHERE HR_EMPLOYEE_ADVANCE.IS_ACTIVE='Y' ORDER BY HR_EMPLOYEE_ADVANCE.ID DESC";
               // SvSql = "SELECT HR_EMPLOYEE_ADVANCE.ADVANCE_ID AS ID, EMPMAST.EMPNAME, HR_EMPLOYEE_ADVANCE.ADVANCE_ID, ADVTYPEMASTER.ATYPE AS ADVANCE_TYPE, HR_EMPLOYEE_ADVANCE.IS_ACTIVE FROM HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID LEFT OUTER JOIN ADVTYPEMASTER ON ADVTYPEMASTER.ID = HR_EMPLOYEE_ADVANCE.ADVANCE_TYPE WHERE HR_EMPLOYEE_ADVANCE.IS_ACTIVE = 'Y' ORDER BY HR_EMPLOYEE_ADVANCE.ID DESC";
                SvSql = "SELECT HR_EMPLOYEE_ADVANCE.ADVANCE_ID AS ID, EMPMAST.EMPNAME, HR_EMPLOYEE_ADVANCE.ADVANCE_ID, ADVTYPEMASTER.ATYPE AS ADVANCE_TYPE, HR_EMPLOYEE_ADVANCE.IS_ACTIVE FROM HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID LEFT OUTER JOIN ADVTYPEMASTER ON ADVTYPEMASTER.ID = HR_EMPLOYEE_ADVANCE.ADVANCE_TYPE WHERE HR_EMPLOYEE_ADVANCE.IS_ACTIVE = 'Y' ORDER BY HR_EMPLOYEE_ADVANCE.ADVANCE_ID DESC";
            }
            else
            {
                //SvSql = "Select ID,EMPMAST.EMPNAME,ADVANCE_ID,ADVANCE_TYPE,HR_EMPLOYEE_ADVANCE.IS_ACTIVE from HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID WHERE HR_EMPLOYEE_ADVANCE.IS_ACTIVE='N' ORDER BY HR_EMPLOYEE_ADVANCE.ID DESC";
                SvSql = "SELECT HR_EMPLOYEE_ADVANCE.ADVANCE_ID AS ID, EMPMAST.EMPNAME, HR_EMPLOYEE_ADVANCE.ADVANCE_ID, ADVTYPEMASTER.ATYPE AS ADVANCE_TYPE, HR_EMPLOYEE_ADVANCE.IS_ACTIVE FROM HR_EMPLOYEE_ADVANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = HR_EMPLOYEE_ADVANCE.EMPLOYEE_ID LEFT OUTER JOIN ADVTYPEMASTER ON ADVTYPEMASTER.ID = HR_EMPLOYEE_ADVANCE.ADVANCE_TYPE WHERE HR_EMPLOYEE_ADVANCE.IS_ACTIVE = 'N' ORDER BY HR_EMPLOYEE_ADVANCE.ADVANCE_ID DESC";

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
                        svSQL = "UPDATE HR_EMPLOYEE_ADVANCE SET IS_ACTIVE ='N' WHERE ADVANCE_ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE HR_EMPLOYEE_ADVANCE SET IS_ACTIVE ='Y' WHERE ADVANCE_ID='" + id + "'";
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
