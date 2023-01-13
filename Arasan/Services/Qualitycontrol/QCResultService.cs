using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Qualitycontrol
{
    public class QCResultService : IQCResultService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QCResultService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetQCResult(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNBLBASIC.DOCID,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,PARTYRCODE.PARTY,QCRESULTBASICID,QCRESULTBASIC.TESTEDBY,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION  from QCRESULTBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=QCRESULTBASIC.ITEMID LEFT OUTER JOIN GRNBLBASIC ON GRNBLBASICID=QCRESULTBASIC.GRNNO  LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND QCRESULTBASIC.QCRESULTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
           adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCResultDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNQTY,INSQTY,REJQTY,ACCQTY from QCRESULTDETAIL Where QCRESULTDETAILID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<QCResult> GetAllQCResult()
        {
            List<QCResult> cmpList = new List<QCResult>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select QCRESULTBASIC.GRNNO,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,PARTYRCODE.PARTY,QCRESULTBASIC.TESTEDBY,QCRESULTBASIC.LOCATION,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION,QCRESULTBASICID from QCRESULTBASIC LEFT OUTER JOIN  PARTYMAST on QCRESULTBASIC.PARTYID=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QCResult cmp = new QCResult
                        {
                            ID = rdr["QCRESULTBASICID"].ToString(), 
                            DocId = rdr["DOCID"].ToString(),
                            GRNNo = rdr["GRNNO"].ToString(),
                            GRNDate = rdr["GRNDATE"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(), 
                            Party = rdr["PARTY"].ToString(),
                            TestedBy = rdr["TESTEDBY"].ToString(),
                            Location = rdr["LOCATION"].ToString(),
                            Remarks = rdr["REMARKS"].ToString(),
                            QcLocation = rdr["QCLOCATION"].ToString(),
                    };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string QCResultCRUD(QCResult cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("QCRESULTPROC", objConn);

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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cy.GRNNo;
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.Date).Value = DateTime.Parse(cy.GRNDate);
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("TESTEDBY", OracleDbType.NVarchar2).Value = cy.TestedBy;
                    objCmd.Parameters.Add("LOCATION", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                    objCmd.Parameters.Add("QCLOCATION", OracleDbType.NVarchar2).Value = cy.QcLocation;
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
                        foreach (QCResultItem ca in cy.QCRLst)
                        {
                            if (ca.Isvalid == "Y" && ca.GrnQty != "0")
                            {   
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("QCRESULTDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("QCRESULTBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("GRNQTY", OracleDbType.NVarchar2).Value = ca.GrnQty;
                                    objCmds.Parameters.Add("INSQTY", OracleDbType.NVarchar2).Value = ca.InsQty;
                                    objCmds.Parameters.Add("REJQTY", OracleDbType.NVarchar2).Value = ca.RejQty;
                                    objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = ca.AccQty;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
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
        public DataTable GetGRN()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,GRNBLBASICID from GRNBLBASIC where GRNBLBASIC.STATUS IS NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYRCODE.ID,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,GRNBLBASICID from GRNBLBASIC LEFT OUTER JOIN  PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID where GRNBLBASIC.GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,GRNBLBASICID,GRNBLDETAILID from GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=GRNBLDETAIL.ITEMID where GRNBLDETAIL.GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYRCODE.PARTY,GRNBLBASICID from GRNBLBASIC LEFT OUTER JOIN  PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID where GRNBLBASIC.GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}
