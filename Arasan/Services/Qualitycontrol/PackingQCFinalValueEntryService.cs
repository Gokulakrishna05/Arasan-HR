using Arasan.Interface.Qualitycontrol;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;

namespace Arasan.Services.Qualitycontrol
{
    public class PackingQCFinalValueEntryService : IPackingQCFinalValueEntry
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PackingQCFinalValueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<PackingQCFinalValueEntry> GetAllPackingQCFinalValueEntry(string active)
        {
            if (string.IsNullOrEmpty(active))
            {
                active = "YES";
            }
            List<PackingQCFinalValueEntry> cmpList = new List<PackingQCFinalValueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = " Select PAKQBASICID,DOCID,to_char(PAKQBASIC.DOCDT,'dd-MON-yyyy')DOCDT,PENTRYID,TESTREQ,PAKQBASIC.ACTIVE from PAKQBASIC  where PAKQBASIC.ACTIVE='" + active + "'  order by PAKQBASIC.PAKQBASICID DESC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PackingQCFinalValueEntry cmp = new PackingQCFinalValueEntry
                        {
                            ID = rdr["PAKQBASICID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDT"].ToString(),
                            PEntryid = rdr["PENTRYID"].ToString(),

                            TestReq = rdr["TESTREQ"].ToString(),
                            active = rdr["ACTIVE"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public string PackingQCFinalValueEntryCRUD(PackingQCFinalValueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                if (cy.ID != null)
                {
                    cy.ID = null;
                }



                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PQC-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "PQC-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PQC-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //string ITEMID = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cy.Itemid + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PAKQBASICPROC", objConn);
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docid;
                    objCmd.Parameters.Add("DOCDT", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("PENTRYID", OracleDbType.NVarchar2).Value = cy.PEntryid;
                    objCmd.Parameters.Add("PENTRYDT", OracleDbType.Date).Value = DateTime.Parse(cy.PEntrydt);
                    objCmd.Parameters.Add("PACKNOTEID", OracleDbType.NVarchar2).Value = cy.PNoteid;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.Schedule;
                    objCmd.Parameters.Add("PACLOTNO", OracleDbType.NVarchar2).Value = cy.PacNo;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("TESTREQ", OracleDbType.NVarchar2).Value = cy.TestReq;
                    objCmd.Parameters.Add("PKDRUMNOS", OracleDbType.NVarchar2).Value = cy.drumnos;
                    objCmd.Parameters.Add("SAMPLETAKENBY", OracleDbType.NVarchar2).Value = cy.Same;
                    objCmd.Parameters.Add("CHECKEDBY", OracleDbType.NVarchar2).Value = cy.Checked;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;

                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "YES";
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

                        
                        foreach (Packingitem ca in cy.DrumLst)
                        {
                            if (ca.Isvalid == "Y" && ca.Drum != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PAKQDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PAKQBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.Drum;
                                    objCmds.Parameters.Add("COMBNO", OracleDbType.NVarchar2).Value = ca.Com;
                                    objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.Batch;
                                    objCmds.Parameters.Add("FINALRESULT", OracleDbType.NVarchar2).Value = ca.Result;
                                    
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }
                            }
                            //}
                        }
                        foreach (PackingGasitem cp in cy.TimeLst)
                        {
                            if (cp.Isvalid == "Y" && cp.Time != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PAKQGEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PAKQBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("MINS", OracleDbType.NVarchar2).Value = cp.Time;
                                    objCmds.Parameters.Add("VOL25C", OracleDbType.NVarchar2).Value = cp.vol25;
                                    objCmds.Parameters.Add("VOL35C", OracleDbType.NVarchar2).Value = cp.vol35;
                                    objCmds.Parameters.Add("VOL45C", OracleDbType.NVarchar2).Value = cp.vol45;
                                    objCmds.Parameters.Add("VOLSTP", OracleDbType.NVarchar2).Value = cp.vol;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }
                        }
                        //updateCMd = " UPDATE QCNOTIFICATION SET IS_COMPLETED ='YES' , FINALRESULT='" + cy.FResult + "' WHERE DOCID ='" + cy.ProNo + "' ";
                        //datatrans.UpdateStatus(updateCMd);

                        // }
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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PAKQBASIC SET ACTIVE ='NO' WHERE PAKQBASICID='" + id + "'";
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

        public DataTable GetViewPacking(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,DOCDT,PENTRYID,PENTRYDT,PACKNOTEID,PSCHNO,PACLOTNO,ITEMID,TESTREQ,PKDRUMNOS,SAMPLETAKENBY,CHECKEDBY,REMARKS from PAKQBASIC where PAKQBASIC.PAKQBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetViewPackingItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DRUMNO,COMBNO,BATCHNO,FINALRESULT FROM PAKQDETAIL where PAKQDETAIL.PAKQBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetViewPackingGas(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT MINS,VOL25C,VOL35C,VOL45C,VOLSTP FROM PAKQGEDETAIL where PAKQGEDETAIL.PAKQBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
