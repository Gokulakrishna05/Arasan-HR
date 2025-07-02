using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services
{
    public class AllowanceMasterService : IAllowanceMaster
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AllowanceMasterService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetEditAllowanceMaster(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,ALLOWANCE_NAME,DESCRIPTION,ALLOWANCE_TYPE,APPLICABLE_LEVEL,IS_RECURRING,EFFECTIVE_DATE from ALLOWANCE_MASTER WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string AllowanceMasterCRUD(AllowanceMaster Cy)
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
                        svSQL = "Insert into ALLOWANCE_MASTER (ALLOWANCE_NAME,DESCRIPTION,ALLOWANCE_TYPE,APPLICABLE_LEVEL,IS_RECURRING,EFFECTIVE_DATE) values ('" + Cy.AllowanceName + "','" + Cy.Description + "','" + Cy.AllowanceType + "','" + Cy.ApplicableLevel + "','" + Cy.IsRecurring + "','" + Cy.EffectiveDate + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE ALLOWANCE_MASTER SET ALLOWANCE_NAME = '" + Cy.AllowanceName + "',DESCRIPTION = '" + Cy.Description + "',ALLOWANCE_TYPE = '" + Cy.AllowanceType + "',APPLICABLE_LEVEL = '" + Cy.ApplicableLevel + "',IS_RECURRING = '" + Cy.IsRecurring + "',EFFECTIVE_DATE = '" + Cy.EffectiveDate + "'  Where ID = '" + Cy.ID + "' ";
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

        public DataTable GetAllAllowanceMasterGrid(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select ID,ALLOWANCE_NAME,ALLOWANCE_TYPE,ALLOWANCE_MASTER.IS_ACTIVE from ALLOWANCE_MASTER WHERE ALLOWANCE_MASTER.IS_ACTIVE='Y' ORDER BY ALLOWANCE_MASTER.ID DESC ";
            }
            else
            {
                SvSql = "Select ID,ALLOWANCE_NAME,ALLOWANCE_TYPE,ALLOWANCE_MASTER.IS_ACTIVE from ALLOWANCE_MASTER WHERE ALLOWANCE_MASTER.IS_ACTIVE='N' ORDER BY ALLOWANCE_MASTER.ID DESC ";
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
                        svSQL = "UPDATE ALLOWANCE_MASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE ALLOWANCE_MASTER SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
