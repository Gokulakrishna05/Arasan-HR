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
            SvSql = "Select ID,EMP_NAME from ASSIGN_ALLOWANCE WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditAssignAllowanceDetails(string? id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ASSALLDETAILID,ID,ALLOWANCE_NAME_ID,REMARKS,ALLOWANCE_TYPE,AMT_PERC,to_char(EFFECTIVE_DATE,'dd-MON-yyyy')EFFECTIVE_DATE from ASSIGN_ALLOWNACE_DETAILS WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string AssignAllowanceCRUD(AssignAllowance Cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ASSALLBASICPROC", objConn);


                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (Cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = Cy.ID;
                    }

                    objCmd.Parameters.Add("EmpName", OracleDbType.NVarchar2).Value = Cy.EmpName;

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (Cy.ID != null)
                        {
                            Pid = Cy.ID;
                        }
                        if (Cy.Allowancelst != null)
                        {
                            if (Cy.ID == null)
                            {
                                foreach (SelectAllowance cp in Cy.Allowancelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.EffectiveDate != "")
                                    {

                                        svSQL = "Insert into ASSIGN_ALLOWNACE_DETAILS (ASSALLDETAILID,ALLOWANCE_NAME_ID,ALLOWANCE_TYPE,AMT_PERC,EFFECTIVE_DATE,REMARKS) VALUES ('" + Pid + "','" + cp.AllowanceName + "','" + cp.AllowanceType + "','" + cp.AmtPerc + "','" + cp.EffectiveDate + "','" + cp.Description + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete ASSIGN_ALLOWNACE_DETAILS WHERE ASSALLDETAILID='" + Cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (SelectAllowance cp in Cy.Allowancelst)
                                {
                                    if (cp.Isvalid == "Y" && cp.EffectiveDate != "")
                                    {

                                        svSQL = "Insert into ASSIGN_ALLOWNACE_DETAILS (ASSALLDETAILID,ALLOWANCE_NAME_ID,ALLOWANCE_TYPE,AMT_PERC,EFFECTIVE_DATE,REMARKS) VALUES ('" + Cy.ID + "','" + cp.AllowanceName + "','" + cp.AllowanceType + "','" + cp.AmtPerc + "','" + cp.EffectiveDate + "','" + cp.Description + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public DataTable GetAllAssignAllowanceGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT AA.ID, EM.EMPNAME, AMN.ALLOWANCE_NAME, AMT.ALLOWANCE_TYPE, AA.IS_ACTIVE FROM ASSIGN_ALLOWANCE AA LEFT JOIN EMPMAST EM ON EM.EMPMASTID = AA.EMP_NAME LEFT JOIN ASSIGN_ALLOWNACE_DETAILS AD ON AD.ID = AA.ID LEFT JOIN ALLOWANCE_MASTER AMN ON AMN.ID = AD.ALLOWANCE_NAME_ID LEFT JOIN ALLOWANCE_MASTER AMT ON AMT.ID = AD.ALLOWANCE_TYPE WHERE AA.IS_ACTIVE = 'Y' ORDER BY AA.ID DESC";
            }
            else
            {
                SvSql = "SELECT AA.ID, EM.EMPNAME, AMN.ALLOWANCE_NAME, AMT.ALLOWANCE_TYPE, AA.IS_ACTIVE FROM ASSIGN_ALLOWANCE AA LEFT JOIN EMPMAST EM ON EM.EMPMASTID = AA.EMP_NAME LEFT JOIN ASSIGN_ALLOWNACE_DETAILS AD ON AD.ID = AA.ID LEFT JOIN ALLOWANCE_MASTER AMN ON AMN.ID = AD.ALLOWANCE_NAME_ID LEFT JOIN ALLOWANCE_MASTER AMT ON AMT.ID = AD.ALLOWANCE_TYPE WHERE AA.IS_ACTIVE = 'N' ORDER BY AA.ID DESC";
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
