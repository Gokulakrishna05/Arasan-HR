using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class ReceiptAgtRetDCService : IReceiptAgtRetDC
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ReceiptAgtRetDCService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public string ReceiptAgtRetDCCRUD(ReceiptAgtRetDC cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Rec-' AND ACTIVESEQUENCE = 'T'  ");
                string Did = string.Format("{0}{1}", "Rec-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Rec-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string PARTY = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYID='" + cy.Party + "' ");

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RECDCBASICPROC", objConn);
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
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DDate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = cy.Dcno;
                    //objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Dcno;
                    objCmd.Parameters.Add("DCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DcDate);
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = PARTY;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = 'Y';

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
                        foreach (ReceiptAgtRetDCItem cp in cy.ReceiptLst)
                        {
                            if (cp.Isvalid == "Y" && cp.item != "0")
                            {

                                string UNIT = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                //string ITEM = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.itemname + "' ");

                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RECDCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("RECDCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("CITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = cp.bin;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Recd;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.Pend;
                                    objCmds.Parameters.Add("REJQTY", OracleDbType.NVarchar2).Value = cp.rej;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = UNIT;
                                    //objCmds.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = cp.serial;
                                    //objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = cp.Acc;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.amount;

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
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetParty() 
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST order by PARTYMASTID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getdocno() 
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID ,RDELBASICID from RDELBASIC order by RDELBASICID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetdocnoS(string id) 
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID ,RDELBASICID from RDELBASIC WHERE DELTYPE = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
         public DataTable Getbin() 
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,BINMASTERID from BINMASTER order by BINMASTERID desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPartys(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST WHERE PARTYMASTID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }public DataTable GetReceipt(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT LOCID,DCNO,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,STKTYPE,NARRATION,EBY,PARTYMAST.PARTYID ,DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE FROM RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetReceiptItem(string id)
        {
            string SvSql = string.Empty;
           // SvSql = " SELECT CITEMID,BINID,QTY,PENDQTY,REJQTY,UNITMAST.UNITID,SERIALYN,ACCQTY,RATE,AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCBASICID = '" + id + "' ";
            SvSql = "  SELECT ITEMMASTER.ITEMID,BINMASTER.BINID,RECDCDETAIL.QTY,RECDCDETAIL.PENDQTY,RECDCDETAIL.REJQTY,UNITMAST.UNITID,RECDCDETAIL.SERIALYN,RECDCDETAIL.ACCQTY,RECDCDETAIL.RATE,RECDCDETAIL.AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID = RECDCDETAIL.CITEMID LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT LEFT OUTER JOIN BINMASTER on BINMASTER.BINMASTERID = RECDCDETAIL.BINID WHERE RECDCDETAIL.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
        public DataTable GetDCDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select to_char(RDELBASIC.DELDATE,'dd-MON-yyyy')DELDATE,STKTYPE,PARTYMAST.PARTYID from RDELBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RDELBASIC.PARTYID where  RDELBASIC.RDELBASICID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getdctype(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select DELTYPE,DOCID from RDELBASIC WHERE RDELBASIC.RDELBASICID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable Getviewdctype(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  select DELTYPE from RDELBASIC WHERE RDELBASIC.DOCID  = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemDetail(string id)
        {

            string SvSql = string.Empty;
            //SvSql = "   select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID  where ITEMMASTER.ITEMDESC   = '" + id + "' ";
            SvSql = "    select UNITMAST.UNITID,ITEMID from ITEMMASTER LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = ITEMMASTER.PRIUNIT where ITEMMASTERID   = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ViewGetReceipt(string id)
        {
            string SvSql = string.Empty;
            SvSql = "   SELECT LOCDETAILS.LOCID,RDELBASIC.DOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,RECDCBASIC.REFNO,to_char(RECDCBASIC.REFDATE,'dd-MON-yyyy')REFDATE,RECDCBASIC.STKTYPE,RECDCBASIC.NARRATION,EMPMAST.EMPNAME,PARTYMAST.PARTYID,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE FROM RECDCBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID = RECDCBASIC.EBY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID WHERE RECDCBASIC.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
         public DataTable ViewGetReceiptitem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "   SELECT ITEMMASTER.ITEMID,RECDCDETAIL.BINID,RECDCDETAIL.QTY,RECDCDETAIL.PENDQTY,RECDCDETAIL.REJQTY,UNITMAST.UNITID,RECDCDETAIL.SERIALYN,RECDCDETAIL.ACCQTY,RECDCDETAIL.RATE,RECDCDETAIL.AMOUNT FROM RECDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = RECDCDETAIL.CITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID = RECDCDETAIL.UNIT WHERE RECDCDETAIL.RECDCBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemgrpDetail(string id)
        {
            string SvSql = string.Empty;
               SvSql = "SELECT ITEMMASTER.ITEMID , RDELDETAIL.ITEMID AS IID,RDELDETAIL.UNIT,RDELDETAIL.QTY,RDELDETAIL.RATE FROM RDELDETAIL LEFT OUTER JOIN  ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RDELDETAIL.ITEMID LEFT OUTER JOIN  RDELBASIC ON RDELBASIC.RDELBASICID = RDELDETAIL.RDELDETAILID  WHERE RDELDETAIL.RDELBASICID =  '" + id + "' ";
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
                    svSQL = "UPDATE RECDCBASIC SET IS_ACTIVE ='N' WHERE RECDCBASICID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RECDCBASIC SET IS_ACTIVE ='Y' WHERE RECDCBASICID = '" + id + "'";
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
        public DataTable GetAllReceipt(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select RECDCBASIC.RECDCBASICID,RECDCBASIC.IS_ACTIVE,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DOCID,PARTYMAST.PARTYID from RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO where RECDCBASIC.IS_ACTIVE = 'Y' ORDER BY RECDCBASICID DESC  ";

            }
            else
            {
                SvSql = "select RECDCBASIC.RECDCBASICID,RECDCBASIC.IS_ACTIVE,RECDCBASIC.DOCID,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RDELBASIC.DOCID,PARTYMAST.PARTYID from RECDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO where RECDCBASIC.IS_ACTIVE = 'N' ORDER BY RECDCBASICID DESC  ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
