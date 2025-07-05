using System.Data;
using System.Data.SqlClient;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class SalaryStructureService : ISalaryStructure
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalaryStructureService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetEditSalaryStructure(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,EMP_NAME,BASIC_SALARY,HRA,ALLOWANCE_AMT,OT_RATE,INCENTIVE,BONUS_ISELIGIBLE from SALARY_STRUCTURE WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SalaryStructureCRUD(SalaryStructure Cy)
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
                        svSQL = "Insert into SALARY_STRUCTURE (EMP_NAME,BASIC_SALARY,HRA,ALLOWANCE_AMT,OT_RATE,INCENTIVE,BONUS_ISELIGIBLE) values ('" + Cy.EmpName + "','" + Cy.Salary + "','" + Cy.HRA + "','" + Cy.AllowanceAmt + "','" + Cy.OTRate + "','" + Cy.Incentive + "','" + Cy.Bonus + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE SALARY_STRUCTURE SET EMP_NAME = '" + Cy.EmpName + "',BASIC_SALARY = '" + Cy.Salary + "',HRA = '" + Cy.HRA + "',ALLOWANCE_AMT = '" + Cy.AllowanceAmt + "',OT_RATE = '" + Cy.OTRate + "',INCENTIVE = '" + Cy.Incentive + "',BONUS_ISELIGIBLE = '" + Cy.Bonus + "'  Where ID = '" + Cy.ID + "' ";
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

        public DataTable GetAllSalaryStructureGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select SALARY_STRUCTURE.ID,EMPMAST.EMPNAME,BONUS_ISELIGIBLE,SALARY_STRUCTURE.IS_ACTIVE from SALARY_STRUCTURE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SALARY_STRUCTURE.EMP_NAME WHERE SALARY_STRUCTURE.IS_ACTIVE='Y' ORDER BY SALARY_STRUCTURE.ID DESC ";
            }
            else
            {
                SvSql = "Select SALARY_STRUCTURE.ID,EMPMAST.EMPNAME,BONUS_ISELIGIBLE,SALARY_STRUCTURE.IS_ACTIVE from SALARY_STRUCTURE LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SALARY_STRUCTURE.EMP_NAME WHERE SALARY_STRUCTURE.IS_ACTIVE='N' ORDER BY SALARY_STRUCTURE.ID DESC ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllAmtDetails(string allamtid)
        {
            string SvSql = string.Empty;
            SvSql = "select ASSIGN_ALLOWANCE.ID,AMT_PERC from ASSIGN_ALLOWANCE WHERE ASSIGN_ALLOWANCE.EMP_NAME='" + allamtid + "' AND ASSIGN_ALLOWANCE.IS_ACTIVE = 'Y'";
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

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    if (tag == "Del")
                    {
                        svSQL = "UPDATE SALARY_STRUCTURE SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE SALARY_STRUCTURE SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
