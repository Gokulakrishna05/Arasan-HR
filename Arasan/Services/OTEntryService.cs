using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Arasan.Services
{
    public class OTEntryService : IOTEntry
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public OTEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetOTEntryEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ID,EMPLOYEE_NAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS from OTENTRY WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllOTEntry(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select ID, EMPMAST.EMPNAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS,OTENTRY.IS_ACTIVE,OTENTRY.STATUS from OTENTRY LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = OTENTRY.EMPLOYEE_NAME WHERE OTENTRY.IS_ACTIVE='Y' ORDER BY OTENTRY.ID DESC";
                
                //SvSql = "Select ID,EMPMAST.EMPNAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS,OTENTRY.IS_ACTIVE from OTENTRY LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = OTENTRY.ID WHERE OTENTRY.IS_ACTIVE='Y' ORDER BY OTENTRY.ID DESC ";
            }
            else
            {
                SvSql = "Select ID, EMPMAST.EMPNAME,to_char(OT_DATE,'dd-MON-yyyy')OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS,OTENTRY.IS_ACTIVE from OTENTRY LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = OTENTRY.EMPLOYEE_NAME WHERE OTENTRY.IS_ACTIVE='N' ORDER BY OTENTRY.ID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string OTEntryCRUD(OTEntry Cy)
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
                        svSQL = "Insert into OTENTRY (EMPLOYEE_NAME,OT_DATE,DESCRIPTION,OT_PERFORMED_ON,OT_HOURS) values ('" + Cy.EmpName + "','" + Cy.Date + "','" + Cy.Description + "','" + Cy.OTON + "','" + Cy.OTHours + "') ";
                    }
                    else
                    {
                        svSQL = " UPDATE OTENTRY SET EMPLOYEE_NAME = '" + Cy.EmpName + "',OT_DATE = '" + Cy.Date + "',DESCRIPTION = '" + Cy.Description + "',OT_PERFORMED_ON='" + Cy.OTON + "',OT_HOURS='" + Cy.OTHours + "' Where ID = '" + Cy.PID + "' ";
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

        public string ViewOTEntry(OTEntry Cy)
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
                        svSQL = "UPDATE OTENTRY SET STATUS = 'Approve' Where ID = '" + Cy.PID + "' ";
                    }
                    else
                    {
                        svSQL = "UPDATE OTENTRY SET STATUS = 'Reject' Where ID = '" + Cy.PID + "' ";
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
                        svSQL = "UPDATE OTENTRY SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE OTENTRY SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
