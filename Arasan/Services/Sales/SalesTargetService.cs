using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;

namespace Arasan.Services.Sales
{
    public class SalesTargetService : ISalesTargetService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesTargetService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTERID,ITEMID FROM ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,VALMETHDES,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG,BINBASIC.BINID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID LEFT OUTER JOIN BINBASIC ON BINBASICID=ITEMMASTER.BINNO Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesTarget(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SALFCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SALFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(SALFCBASIC.FDAY,'dd-MON-yyyy')FDAY,to_char(SALFCBASIC.EDAY,'dd-MON-yyyy')EDAY,MON,FINYR FROM SALFCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SALFCBASIC.BRANCHID Where SALFCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesTargetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,PARTYMAST.PARTYNAME,QTY,RATE,SAMOUNT FROM SALFCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SALFCDETAIL.ITEMID LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SALFCDETAIL.PARTYID Where SALFCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListSalesTargetItem(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SALFCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SALFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,MON FROM SALFCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SALFCBASIC.BRANCHID WHERE SALFCBASIC.IS_ACTIVE='Y' ORDER BY SALFCBASIC.SALFCBASICID DESC";
            }
            else
            {
                SvSql = "SELECT SALFCBASICID,BRANCHMAST.BRANCHID,DOCID,to_char(SALFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,MON FROM SALFCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=SALFCBASIC.BRANCHID WHERE SALFCBASIC.IS_ACTIVE='N' ORDER BY SALFCBASIC.SALFCBASICID DESC";
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
                    svSQL = "UPDATE SALFCBASIC SET IS_ACTIVE ='N' WHERE SALFCBASICID='" + id + "'";
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
        public string SalesTargetCRUD(SalesTarget cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Tfr-' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "Tfr-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Tfr-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = DocId;
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SALFCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "ADDBASICPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("FDAY", OracleDbType.NVarchar2).Value = cy.FDay;
                    objCmd.Parameters.Add("EDAY", OracleDbType.NVarchar2).Value = cy.TDay;
                    objCmd.Parameters.Add("MON", OracleDbType.NVarchar2).Value = cy.FMonth;
                    objCmd.Parameters.Add("FINYR", OracleDbType.NVarchar2).Value = cy.FYear;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //objConn.Close();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        foreach (SalesTargetItem cp in cy.Targetlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {

                                OracleCommand objCmds = new OracleCommand("SALFCDETAILPROC", objConn);
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
                                objCmds.Parameters.Add("SALFCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cp.PartyId;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmds.Parameters.Add("SAMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();

                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
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
    }
}
