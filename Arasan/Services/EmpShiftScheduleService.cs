using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Arasan.Services
{
    public class EmpShiftScheduleService : IEmpShiftSchedule
    {
       
        private readonly string _connectionString;
        DataTransactions datatrans;

        public EmpShiftScheduleService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
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
        public DataTable GetDep()
        {
            string SvSql = string.Empty;
            SvSql = "Select DDBASICID,DEPTNAME from DDBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmplId()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPNAME || '/'||EMPID as empid from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetShift()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmployeeDetail(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EMPMASTID, EMPID, EMPNAME FROM EMPMAST WHERE EMPMAST.EMPPAYCAT='" + ItemId + "' AND EMPMAST.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllEmpShift(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select EMPSHIFTBASICID,EMPSHIFTBASIC.DOCID,MONTH,PCBASIC.PAYCATEGORY,DDBASIC.DEPTCODE ,EMPSHIFTBASIC.IS_ACTIVE from EMPSHIFTBASIC left outer join PCBASIC ON PCBASICID=EMPSHIFTBASIC.PAYCATEGORY left outer join DDBASIC ON DDBASICID=EMPSHIFTBASIC.DEPARTMENT WHERE EMPSHIFTBASIC.IS_ACTIVE='Y' ORDER BY EMPSHIFTBASIC.EMPSHIFTBASICID DESC ";

            }
            else
            {
                SvSql = "Select EMPSHIFTBASICID,EMPSHIFTBASIC.DOCID,MONTH,PCBASIC.PAYCATEGORY,DDBASIC.DEPTCODE ,EMPSHIFTBASIC.IS_ACTIVE from EMPSHIFTBASIC left outer join PCBASIC ON PCBASICID=EMPSHIFTBASIC.PAYCATEGORY left outer join DDBASIC ON DDBASICID=EMPSHIFTBASIC.DEPARTMENT WHERE EMPSHIFTBASIC.IS_ACTIVE='N' ORDER BY EMPSHIFTBASIC.EMPSHIFTBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetShiftScheduleBasicEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPSHIFTBASICID,DOCID,MONTH,PAYCATEGORY,DEPARTMENT from EMPSHIFTBASIC WHERE EMPSHIFTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetShiftScheduleDetailEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPSHIFTDETAILID,EMPLID,EMPID,EMPNAME,to_char(STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(ENDDATE,'dd-MON-yyyy')ENDDATE,SHVALL,SHIFT,STTIME,ENDTIME,WOFF from  EMPSHIFTDETAIL WHERE EMPSHIFTBASICID='" + id + "'";
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
                        svSQL = "UPDATE EMPSHIFTBASIC SET IS_ACTIVE ='N' WHERE EMPSHIFTBASICID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE EMPSHIFTBASIC SET IS_ACTIVE ='Y' WHERE EMPSHIFTBASICID='" + id + "'";
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


        public string GetInsEmp(EmpShiftScheduleModel Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                if (Em.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SHI#' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "SHI#", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SHI#' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    Em.DocId = docid;
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into EMPSHIFTBASIC (DOCID,MONTH,PAYCATEGORY,DEPARTMENT) values ('" + Em.DocId + "','" + Em.Month + "','" + Em.EmpCategory + "','" + Em.Dep + "') RETURNING EMPSHIFTBASICID  INTO: OUTID";
                    }
                    else
                    {
                        svSQL = " UPDATE EMPSHIFTBASIC SET DOCID = '" + Em.DocId + "', MONTH = '" + Em.Month + "', PAYCATEGORY = '" + Em.EmpCategory + "', DEPARTMENT = '" + Em.Dep + "'  Where EMPSHIFTBASICID = '" + Em.ID + "'";
                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    if (Em.ID == null)
                    {
                        Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    }

                    if (Em.EmpShiftSchedulelist != null)
                    {
                        if (Em.ID == null)
                        {
                            int r = 1;
                            foreach (EmployeeShift cp in Em.EmpShiftSchedulelist)
                            {
                                if (cp.Isvalid == "Y" && cp.empid != "")
                                {
                                    svSQL = "Insert into EMPSHIFTDETAIL (EMPSHIFTBASICID,EMPID,EMPNAME,STARTDATE,ENDDATE,SHVALL,SHIFT,STTIME,ENDTIME,WOFF) VALUES ('" + Pid + "','" + cp.empid + "','" + cp.empname + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.ShVal + "','" + cp.Shift + "','" + cp.StTime + "','" + cp.EndTime + "','" + cp.WOFF + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();
                                }
                               
                                r++;
                            }
                        }
                        else
                        {
                            svSQL = "Delete EMPSHIFTDETAIL WHERE EMPSHIFTBASICID='" + Em.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (EmployeeShift cp in Em.EmpShiftSchedulelist)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.empid != "")
                                {
                                    svSQL = "Insert into EMPSHIFTDETAIL (EMPSHIFTBASICID,EMPID,EMPNAME,STARTDATE,ENDDATE,SHVALL,SHIFT,STTIME,ENDTIME,WOFF) VALUES ('" + Em.ID + "','" + cp.empid + "','" + cp.empname + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.ShVal + "','" + cp.Shift + "','" + cp.StTime + "','" + cp.EndTime + "','" + cp.WOFF + "')";
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
                throw;
            }
            return msg;
        }

    }
}
