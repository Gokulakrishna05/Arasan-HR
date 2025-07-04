﻿using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;



namespace Arasan.Services
{
    public class ExportInvoiceService : IExportInvoice
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ExportInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE IGROUP = 'FINISHED'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LOCDETAILSID,LOCID FROM LOCDETAILS WHERE LOCDETAILSID IN ('12423000000238','12418000000423','10001000000827')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier2()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('TRANSPORTER')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCustomerDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select COUNTRY from PARTYMAST Where PARTYMAST.PARTYMASTID = '" + id + "' ";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExRateDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURRNAME, EXRATE, CURRID, CRATEID, MODIFIEDON  FROM CRATE WHERE CURRID = '"+ id +"' ORDER BY MODIFIEDON DESC FETCH FIRST 1 ROWS ONLY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string slaesinvoiceCRUD(ExportInvoice cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'eexiv' AND ACTIVESEQUENCE = 'T'");
                string DCNo = string.Format("{0}{1}", "EXP-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'eexiv' AND ACTIVESEQUENCE ='T'";

                cy.InvNo = DCNo;
                string INvtype = "";
                DataTable dtacc = new DataTable();
                if (cy.igst == 0)
                {
                    INvtype = "GST SALES";
                    dtacc = datatrans.GetData("select ADACCOUNT,ADNAME,H.ADCOMPHID from ADCOMPH H,ADCOMPD D where H.ADCOMPHID=D.ADCOMPHID AND H.ADSCHEME='EI-GST 18%' AND H.ACTIVE='Yes'");
                }
                else
                {
                    INvtype = "IGST SALES";
                    dtacc = datatrans.GetData("select ADACCOUNT,ADNAME,H.ADCOMPHID from ADCOMPH H,ADCOMPD D where H.ADCOMPHID=D.ADCOMPHID AND H.ADSCHEME='EI-IGST 18%' AND H.ACTIVE='Yes'");
                }
                string Grossledger = "";
                string frieghtledger = "";
                string discledger = "";
                string roundoffledger = "";
                string cgstledger = "";
                string sgstledger = "";
                string igstledger = "";
                if (dtacc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtacc.Rows.Count; i++)
                    {
                        if (dtacc.Rows[i]["ADNAME"].ToString() == "GROSS")
                        {
                            Grossledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString() == "FREIGHT")
                        {
                            frieghtledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString() == "DISCOUNT")
                        {
                            discledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString() == "ROUND OFF")
                        {
                            roundoffledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString().Contains("CGST"))
                        {
                            cgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString().Contains("SGST"))
                        {
                            sgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                        if (dtacc.Rows[i]["ADNAME"].ToString().Contains("IGST"))
                        {
                            igstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                        }
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.InvoiceLst != null)
                    {
                        int totdrums = 0;
                        int totqty = 0;
                        double totdis = 0;
                        double totqtydis = 0;
                        double totcashdisc = 0;
                        foreach (ExportInvoiceItem cp in cy.InvoiceLst)
                        {
                            if (cp.Isvalid == "Y")
                            {
                                if (!string.IsNullOrEmpty(cp.DrumIds))
                                {
                                    string drumids = cp.DrumIds;
                                    string[] drumidlist = drumids.Split(",");
                                    int tot = drumidlist.Length;
                                    totdrums += tot;
                                    totqty += Convert.ToInt32(cp.quantity);



                                }
                            }
                        }
                        using (OracleCommand command = objConn.CreateCommand())
                        {
                            using (OracleTransaction transaction = objConn.BeginTransaction(IsolationLevel.ReadCommitted))
                            {
                                command.Transaction = transaction;

                                try
                                {
                                    DataTable dtParty = datatrans.GetData("select distinct P.CREDITDAYS,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.ratecode,a.limit,P.PANNO,P.CREDITLIMIT,P.TRANSLMT from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and P.PARTYMASTID='" + cy.Customer + "'");
                                    double limit = dtParty.Rows[0]["limit"].ToString() == "" ? 0 : (long)Convert.ToDouble(dtParty.Rows[0]["limit"].ToString());
                                    string partyarc = dtParty.Rows[0]["ratecode"].ToString();
                                    string PartyG = dtParty.Rows[0]["PartyGroup"].ToString();
                                    string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                                    DataTable psaledt = datatrans.GetData("Select nvl(sum(net),0) nets from ( Select sum(net) net from exinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode ='" + partyarc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID = '" + cy.Customer + "' or(P.PARTYGROUP ='" + PartyG + "' and 'None' <> '" + PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE = 'ORDER' Union All Select sum(net) net from Depinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode = '" + partyarc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID ='" + cy.Customer + "' or(P.PARTYGROUP = '" + PartyG + "' and 'None' <> '" + PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE = 'ORDER')");
                                    double asale = (long)Convert.ToDouble(psaledt.Rows[0]["nets"].ToString());
                                    DataTable partybdt = datatrans.GetData("Select nvl(Sum(debit-Credit),0) bal from dailytrans where mid='" + mid + "' and t2vchdt<='" + cy.InvDate + "'");
                                    double PartyB = (long)Convert.ToDouble(partybdt.Rows[0]["bal"].ToString());
                                    DateTime duedate = DateTime.Now;
                                    if (dtParty.Rows[0]["CREDITDAYS"].ToString() != "")
                                    {
                                        int creditdays = dtParty.Rows[0]["CREDITDAYS"].ToString() == "" ? 0 : Convert.ToInt32(dtParty.Rows[0]["CREDITDAYS"].ToString());
                                        duedate = DateTime.Now.AddDays(creditdays);
                                    }

                                    string loctype = datatrans.GetDataString("SELECT LOCATIONTYPE FROM LOCDETAILS WHERE LOCDETAILSID='" + cy.Location + "'");
                                    string sympal = datatrans.GetDataString("SELECT SYMBOL FROM CURRENCY WHERE CURRENCYID='" + cy.Currency + "'");
                                    command.CommandText = "INSERT INTO EEXINVBASIC (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, BRANCHID, LOCID, VTYPE, ADSCHEME, DOCID, DOCDATE, REFNO, REFDATE, MAINCURRENCY, SYMBOL, EXRATE, PARTYID, PARTYNAME, CUSTACC, TEMPID, ADTGMCONTROL, RNDOFF, CREDITLIMIT, TRANSLIMIT, PARTYBALANCE, CREDITDAYS, DUEDATE, MOBILE, TYPE, LSCHEME, LICENCENO, CANDF, LOADPORT, DISCHPORT, FDESTINATION, FRIEGHT, GROSS, NET, NARRATION, FBED, FCESS, FHSE, DUPINVNO, DUPINVID, BINYN, TERM, COUNTRY, RUPEES, EXCHANGERATE, BTERMS, GROSSQTY, DGROSS, GDRUM, GGROSS, GNET, BEDTRCH, BEDTRAMT, REMA, INVDESC, FCFLAG, BTOTPORLAMT, AMTWORDS, CNO, MAINCURR, EXCHRATE, PDOCID, CANDFCODE, EXSTATUS, ORTY, INVTYPE, GOODSORIGIN, LOCDETAILSID, FRININS, VESFLY, CONTNO, CENEXS, BDISCOUNT, GAMTWORDS, CHACFS, BIGST, BSGST, BCGST, INSURAMT, CONCODE, TRANSP, ROFF, TBASEAMOUNT, SHIPBNO, SHIPBDT, TDIST, BOFLAD, BOFLADDT, POFDEL, POFACC, BFREIGHT, BINSAMT, LOADPORTP, LOADPORTS, ROFFF)  " +
                                        "VALUES ('0','0','F','0','0','" + cy.Branch + "','" + cy.Location + "','R','10056000000070','" + cy.InvNo + "','" + cy.InvDate + "','" + cy.RefNo + "','" + cy.RefNo + "','" + cy.Currency + "','" + sympal + "','" + cy.ExRate + "','" + cy.Customer + "','" + dtParty.Rows[0]["PARTYNAME"].ToString() + "','" + dtParty.Rows[0]["ACCOUNTNAME"].ToString() + "','0','T','" + cy.Round + "','" + limit + "','" + limit + "','" + PartyB + "','" + duedate + "','','','R','ADVANCE LISCENSE SCHEME','NONE','" + cy.Country + "','" + cy.Port + "','" + cy.DisCharge + "','" + cy.Destination + "','" + cy.FrightCharge + "','" + cy.Gross + "','" + cy.Net + "','" + cy.Narration + "','0','0','0','0','0','NO','" + cy.Terms + "','" + cy.Country + "','Rupees','1','nil','" + totqty + "','" + cy.Gross + "','" + totdrums + ",'" + cy.Gross + "','" + totqty + "','','','0','','" + cy.Words + "','','0','0','" + cy.CCode + "','" + cy.Order + "','Export Sales','INDIA','" + cy.Location + "','0','','','','" + cy.Discount + "','','','0','0','" + cy.igst + "','" + cy.CCode + "','" + cy.ByRoad + "','" + cy.Round + "','','" + cy.Shipment + "','" + cy.ShipDate + "','" + cy.Distance + "','-','" + cy.LadingDate + "','" + cy.Lading + "') RETURNING EEXINVBASIC INTO :LASTCID";
                                    command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    command.ExecuteNonQuery();
                                    string Pid = command.Parameters["LASTCID"].Value.ToString();

                                    command.Parameters.Clear();

                                    //string Pid = "0";
                                    if (cy.ID != null)
                                    {
                                        Pid = cy.ID;
                                    }
                                    if (cy.InvoiceLst != null)
                                    {
                                        if (cy.ID == null)
                                        {
                                            int r = 1;
                                            foreach (ExportInvoiceItem cp in cy.InvoiceLst)
                                            {
                                                string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");
                                                DataTable item = datatrans.GetData("SELECT LOTYN,SERIALYN,ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId + "'");
                                                if (cp.saveItemId != "0")
                                                {
                                                    string[] drumidlist = null;
                                                    double brate = Math.Round(cp.AcessAmount / cp.quantity, 2);
                                                    int totdrumscount = 0;
                                                    double Crate1 = 0;
                                                    double Camount = 0;
                                                    string crate = "0";
                                                    if (!string.IsNullOrEmpty(cp.DrumIds))
                                                    {
                                                        string drumids = cp.DrumIds;
                                                        drumidlist = drumids.Split(",");
                                                        totdrumscount = drumidlist.Length;
                                                        crate = datatrans.GetDataString("SELECT ROUND((SUM(PL.Amount) / SUM(PL.QTY)),2) as CRATE  FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.plstockvalueID IN (" + cp.DrumIds + ")");

                                                        Crate1 = Convert.ToDouble(crate);
                                                        Camount = Crate1 * cp.quantity;
                                                    }
                                                    command.CommandText = "INSERT INTO EEXINVDETAIL (EEXINVBASICID, EEXINVDETAILROW, JOBNO, ITEMID, MATSUPP, UNIT, LOTYN, SERIALYN, ORDQTY, PENDQTY, OLDEIQTY, SUBQTY, EXCISEQTY, PRIUNIT, QTY, RATE, SL, AMOUNT, BASERATE, COSTRATE, EXINVDETAILROW, RETURNEDQTY, JODETAILID, JOSCHEDULEID, EXCISETYPE, TARIFFID, BEDPER, BEDAMT, CESSPER, CESSAMT, SHECESSPER, SHECESSAMT, AEDPER, AEDAMT, SEDPER, SEDAMT, BCDPER, BCDAMT, TOTAMT, TOTEXAMT, BINID, CRATE, CAMOUNT, ORDTYPE, SLNO, EXPDCNO, EXPDCDT, GWT, NWT, TWT, NDRUM, DOP, DOE, EJOBASICID, EXPDCDETAILID, TEDCBASICID, DAMOUNT, MQTY, QDISC, IDISC, CDISC, TDISC, ADISC, SDISC, DISCOUNT, FRECH, FREIGHT, BASEAMOUNT1, BASEAMOUNT, ITEMTYPE, NRATE, ASDQTY, SDNO, ITEMSPEC, SPORL, LLAMOUNT, PORLAMOUNT, EORDTYPE, OTHERDISC, NAMOUNT, IGSTP, IGST, UNITC, HSN, PACKSPEC, PTERMS, INSCH, INSAMT, JOBDT, DRUMDESC)" +
                                                        " VALUES ('" + Pid + "', '" + r + "', '" + cp.work + "', '" + cp.saveItemId + "', '', '" + unit + "', '" + item.Rows[0]["LOTYN"].ToString() + "', '" + item.Rows[0]["SERIALYN"].ToString() + "', '0', '0', '0', '" + cp.quantity + "', '" + cp.quantity + "', '" + cp.Unit + "', '" + cp.quantity + "','" + cp.rate + "', '" + r + "', '" + cp.Amount + "', '" + brate + "', '" + crate + "', '0', '0', '0', '0', 'INPUTS', '" + cp.Traiff + "', '0','0', '0', '0', '0','0', '0', '0', '0','0', '0','0', '" + cp.TotalAmount + "', '0', '0', '" + crate + "', '" + Camount + "', '', '" + r + "', '" + cp.dcno + "', '" + cp.dcdate + "', '" + cp.gwt + "', '" + cp.nwt + "', '" + cp.twt + "', '" + totdrums + "', '', '', '" + cp.workid + "', '" + cp.dcid + "', '', '','', '" + cp.discamt + "', '" + cp.introdisc + "',  '" + cp.cashdis + "', '" + cp.tradedis + "',  '" + cp.adddis + "', '" + cp.specdis + "', '" + cp.Discount + "', '" + cp.frigcharges + "', '" + cp.frigcharges + "', '', '', '" + cp.itemtypes + "', '', '', '', '" + cp.itemdesc + "', '', '','', '', '', '','', '', '', '" + cp.Pack + "', '', '', '', '', '" + cp.drum + "') RETURNING EEXINVBASIC INTO :stkid";
                                                    command.Parameters.Add("stkid", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                                    command.ExecuteNonQuery();
                                                    string did = command.Parameters["stkid"].Value.ToString();

                                                    command.Parameters.Clear();

                                                    if (!string.IsNullOrEmpty(cp.DrumIds))
                                                    {
                                                        int n = 1;
                                                        foreach (string drumid in drumidlist)
                                                        {
                                                            long plstockvalue = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='plstockvalue'");
                                                            DataTable dt = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE as LOTRATE,ps.PLUSQTY,ps.DRUMNO,ps.RATE,ps.LOTNO,ps.STOCKVALUE,PL.Amount FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.plstockvalueID='" + drumid + "'");
                                                            // command.CommandText = "Insert into plstockvalue (T1SOURCEID,DOCID,DOCDATE,LOTNO,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,STOCKTRANSTYPE,APPROVAL,MAXAPPROVED,CANCEL,PLUSQTY,FROMLOCID) VALUES ('" + Pid + "','" + cy.InvNo + "','" + cy.InvDate + "','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cp.saveItemId + "','" + cy.Location + "','Ex.Invoice','0','0','F','0','0')";
                                                            //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                            // command.ExecuteNonQuery();

                                                            //DataTable dt2 = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.PLSTOCKVALUEID='"+ plstockvalue + "'");
                                                            double amt = Convert.ToDouble(dt.Rows[0]["LOTRATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                                            double amt12 = Convert.ToDouble(dt.Rows[0]["RATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                                            command.CommandText = "INSERT INTO  EEXINVLOT (EEXINVBASICID, PARENTRECORDID, EEXINVLOTROW, LITEMID, LLOCID, LOTNO, DRUMNO, SERIALNO, LOTSTOCK, LQTY, LRATE, LAMOUNT, PARENTROW, SLRATE, PORL, LITEMMASTERID) " +
                                                                "VALUES ('" + Pid + "', '" + did + "', '" + n + "', '" + cp.saveItemId + "', '" + cy.Location + "', '" + dt.Rows[0]["LOTNO"].ToString() + "', '" + dt.Rows[0]["DRUMNO"].ToString() + "', '', '0', '" + dt.Rows[0]["PLUSQTY"].ToString() + "', '" + dt.Rows[0]["LOTRATE"].ToString() + "', '" + amt + "', '" + r + "','', '" + cp.saveItemId + "')";
                                                            //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                            command.ExecuteNonQuery();
                                                            command.CommandText = "Insert into plstockvalue (T1SOURCEID,DOCID,DOCDATE,LOTNO,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,STOCKTRANSTYPE,APPROVAL,MAXAPPROVED,CANCEL,PLUSQTY,FROMLOCID) VALUES ('" + did + "','" + cy.InvNo + "','" + cy.InvDate + "','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cp.saveItemId + "','" + cy.Location + "','Ex.Invoice','0','0','F','0','0')";
                                                            //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                            command.ExecuteNonQuery();
                                                            //datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (plstockvalue + 1).ToString() + "' where NAME='plstockvalue'");
                                                            n = n + 1;
                                                        }


                                                    }
                                                    string locname = datatrans.GetDataString("SELECT LOCID FROM LOCDETAILS WHERE LOCDETAILSID='" + cy.Location + "'");
                                                    string itemnames = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId + "'");
                                                    long STOCKVALUE = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='STOCKVALUE'");
                                                    command.CommandText = "Insert into STOCKVALUE (STOCKVALUEID,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,MASTERID,LOCID,STOCKTRANSTYPE,BINID,CANCEL,LOCIDC,ITEMIDC) VALUES ('" + STOCKVALUE + "','" + did + "','m','" + cp.saveItemId + "','" + cy.InvDate + "','" + cp.quantity + "','" + Camount + "','" + mid + "','" + cy.Location + "','SALES','','F','" + locname + "',q'[" + itemnames + "]')";
                                                    //OracleCommand objCmddt = new OracleCommand(svSQL, objConn);
                                                    command.ExecuteNonQuery();
                                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (STOCKVALUE + 1).ToString() + "' where NAME='STOCKVALUE'");

                                                    long STOCKVALUE2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='STOCKVALUE2'");
                                                    command.CommandText = "Insert into STOCKVALUE2 (STOCKVALUE2ID,STOCKVALUEID,DOCID,RATE,NARRATION) VALUES ('" + STOCKVALUE2 + "','" + STOCKVALUE + "','" + cy.InvNo + "','" + crate + "','" + cy.Narration + "')";
                                                    //OracleCommand objCmddts = new OracleCommand(svSQL, objConn);
                                                    command.ExecuteNonQuery();
                                                }


                                                r++;
                                            }
                                        }

                                    }

                                    command.CommandText = "Insert into EXINVSADD (EXINVBASICID,STYPE,SADD1,SADD2,SADD3,SSTATE,SCITY,SPINCODE,SPHONE) VALUES ('" + Pid + "','" + cy.Area + "','','" + cy.Address + "','" + cy.add2 + "','" + cy.add3 + "','" + cy.State + "','" + cy.City + "','" + cy.Pincode + "','" + cy.Phone + "')";
                                    command.ExecuteNonQuery();

                                    int t2cunt = 2;
                                    double totgst = 0;
                                    if (cy.Discount > 0)
                                    {
                                        t2cunt += 1;
                                    }
                                    if (cy.FrightCharge > 0)
                                    {
                                        t2cunt += 1;
                                    }
                                    

                                    if (cy.igst > 0)
                                    {
                                        t2cunt += 1;
                                        totgst = cy.igst;
                                    }
                                    if (cy.Round > 0)
                                    {
                                        t2cunt += 1;
                                    }
                                    
                                    long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
                                    
                                    string mno = DateTime.Now.ToString("yyyyMM");
                                    command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID,TOTGST,ENTBY,USERID)" +
                                        " VALUES ('" + TRANS1 + "','0','" + Pid + "','N','sa','" + cy.InvNo + "','1','1','" + cy.InvNo + "','" + cy.InvDate + "','" + mno + "','a','" + mid + "','" + cy.Net + "','" + Grossledger + "','" + cy.Gross + "','" + mid + "','" + cy.Net + "','vchsl','" + cy.Branch + "','" + cy.Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Narration + "','" + t2cunt + "','Y','0','R','T','" + cy.Words + "','" + cy.user + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0','" + totgst + "','" + cy.user + "','" + cy.user + "')";
                                    //OracleCommand objCmdA = new OracleCommand(svSQL, objConn);
                                    command.ExecuteNonQuery();

                                    
                                    long groupno = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='GROUPNO'");
                                    command.CommandText = "Insert into RPDETAILS (TRANSID,DOCID,DOCDATE,PARTYNAME,AMOUNT,GROUPNO,GROUPORDER,PARENTDOCID,MATCHDATE,BRANCHID,EXRATE,MAINCURR,USERID,DUEDATE,CANCEL)VALUES('exinv','" + cy.InvNo + "','" + cy.InvDate + "','" + mid + "','" + cy.Net + "','" + groupno + "','1','" + Pid + "','" + cy.InvDate + "','" + cy.Branch + "','1','1','" + cy.user + "','" + cy.InvDate + "','F')";
                                    command.ExecuteNonQuery();
                                    long TRANS2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID,RPAMOUNT) VALUES ('" + TRANS2 + "','" + TRANS1 + "','Dr','" + mid + "','" + cy.Net + "','" + cy.Net + "','0','0','1','1','" + cy.Net + "','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + Grossledger + "','" + cy.Net + "')";
                                    //  OracleCommand objCmdas = new OracleCommand(svSQL, objConn);
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2 + 1).ToString() + "' where NAME='TRANS2'");

                                    ////////////////////Gross Ledger
                                    long TRANS2G = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2G + "','" + TRANS1 + "','Cr','" + Grossledger + "','0','0','" + cy.Gross + "','" + cy.Gross + "','1','1','0','" + cy.Gross + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                    //OracleCommand objCmdag = new OracleCommand(svSQL, objConn);
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2G + 1).ToString() + "' where NAME='TRANS2'");

                                    ////////////////////Discount
                                    if (cy.Discount > 0)
                                    {
                                        long TRANS2D = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                        command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2D + "','" + TRANS1 + "','Dr','" + discledger + "','" + cy.Discount + "','" + cy.Discount + "','0','0','1','1','" + cy.Net + "','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                        //OracleCommand objCmdaD = new OracleCommand(svSQL, objConn);
                                        command.ExecuteNonQuery();
                                        datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2D + 1).ToString() + "' where NAME='TRANS2'");
                                    }

                                    /////////////////////Frieght

                                    if (cy.FrightCharge > 0)
                                    {
                                        long TRANS2F = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                        command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2F + "','" + TRANS1 + "','Cr','" + frieghtledger + "','0','0','" + cy.FrightCharge + "','" + cy.FrightCharge + "','1','1','0','" + cy.FrightCharge + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                        //OracleCommand objCmdaf = new OracleCommand(svSQL, objConn);
                                        command.ExecuteNonQuery();
                                        datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2F + 1).ToString() + "' where NAME='TRANS2'");
                                    }







                                    ///////////////////CGST
                                    if (cy.igst > 0)
                                    {
                                        long TRANS2I = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                        command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2I + "','" + TRANS1 + "','Cr','" + igstledger + "','0','0','" + cy.igst + "','" + cy.igst + "','1','1','0','" + cy.igst + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                        //OracleCommand objCmdaf = new OracleCommand(svSQL, objConn);
                                        command.ExecuteNonQuery();
                                        datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2I + 1).ToString() + "' where NAME='TRANS2'");
                                    }

                                    /////////////////////ROUND OFF

                                    if (cy.Round > 0)
                                    {
                                        long TRANS2R = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                        command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2R + "','" + TRANS1 + "','Cr','" + roundoffledger + "','0','0','" + cy.Round + "','" + cy.Round + "','1','1','0','" + cy.Round + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                        //OracleCommand objCmdaf = new OracleCommand(svSQL, objConn);
                                        command.ExecuteNonQuery();
                                        datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2R + 1).ToString() + "' where NAME='TRANS2'");
                                    }
                                    string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
                                    datatrans.UpdateStatus(updatetrans);
                                    command.CommandText = "delete from dailytrans where t2vchdt='" + DateTime.Now.ToString("dd-MMM-yyyy") + "'";
                                    command.ExecuteNonQuery();

                                    command.CommandText = @"insert into dailytrans 
