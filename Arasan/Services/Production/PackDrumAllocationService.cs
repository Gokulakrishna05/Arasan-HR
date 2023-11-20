using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services 
{
    public class PackDrumAllocationService :IPackDrumAllocation
    {
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;

        public PackDrumAllocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetLoc()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCID,LOCDETAILSID from LOCDETAILS where LOCID IN ('APS PACKING','DG PASTE PACKING','PACKING','PASTE PACKING','POLISH PACKING','PYRO PACKING') ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public DataTable GetDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select SPREFIX,STARTNO,LASTNO,PDABASICID,TOTDRUMS from PDABASIC where PACKLOCID ='" + id+"' ORDER BY LASTNO DESC fetch  first rows only ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackDrum(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,PDABASIC.DOCID,to_char(PDABASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LASTNO,STARTNO,ENTEREDBY,SPREFIX,TOTDRUMS from PDABASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PDABASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=PDABASIC.PACKLOCID where PDABASICID ='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditDrumDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TSTATUS,DRUMNO from PDADETAIL where PDABASICID ='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PackDrumCRUD(PackDrumAllocation cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PDA#' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "PDA#", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PDA#' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.Docid = docid;
                string user = datatrans.GetDataString("Select USERID from EMPMAST where EMPMASTID='" + cy.Enter + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PACKDRUMPROC", objConn);
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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = "0";
                    objCmd.Parameters.Add("PACKLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    
                    objCmd.Parameters.Add("STOCK", OracleDbType.NVarchar2).Value = "0";
                    objCmd.Parameters.Add("STARTNO", OracleDbType.NVarchar2).Value = cy.Start;
                    objCmd.Parameters.Add("LASTNO", OracleDbType.NVarchar2).Value = cy.End;
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = user;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = user;
                    objCmd.Parameters.Add("SPREFIX", OracleDbType.NVarchar2).Value = cy.Pri;
                    objCmd.Parameters.Add("TOTDRUMS", OracleDbType.NVarchar2).Value = cy.Totdrum;                    
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
                        foreach (DrumCreate ca in cy.drumlst)
                        {
                            if ( ca.packdrum != "0")
                            {
                               
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PACKDRUMDETPROC", objConns);
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

                                    objCmds.Parameters.Add("PDABASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.packdrum;
                                    objCmds.Parameters.Add("TDRUMNO", OracleDbType.NVarchar2).Value = ca.totaldrum;
                                    objCmds.Parameters.Add("TSTATUS", OracleDbType.NVarchar2).Value = ca.packyn;
                                    objCmds.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "1";
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

        public DataTable GetAllPackDerum(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,PDABASICID,BRANCH,ITEMID,LOCDETAILS.LOCID,PACKLOCID,STOCK ,STARTNO,LASTNO,USERID,ENTEREDBY,SPREFIX,TOTDRUMS from PDABASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=PDABASIC.PACKLOCID WHERE IS_ACTIVE='Y' ORDER BY  PDABASICID DESC";
            }
            else
            {
                SvSql = "select DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,PDABASICID,BRANCH,ITEMID,LOCDETAILS.LOCID,PACKLOCID,STOCK ,STARTNO,LASTNO,USERID,ENTEREDBY,SPREFIX,TOTDRUMS from PDABASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=PDABASIC.PACKLOCID WHERE IS_ACTIVE='N' ORDER BY  PDABASICID DESC";

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
                    svSQL = "UPDATE PDABASIC SET IS_ACTIVE ='N' WHERE PDABASICID='" + id + "'";
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
