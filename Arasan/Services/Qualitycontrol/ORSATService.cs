using Arasan.Interface;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Qualitycontrol
{
    public class ORSATService : IORSAT
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ORSATService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        //public string ORSATCRUD(ORSAT cy)
        //{
        //    string msg = "";
        //    try
        //    {
        //        string StatementType = string.Empty; string svSQL = "";
        //        datatrans = new DataTransactions(_connectionString);


        //        if (cy.ID != null)
        //        {
        //            cy.ID = null;
        //        }



        //        int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'OSA-' AND ACTIVESEQUENCE = 'T'  ");
        //        string DocId = string.Format("{0}{1}", "OSA-", (idc + 1).ToString());

        //        string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='OSA-' AND ACTIVESEQUENCE ='T'  ";
        //        try
        //        {
        //            datatrans.UpdateStatus(updateCMd);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        //string ITEMID = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cy.Itemid + "' ");
        //        using (OracleConnection objConn = new OracleConnection(_connectionString))
        //        {
        //            OracleCommand objCmd = new OracleCommand("ORSATBASICPROC", objConn);
        //            objCmd.CommandType = CommandType.StoredProcedure;
        //            if (cy.ID == null)
        //            {
        //                StatementType = "Insert";
        //                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                StatementType = "Update";
        //                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
        //            }
        //            objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
        //            objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = DocId;
        //            objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
        //            objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
        //            objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
        //            objCmd.Parameters.Add("CDRUMNO", OracleDbType.NVarchar2).Value = cy.DrumNo;
        //            objCmd.Parameters.Add("BATCH", OracleDbType.NVarchar2).Value = cy.Batch;
        //            objCmd.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = cy.BatchNo;
        //            objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ITEMID;
        //            objCmd.Parameters.Add("PRODID", OracleDbType.NVarchar2).Value = cy.ProNo;
        //            objCmd.Parameters.Add("RATEPHR", OracleDbType.NVarchar2).Value = cy.Rate;
        //            objCmd.Parameters.Add("PRODDATE", OracleDbType.NVarchar2).Value = cy.ProDate;
        //            objCmd.Parameters.Add("SAMPLENO", OracleDbType.NVarchar2).Value = cy.SampleNo;
        //            objCmd.Parameters.Add("NOZZLENO", OracleDbType.NVarchar2).Value = cy.NozzleNo;
        //            objCmd.Parameters.Add("AIRPRESS", OracleDbType.NVarchar2).Value = cy.AirPress;
        //            objCmd.Parameters.Add("ADDCH", OracleDbType.NVarchar2).Value = cy.Additive;
        //            objCmd.Parameters.Add("STIME", OracleDbType.NVarchar2).Value = cy.Stime;
        //            objCmd.Parameters.Add("BCT", OracleDbType.NVarchar2).Value = cy.CTemp;
        //            objCmd.Parameters.Add("FINALRESULT", OracleDbType.NVarchar2).Value = cy.FResult;
        //            objCmd.Parameters.Add("RESULTTYPE", OracleDbType.NVarchar2).Value = cy.RType;
        //            objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
        //            objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Reamarks;
        //            objCmd.Parameters.Add("APPROID", OracleDbType.NVarchar2).Value = cy.ApId;
        //            //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
        //            objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
        //            objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
        //            try
        //            {
        //                objConn.Open();
        //                objCmd.ExecuteNonQuery();
        //                Object Pid = objCmd.Parameters["OUTID"].Value;
        //                //string Pid = "0";
        //                if (cy.ID != null)
        //                {
        //                    Pid = cy.ID;
        //                }


        //                foreach (QCFinalValueEntryItem ca in cy.QCFlst)
        //                {
        //                    if (ca.Isvalid == "Y" && ca.des != "0")
        //                    {
        //                        using (OracleConnection objConns = new OracleConnection(_connectionString))
        //                        {
        //                            OracleCommand objCmds = new OracleCommand("FQTVEDETAILPROC", objConns);
        //                            if (cy.ID == null)
        //                            {
        //                                StatementType = "Insert";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
        //                            }
        //                            else
        //                            {
        //                                StatementType = "Update";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
        //                            }
        //                            objCmds.CommandType = CommandType.StoredProcedure;
        //                            objCmds.Parameters.Add("FQTVEBASICID", OracleDbType.NVarchar2).Value = Pid;
        //                            objCmds.Parameters.Add("TDESC", OracleDbType.NVarchar2).Value = ca.des;
        //                            objCmds.Parameters.Add("VALUEORMANUAL", OracleDbType.NVarchar2).Value = ca.value;
        //                            objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = ca.unit;
        //                            objCmds.Parameters.Add("STARTVALUE", OracleDbType.NVarchar2).Value = ca.sta;
        //                            objCmds.Parameters.Add("ENDVALUE", OracleDbType.NVarchar2).Value = ca.en;
        //                            objCmds.Parameters.Add("TESTVALUE", OracleDbType.NVarchar2).Value = ca.test;
        //                            objCmds.Parameters.Add("MANUALVALUE", OracleDbType.NVarchar2).Value = ca.manual;
        //                            objCmds.Parameters.Add("ACTTESTVALUE", OracleDbType.NVarchar2).Value = ca.actual;
        //                            objCmds.Parameters.Add("TESTRESULT", OracleDbType.NVarchar2).Value = ca.result;
        //                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
        //                            objConns.Open();
        //                            objCmds.ExecuteNonQuery();
        //                            objConns.Close();
        //                        }
        //                    }

        //                }
        //                foreach (QCFVItemDeatils cp in cy.QCFVDLst)
        //                {
        //                    if (cp.Isvalid == "Y" && cp.Vol != "0")
        //                    {
        //                        using (OracleConnection objConns = new OracleConnection(_connectionString))
        //                        {
        //                            OracleCommand objCmds = new OracleCommand("FQTVEGEDETAILPROC", objConns);
        //                            if (cy.ID == null)
        //                            {
        //                                StatementType = "Insert";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
        //                            }
        //                            else
        //                            {
        //                                StatementType = "Update";
        //                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
        //                            }
        //                            objCmds.CommandType = CommandType.StoredProcedure;
        //                            objCmds.Parameters.Add("FQTVEBASICID", OracleDbType.NVarchar2).Value = Pid;
        //                            objCmds.Parameters.Add("MINS", OracleDbType.NVarchar2).Value = cp.Time;
        //                            objCmds.Parameters.Add("VOL25C", OracleDbType.NVarchar2).Value = cp.Vol;
        //                            objCmds.Parameters.Add("VOL35C", OracleDbType.NVarchar2).Value = cp.Volat;
        //                            objCmds.Parameters.Add("VOL45C", OracleDbType.NVarchar2).Value = cp.Volc;
        //                            objCmds.Parameters.Add("VOLSTP", OracleDbType.NVarchar2).Value = cp.Stp;
        //                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
        //                            objConns.Open();
        //                            objCmds.ExecuteNonQuery();
        //                            objConns.Close();
        //                        }

        //                    }
        //                }
        //                updateCMd = " UPDATE QCNOTIFICATION SET IS_COMPLETED ='YES' , FINALRESULT='" + cy.FResult + "' WHERE DOCID ='" + cy.ProNo + "' ";
        //                datatrans.UpdateStatus(updateCMd);


        //            }
        //            catch (Exception ex)
        //            {
        //                //System.Console.WriteLine("Exception: {0}", ex.ToString());
        //            }
        //            objConn.Close();
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error Occurs, While inserting / updating Data";
        //        throw ex;
        //    }

        //    return msg;
        //}
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,BRANCHMASTID from BRANCHMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }

    
}
