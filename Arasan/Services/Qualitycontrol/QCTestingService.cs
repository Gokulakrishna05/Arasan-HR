using Arasan.Interface;

using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class QCTestingService : IQCTestingService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QCTestingService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<QCTesting> GetAllQCTesting(string st, string ed)
        {
            List<QCTesting> cmpList = new List<QCTesting>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select ITEMMASTER.ITEMID,QCVALUEBASIC.GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYMAST.PARTYNAME,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTBY,QCVALUEBASIC.REMARKS, QCVALUEBASIC.STATUS,QCVALUEBASIC.GRNPROD,QCVALUEBASIC.TESTPROCEDURE from QCVALUEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=QCVALUEBASIC.ITEMID LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID  WHERE QCVALUEBASIC.DOCDATE BETWEEN '" + st + "'  AND ' " + ed + "' order by QCVALUEBASICID desc";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            QCTesting cmp = new QCTesting
                            {
                                ID = rdr["QCVALUEBASICID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                GRNNo = rdr["GRNNO"].ToString(),
                                GRNDate = rdr["GRNDATE"].ToString(),
                                DocDate = rdr["DOCDATE"].ToString(),
                                ClassCode = rdr["CLASSCODE"].ToString(),
                                SNo = rdr["SLNO"].ToString(),
                                LotNo = rdr["LOTSERIALNO"].ToString(),
                                Party = rdr["PARTYNAME"].ToString(),
                                ItemId = rdr["ITEMID"].ToString(),
                                TestResult = rdr["TESTRESULT"].ToString(),
                                TestBy = rdr["TESTBY"].ToString(),
                                Remarks = rdr["REMARKS"].ToString(),
                                Stat = rdr["STATUS"].ToString(),
                                GRNProd = rdr["GRNPROD"].ToString(),
                                Procedure = rdr["TESTPROCEDURE"].ToString()


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

                        cmd.CommandText = "Select ITEMMASTER.ITEMID,QCVALUEBASIC.GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYMAST.PARTYNAME,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTBY,QCVALUEBASIC.REMARKS, QCVALUEBASIC.STATUS from QCVALUEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=QCVALUEBASIC.ITEMID LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID  WHERE QCVALUEBASIC.DOCDATE > sysdate-30 order by QCVALUEBASICID desc";

                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            QCTesting cmp = new QCTesting
                            {

                                ID = rdr["QCVALUEBASICID"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                GRNNo = rdr["GRNNO"].ToString(),
                                GRNDate = rdr["GRNDATE"].ToString(),
                                DocDate = rdr["DOCDATE"].ToString(),
                                ClassCode = rdr["CLASSCODE"].ToString(),
                                SNo = rdr["SLNO"].ToString(),
                                LotNo = rdr["LOTSERIALNO"].ToString(),
                                Party = rdr["PARTYNAME"].ToString(),
                                ItemId = rdr["ITEMID"].ToString(),
                                TestResult = rdr["TESTRESULT"].ToString(),
                                TestBy = rdr["TESTBY"].ToString(),
                                Remarks = rdr["REMARKS"].ToString()

                            };
                            cmpList.Add(cmp);
                        }
                    }
                }
            }
            return cmpList;
        }
        public string QCTestingCRUD(QCTesting cy)
        {
            string msg = "";
            try
            {
                if (cy.ID != null)
                {
                    cy.ID = null;
                }
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);

                if (cy.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'TE--' AND ACTIVESEQUENCE = 'T'  ");
                    string DocId = string.Format("{0}{1}", "TE--", (idc + 1).ToString());
                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='TE--' AND ACTIVESEQUENCE ='T'  ";
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

                using ( OracleConnection objConn = new OracleConnection(_connectionString))
                 {
                    OracleCommand objCmd = new OracleCommand("QCTESTINGPROC", objConn);
                   
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
                    objCmd.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cy.APID;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value =  cy.DocDate ;
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.NVarchar2).Value =  cy.GRNDate ;
                    objCmd.Parameters.Add("CLASSCODE", OracleDbType.NVarchar2).Value = cy.ClassCode;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Partyid;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("SLNO", OracleDbType.NVarchar2).Value = cy.SNo;
                    objCmd.Parameters.Add("LOTSERIALNO", OracleDbType.NVarchar2).Value = cy.LotNo;
                    objCmd.Parameters.Add("TESTRESULT", OracleDbType.NVarchar2).Value = cy.TestResult;
                    objCmd.Parameters.Add("TESTBY", OracleDbType.NVarchar2).Value = cy.TestBy;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("GRNPROD", OracleDbType.NVarchar2).Value = cy.GRNProd;
                    objCmd.Parameters.Add("TESTPROCEDURE", OracleDbType.NVarchar2).Value = cy.Procedure;
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

                        foreach (QCGRNItem cp in cy.QCGRNLst)
                        {
                            if (cp.Isvalid == "Y" && cp.testid != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("QCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("QCVALUEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("TESTDESC", OracleDbType.NVarchar2).Value = cp.testid;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.unit;
                                    objCmds.Parameters.Add("VALUEORMANUAL", OracleDbType.NVarchar2).Value = cp.value;
                                    objCmds.Parameters.Add("STARTVALUE", OracleDbType.NVarchar2).Value = cp.startvalue;
                                    objCmds.Parameters.Add("ENDVALUE", OracleDbType.NVarchar2).Value = cp.endvalue;
                                    objCmds.Parameters.Add("TESTVALUE", OracleDbType.NVarchar2).Value = cp.test;
                                    objCmds.Parameters.Add("MANUALVALUE", OracleDbType.NVarchar2).Value = cp.manual;
                                    objCmds.Parameters.Add("ACTTESTVALUE", OracleDbType.NVarchar2).Value = cp.actual;
                                    objCmds.Parameters.Add("RESULT", OracleDbType.NVarchar2).Value = cp.testresult;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    svSQL = "Update GRNBLBASIC set QCSTATUS ='Qc Completed' WHERE  GRNBLBASICID='" + cy.APID + "'";
                                    OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                    objCmdd.ExecuteNonQuery();
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
        public string POQCTestingCRUD(QCTesting cy)
        {
            string msg = "";
            try
            {
                if (cy.ID != null)
                {
                    cy.ID = null;
                }
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                if (cy.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'TE--' AND ACTIVESEQUENCE = 'T'  ");
                    string DocId = string.Format("{0}{1}", "TE--", (idc + 1).ToString());
                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='TE--' AND ACTIVESEQUENCE ='T'  ";
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
                    OracleCommand objCmd = new OracleCommand("QCTESTINGPROC", objConn);

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
                    objCmd.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cy.PoId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.NVarchar2).Value = cy.PoDate;
                    objCmd.Parameters.Add("CLASSCODE", OracleDbType.NVarchar2).Value = cy.ClassCode;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Par;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("SLNO", OracleDbType.NVarchar2).Value = cy.SNo;
                    objCmd.Parameters.Add("LOTSERIALNO", OracleDbType.NVarchar2).Value = cy.LotNo;
                    objCmd.Parameters.Add("TESTRESULT", OracleDbType.NVarchar2).Value = cy.TestResult;
                    objCmd.Parameters.Add("TESTBY", OracleDbType.NVarchar2).Value = cy.TestBy;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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

                        foreach (QCPOItem cp in cy.QCPOLst)
                        {
                            if (cp.Isvalid == "Y" && cp.unit != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("QCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("QCVALUEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("TESTDESC", OracleDbType.NVarchar2).Value = cp.testid;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.unit;
                                    objCmds.Parameters.Add("VALUEORMANUAL", OracleDbType.NVarchar2).Value = cp.value;
                                    objCmds.Parameters.Add("STARTVALUE", OracleDbType.NVarchar2).Value = cp.startvalue;
                                    objCmds.Parameters.Add("ENDVALUE", OracleDbType.NVarchar2).Value = cp.endvalue;
                                    objCmds.Parameters.Add("TESTVALUE", OracleDbType.NVarchar2).Value = cp.test;
                                    objCmds.Parameters.Add("MANUALVALUE", OracleDbType.NVarchar2).Value = cp.manual;
                                    objCmds.Parameters.Add("ACTTESTVALUE", OracleDbType.NVarchar2).Value = cp.actual;
                                    objCmds.Parameters.Add("RESULT", OracleDbType.NVarchar2).Value = cp.testresult;
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
        public DataTable GetGRN(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,GRNBLBASICID from GRNBLBASIC where GRNBLBASIC.STATUS IS NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPO(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,POBASICID from POBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
     
        public DataTable GetQCTesting(string id)  
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMID,GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYID,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTBY,QCVALUEBASIC.REMARKS from QCVALUEBASIC Where QCVALUEBASIC.QCVALUEBASICID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,GRNBLBASICID from GRNBLBASIC where GRNBLBASIC.GRNBLBASICID='" + id +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPODetails(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select to_char(POBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,POBASICID from POBASIC  where POBASIC.POBASICID='" + id + "'";
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
        public DataTable GetPOItembyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,POBASICID,PODETAIL.ITEMID as item,PODETAILID from PODETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID where PODETAIL.POBASICID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYNAME,GRNBLBASIC.PARTYID,GRNBLBASICID from GRNBLBASIC LEFT OUTER JOIN  PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID where GRNBLBASIC.GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYNAME,POBASICID,POBASIC.PARTYID from POBASIC LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID where POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select TESTDESC,ACVAL,TESTVALUE,RESULT,MANUALVALUE,ACTTESTVALUE from QCVALUEDETAIL Where QCVALUEBASICID='" + id + "'";
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
                    svSQL = "UPDATE QCVALUEBASIC SET STATUS ='ISACTIVE' WHERE QCVALUEBASICID='" + id + "'";
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

        public DataTable GetPoQcTesting(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select POBASICID ,DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,POBASIC.PARTYID as par FROM POBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=POBASIC.PARTYID WHERE POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGetPoQcTestingDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,PODETAIL.ITEMID as item,QTY from PODETAIL  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID  WHERE POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPOItemDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TESTDESC,UNITMAST.UNITID,VALUEORMANUAL,STARTVALUE,ENDVALUE,TESTTDETAILID from TESTTDETAIL left outer join UNITMAST ON UNITMASTID =TESTTDETAIL.UNIT   WHERE TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetViewQCTesting(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMID,GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYID,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTBY,QCVALUEBASIC.REMARKS ,QCVALUEBASIC.GRNPROD ,QCVALUEBASIC.TESTPROCEDURE from QCVALUEBASIC Where QCVALUEBASIC.QCVALUEBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetViewQCDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select TESTDESC,ACVAL,TESTVALUE,RESULT,MANUALVALUE,ACTTESTVALUE from QCVALUEDETAIL Where QCVALUEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGrnDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT GRNBLBASICID,DOCID, to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYNAME,PARTYID FROM GRNBLBASIC Where GRNBLBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGrnItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,GRNBLDETAIL.ITEMID as item,QTY,GRNBLDETAILID from GRNBLDETAIL  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=GRNBLDETAIL.ITEMID   WHERE GRNBLBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TESTDESC,UNITMAST.UNITID,VALUEORMANUAL,STARTVALUE,ENDVALUE,TESTTDETAILID from TESTTDETAIL left outer join UNITMAST ON UNITMASTID =TESTTDETAIL.UNIT   WHERE TESTTBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
