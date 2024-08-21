using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class ActPlanningService : IActPlanning
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ActPlanningService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetMachine()
        {
            string SvSql = string.Empty;
            SvSql = "Select MACHINEINFOBASICID,MCODE,IS_ACTIVE from MACHINEINFOBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }           

        public DataTable GetAlloted()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMASTID,PARTYNAME,IS_ACTIVE from PARTYMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTERID,ITEMID,IS_ACTIVE from ITEMMASTER  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ActPlanningCRUD(ActPlanning cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACTPLANPROC", objConn);


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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docno;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("MACNO", OracleDbType.NVarchar2).Value = cy.MtId;
                    objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.AcTyp;
                    objCmd.Parameters.Add("ACTDATE", OracleDbType.NVarchar2).Value = cy.AcDat;
                    objCmd.Parameters.Add("ACTFRE", OracleDbType.NVarchar2).Value = cy.AcFre;
                    objCmd.Parameters.Add("DTYPE", OracleDbType.NVarchar2).Value = cy.DeTyp;
                    objCmd.Parameters.Add("MTYPE", OracleDbType.NVarchar2).Value = cy.MaTyp;
                    objCmd.Parameters.Add("FROMDATE", OracleDbType.NVarchar2).Value = cy.FrDat;
                    objCmd.Parameters.Add("TODATE", OracleDbType.NVarchar2).Value = cy.ToDat;
                    objCmd.Parameters.Add("FROMTIME", OracleDbType.NVarchar2).Value = cy.FrTim;
                    objCmd.Parameters.Add("TOTIME", OracleDbType.NVarchar2).Value = cy.ToTim;
                    objCmd.Parameters.Add("PREORBRE", OracleDbType.NVarchar2).Value = cy.PB;
                    objCmd.Parameters.Add("JOBTYPE", OracleDbType.NVarchar2).Value = cy.JTyp;
                    objCmd.Parameters.Add("ALLOTEDTO", OracleDbType.NVarchar2).Value = cy.AlTo;
                    objCmd.Parameters.Add("BDREF", OracleDbType.NVarchar2).Value = cy.BrDow;
                    objCmd.Parameters.Add("ACTDESC", OracleDbType.NVarchar2).Value = cy.ADes;
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
                        if (cy.Prolst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Promst cp in cy.Prolst)
                                {


                                    svSQL = "Insert into SCHMSPARES (SCHMAINBASICID,TOOLS) VALUES ('" + Pid + "','" + cp.tools + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                }

                            }
                            else
                            {
                                svSQL = "Delete SCHMSPARES WHERE SCHMAINBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Promst cp in cy.Prolst)
                                {

                                    svSQL = "Insert into SCHMSPARES (SCHMAINBASICID,TOOLS) VALUES ('" + cy.ID + "','" + cp.tools + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                }
                            }
                        }
                        if (cy.wclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Wcid cp in cy.wclst)
                                {


                                    svSQL = "Insert into SCHMREASON (SCHMAINBASICID,RDATE,REASON) VALUES ('" + Pid + "','" + cp.date + "','" + cp.rea + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                }

                            }
                            else
                            {
                                svSQL = "Delete SCHMREASON WHERE SCHMAINBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Wcid cp in cy.wclst)
                                {

                                    svSQL = "Insert into SCHMREASON (SCHMAINBASICID,RDATE,REASON) VALUES ('" + cy.ID + "','" + cp.date + "','" + cp.rea + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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


        public DataTable GetAllActPlanning(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SCHMAINBASIC.DOCID,to_char(SCHMAINBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,MACHINEINFOBASIC.MCODE,SCHMAINBASIC.TYPE,SCHMAINBASIC.IS_ACTIVE,SCHMAINBASIC.SCHMAINBASICID FROM SCHMAINBASIC left outer join MACHINEINFOBASIC ON MACHINEINFOBASICID=SCHMAINBASIC.MACNO WHERE SCHMAINBASIC.IS_ACTIVE = 'Y' ";

            }
            else
            {
                SvSql = "SELECT SCHMAINBASIC.DOCID,to_char(SCHMAINBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,MACHINEINFOBASIC.MCODE,SCHMAINBASIC.TYPE,SCHMAINBASIC.IS_ACTIVE,SCHMAINBASIC.SCHMAINBASICID FROM SCHMAINBASIC left outer join MACHINEINFOBASIC ON MACHINEINFOBASICID=SCHMAINBASIC.MACNO WHERE SCHMAINBASIC.IS_ACTIVE = 'N' ";

            }

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditActPlan(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID, to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,MACNO,TYPE,to_char(ACTDATE,'dd-MON-yyyy')ACTDATE,ACTFRE,DTYPE, MTYPE, to_char(FROMDATE,'dd-MON-yyyy')FROMDATE,to_char(TODATE,'dd-MON-yyyy')TODATE,FROMTIME, TOTIME, PREORBRE, JOBTYPE,ALLOTEDTO,BDREF,ACTDESC from SCHMAINBASIC where SCHMAINBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditTool(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TOOLS from SCHMSPARES where SCHMAINBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditReason(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(RDATE,'dd-MON-yyyy')RDATE, REASON from SCHMREASON where SCHMAINBASICID = '" + id + "' ";
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
                    svSQL = "UPDATE SCHMAINBASIC SET IS_ACTIVE ='N' WHERE SCHMAINBASICID='" + id + "'";
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
                    svSQL = "UPDATE SCHMAINBASIC SET IS_ACTIVE ='Y' WHERE SCHMAINBASICID='" + id + "'";
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
