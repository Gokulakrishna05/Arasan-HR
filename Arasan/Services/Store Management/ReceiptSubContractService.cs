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

        public DataTable GetSupplier() 
        {
            string SvSql = string.Empty;
            //SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            SvSql= "Select PartyMASTID ,  P.PartyID, W.WIPLOCID,W.WIPITEMID b From PartyMast P,WCBASIC w Where CONVITEMID<>0 AND w.PARTYID=P.PARTYMASTID  Order By 2";
           
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
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILSID ,  LocID From LOCDETAILS Where LocID in ('APS','STORES','FG GODOWN-SVKS FACTORY','AP') Order by 2";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRecItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT IM.ITEMMASTERID , IM.ITEMID , IM.ITEMDESC, U.UNITID,Im.ITEMACC, im.LATPURPRICE,IM.VALMETHOD,initcap(IM.Itemid) iid, IM.LOTYN  FROM ITEMMASTER IM , UNITMAST U WHERE IM.Priunit= U.UNITMASTID  And IM.Active='Y' Group by IM.ITEMMASTERID , IM.ITEMID , IM.ITEMDESC, U.UNITID, IM.VALMETHOD, IM.LOTYN , IM.SERIALYN,Im.ITEMACC, IM.DRUMYN,im.LATPURPRICE ORDER BY 8";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,WIPITEMID FROM WCBASIC LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=WCBASIC.WIPITEMID WHERE PARTYID='" + id + "' GROUP BY ITEMMASTER.ITEMID,WIPITEMID";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDCDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select  P.ADD1,P.ADD2,P.CITY From PartyMast P Where P.PARTYMASTID='" + id + "'  ";

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
        public DataTable GetItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT IM.ITEMMASTERID , IM.ITEMID , IM.ITEMDESC, U.UNITID,Im.ITEMACC, im.LATPURPRICE,IM.VALMETHOD,initcap(IM.Itemid) iid,Im.LOTYN FROM ITEMMASTER IM , UNITMAST U WHERE IM.Priunit= U.UNITMASTID AND IM.ItemMasterID='"+id+"' And IM.Active='Y'";

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
                    objCmd.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cy.DCNo;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier; 
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value =cy.Location;
                    objCmd.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Add1;
                    objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = cy.Add2;
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
                                int r = 0;
                                foreach (ReceiptDeliverItem cp in cy.Delilst)
                                {
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into RECFSUBEDET (RECFSUBBASICID,RITEM,RITEMMASTERID,RUNIT,RSUBQTY,ERQTY,ERATE,EAMOUNT,RVALMETHOD) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.unit + "','" + cp.qty + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "','"+cp.val+"') RETURNING RECFSUBEDETID INTO :LASTCID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                        objCmds.ExecuteNonQuery();

                                        string detid = objCmds.Parameters["LASTCID"].Value.ToString();

                                        string[] Ddrum = new string[] { };
                                        if (cp.drumno != "" && cp.drumno != null)
                                        {
                                            Ddrum = cp.drumno.Split('-');
                                        }
                                         string[] Dqty = cp.dqty.Split('-');
                                        string[] Drate = cp.drate.Split('-');
                                        string[] Damount = cp.damount.Split('-');
                                        for (int i = 0; i < Ddrum.Length; i++)
                                        {

                                            string dddrum = "";
                                            if (Ddrum.Length > 0)
                                            {
                                                dddrum = Ddrum[i];
                                            }
                                            string ddqty = Dqty[i];
                                            string ddrate = Drate[i];
                                            string ddamount = Damount[i];
                                            string itemname = datatrans.GetDataString("Select ITEMID  FROM ITEMMASTER where   ITEMMASTERID='" + cp.itemid + "'");

                                            string drumname = datatrans.GetDataString("Select DRUMNO  FROM DRUMMAST where DRUMMASTID='" + dddrum + "'");
                                            string partyname = datatrans.GetDataString("Select PARTYNAME  FROM PARTYMAST where PARTYMASTID='" + cy.Supplier + "'");

                                            string item = itemname;
                                            string sup = partyname;
                                            string drum = drumname;
                                            string doc = cy.DocNo;

                                            string lotnumber = string.Format("{0}--{1}--{2}--{3}", item, sup, drum, doc, r.ToString());
                                            r++;

                                            if (cp.Isvalid == "Y" && cp.drumno != "0")
                                            {

                                                svSQL = "Insert into RECFSUBBATCH (RECFSUBBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMMASTID,BQTY,BRATE,BAMOUNT,DRUMNO,LOTNO) VALUES ('" + Pid + "','"+ detid+"','" + cp.itemid + "','" + cp.itemid + "','" + dddrum + "','" + ddqty + "','" + ddrate + "','" + ddamount + "','" + dddrum + "','" + lotnumber + "')";
                                                objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.ExecuteNonQuery();
                                                string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='" + cy.Location + "' ");
                                                if (cp.drumno != "" && cp.drumno != null)
                                                {
                                                    
                                                    OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                                    objCmdIn.Parameters.Add("DOC_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = dddrum;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = drumname;
                                                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                    objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                    objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "RECSUB DC";
                                                    objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                                    objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                                    objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ddqty;
                                                    objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = ddqty;
                                                    objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                    objCmdIn.ExecuteNonQuery();
                                                    Object Pid1 = objCmdIn.Parameters["OUTID"].Value;
 
                                                    OracleCommand objCmdInp = new OracleCommand("DRUMSTKDETPROC", objConn);
                                                    objCmdInp.CommandType = CommandType.StoredProcedure;
                                                    objCmdInp.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdInp.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = Pid1;
                                                    objCmdInp.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                                    objCmdInp.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdInp.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = dddrum;
                                                    objCmdInp.Parameters.Add("DRUM", OracleDbType.NVarchar2).Value = drumname;
                                                    objCmdInp.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                    objCmdInp.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                    objCmdInp.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "RECSUB DC";
                                                    objCmdInp.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                                    objCmdInp.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;

                                                    objCmdInp.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = ddqty;
                                                    objCmdInp.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdInp.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdInp.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = lotnumber;
                                                    objCmdInp.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";

                                                    objCmdInp.ExecuteNonQuery();
                                                }
                                                else
                                                {
                                                    using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                                    {
                                                        OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                        objCmdI.CommandType = CommandType.StoredProcedure;
                                                        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.itemid;
                                                        objCmdI.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                        objCmdI.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                                        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                                        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
                                                        objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                                        objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;

                                                        objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                        objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = lotnumber;
                                                        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                        objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                        objConnI.Open();
                                                        objCmdI.ExecuteNonQuery();
                                                        Object Invid = objCmdI.Parameters["OUTID"].Value;



                                                        OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.itemid;
                                                        objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                                        objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "RECSUB DC";
                                                        objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                        objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                                        objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "RECSUB DC ";
                                                        objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
                                                        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;
                                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                        objCmdIn.ExecuteNonQuery();
                                                    }
                                                }

                                            }
                                        }
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
                                    
                                        

                                    string itemname = datatrans.GetDataString("Select ITEMID  FROM ITEMMASTER where   ITEMMASTERID='" + cp.itemid + "'");
                                    string lotnumber = string.Format("{0} {1} ", itemname, "TO BE (MINUS) IN CLOSING STOCK");
                                        
                                    
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into RECFSUBDDETAIL (RECFSUBBASICID,ITEMID,ITEMDESC,UNIT,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.itemid + "','" + lotnumber + "','" + cp.unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        //if (cy.drumlst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (DrumItem cp in cy.drumlst)
                        //        {

                        //            string item = cp.item;
                        //            string sup = cy.Supplier;
                        //            string drum = cp.drumno;
                        //            string doc = cy.DocNo;

                        //            string lotnumber = string.Format("{0}--{1}--{2}--{3}", item, sup, drum, doc);


                        //            if (cp.Isvalid == "Y" && cp.itemid != "0")
                        //            {

                        //                svSQL = "Insert into RECFSUBBATCH (RECFSUBBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMMASTID,BQTY,BRATE,BAMOUNT,DRUMNO,LOTNO) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.drumno + "','" + cp.qty + "','"+cp.rate+"','" + cp.amount + "','"+cp.drumno+"','"+ lotnumber+"')";
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
        public DataTable GetReceiptSubContract(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select RECFSUBBASIC.DOCID,to_char(RECFSUBBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RECFSUBBASIC.T1SOURCEID,PARTYMAST.PARTYNAME,LOCDETAILS.LOCID,RECFSUBBASIC.ADD1,RECFSUBBASIC.ADD2,RECFSUBBASIC.CITY,THROUGH,TOTQTY,TOTRECQTY,EBY,CHELLAN,REFNO,REFDATE,NARRATION from RECFSUBBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=RECFSUBBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=RECFSUBBASIC.LOCID WHERE RECFSUBBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRecemat(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select RECFSUBBASICID,RITEM,ITEMMASTER.ITEMID,RITEMMASTERID,RUNIT,RSUBQTY,ERQTY,ERATE,EAMOUNT,RECFSUBEDETID from RECFSUBEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RECFSUBEDET.RITEM WHERE RECFSUBBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDeliItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,RECFSUBDDETAIL.UNIT,RECFSUBDDETAIL.RATE,RECFSUBDDETAIL.AMOUNT,RECFSUBDDETAIL.QTY,RECFSUBDDETAIL.ITEMDESC,RECFSUBDDETAILID from RECFSUBDDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=RECFSUBDDETAIL.ITEMID WHERE RECFSUBBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrimdetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select RECFSUBBASICID,PARENTRECORDID,RECFSUBBATCH.BQTY,RECFSUBBATCH.BRATE,RECFSUBBATCH.BAMOUNT,DRUMMAST.DRUMNO,RECFSUBBATCH.LOTNO from RECFSUBBATCH LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=RECFSUBBATCH.DRUMMASTID WHERE PARENTRECORDID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
