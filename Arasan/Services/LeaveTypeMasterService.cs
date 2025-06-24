using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class LeaveTypeMasterService : ILeaveTypeMaster
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public LeaveTypeMasterService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetLeaveTypeMasterEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select ID,LEAVETYPENAME,DESCRIPTION,MAXIMUMALLOWEDPERYEAR  from LEAVETYPEMASTER WHERE ID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public string GetInsLTM((LeaveTypeMaster Em)
        //{

        //}


        public DataTable GetAllLeaveTypeMaster(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select ID,LEAVETYPENAME,DESCRIPTION,MAXIMUMALLOWEDPERYEAR,LEAVETYPEMASTER.IS_ACTIVE from LEAVETYPEMASTER WHERE LEAVETYPEMASTER.IS_ACTIVE='Y' ORDER BY LEAVETYPEMASTER.ID DESC ";

            }
            else
            {
                SvSql = "Select ID,LEAVETYPENAME,DESCRIPTION,MAXIMUMALLOWEDPERYEAR,LEAVETYPEMASTER.IS_ACTIVE from LEAVETYPEMASTER WHERE LEAVETYPEMASTER.IS_ACTIVE='N' ORDER BY LEAVETYPEMASTER.ID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string GetInsLTM(LeaveTypeMaster Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into LEAVETYPEMASTER (ID,LEAVETYPENAME,DESCRIPTION,MAXIMUMALLOWEDPERYEAR) values ('" + Em.ID + "','" + Em.LTName + "','" + Em.Des + "','" + Em.Mapy + "') ";
                    }

                    else
                    {
                        svSQL = " UPDATE LEAVETYPEMASTER SET LEAVETYPENAME = '" + Em.LTName + "',DESCRIPTION = '" + Em.Des + "',MAXIMUMALLOWEDPERYEAR = '" + Em.Mapy+ "'  Where ID = '" + Em.ID + "' ";

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
                        svSQL = "UPDATE LEAVETYPEMASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE LEAVETYPEMASTER SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
