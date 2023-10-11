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
            SvSql = "Select QCRESULTBASIC.GRNNO,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCRESULTBASIC.PARTYID,QCRESULTBASICID,QCRESULTBASIC.TESTEDBY,QCRESULTBASIC.LOCATION,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION  from QCRESULTBASIC  WHERE QCRESULTBASIC.QCRESULTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
           adapter.Fill(dtt);
            return dtt;
        }
        
        //public DataTable GetLocation()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetQCResultDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,GRNQTY,REJQTY,ACCQTY,COSTRATE,QCRESULTBASICID from QCRESULTDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=QCRESULTDETAIL.ITEMID Where QCRESULTDETAIL.QCRESULTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<QCResult> GetAllQCResult(string st, string ed)
        {
            List<QCResult> cmpList = new List<QCResult>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        //cmd.CommandText = "Select QCRESULTBASIC.GRNNO,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,PARTYMAST.PARTYNAME,QCRESULTBASIC.TESTEDBY,LOCDETAILS.LOCID,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION,QCRESULTBASICID,QCRESULTBASIC.STATUS from QCRESULTBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=QCRESULTBASIC.LOCATION LEFT OUTER JOIN  PARTYMAST on QCRESULTBASIC.PARTYID=PARTYMAST.PARTYMASTID  WHERE QCRESULTBASIC.DOCDATE BETWEEN '" + st + "'  AND ' " + ed + "' AND  QCRESULTBASIC.STATUS = 'ACTIVE' order by QCRESULTBASICID desc";

                         cmd.CommandText = "Select QCRESULTBASIC.GRNNO,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,PARTYMAST.PARTYNAME,QCRESULTBASIC.TESTEDBY,LOCDETAILS.LOCID,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION,QCRESULTBASICID,QCRESULTBASIC.STATUS from QCRESULTBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=QCRESULTBASIC.LOCATION LEFT OUTER JOIN  PARTYMAST on QCRESULTBASIC.PARTYID=PARTYMAST.PARTYMASTID where QCRESULTBASIC.STATUS = 'ACTIVE' ";
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
                                Party = rdr["PARTYNAME"].ToString(),
                                TestedBy = rdr["TESTEDBY"].ToString(),
                                Location = rdr["LOCID"].ToString(),
                                Remarks = rdr["REMARKS"].ToString(),
                                QcLocation = rdr["QCLOCATION"].ToString(),
                                Stat = rdr["STATUS"].ToString()
                            };
                            cmpList.Add(cmp);
                        }
                    }
                }
                else
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select QCRESULTBASIC.GRNNO,QCRESULTBASIC.DOCID,to_char(QCRESULTBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(QCRESULTBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,PARTYMAST.PARTYNAME,QCRESULTBASIC.TESTEDBY,LOCDETAILS.LOCID,QCRESULTBASIC.REMARKS,QCRESULTBASIC.QCLOCATION,QCRESULTBASICID,QCRESULTBASIC.STATUS from QCRESULTBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=QCRESULTBASIC.LOCATION LEFT OUTER JOIN  PARTYMAST on QCRESULTBASIC.PARTYID=PARTYMAST.PARTYMASTID  WHERE QCRESULTBASIC.DOCDATE > sysdate-30 order by QCRESULTBASICID desc";
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
                                Party = rdr["PARTYNAME"].ToString(),
                                TestedBy = rdr["TESTEDBY"].ToString(),
                                Location = rdr["LOCID"].ToString(),
                                Remarks = rdr["REMARKS"].ToString(),
                                QcLocation = rdr["QCLOCATION"].ToString(),
                            };
                            cmpList.Add(cmp);
                        }
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
                datatrans = new DataTransactions(_connectionString);


                if (cy.ID == null)
                {

                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'INS-' AND ACTIVESEQUENCE = 'T'  ");
                    string DocId = string.Format("{0}{1}", "INS-", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='INS-' AND ACTIVESEQUENCE ='T'  ";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocId = DocId;
                }
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
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.NVarchar2).Value =  cy.GRNDate;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("TESTEDBY", OracleDbType.NVarchar2).Value = cy.TestedBy;
                    objCmd.Parameters.Add("LOCATION", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                    objCmd.Parameters.Add("QCLOCATION", OracleDbType.NVarchar2).Value = cy.QcLocation;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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
                        foreach (QCResultItem ca in cy.QResLst)
                        {
                            if (ca.Isvalid == "Y" && ca.ItemId != "0")
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
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("QCRESULTBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ca.ItemId;
                                    objCmds.Parameters.Add("GRNQTY", OracleDbType.NVarchar2).Value = ca.GrnQty;
                                    //objCmds.Parameters.Add("INSQTY", OracleDbType.NVarchar2).Value = ca.InsQty;
                                    objCmds.Parameters.Add("REJQTY", OracleDbType.NVarchar2).Value = ca.RejQty;
                                    objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = ca.AccQty;
                                    objCmds.Parameters.Add("COSTRATE", OracleDbType.NVarchar2).Value = ca.CostRate;
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
            SvSql = "Select GRNBLBASIC.DOCID,QCVALUEBASIC.GRNNO from QCVALUEBASIC  left outer join GRNBLBASIC on GRNBLBASIC.GRNBLBASICID=QCVALUEBASIC.GRNNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,QCVALUEBASICID from QCVALUEBASIC where QCVALUEBASIC.GRNNO='" + id + "'";
            SvSql = "Select to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,QCVALUEBASIC.QCVALUEBASICID,PARTYMAST.PARTYNAME,PARTYMAST.PARTYMASTID from QCVALUEBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = QCVALUEBASIC.PARTYID where QCVALUEBASIC.GRNNO ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetPartybyId(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select SALES_QUOTE.SALESQUOTEID,ENQ_TYPE,SALESENQUIRYID from SALES_ENQUIRY LEFT OUTER JOIN SALES_QUOTE ON SALESQUOTEID=SALES_ENQUIRY.SALESENQUIRYID WHERE SALES_ENQUIRY.SALESENQUIRYID='" + id + "'";

        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetGRNItemDetails(string id) 
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNBLDETAIL.QTY,GRNBLDETAIL.ACCQTY,GRNBLDETAIL.REJQTY,GRNBLDETAIL.COSTRATE,GRNBLDETAILID from GRNBLDETAIL where GRNBLDETAIL.ITEMID   ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,GRNBLDETAIL.ITEMID as item,GRNBLBASICID,GRNBLDETAILID from GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=GRNBLDETAIL.ITEMID where GRNBLDETAIL.GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYNAME, QCVALUEBASIC.PARTYID,QCVALUEBASICID from QCVALUEBASIC LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID where QCVALUEBASIC.GRNNO='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE QCRESULTBASIC SET STATUS ='ISACTIVE' WHERE QCRESULTBASICID='" + id + "'";
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
