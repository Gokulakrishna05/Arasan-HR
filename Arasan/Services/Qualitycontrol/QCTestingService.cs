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
        public IEnumerable<QCTesting> GetAllQCTesting()
        {
            List<QCTesting> cmpList = new List<QCTesting>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ITEMMASTER.ITEMID,QCVALUEBASIC.GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYRCODE.PARTY,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTEDBY,QCVALUEBASIC.REMARKS from QCVALUEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=QCVALUEBASIC.ITEMID LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID";
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
                            Party = rdr["PARTY"].ToString(),
                            ItemId = rdr["ITEMID"].ToString(),
                            TestResult = rdr["TESTRESULT"].ToString(),
                            TestedBy = rdr["TESTEDBY"].ToString(),
                             Remarks = rdr["REMARKS"].ToString()
                           
                           



                        };
                        cmpList.Add(cmp);
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
                string StatementType = string.Empty; string svSQL = "";

                using (           OracleConnection objConn = new OracleConnection(_connectionString))
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
                    objCmd.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cy.GRNNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.Date).Value = DateTime.Parse(cy.GRNDate);
                    objCmd.Parameters.Add("CLASSCODE", OracleDbType.NVarchar2).Value = cy.ClassCode;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.ItemId;
                    objCmd.Parameters.Add("SLNO", OracleDbType.NVarchar2).Value = cy.SNo;
                    objCmd.Parameters.Add("LOTSERIALNO", OracleDbType.NVarchar2).Value = cy.LotNo;
                    objCmd.Parameters.Add("TESTRESULT", OracleDbType.NVarchar2).Value = cy.TestResult;
                    objCmd.Parameters.Add("TESTEDBY", OracleDbType.NVarchar2).Value = cy.TestedBy;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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
                        foreach (QCItem cp in cy.QCLst)
                        {
                            
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("DPDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("QCVALUEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("TESTDESC", OracleDbType.NVarchar2).Value = cp.TestDec;
                                    objCmds.Parameters.Add("ACVAL", OracleDbType.NVarchar2).Value = cp.AccVale;
                                    objCmds.Parameters.Add("TESTVALUE", OracleDbType.NVarchar2).Value = cp.TestValue;
                                    objCmds.Parameters.Add("RESULT", OracleDbType.NVarchar2).Value = cp.Result;
                                    objCmds.Parameters.Add("MANUALVALUE", OracleDbType.NVarchar2).Value = cp.ManualValue;
                                    objCmds.Parameters.Add("ACTTESTVALUE", OracleDbType.NVarchar2).Value = cp.AcTestValue;
                                
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
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
        public DataTable GetQCTesting(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,QCVALUEBASIC.GRNNO,QCVALUEBASIC.DOCID,to_char(QCVALUEBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,to_char(QCVALUEBASIC.GRNDATE,'dd-MON-yyyy')GRNDATE,QCVALUEBASIC.CLASSCODE,PARTYRCODE.PARTY,QCVALUEBASICID,QCVALUEBASIC.LOTSERIALNO,SLNO,QCVALUEBASIC.TESTRESULT,QCVALUEBASIC.TESTEDBY,QCVALUEBASIC.REMARKS from QCVALUEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=QCVALUEBASIC.ITEMID LEFT OUTER JOIN  PARTYMAST on QCVALUEBASIC.PARTYID=PARTYMAST.PARTYMASTID  LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND QCVALUEBASIC.QCVALUEBASICID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYRCODE.ID,to_char(GRNDATE,'dd-MON-yyyy')GRNDATE,GRNBLBASICID from GRNBLBASIC LEFT OUTER JOIN  PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID where GRNBLBASIC.GRNBLBASICID='" + id +"'";
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
        //public DataTable GetGRN(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select DOCID,GRNBLBASICID from GRNBLBASIC where='"+id+ "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
    }
}
