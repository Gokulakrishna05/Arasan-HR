using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class AdvanceTMService : IAdvanceTM
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public AdvanceTMService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetAdvanceTMEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select ID,ATYPE,MXLIMIT,EGTYRULES,RETYPE,NOFINS,DEON,REMARKS  from ADVTYPEMASTER WHERE ID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllAdvanceTM(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select ID,ATYPE,MXLIMIT,EGTYRULES,RETYPE,NOFINS,DEON,REMARKS,IS_ACTIVE from ADVTYPEMASTER WHERE ADVTYPEMASTER.IS_ACTIVE='Y' ORDER BY ADVTYPEMASTER.ID DESC ";
            }
            else
            {
                SvSql = "Select ID,ATYPE,MXLIMIT,EGTYRULES,RETYPE,NOFINS,DEON,REMARKS,IS_ACTIVE from ADVTYPEMASTER WHERE ADVTYPEMASTER.IS_ACTIVE='N' ORDER BY ADVTYPEMASTER.ID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public string GetAdvanceT(AdvanceTM Em)
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
                        svSQL = "Insert into ADVTYPEMASTER (ID,ATYPE,MXLIMIT,EGTYRULES,RETYPE,NOFINS,DEON,REMARKS) values ('" + Em.ID + "','" + Em.AType + "','" + Em.MALmt + "','" + Em.ERules + "','" + Em.RPType + "','" + Em.NOIns + "','"+ Em.Dedn + "','" + Em.Rmarks + "') ";
                    }

                    else
                    {
                        svSQL = "UPDATE ADVTYPEMASTER SET ATYPE = '" + Em.AType + "',MXLIMIT = '" + Em.MALmt + "',EGTYRULES = '" + Em.ERules + "',RETYPE='" + Em.RPType + "',NOFINS='" + Em.NOIns + "',DEON='" + Em.Dedn + "',REMARKS='"+ Em.Rmarks + "'  Where ID = '" + Em.ID + "' ";

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
                        svSQL = "UPDATE ADVTYPEMASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE ADVTYPEMASTER SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
