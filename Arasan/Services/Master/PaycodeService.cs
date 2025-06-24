using Arasan.Interface;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services 
{
    public class PaycodeService : IPaycodeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PaycodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string PaycodeCRUD(Paycode by)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (by.ID == null)
                    {
                        svSQL = "Insert into PARAMETERBASIC (DOCID,DOCDATE) values ('PAY CODES','" + by.Set + "') RETURNING PARAMETERBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = " UPDATE PARAMETERBASIC SET  DOCID = 'PAY CODES',  DOCDATE =  '" + by.Set + "'  Where PARAMETERBASICID = '" + by.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    string Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    if (by.PayLists != null)
                    {
                            if (by.ID == null)
                        {
                            foreach (subgroup cp in by.PayLists)
                            {
                                if (cp.Isvalid == "Y" && cp.Paycode != "")
                                {

                                    svSQL = " Insert into PARAMETERDETAIL(PARAMETERBASICID, PAYCODE, PRINT, PRINTAS, ADDORLESS,PAYCODEVALUE, PAYCALCULATEFOR, SNO, FORMULA, DISPLAY) VALUES('" + Pid+"', '" + cp.Paycode + "', '" + cp.Print + "', '" + cp.PrintAs + "', '" + cp.Addorless + "', '" + cp.CalculateFrom + "', 'ONLY WORKINGDAYS', '" + cp.Sno + "', '" + cp.Formula + "', '" + cp.Display + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }

                        }
                        else
                        {
                            svSQL = "Delete PARAMETERDETAIL WHERE PAYCODEFOBASICID='" + by.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (subgroup cp in by.PayLists)
                            {
                                if (cp.Isvalid == "Y" && cp.Paycode != "")
                                {
                                    svSQL = " Insert into PARAMETERDETAIL(PARAMETERBASICID, PAYCODE, PRINT, PRINTAS, ADDORLESS,PAYCODEVALUE, PAYCALCULATEFOR,  SNO, FORMULA, DISPLAY) VALUES('" + Pid + "', '" + cp.Paycode + "', '" + cp.Print + "', '" + cp.PrintAs + "', '" + cp.Addorless + "', '" + cp.CalculateFrom + "','ONLY WORKINGDAYS', '" + cp.Sno + "', '" + cp.Formula + "', '" + cp.Display + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                    }
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
                //string status = tag == "Del" ? "N" : "Y";
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PARAMETERBASIC SET IS_ACTIVE ='N' WHERE PARAMETERBASICID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PARAMETERBASIC SET IS_ACTIVE ='Y' WHERE PARAMETERBASICID='" + id + "'";
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

        public DataTable GetAlLPaycode(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select DOCID ,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE ,PARAMETERBASICID,IS_ACTIVE from PARAMETERBASIC  WHERE IS_ACTIVE='Y' ";

            }
            else
            {
                SvSql = " Select DOCID ,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE ,PARAMETERBASICID,IS_ACTIVE from PARAMETERBASIC  WHERE IS_ACTIVE='N' ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPaycode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE FROM PARAMETERBASIC WHERE PARAMETERBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPayCodes(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARAMETERDETAILID,PARAMETERBASICID, PAYCODE, PRINT, PRINTAS, ADDORLESS, PAYCODEVALUE, PAYCALCULATEFOR,  SNO, FORMULA, DISPLAY FROM PARAMETERDETAIL  WHERE PARAMETERDETAIL.PARAMETERBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
