using Arasan.Interface;
using Arasan.Models;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Arasan.Services
{
    public class BatchCreationService : IBatchCreation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BatchCreationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<BatchCreation> GetAllBatchCreation(string st, string ed)
        {
            List<BatchCreation> cmpList = new List<BatchCreation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                    con.Open();
                    cmd.CommandText = " Select   BRANCHMAST.BRANCHID,DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,BCPRODBASICID,BCPRODBASIC.STATUS from BCPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BCPRODBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID  WHERE BCPRODBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'  order by BCPRODBASICID desc   ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BatchCreation cmp = new BatchCreation
                        {

                            ID = rdr["BCPRODBASICID"].ToString(),

                            Branch = rdr["BRANCHID"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),

                            DocDate = rdr["DOCDATE"].ToString(),
                          

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
                        cmd.CommandText = " Select   BRANCHMAST.BRANCHID,DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PROCESSMAST.PROCESSID,WCBASIC.WCID,BCPRODBASICID,BCPRODBASIC.STATUS from BCPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BCPRODBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID  WHERE BCPRODBASIC.DOCDATE > sysdate-30   order by BCPRODBASICID desc   ";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            BatchCreation cmp = new BatchCreation
                            {
                                ID = rdr["BCPRODBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                WorkCenter = rdr["WCID"].ToString(),
                                Process = rdr["PROCESSID"].ToString(),
                                DocDate = rdr["DOCDATE"].ToString(),
                            };
                            cmpList.Add(cmp);
                        }
                    }
                }

            }
                return cmpList;
        }
        public string BatchCRUD(BatchCreation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("BATCHCREATIONPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "BATCHCREATIONPROC";*/

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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenterid;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.BatchNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("WPROCESSID", OracleDbType.NVarchar2).Value = cy.Processid;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
                    objCmd.Parameters.Add("SEQYN", OracleDbType.NVarchar2).Value = cy.Seq;
                    objCmd.Parameters.Add("IORATIOFROM", OracleDbType.NVarchar2).Value = cy.IOFrom;
                    objCmd.Parameters.Add("IORATIOTO", OracleDbType.NVarchar2).Value = cy.IOTo;
                    objCmd.Parameters.Add("MTONO", OracleDbType.NVarchar2).Value = cy.MTO;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.Prod;
                    objCmd.Parameters.Add("PTYPE", OracleDbType.NVarchar2).Value = cy.Shall;
                    objCmd.Parameters.Add("BTYPE", OracleDbType.NVarchar2).Value = cy.Leaf;
                    objCmd.Parameters.Add("REFDOCID", OracleDbType.NVarchar2).Value = cy.RefBatch;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narr;
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
                        //if (cy.BatchLst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (BatchItem cp in cy.BatchLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.WorkId != "0")
                        //            {

                        //                svSQL = "Insert into BCPRODDETAIL (BCPRODBASICID,BWCID,PROCESSID,PSEQ,INSREQ) VALUES ('" + Pid + "','" + cp.WorkId + "','" + cp.ProcessId + "','" + cp.Seq + "','" + cp.Req + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();
                        //            }
                        //        }

                        //    }
                        //    else
                        //    {
                        //        svSQL = "Delete BCPRODDETAIL WHERE BCPRODBASICID='" + cy.ID + "'";
                        //        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        //        objCmdd.ExecuteNonQuery();
                        //        foreach (BatchItem cp in cy.BatchLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.WorkId != "0")
                        //            {
                        //                svSQL = "Insert into BCPRODDETAIL (BCPRODBASICID,BWCID,PROCESSID,PSEQ,INSREQ) VALUES ('" + Pid + "','" + cp.WorkId + "','" + cp.ProcessId + "','" + cp.Seq + "','" + cp.Req + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }
                        //        }
                        //    }
                        //}
                        if (cy.BatchInLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (BatchInItem cp in cy.BatchInLst)
                                {
                                    if ( cp.saveitemid != "0")
                                    {
                                        svSQL = "Insert into BCINPUTDETAIL (BCPRODBASICID,IPROCESSID,IITEMID,IUNIT,IQTY) VALUES ('" + Pid + "','" + cy.Processid + "','" + cp.saveitemid + "','" + cp.unit + "','" + cp.qty + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete BCINPUTDETAIL WHERE BCPRODBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (BatchInItem cp in cy.BatchInLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveitemid != "0")
                                    {
                                        svSQL = "Insert into BCINPUTDETAIL (BCPRODBASICID,IPROCESSID,IITEMID,IUNIT,IQTY) VALUES ('" + Pid + "','" + cp.Process + "','" + cp.saveitemid + "','" + cp.unit + "','" + cp.qty + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                        }

                        if (cy.BatchOutLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (BatchOutItem cp in cy.BatchOutLst)
                                {
                                    if ( cp.saveitemid != "0")
                                    {

                                        svSQL = "Insert into BCOUTPUTDETAIL (BCPRODBASICID,OPROCESSID,OITEMID,OUNIT,OQTY,OTYPE,GPER,VMPER,OWPER) VALUES ('" + Pid + "','" + cy.Processid + "','" + cp.saveitemid + "','" + cp.ounit + "','" + cp.oqty + "','" + cp.outtype + "','" + cp.Vmper + "','" + cp.Greas + "','" + cp.Waste + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete BCOUTPUTDETAIL WHERE BCPRODBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (BatchOutItem cp in cy.BatchOutLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.saveitemid != "0")
                                    {

                                        svSQL = "Insert into BCOUTPUTDETAIL (BCPRODBASICID,OPROCESSID,OITEMID,OUNIT,OQTY,OTYPE,GPER,VMPER,OWPER) VALUES ('" + Pid + "','" + cp.OProcess + "','" + cp.saveitemid + "','" + cp.ounit + "','" + cp.oqty + "','" + cp.outtype + "','" + cp.Vmper + "','" + cp.Greas + "','" + cp.Waste + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        //if (cy.BatchOtherLst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (BatchOtherItem cp in cy.BatchOtherLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.OtProcessId != "0")
                        //            {
                        //                svSQL = "Insert into BCOTHERDETAIL (BCPRODBASICID,EPROCESSID,ESDT,EEDT,EST,EET,EPSEQ,ETOTHRS,EBRHRS,ERUNHRS,ENARR) VALUES ('" + Pid + "','" + cp.OtProcessId + "','" + cp.Start + "','" + cp.End + "','" + cp.StartT + "','" + cp.EndT + "','" + cp.Seqe + "','" + cp.Total + "','" + cp.Break + "','" + cp.RunHrs + "','" + cp.Remark + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();


                        //            }
                        //        }


                        //    }
                        //    else
                        //    {
                        //        svSQL = "Delete BCOTHERDETAIL WHERE BCPRODBASICID='" + cy.ID + "'";
                        //        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        //        objCmdd.ExecuteNonQuery();
                        //        foreach (BatchOtherItem cp in cy.BatchOtherLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.OtProcessId != "0")
                        //            {
                        //                svSQL = "Insert into BCOTHERDETAIL (BCPRODBASICID,EPROCESSID,ESDT,EEDT,EST,EET,EPSEQ,ETOTHRS,EBRHRS,ERUNHRS,ENARR) VALUES ('" + Pid + "','" + cp.OtProcessId + "','" + cp.Start + "','" + cp.End + "','" + cp.StartT + "','" + cp.EndT + "','" + cp.Seqe + "','" + cp.Total + "','" + cp.Break + "','" + cp.RunHrs + "','" + cp.Remark + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();


                        //            }
                        //        }
                        //    }
                        //}

                        //if (cy.BatchParemLst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (BatchParemItem cp in cy.BatchParemLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.Param != "0")
                        //            {
                        //                svSQL = "Insert into BCPARAMDETAIL (BCPRODBASICID,PROCPARAM,PSDT,PEDT,PSTIME,PETIME,PARAMUNIT,PARAMVALUE) VALUES ('" + Pid + "','" + cp.Param + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.PUnit + "','" + cp.Value + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }

                        //        }
                        //    }
                        //    else
                        //    {

                        //        svSQL = "Delete BCPARAMDETAIL WHERE BCPRODBASICID='" + cy.ID + "'";
                        //        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        //        objCmdd.ExecuteNonQuery();
                        //        foreach (BatchParemItem cp in cy.BatchParemLst)
                        //        {
                        //            if (cp.Isvalid == "Y" && cp.Param != "0")
                        //            {
                        //                svSQL = "Insert into BCPARAMDETAIL (BCPRODBASICID,PROCPARAM,PSDT,PEDT,PSTIME,PETIME,PARAMUNIT,PARAMVALUE) VALUES ('" + Pid + "','" + cp.Param + "','" + cp.StartDate + "','" + cp.EndDate + "','" + cp.StartTime + "','" + cp.EndTime + "','" + cp.PUnit + "','" + cp.Value + "')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }

                        //        }
                        //    }
                        //}
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
        public  DataTable GetBatchCreationByName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,PSBASIC.DOCID as scno,BCPRODBASIC.DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFDOCID,BCPRODBASIC.ENTEREDBY,BCPRODBASIC.NARR,SEQYN,PTYPE,BTYPE,IORATIOFROM,IORATIOTO,MTONO,PROCESSMAST.PROCESSID,WCBASIC.WCID,BCPRODBASICID from BCPRODBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=BCPRODBASIC.BRANCHID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=BCPRODBASIC.PSCHNO Where  BCPRODBASIC.BCPRODBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcess()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID ,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProd()
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,PSBASICID from PSBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcessid()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID,PROCESSMASTID from PROCESSMAST ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,WCID,DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WPROCESSID,ENTEREDBY,SEQYN,IORATIOFROM,IORATIOTO,MTONO,PSCHNO,PTYPE,BTYPE,REFDOCID,NARR,BCPRODBASICID from BCPRODBASIC Where BCPRODBASICID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreationDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,BWCID,PROCESSID,PSEQ,INSREQ from BCPRODDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreationInputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,IPROCESSID,IITEMID,IUNIT,IQTY from BCINPUTDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreationOutputDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,OPROCESSID,OITEMID,OUNIT,OQTY,OTYPE,GPER,VMPER,OWPER from BCOUTPUTDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public List<BatchInItem> GetBatchInItems(string schid)
        {
           List<BatchInItem> lst = new List<BatchInItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT PSBASICID,ITEMMASTER.ITEMID,RITEMID,RITEMDESC,RUNIT,RQTY FROM PSINPDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PSINPDETAIL.RITEMID WHERE PSBASICID='"+schid+"'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BatchInItem cmp = new BatchInItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            unit = rdr["RUNIT"].ToString(),
                            qty = rdr["RQTY"].ToString(),
                            saveitemid = rdr["RITEMID"].ToString(),
                            Isvalid = "Y",

                        };
                        lst.Add(cmp);
                    }
                }
            }
            return lst; 
        }
        public List<BatchOutItem> GetBatchOutItems(string schid)
        {
            List<BatchOutItem> lst = new List<BatchOutItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT PSBASICID,ITEMMASTER.ITEMID,OITEMID,OITEMDESC,OUNIT,OPER,ALPER,OTYPE,SCHQTY,PQTY FROM PSOUTDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PSOUTDETAIL.OITEMID WHERE PSBASICID='" + schid+"'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BatchOutItem cmp = new BatchOutItem
                        {
                            oitem = rdr["ITEMID"].ToString(),
                            ounit = rdr["OUNIT"].ToString(),
                            oqty = rdr["SCHQTY"].ToString(),
                            outtype = rdr["OTYPE"].ToString(),
                            saveitemid = rdr["OITEMID"].ToString(),
                            Isvalid = "Y",
                        };
                        lst.Add(cmp);
                    }
                }
            }
            return lst;
        }
        public DataTable GetBatchCreationOtherDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,EPROCESSID,to_char(BCOTHERDETAIL.ESDT,'dd-MON-yyyy')ESDT,to_char(BCOTHERDETAIL.EEDT,'dd-MON-yyyy')EEDT,EST,EET,EPSEQ,ETOTHRS,EBRHRS,ERUNHRS,ENARR from BCOTHERDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatchCreationParmDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,PROCPARAM,to_char(BCPARAMDETAIL.PSDT,'dd-MON-yyyy')PSDT,to_char(BCPARAMDETAIL.PEDT,'dd-MON-yyyy')PEDT,PSTIME,PETIME,PARAMUNIT,PARAMVALUE from BCPARAMDETAIL where BCPRODBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BatchDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,WCBASIC.WCID,PROCESSMAST.PROCESSID,PSEQ,INSREQ from BCPRODDETAIL LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODDETAIL.PROCESSID LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODDETAIL.BWCID where BCPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BatchInDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,PROCESSMAST.PROCESSID,ITEMMASTER.ITEMID,IUNIT,IQTY from BCINPUTDETAIL LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCINPUTDETAIL.IPROCESSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=BCINPUTDETAIL.IITEMID where BCPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BatchOutDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,PROCESSMAST.PROCESSID,ITEMMASTER.ITEMID,OUNIT,OQTY,OTYPE,GPER,VMPER,OWPER from BCOUTPUTDETAIL LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCOUTPUTDETAIL.OPROCESSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=BCOUTPUTDETAIL.OITEMID where BCPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BatchOtherDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,PROCESSMAST.PROCESSID,to_char(BCOTHERDETAIL.ESDT,'dd-MON-yyyy')ESDT,to_char(BCOTHERDETAIL.EEDT,'dd-MON-yyyy')EEDT,EST,EET,EPSEQ,ETOTHRS,EBRHRS,ERUNHRS,ENARR from BCOTHERDETAIL LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCOTHERDETAIL.EPROCESSID where BCPRODBASICID='" + name + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BatchParemItemDetail(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,PROCPARAM,to_char(BCPARAMDETAIL.PSDT,'dd-MON-yyyy')PSDT,to_char(BCPARAMDETAIL.PEDT,'dd-MON-yyyy')PEDT,PSTIME,PETIME,PARAMUNIT,PARAMVALUE from BCPARAMDETAIL where BCPRODBASICID='" + name + "' ";
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
                    svSQL = "UPDATE BCPRODBASIC SET STATUS ='ISACTIVE' WHERE BCPRODBASICID='" + id + "'";
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
        public DataTable GetAllBatch(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select WCBASIC.WCID,BCPRODBASICID,BCPRODBASIC.DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,PROCESSMAST.PROCESSID,WPROCESSID,SEQYN,IORATIOFROM,IORATIOTO,MTONO,PSBASIC.DOCID as psno,PSCHNO,PTYPE,BTYPE,REFDOCID from BCPRODBASIC  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=BCPRODBASIC.PSCHNO WHERE BCPRODBASIC.IS_ACTIVE='Y' ORDER BY BCPRODBASICID DESC ";
            }
            else
            {
                SvSql = "Select WCBASIC.WCID,BCPRODBASICID,BCPRODBASIC.DOCID,to_char(BCPRODBASIC.DOCDATE,'dd-MM-yy')DOCDATE,PROCESSMAST.PROCESSID,WPROCESSID,SEQYN,IORATIOFROM,IORATIOTO,MTONO,PSBASIC.DOCID as psno,PSCHNO,PTYPE,BTYPE,REFDOCID from BCPRODBASIC  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=BCPRODBASIC.WPROCESSID  LEFT OUTER JOIN WCBASIC ON WCBASICID=BCPRODBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=BCPRODBASIC.PSCHNO WHERE BCPRODBASIC.IS_ACTIVE='N' ORDER BY BCPRODBASICID DESC ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
