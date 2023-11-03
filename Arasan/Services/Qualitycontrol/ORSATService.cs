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


        public IEnumerable<ORSAT> GetAllORSAT(string st, string ed)
        {
            List<ORSAT> cmpList = new List<ORSAT>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
               
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        if (st != null && ed != null)
                        {
                        cmd.CommandText = "SELECT BRANCHMAST.BRANCHID,ORSATBASICID,DOCID,to_char(ORSATBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,SHIFTNO,WCID,to_char(ORSATBASIC.ENTDATE,'dd-MON-yyyy') ENTDATE,ETIME,REMARKS,ACTIVE FROM ORSATBASIC left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID = ORSATBASIC.BRANCH AND ORSATBASIC.ACTIVE = 'YES' WHERE  ORSATBASIC.DOCDATE BETWEEN '" + st + "'  AND ' " + ed + "' order by ORSATBASICID desc";

                    }
                    else
                        {
                        cmd.CommandText = "SELECT BRANCHMAST.BRANCHID,ORSATBASICID,DOCID,to_char(ORSATBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,SHIFTNO,WCID,to_char(ORSATBASIC.ENTDATE,'dd-MON-yyyy') ENTDATE,ETIME,REMARKS,ACTIVE FROM ORSATBASIC left outer join BRANCHMAST on BRANCHMAST.BRANCHMASTID = ORSATBASIC.BRANCH AND ORSATBASIC.ACTIVE = 'YES' WHERE  ORSATBASIC.DOCDATE > sysdate-30 order by ORSATBASICID desc ";

                        }
                        

                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ORSAT cmp = new ORSAT
                            {
                                ID = rdr["ORSATBASICID"].ToString(),
                                //Branch = rdr["BRANCHID"].ToString(),
                                docid = rdr["DOCID"].ToString(),
                                docdate = rdr["DOCDATE"].ToString(),
                                entry = rdr["ENTDATE"].ToString(),
                                time = rdr["ETIME"].ToString(),
                                 active= rdr["ACTIVE"].ToString()

                            };
                            cmpList.Add(cmp);
                        }
                    }
                
            }
            return cmpList;
        }
        public string ORSATCRUD(ORSAT cy)
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



                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'OSA-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "OSA-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='OSA-' AND ACTIVESEQUENCE ='T'  ";
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
                    OracleCommand objCmd = new OracleCommand("ORSATBASICPROC", objConn);
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
                    //objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = cy.docdate;
                    objCmd.Parameters.Add("SHIFTNO", OracleDbType.NVarchar2).Value = cy.shift;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.work;
                    objCmd.Parameters.Add("ENTDATE", OracleDbType.Date).Value = DateTime.Parse(cy.entry);
                    objCmd.Parameters.Add("ETIME", OracleDbType.NVarchar2).Value = cy.time;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.remarks;
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
                        foreach (ORSATdetails cp in cy.ORSATlst)
                        {
                            if (cp.Isvalid == "Y" && cp.para != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("ORSATDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("ORSATBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PARAMETERS", OracleDbType.NVarchar2).Value = cp.para;
                                    objCmds.Parameters.Add("PARAMVAL", OracleDbType.NVarchar2).Value = cp.value;

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
        //public DataTable GetBranch()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select BRANCHID,BRANCHMASTID from BRANCHMAST";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable Getshift()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SHIFTMASTID,SHIFTNO FROM SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getwork()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT WCBASICID,WCID FROM WCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetViewORSAT(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ORSATBASICID,DOCID,to_char(ORSATBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,SHIFTMAST.SHIFTNO,WCID,to_char(ORSATBASIC.ENTDATE,'dd-MON-yyyy') ENTDATE,ETIME,REMARKS,ACTIVE FROM ORSATBASIC LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID = ORSATBASIC.ORSATBASICID WHERE ORSATBASIC.ORSATBASICID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetViewORSATDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARAMETERS,PARAMVAL FROM ORSATDETAIL WHERE ORSATDETAIL.ORSATBASICID ='" + id + "'";
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
                    svSQL = "UPDATE ORSATBASIC SET ACTIVE ='NO' WHERE ORSATBASICID='" + id + "'";
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
