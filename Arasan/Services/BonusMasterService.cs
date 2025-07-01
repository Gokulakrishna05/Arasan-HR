using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class BonusMasterService : IBonusMaster
    {
         
        private readonly string _connectionString;
        DataTransactions datatrans;

        public BonusMasterService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetBonusMasterEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select ID,BONUSTYPE,ADESIGNATIONS,CALCULATION,BONUSVALUE,to_char(EFFECTIVEFROM,'dd-MON-yyyy')EFFECTIVEFROM,REMARKS  from BONUSMASTER WHERE ID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetAllBonusMaster(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                //SvSql = "Select ID,BONUSTYPE,ADESIGNATIONS,BONUSVALUE,BONUSMASTER.IS_ACTIVE from BONUSMASTER WHERE BONUSMASTER.IS_ACTIVE='Y' ORDER BY BONUSMASTER.ID DESC ";
                SvSql = "SELECT BONUSMASTER.ID, BONUSMASTER.BONUSTYPE, DDDETAIL.DESIGNATION, BONUSMASTER.BONUSVALUE, BONUSMASTER.IS_ACTIVE FROM BONUSMASTER LEFT OUTER JOIN DDDETAIL ON DDDETAIL.DDDETAILID = BONUSMASTER.ADESIGNATIONS WHERE BONUSMASTER.IS_ACTIVE = 'Y' ORDER BY BONUSMASTER.ID DESC";
            }
            else
            {
                SvSql = "SELECT BONUSMASTER.ID, BONUSMASTER.BONUSTYPE, DDDETAIL.DESIGNATION, BONUSMASTER.BONUSVALUE, BONUSMASTER.IS_ACTIVE FROM BONUSMASTER LEFT OUTER JOIN DDDETAIL ON DDDETAIL.DDDETAILID = BONUSMASTER.ADESIGNATIONS WHERE BONUSMASTER.IS_ACTIVE = 'N' ORDER BY BONUSMASTER.ID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string GetInsBonusM(BonusMaster Em)
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
                        svSQL = "Insert into BONUSMASTER (ID,BONUSTYPE,ADESIGNATIONS,CALCULATION,BONUSVALUE,EFFECTIVEFROM,REMARKS) values ('" + Em.ID + "','" + Em.BType + "','" + Em.ADes + "','" + Em.CType + "','"+Em.BValue + "','"+Em.EFrom+"','"+Em.Remarks+"') ";
                    }

                    else
                    {
                        svSQL = "UPDATE BONUSMASTER SET BONUSTYPE = '" + Em.BType + "',ADESIGNATIONS = '" + Em.ADes + "',CALCULATION = '" + Em.CType + "',BONUSVALUE='"+ Em.BValue + "',EFFECTIVEFROM='"+Em.EFrom + "',REMARKS='"+ Em.Remarks +"'  Where ID = '" + Em.ID + "' ";

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

        public DataTable GetDesignation()
        {
            string SvSql = string.Empty;
            SvSql = "Select DDDETAILID,DESIGNATION from DDDETAIL";
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
                        svSQL = "UPDATE BONUSMASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE BONUSMASTER SET IS_ACTIVE ='Y' WHERE ID='" + id + "'";
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
