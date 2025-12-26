using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class AssignAllowanceService : IAssignAllowance
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AssignAllowanceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetEditAssignAllowance(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,EMP_NAME,ALLOWANCE_NAME_ID,DESCRIPTION,ALLOWANCE_TYPE_ID,AMT_PERC,to_char(EFFECTIVE_DATE,'dd-MON-yyyy')EFFECTIVE_DATE from ASSIGN_ALLOWANCE WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditAssignAllowanceDetails(string? id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,EMP_NAME,ALLOWANCE_NAME_ID,DESCRIPTION,ALLOWANCE_TYPE_ID,AMT_PERC,to_char(EFFECTIVE_DATE,'dd-MON-yyyy')EFFECTIVE_DATE from ASSIGN_ALLOWANCE WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string AssignAllowanceCRUD(AssignAllowance Cy)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Cy.ID == null)
                    {
                        //svSQL = "Insert into ASSIGN_ALLOWANCE (EMP_NAME,ALLOWANCE_NAME_ID,ALLOWANCE_TYPE_ID,AMT_PERC,EFFECTIVE_DATE,DESCRIPTION) values ('" + Cy.EmpName + "','" + Cy.AllowanceName + "','" + Cy.AllowanceType + "','" + Cy.AmtPerc + "','" + Cy.EffectiveDate + "','" + Cy.Description + "') ";
                    }
                    else
                    {
                        //svSQL = " UPDATE ASSIGN_ALLOWANCE SET EMP_NAME = '" + Cy.EmpName + "',ALLOWANCE_NAME_ID = '" + Cy.AllowanceName + "',ALLOWANCE_TYPE_ID = '" + Cy.AllowanceType + "',AMT_PERC = '" + Cy.AmtPerc + "',EFFECTIVE_DATE = '" + Cy.EffectiveDate + "',DESCRIPTION = '" + Cy.Description + "'  Where ID = '" + Cy.ID + "' ";
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

        public DataTable GetAllAssignAllowanceGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select ASSIGN_ALLOWANCE.ID,EMPMAST.EMPNAME,ALLOWANCE_MASTER.ALLOWANCE_NAME,ALLOWANCE_MASTER.ALLOWANCE_TYPE,ASSIGN_ALLOWANCE.IS_ACTIVE from ASSIGN_ALLOWANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = ASSIGN_ALLOWANCE.EMP_NAME LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_NAME_ID LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_TYPE_ID WHERE ASSIGN_ALLOWANCE.IS_ACTIVE='Y' ORDER BY ASSIGN_ALLOWANCE.ID DESC ";
            }
            else
            {
                SvSql = "Select ASSIGN_ALLOWANCE.ID,EMPMAST.EMPNAME,ALLOWANCE_MASTER.ALLOWANCE_NAME,ALLOWANCE_MASTER.ALLOWANCE_TYPE,ASSIGN_ALLOWANCE.IS_ACTIVE from ASSIGN_ALLOWANCE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = ASSIGN_ALLOWANCE.EMP_NAME LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_NAME_ID LEFT OUTER JOIN ALLOWANCE_MASTER ON ALLOWANCE_MASTER.ID = ASSIGN_ALLOWANCE.ALLOWANCE_TYPE_ID WHERE ASSIGN_ALLOWANCE.IS_ACTIVE='N' ORDER BY ASSIGN_ALLOWANCE.ID DESC ";
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
            SvSql = "Select EMPMASTID,EMPNAME from EMPMAST WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllowanceName()
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,ALLOWANCE_NAME from ALLOWANCE_MASTER WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllowanceType(string alltypeid)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,ALLOWANCE_TYPE from ALLOWANCE_MASTER WHERE ID = '" + alltypeid + "' AND IS_ACTIVE='Y'";
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
                        svSQL = "UPDATE ASSIGN_ALLOWANCE SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE ASSIGN_ALLOWANCE SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
