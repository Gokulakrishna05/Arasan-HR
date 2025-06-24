using Arasan.Interface;
using Arasan.Models;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services

{
    public class PayPeriodService : IPayPeriodService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PayPeriodService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string PayPeriodCRUD(PayPeriod pp)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            try
            {
                if (pp.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PP-F' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "PP-F", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PP-F' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    pp.DocId = docid;
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (pp.ID == null)
                    {
                        svSQL = "Insert into PPBASIC (DOCID,DOCDATE,PAYPERIODTYPE,STARTINGDATE,ENDINGDATE,SALDATE) values ('" + pp.DocId + "','" + pp.Set + "','" + pp.PayPeriodType + "','" + pp.StartingDate + "','" + pp.EndingDate + "','" + pp.SalaryDate + "') RETURNING PPBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = " UPDATE PPBASIC   DOCID = '" + pp.DocId + "',  DOCDATE =  '" + pp.Set + "',PAYPERIODTYPE = '" + pp.PayPeriodType + "',STARTINGDATE = '" + pp.StartingDate + "',ENDINGDATE = '" + pp.EndingDate + "',SALDATE = '" + pp.SalaryDate + "'  Where PPBASICID = '" + pp.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    string Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    if (pp.PayLists != null)
                    {
                        if (pp.ID == null)
                        {
                            int r = 1;
                            foreach (Pay cp in pp.PayLists)
                            {
                                if (cp.Isvalid == "Y" && cp.PayPeriods != "")
                                {

                                    svSQL = " Insert into PPDETAIL(PPBASICID, PAYPERIOD, STARTSAT, ENDSAT,SALARYDATE, PAYPERIODDAYS,NOOFWEEKLYHOLIDAYS, MONTHLYHOLIDAYS, OTHERHOLS, WORKINGDAYS) VALUES('" + Pid + "','" + cp.PayPeriods + "', '" + cp.StartsAt + "', '" + cp.EndsAt + "', '" + cp.SalaryDate + "','" + cp.PayPeriodDays + "', '" + cp.WeeklyHolidays + "', '" + cp.MonthlyHolidays + "', '" + cp.OtherHols + "', '" + cp.WorkingDays + "')";

                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }


                        }
                        else
                        {
                            svSQL = "Delete PPDETAIL WHERE PPBASICID='" + pp.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Pay cp in pp.PayLists)
                            {
                                if (cp.Isvalid == "Y" && cp.Paycode != "")
                                {
                                    svSQL = " Insert into PPDETAIL(PPBASICID, PAYPERIOD, STARTSAT, ENDSAT,SALARYDATE, PAYPERIODDAYS,NOOFWEEKLYHOLIDAYS, MONTHLYHOLIDAYS, OTHERHOLS, WORKINGDAYS) VALUES('" + pp.ID + "','" + cp.PayPeriods + "', '" + pp.StartingDate + "', '" + pp.EndingDate + "', '" + pp.SalaryDate + "','" + pp.PayPeriodType + "', '" + cp.WeeklyHolidays + "', '" + cp.MonthlyHolidays + "', '" + cp.OtherHols + "', '" + cp.WorkingDays + "')";

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
                    svSQL = "UPDATE PPBASIC SET IS_ACTIVE ='N' WHERE PPBASICID='" + id + "'";
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
                    svSQL = "UPDATE PPBASIC SET IS_ACTIVE ='Y' WHERE PPBASICID='" + id + "'";
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
        public DataTable GetAlLPayPeriod(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select DOCID ,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE ,PAYPERIODTYPE,to_char(STARTINGDATE,'dd-MON-yyyy')STARTINGDATE,to_char(ENDINGDATE,'dd-MON-yyyy')ENDINGDATE,to_char(SALDATE,'dd-MON-yyyy')SALDATE, PPBASICID,IS_ACTIVE from PPBASIC  WHERE IS_ACTIVE='Y' ";

            }
            else
            {
                SvSql = " Select DOCID ,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE ,PAYPERIODTYPE,to_char(STARTINGDATE,'dd-MON-yyyy')STARTINGDATE,to_char(ENDINGDATE,'dd-MON-yyyy')ENDINGDATE,to_char(SALDATE,'dd-MON-yyyy')SALDATE, PPBASICID,IS_ACTIVE from PPBASIC  WHERE IS_ACTIVE='N' ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPayPeriod(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,PAYPERIODTYPE,to_char(STARTINGDATE,'dd-MON-yyyy')STARTINGDATE,to_char(ENDINGDATE,'dd-MON-yyyy')ENDINGDATE,to_char(SALDATE,'dd-MON-yyyy')SALDATE FROM PPBASIC WHERE PPBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPayPeriod(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PPDETAILID,PPBASICID, PAYPERIOD, to_char(STARTSAT,'dd-MON-yyyy')STARTSAT, to_char(ENDSAT,'dd-MON-yyyy')ENDSAT,  to_char(SALARYDATE,'dd-MON-yyyy')SALARYDATE, PAYPERIODDAYS, NOOFWEEKLYHOLIDAYS,  MONTHLYHOLIDAYS,OTHERHOLS, WORKINGDAYS FROM PPDETAIL  WHERE PPDETAIL.PPBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
