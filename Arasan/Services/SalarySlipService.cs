using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class SalarySlipService : ISalarySlipService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalarySlipService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllSalarySlipGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select SS.SALARY_SLIP_ID,EMPMAST.EMPNAME,SS.DEPT,SS.DESIG,SS.IS_ACTIVE from SALARY_SLIP SS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SS.EMP_NAME WHERE SS.IS_ACTIVE='Y' ORDER BY SS.SALARY_SLIP_ID DESC ";
            }
            else
            {
                SvSql = "Select SS.SALARY_SLIP_ID,EMPMAST.EMPNAME,SS.DEPT,SS.DESIG,SS.IS_ACTIVE from SALARY_SLIP SS LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = SS.EMP_NAME WHERE SS.IS_ACTIVE='N' ORDER BY SS.SALARY_SLIP_ID DESC ";
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

        public string SalarySlipCRUD(SalarySlip Cy)
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
                        svSQL = "Insert into SALARY_SLIP (EMP_NAME,EMP_CODE,DOJ,DEPT,DESIG,FATHER_NAME,DOB,BANK_NAME,ACC_NO,IFSC,PF_NO,ESI_NO,SAL_DIST_DATE,GROSS_SAL_DAY,BASIC_SALARY,DA,HRA,CONVEYANCE,OT_RATE,WASH_ALL,EDU_ALL,SPEC_ALL,PF,ESI,LOAN_ADV,INSURANCE,MEALS,FINE,TDS,OTH_DEDS,TOT_WORK_DAYS,NH_DAYS,WEEKOFF,WORKED_DAYS,LEAVE_DAYS,OP_CL,CL_TAKEN,CLO_CL,SALARY_DAYS,NET_SALARY) values ('" + Cy.EmpName + "','" + Cy.EmpCode + "','" + Cy.DOJ + "','" + Cy.Dept + "','" + Cy.Desg + "','" + Cy.FatherName + "','" + Cy.DOB + "','" + Cy.BankName + "','" + Cy.AccNo + "','" + Cy.IFSC + "','" + Cy.PFNo + "','" + Cy.ESINo + "','" + Cy.SalDistDate + "','" + Cy.GrossSalaryDay + "','" + Cy.BasicSalary + "','" + Cy.DA + "','" + Cy.HRA + "','" + Cy.Conveyance + "','" + Cy.OT + "','" + Cy.WA + "','" + Cy.EA + "','" + Cy.SA + "','" + Cy.PF + "','" + Cy.ESI + "','" + Cy.LoanAdv + "','" + Cy.Insurance + "','" + Cy.Meals + "','" + Cy.Fine + "','" + Cy.TDS + "','" + Cy.OtherDeductions + "','" + Cy.TotWorkDays + "','" + Cy.NHDays + "','" + Cy.WeekOff + "','" + Cy.WorkedDays + "','" + Cy.LeaveDays + "','" + Cy.OpCL + "','" + Cy.CLTaken + "','" + Cy.CloCL + "','" + Cy.SalaryDays + "','" + Cy.NetSalary + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE SALARY_SLIP SET EMP_NAME = '" + Cy.EmpName + "',EMP_CODE = '" + Cy.EmpCode + "',DOJ = '" + Cy.DOJ + "',DEPT = '" + Cy.Dept + "',DESIG = '" + Cy.Desg + "',FATHER_NAME = '" + Cy.FatherName + "',DOB = '" + Cy.DOB + "',BANK_NAME = '" + Cy.BankName + "',ACC_NO = '" + Cy.AccNo + "',IFSC = '" + Cy.IFSC + "',PF_NO = '" + Cy.PFNo + "',ESI_NO = '" + Cy.ESINo + "',SAL_DIST_DATE = '" + Cy.SalDistDate + "',GROSS_SAL_DAY = '" + Cy.GrossSalaryDay + "',BASIC_SALARY = '" + Cy.BasicSalary + "',DA = '" + Cy.DA + "',HRA = '" + Cy.HRA + "',CONVEYANCE = '" + Cy.Conveyance + "',OT_RATE = '" + Cy.OT + "',WASH_ALL = '" + Cy.WA + "',EDU_ALL = '" + Cy.EA + "',SPEC_ALL = '" + Cy.SA + "',PF = '" + Cy.PF + "',ESI = '" + Cy.ESI + "',LOAN_ADV = '" + Cy.LoanAdv + "',INSURANCE = '" + Cy.Insurance + "',MEALS = '" + Cy.Meals + "',FINE = '" + Cy.Fine + "',TDS = '" + Cy.TDS + "',OTH_DEDS = '" + Cy.OtherDeductions + "',TOT_WORK_DAYS = '" + Cy.TotWorkDays + "',NH_DAYS = '" + Cy.NHDays + "',WEEKOFF = '" + Cy.WeekOff + "',WORKED_DAYS = '" + Cy.WorkedDays + "',LEAVE_DAYS = '" + Cy.LeaveDays + "',OP_CL = '" + Cy.OpCL + "',CL_TAKEN = '" + Cy.CLTaken + "',CLO_CL = '" + Cy.CloCL + "',SALARY_DAYS = '" + Cy.SalaryDays + "',NET_SALARY = '" + Cy.NetSalary + "'  Where SALARY_SLIP_ID = '" + Cy.ID + "' ";
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
                        svSQL = "UPDATE SALARY_SLIP SET IS_ACTIVE ='N' WHERE SALARY_SLIP_ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE SALARY_SLIP SET IS_ACTIVE ='Y' WHERE SALARY_SLIP_ID='" + id + "'";
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
