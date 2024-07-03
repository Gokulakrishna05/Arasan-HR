using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class QcTemplateService: IQcTemplateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QcTemplateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<QcTemplate> GetAllQcTemplate()
        {
          
            List<QcTemplate> brList = new List<QcTemplate>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT TESTTBASICID,TEMPLATEID,TESTTYPE,TEMPLATEDESC,IS_ACTIVE FROM TESTTBASIC  order by TESTTBASIC.TESTTBASICID DESC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QcTemplate br = new QcTemplate
                        {
                            ID = rdr["TESTTBASICID"].ToString(),
                            Qc = rdr["TEMPLATEID"].ToString(),
                            Test = rdr["TESTTYPE"].ToString(),
                            Description = rdr["TEMPLATEDESC"].ToString(),
                        };
                        brList.Add(br);
                    }
                }
            }
            return brList;
        }

        public DataTable GetDesc()
        {
            string SvSql = string.Empty;
            SvSql = " SELECT TESTDESCMASTERID,TESTDESC FROM TESTDESCMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQcTemplateEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTTBASICID,TEMPLATEID,SETBY,TESTTYPE,TEMPLATEDESC,QCTYPE,TESTPROCEDURE,SAMPLINGPER,ILEVEL FROM TESTTBASIC where TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQcTemplateItemEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTTBASICID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL,STARTVALUE,ENDVALUE FROM TESTTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTTDETAIL.UNIT where TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQcTemplateViewEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTTBASICID,TEMPLATEID,EMPMAST.EMPNAME,TESTTYPE,TEMPLATEDESC,QCTYPE,TESTPROCEDURE,SAMPLINGPER,ILEVEL FROM TESTTBASIC LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID =TESTTBASIC.SETBY where TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQcTemplateViewItemEdit(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTTBASICID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL,STARTVALUE,ENDVALUE FROM TESTTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTTDETAIL.UNIT where TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetQCTestDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTDESCMASTERID,UNITMAST.UNITID,VALUEORMANUAL,TESTDESCMASTER.UNIT FROM TESTDESCMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTDESCMASTER.UNIT where TESTDESC='" + itemId + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string QcTemplateCRUD(QcTemplate cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("TESTTBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DRUM_PROCEDURE";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    }

                    objCmd.Parameters.Add("TEMPLATEID", OracleDbType.NVarchar2).Value = cy.Qc;
                    objCmd.Parameters.Add("TESTTYPE", OracleDbType.NVarchar2).Value = cy.Test;
                    objCmd.Parameters.Add("SETBY", OracleDbType.NVarchar2).Value = cy.Set;
                    objCmd.Parameters.Add("TEMPLATEDESC", OracleDbType.NVarchar2).Value = cy.Description;
                    objCmd.Parameters.Add("QCTYPE", OracleDbType.NVarchar2).Value = cy.Type;
                    objCmd.Parameters.Add("TESTPROCEDURE", OracleDbType.NVarchar2).Value = cy.Procedure;
                    objCmd.Parameters.Add("SAMPLINGPER", OracleDbType.NVarchar2).Value = cy.Samplingper;
                    objCmd.Parameters.Add("ILEVEL", OracleDbType.NVarchar2).Value = cy.Level;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        if (cy.QcLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (QcTemplateItem cp in cy.QcLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ItemDesc != "0")
                                    {

                                        svSQL = "Insert into TESTTDETAIL (TESTTBASICID,TESTDESC,UNIT,VALUEORMANUAL,STARTVALUE,ENDVALUE) VALUES ('" + Pid + "','" + cp.ItemDesc + "','" + cp.Un + "','" + cp.Value + "','" + cp.Start + "','" + cp.End + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete TESTTDETAIL WHERE TESTTBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (QcTemplateItem cp in cy.QcLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ItemDesc != "0")
                                    {
                                        svSQL = "Insert into TESTTDETAIL (TESTTBASICID,TESTDESC,UNIT,VALUEORMANUAL,STARTVALUE,ENDVALUE) VALUES ('" + Pid + "','" + cp.ItemDesc + "','" + cp.Un + "','" + cp.Value + "','" + cp.Start + "','" + cp.End + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
        public DataTable GetAllQCTemp(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT TESTTBASICID,TEMPLATEID,TESTTYPE,TEMPLATEDESC,IS_ACTIVE FROM TESTTBASIC  order by TESTTBASIC.TESTTBASICID DESC ";
            }
            else
            {
                SvSql = "SELECT TESTTBASICID, TEMPLATEID, TESTTYPE, TEMPLATEDESC,IS_ACTIVE FROM TESTTBASIC  order by TESTTBASIC.TESTTBASICID DESC ";
            }
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
                    svSQL = "UPDATE TESTTBASIC SET IS_ACTIVE ='N' WHERE TESTTBASICID='" + id + "'";
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
                    svSQL = "UPDATE TESTTBASIC SET IS_ACTIVE = 'Y' WHERE TESTTBASICID='" + id + "'";
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
