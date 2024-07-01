using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using System;
using Arasan.Interface.Master;

namespace Arasan.Services.Master
{
    public class RateService : IRateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RATEBASIC SET IS_ACTIVE ='N' WHERE RATEBASICID='" + id + "'";
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
        public string RateCodeCRUD(string Ratecode,string RateDsc)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RATECODEPROC", objConn);
                
                    objCmd.CommandType = CommandType.StoredProcedure;
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                    objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = Ratecode;
                    objCmd.Parameters.Add("RATEDESC", OracleDbType.NVarchar2).Value = RateDsc;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
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
        public DataTable GetRateCode()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATECODEMASTID,RATECODE FROM RATECODEMAST";
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
        public DataTable GetAllListRateItem(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT RATEBASICID,DOCID,to_char(RATEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RATECODE,RATENAME,UF,RATETYPE FROM RATEBASIC WHERE RATEBASIC.IS_ACTIVE='Y' ORDER BY RATEBASIC.RATEBASICID DESC";
            }
            else
            {
                SvSql = "SELECT RATEBASICID,DOCID,to_char(RATEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RATECODE,RATENAME,UF,RATETYPE FROM RATEBASIC WHERE RATEBASIC.IS_ACTIVE='N' ORDER BY RATEBASIC.RATEBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string RateCRUD(Rate cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'RM-F' AND ACTIVESEQUENCE = 'T'  ");
                string DocId = string.Format("{0}{1}", "RM-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='RM-F' AND ACTIVESEQUENCE ='T'  ";
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
                    OracleCommand objCmd = new OracleCommand("RATEBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STORESACCBASICPROC";*/

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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = cy.Ratecode;
                    objCmd.Parameters.Add("RATENAME", OracleDbType.NVarchar2).Value = cy.RateName;
                    objCmd.Parameters.Add("VALIDFROM", OracleDbType.NVarchar2).Value = cy.ValidFrom;
                    objCmd.Parameters.Add("VALIDTO", OracleDbType.NVarchar2).Value = cy.ValidTo;
                    objCmd.Parameters.Add("UF", OracleDbType.NVarchar2).Value = cy.UF;
                    objCmd.Parameters.Add("RATETYPE", OracleDbType.NVarchar2).Value = cy.RateType;
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
                        foreach (RateItem cp in cy.RATElist)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RATEDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("RATEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("RCODE", OracleDbType.NVarchar2).Value = cp.RCode;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("VFROM", OracleDbType.NVarchar2).Value = cp.Validfrom;
                                    objCmds.Parameters.Add("VTO", OracleDbType.NVarchar2).Value = cp.Validto;
                                    objCmds.Parameters.Add("RTYPE", OracleDbType.NVarchar2).Value = cp.Type;
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

        public DataTable GetEditRate(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATEBASICID,DOCID,to_char(RATEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RATECODE,RATENAME,to_char(RATEBASIC.VALIDFROM,'dd-MON-yyyy')VALIDFROM,to_char(RATEBASIC.VALIDTO,'dd-MON-yyyy')VALIDTO,UF,RATETYPE FROM RATEBASIC Where RATEBASIC.RATEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditRateDeatil(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATEBASICID,RCODE,ITEMID,UNIT,RATE,to_char(RATEDETAIL.VFROM,'dd-MON-yyyy')VFROM,to_char(RATEDETAIL.VTO,'dd-MON-yyyy')VTO,RTYPE FROM RATEDETAIL Where RATEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditRateDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATEBASICID,RCODE,ITEMMASTER.ITEMID,UNIT,RATE,to_char(RATEDETAIL.VFROM,'dd-MON-yyyy')VFROM,to_char(RATEDETAIL.VTO,'dd-MON-yyyy')VTO,RTYPE FROM RATEDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RATEDETAIL.ITEMID Where RATEBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
