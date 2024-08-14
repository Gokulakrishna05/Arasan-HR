using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.PortableExecutable;
namespace Arasan.Services
{
    public class EmpLoginDetService : IEmpLoginDet
    {

        private readonly string _connectionString;
        DataTransactions datatrans;

        public EmpLoginDetService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //listjoin
        public DataTable GetAllEmpLog(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select EMPLOGINDETBASICID,EMPLOGINDETBASIC.DOCID,to_char(EMPLOGINDETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(EMPLOGINDETBASIC.ATTDATE,'dd-MON-yyyy')ATTDATE,EMPLOGINDETBASIC.HOLIDAY,EMPLOGINDETBASIC.MONTH,PCBASIC.PAYCATEGORY,EMPLOGINDETBASIC.IS_ACTIVE from EMPLOGINDETBASIC left outer join PCBASIC ON PCBASICID=EMPLOGINDETBASIC.PAYCATEGORY  WHERE EMPLOGINDETBASIC.IS_ACTIVE='Y' ORDER BY EMPLOGINDETBASIC.EMPLOGINDETBASICID DESC ";

            }
            else
            {
                SvSql = "Select EMPLOGINDETBASICID,EMPLOGINDETBASIC.DOCID,to_char(EMPLOGINDETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(EMPLOGINDETBASIC.ATTDATE,'dd-MON-yyyy')ATTDATE,EMPLOGINDETBASIC.HOLIDAY,EMPLOGINDETBASIC.MONTH,PCBASIC.PAYCATEGORY,EMPLOGINDETBASIC.IS_ACTIVE from EMPLOGINDETBASIC left outer join PCBASIC ON PCBASICID=EMPLOGINDETBASIC.PAYCATEGORY  WHERE EMPLOGINDETBASIC.IS_ACTIVE='Y' ORDER BY EMPLOGINDETBASIC.EMPLOGINDETBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetInslog(EmpLoginDetModel Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                if (Em.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Att-' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "Att-", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Att-' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    Em.DocId = docid;
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into EMPLOGINDETBASIC (DOCID,DOCDATE,ATTDATE,HOLIDAY,MONTH,PAYCATEGORY) values ('" + Em.DocId + "','" + Em.DocDate + "','" + Em.AttDate + "','" + Em.Holiday + "','" + Em.Month + "','" + Em.PayCategory + "') RETURNING EMPLOGINDETBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = "Insert into EMPLOGINDETBASIC (DOCID,DOCDATE,ATTDATE,HOLIDAY,MONTH,PAYCATEGORY) values ('" + Em.DocId + "','" + Em.DocDate + "','" + Em.AttDate + "','" + Em.Holiday + "','" + Em.Month + "','" + Em.PayCategory + "') RETURNING EMPLOGINDETBASICID  INTO: OUTID";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    if (Em.ID == null)
                    {
                        Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    }

                    if (Em.EmpLoginDetlists != null)
                    {
                        if (Em.ID == null)
                        {
                            int r = 1;
                            foreach (EmployeeLogin cp in Em.EmpLoginDetlists)
                            {
                                if (cp.Isvalid == "Y" && cp.EmpName != "")
                                {


                                    svSQL = "Insert into EMPLOGINDETDETAIL (EMPLOGINDETBASICID,EMPNAME,MISSION,ODTAKEN,ODHRS,WEEKOFF,SHIFTLOGINDATE,SHIFTLOGINTIME,LOGINDATE,LOGINTIME,SHIFTLOGOUTDATE,SHIFTLOGOUTTIME,LOGOUTDATE,LOGOUTTIME,INTDIFF,OUTTDIFF,TIMEDIFF,RMISSION,WHRS1,WHRS2,WORKEDHOURS,HA,OTHRS,ENSATION,STATUS) VALUES ('" + Pid + "','" + cp.EmpName + "','" + cp.Mission + "','" + cp.ODTaken + "','" + cp.ODHrs + "','" + cp.WeekOff + "','" + cp.ShiftLoginDate + "','" + cp.ShiftLoginTime + "','" + cp.LoginDate + "','" + cp.LoginTime + "','" + cp.ShiftLogoutDate + "','" + cp.ShiftLogoutTime + "','" + cp.LogoutDate + "','" + cp.LogoutTime + "','" + cp.IntDiff + "','" + cp.OuttDiff + "','" + cp.TimeDiff + "','" + cp.rmission + "','" + cp.WHrs1 + "','" + cp.WHrs2 + "','" + cp.WorkedHrs + "','" + cp.HA + "','" + cp.OTHrs + "','" + cp.ensation + "','" + cp.Status + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }

                                r++;
                            }

                        }
                        else
                        {
                            svSQL = "Delete EMPLOGINDETDETAIL WHERE EMPLOGINDETBASICID='" + Em.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (EmployeeLogin cp in Em.EmpLoginDetlists)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.EmpName != "")
                                {
                                    svSQL = "Insert into EMPLOGINDETDETAIL (EMPLOGINDETBASICID,EMPNAME,MISSION,ODTAKEN,ODHRS,WEEKOFF,SHIFTLOGINDATE,SHIFTLOGINTIME,LOGINDATE,LOGINTIME,SHIFTLOGOUTDATE,SHIFTLOGOUTTIME,LOGOUTDATE,LOGOUTTIME,INTDIFF,OUTTDIFF,TIMEDIFF,RMISSION,WHRS1,WHRS2,WORKEDHOURS,HA,OTHRS,ENSATION,STATUS) VALUES ('" + Em.ID + "','" + cp.EmpName + "','" + cp.Mission + "','" + cp.ODTaken + "','" + cp.ODHrs + "','" + cp.WeekOff + "','" + cp.ShiftLoginDate + "','" + cp.ShiftLoginTime + "','" + cp.LoginDate + "','" + cp.LoginTime + "','" + cp.ShiftLogoutDate + "','" + cp.ShiftLogoutTime + "','" + cp.LogoutDate + "','" + cp.LogoutTime + "','" + cp.IntDiff + "','" + cp.OuttDiff + "','" + cp.TimeDiff + "','" + cp.rmission + "','" + cp.WHrs1 + "','" + cp.WHrs2 + "','" + cp.WorkedHrs + "','" + cp.HA + "','" + cp.OTHrs + "','" + cp.ensation + "','" + cp.Status + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
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
        
        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PCBASICID,PAYCATEGORY from PCBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public DataTable GetEmpName()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPNAME from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetEmploginDetBasicEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select EMPLOGINDETBASICID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(ATTDATE,'dd-MON-yyyy')ATTDATE,HOLIDAY,EMPLOGINDETBASIC.MONTH,PAYCATEGORY from EMPLOGINDETBASIC WHERE EMPLOGINDETBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmploginDetDetailEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select EMPLOGINDETDETAILID,EMPLOGINDETBASICID,EMPNAME,MISSION,ODTAKEN,ODHRS,WEEKOFF,to_char(SHIFTLOGINDATE,'dd-MON-yyyy')SHIFTLOGINDATE,SHIFTLOGINTIME,to_char(LOGINDATE,'dd-MON-yyyy')LOGINDATE,LOGINTIME,to_char(SHIFTLOGOUTDATE,'dd-MON-yyyy')SHIFTLOGOUTDATE,SHIFTLOGOUTTIME,to_char(LOGOUTDATE,'dd-MON-yyyy')LOGOUTDATE,LOGOUTTIME,INTDIFF,OUTTDIFF,TIMEDIFF,RMISSION,WHRS1,WHRS2,WORKEDHOURS,HA,OTHRS,ENSATION,STATUS from  EMPLOGINDETDETAIL WHERE EMPLOGINDETBASICID='" + id + "'";


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
                        svSQL = "UPDATE EMPLOGINDETBASIC SET IS_ACTIVE ='N' WHERE EMPLOGINDETBASICID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE EMPLOGINDETBASIC SET IS_ACTIVE ='Y' WHERE EMPLOGINDETBASICID='" + id + "'";
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
