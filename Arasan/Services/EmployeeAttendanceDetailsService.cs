 
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.PortableExecutable;


namespace Arasan.Services 
{
    public class EmployeeAttendanceDetailsService : IEmployeeAttendanceDetails
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public EmployeeAttendanceDetailsService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetEmployee(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,DOCDATE from EMPMISSINGPUNCHBASIC where EMPMISSINGPUNCHBASIC.EMPMISSINGPUNCHBASICID= '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPID from EMPMAST  WHERE IS_ACTIVE='Y'";
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
                        svSQL = "UPDATE EMPMISSINGPUNCHBASIC SET IS_ACTIVE ='N' WHERE EMPMISSINGPUNCHBASICID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE EMPMISSINGPUNCHBASIC SET IS_ACTIVE ='Y' WHERE EMPMISSINGPUNCHBASICID='" + id + "'";
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



        public DataTable GetEmployee()
        {
            throw new NotImplementedException();
        }

        public DataTable GetEmployeeAttendanceBasicEdit(string id)
        {
            string SvSql = string.Empty;

           SvSql = "Select EMPMISSINGPUNCHBASICID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE  from EMPMISSINGPUNCHBASIC WHERE EMPMISSINGPUNCHBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmployeeAttendanceDetailEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select EMPMISSINGPUNCHDETAILID,EMPID,EMPNAME,DEPARTMENT,INOUT,TIME from  EMPMISSINGPUNCHDETAIL WHERE EMPMISSINGPUNCHDETAILID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }





        public DataTable GetAllEmployeeDetail(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select EMPMISSINGPUNCHBASICID,DOCID,DOCDATE,EMPMISSINGPUNCHBASIC.IS_ACTIVE from EMPMISSINGPUNCHBASIC WHERE EMPMISSINGPUNCHBASIC.IS_ACTIVE='Y' ORDER BY EMPMISSINGPUNCHBASIC.EMPMISSINGPUNCHBASICID DESC ";

            }
            else
            {
                SvSql = "Select EMPMISSINGPUNCHBASICID,DOCID,DOCDATE,EMPMISSINGPUNCHBASIC.IS_ACTIVE from EMPMISSINGPUNCHBASIC WHERE EMPMISSINGPUNCHBASIC.IS_ACTIVE='Y' ORDER BY EMPMISSINGPUNCHBASIC.EMPMISSINGPUNCHBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }





        public string GetInsEmp(EmployeeAtttendanceDetailsModel Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                if (Em.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'MISP' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "MISP", (idc + 1).ToString());

                    updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='MISP' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    Em.DocId = docid;
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into EMPMISSINGPUNCHBASIC (DOCID,DOCDATE) values ('" + Em.DocId + "','" + Em.Docdate + "') RETURNING EMPMISSINGPUNCHBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = " UPDATE EMPMISSINGPUNCHBASIC SET DOCID = '" + Em.DocId + "', DOCDATE = '" + Em.Docdate + "'  Where EMPMISSINGPUNCHBASICID = '" + Em.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    if (Em.ID == null)
                    {
                        Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    }

                        if (Em.EmployeeAttendanceDetailslist != null)
                        {
                            if (Em.ID == null)
                            {
                                int r = 1;
                                foreach (AttendanceDetails cp in Em.EmployeeAttendanceDetailslist)
                                {
                                    if (cp.Isvalid == "Y" && cp.EmpID != "")
                                    {


                                        svSQL = "Insert into EMPMISSINGPUNCHDETAIL (EMPMISSINGPUNCHBASICID,EMPID,EMPNAME,DEPARTMENT,INOUT,TIME) VALUES ('" + Pid + "','" + cp.EmpID + "','" + cp.EmpName + "','" + cp.Depart + "','" + cp.InOut + "','" + cp.Time + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                        objCmds.ExecuteNonQuery();

                                    }

                                    r++;
                                }

                            }
                            else
                            {
                                svSQL = "Delete EMPMISSINGPUNCHDETAIL WHERE EMPMISSINGPUNCHBASICID='" + Em.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                                objCmdd.ExecuteNonQuery();
                                foreach (AttendanceDetails cp in Em.EmployeeAttendanceDetailslist)
                                {
                                    int r = 1;
                                    if (cp.Isvalid == "Y" && cp.EmpID != "")
                                    {
                                        svSQL = "Insert into EMPMISSINGPUNCHDETAIL (EMPMISSINGPUNCHBASICID,EMPID,EMPNAME,DEPARTMENT,INOUT,TIME) VALUES ('" + Em.ID + "','" + cp.EmpID + "','" + cp.EmpName + "','" + cp.Depart + "','" + cp.InOut + "','" + cp.Time + "')";
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

      
    }

}