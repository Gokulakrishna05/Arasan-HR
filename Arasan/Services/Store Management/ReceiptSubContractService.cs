using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Arasan.Services 
{
    public class ReceiptSubContractService : IReceiptSubContract
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ReceiptSubContractService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetSupplier(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            SvSql= "Select PARTYMAST.PARTYNAME,SUBCONTDCBASIC.PARTYID from SUBCONTDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SUBCONTDCBASIC.PARTYID  Where SUBCONTDCBASICID='" + id+"'";
           
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDC( )
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,SUBCONTDCBASICID from SUBCONTDCBASIC";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDCDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADD1,ADD2,CITY from SUBCONTDCBASIC WHERE SUBCONTDCBASICID='"+id+"'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRecvItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBCONTEDET.RITEM,ITEMMASTER.ITEMID,RUNIT,ERATE,EAMOUNT,ERQTY,SUBCONTEDETID from SUBCONTEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTEDET.RITEM WHERE SUBCONTDCBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDelivItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBCONTDCDETAIL.ITEMID as item,ITEMMASTER.ITEMID,SUBCONTDCDETAIL.UNIT,SUBCONTDCDETAIL.QTY,SUBCONTDCDETAIL.RATE,SUBCONTDCDETAIL.AMOUNT,SUBCONTDCDETAILID from SUBCONTDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTDCDETAIL.ITEMID WHERE SUBCONTDCBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBCONTDCDETAIL.ITEMID as item,ITEMMASTER.ITEMID,UNIT,RATE,AMOUNT,QTY,SUBCONTDCDETAILID from SUBCONTDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTDCDETAIL.ITEMID WHERE SUBCONTDCDETAILID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDrum()
        {
            string SvSql = string.Empty;
            SvSql = "Select DRUMNO,DRUMMASTID from DRUMMAST";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ReceiptSubContractCRUD(ReceiptSubContract cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'RS-F' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "RS-F", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='RS-F' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocNo = docid;
                }


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RECFSUBPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                     

                    objCmd.Parameters.Add("Branch", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier; 
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value =cy.Location;
                    objCmd.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = cy.Add1;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Add2;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                    objCmd.Parameters.Add("THROUGH", OracleDbType.NVarchar2).Value = cy.Through;
                    objCmd.Parameters.Add("TOTQTY", OracleDbType.NVarchar2).Value = cy.qtyrec;
                    objCmd.Parameters.Add("TOTRECQTY", OracleDbType.NVarchar2).Value = cy.TotRecqty;
                    objCmd.Parameters.Add("EBY", OracleDbType.NVarchar2).Value = cy.enterd;
                    objCmd.Parameters.Add("CHELLAN", OracleDbType.NVarchar2).Value = cy.Chellan;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.enterd;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
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
                        if (cy.Reclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ReceiptDeliverItem cp in cy.Delilst)
                                {
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        
                                            svSQL = "Insert into RECFSUBEDET (RECFSUBBASICID,RITEM,RITEMMASTERID,RUNIT,RSUBQTY,ERQTY,ERATE,EAMOUNT) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.unit + "','" + cp.qty + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "')";
                                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                            objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        if (cy.Reclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ReceiptRecivItem cp in cy.Reclst)
                                {
                                    
                                        string item = cp.item;
                                        

                                        string lotnumber = string.Format("{0} {1} ", item, "TO BE (MINUS) IN CLOSING STOCK");
                                        
                                    
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into RECFSUBEDET (RECFSUBBASICID,ITEMID,ITEMDESC,UNIT,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.itemid + "','" + lotnumber + "','" + cp.unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        if (cy.drumlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DrumItem cp in cy.drumlst)
                                {

                                    string item = cp.item;
                                    string sup = cy.Supplier;
                                    string drum = cp.drumno;
                                    string doc = cy.DocNo;

                                    string lotnumber = string.Format("{0}--{1}--{2}--{3}", item, sup, drum, doc);


                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into RECFSUBEDET (RECFSUBBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMMASTID,BQTY,BRATE,BAMOUNT,DRUMNO,LOTNO) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.drumno + "','" + cp.qty + "','"+cp.rate+"','" + cp.amount + "','"+cp.drumno+"','"+ lotnumber+"')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
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
        public DataTable GetAllReceiptSubContractItem(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DOCID,RECFSUBBASICID,PARTYMAST.PARTYNAME,to_char(RECFSUBBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,LOCDETAILS.LOCID from RECFSUBBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=RECFSUBBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=RECFSUBBASIC.LOCID WHERE RECFSUBBASIC.IS_ACTIVE='Y'";
            }
            else
            {
                SvSql = "Select DOCID,RECFSUBBASICID,PARTYMAST.PARTYNAME,to_char(RECFSUBBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,LOCDETAILS.LOCID from RECFSUBBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=RECFSUBBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=RECFSUBBASIC.LOCID WHERE RECFSUBBASIC.IS_ACTIVE='N'";

            }
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