Select t2.T2VCHDT, t2.MID, t1.VCHSTATUS EMODE, t1.MSTATUS MSTATUS, t1.MONTHNO MONTHNO,
   Sum(t2.DBAMOUNT) DEBIT,Sum(t2.CRAMOUNT) CREDIT, t1.BRANCHID, t1.vtype DTYPE
   FRom trans2 t2,trans1 t1 where t1.TRANS1ID = t2.TRANS1ID and t2.T2VCHDT ='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Group by t1.BRANCHID, t1.vtype,t2.T2VCHDT, t2.MID, t1.VCHSTATUS,t1.MSTATUS,t1.MONTHNO  ";
                                    command.ExecuteNonQuery();
                                 
                                    //if (cy.ScrapLst != null)
                                    //{
                                    //    if (cy.ID == null)
                                    //    {
                                    //        int r = 1;
                                    //        foreach (ScrapItem cp in cy.ScrapLst)
                                    //        {
                                    //            string unit1 = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                //            if (cp.saveItemId != "0")
                                //            {
                                //                svSQL = "Insert into EDCREJDETAILS (EDCBASICID,RLOCID,RITEMID,RUNIT,RCF,SL1,RQTY,RCOSTRATE,BILLEDQTY) VALUES ('" + Pid + "','" + cp.Rejected + "','" + cp.ItemId + "','" + unit1 + "','" + cp.CF + "','" + cp.Stock + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
                                //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                //                objCmds.ExecuteNonQuery();

                                //            }

                                //            r++;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        svSQL = "Delete EDCREJDETAILS WHERE EDCBASICID='" + cy.ID + "'";
                                //        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                //        objCmdd.ExecuteNonQuery();
                                //        foreach (ScrapItem cp in cy.ScrapLst)
                                //        {
                                //            string unit1 = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                //            if (cp.Isvalid1 == "Y" && cp.saveItemId != "0")
                                //            {
                                //                svSQL = "Insert into EDCREJDETAILS (EDCBASICID,RLOCID,RITEMID,RUNIT,RCF,SL1,RQTY,RCOSTRATE,BILLEDQTY) VALUES ('" + Pid + "','" + cp.Rejected + "','" + cp.ItemId + "','" + unit1 + "','" + cp.CF + "','" + cp.Stock + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
                                //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                //                objCmds.ExecuteNonQuery();


                                //            }


                                //        }
                                //    }
                                    transaction.Commit();
                                datatrans.UpdateStatus(updateCMd);
                            }
                         
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    Console.WriteLine(ex.ToString());
                                    msg = ex.ToString();
                                    Console.WriteLine("Neither record was written to database.");
                                }
                            }
                        }
                        objConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetAllListExportInv(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = @"SELECT A.DOCID INVNO,to_char(A.DOCDATE,'dd-MON-yyyy') INVDATE,P.PARTYID,C.MAINCURR,A.NET,A.GROSS,A.BCGST,A.BIGST,A.BSGST, A.EEXINVBASICID
FROM EEXINVBASIC A,PARTYMAST P,CURRENCY C,EEXINVDETAIL D
WHERE  C.CURRENCYID=A.MAINCURRENCY
AND P.PARTYMASTID=A.PARTYID AND A.EEXINVBASICID=D.EEXINVBASICID ORDER BY to_date(DOCDATE) DESC,DOCID DESC";
            }
             
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
