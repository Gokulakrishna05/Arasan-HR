using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
//using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;
namespace Arasan.Services
{
    public class ResignationService : IResignation
    {

        private readonly string _connectionString;
        DataTransactions datatrans;

        public ResignationService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


       
        public DataTable GetEmpId()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPMASTID from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetEditResignation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select RESIGID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EMPID,EMPNAME,to_char(JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,REASONTYPE,REASON FROM RESIG WHERE RESIGID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ResignationCRUD(ResignationModel R)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {


                    objConn.Open();


                    if (R.ID == null)
                    {

                        svSQL = "Insert into RESIG (DOCID,DOCDATE,EMPID,EMPNAME,JOINDATE,RESIGNDATE,REASONTYPE,REASON) VALUES ('" + R.DocId + "','" + R.Date + "','" + R.EmpID + "','" + R.EmpName + "','" + R.EmpJoin + "','" + R.EmpResignation + "','" + R.TReason + "','" + R.Reason + "')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = " UPDATE RESIG SET DOCID ='" + R.DocId + "',DOCDATE='" + R.Date + "',EMPID='" + R.EmpID + "',EMPNAME='" + R.EmpName + "',JOINDATE='" + R.EmpJoin + "',RESIGNDATE='" + R.EmpResignation + "',REASONTYPE='" + R.TReason + "',REASON='" + R.Reason + "'   Where RESIGID = '" + R.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }



                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string StatusDelete(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RESIG SET IS_ACTIVE ='N' WHERE RESIGID='" + id + "'";
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
        //LIST PAGE AND FOR DELETE(GRID)
        public DataTable GetAllResignation(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select RESIGID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EMPMAST.EMPID,RESIG.EMPNAME,to_char(RESIG.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(RESIG.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,REASONTYPE,REASON,RESIG.IS_ACTIVE FROM RESIG left outer join EMPMAST ON EMPMASTID=RESIG.EMPID WHERE RESIG.IS_ACTIVE='Y'";
            }
            else
            {
                SvSql = "Select RESIGID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EMPMAST.EMPID,RESIG.EMPNAME,to_char(RESIG.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(RESIG.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,REASONTYPE,REASON,RESIG.IS_ACTIVE FROM RESIG left outer join EMPMAST ON EMPMASTID=RESIG.EMPID WHERE RESIG.IS_ACTIVE='Y'";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }






    }
}
