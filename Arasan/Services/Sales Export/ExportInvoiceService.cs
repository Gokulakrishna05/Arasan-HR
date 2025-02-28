using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
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
            SvSql = "SELECT CURRNAME,EXRATE,CURRID,CRATEID,MODIFIEDON FROM CRATE WHERE CURRID = " + id + " ORDER BY MODIFIEDON DESC FETCH FIRST 1 ROWS ONLY";
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
                                        "VALUES ('0','0','F','0','0','" + cy.Branch + "','" + cy.Location + "','R','10056000000070','" + cy.InvNo + "','" + cy.InvDate + "','" + cy.RefNo + "','" + cy.RefNo + "','" + cy.Currency + "','" + sympal + "','" + cy.ExRate + "','"+cy.Customer+"','" + dtParty.Rows[0]["PARTYNAME"].ToString() + "','" + dtParty.Rows[0]["ACCOUNTNAME"].ToString() + "','0','T','" + cy.Round + "','" + limit + "','"+ limit + "','"+ PartyB + "','"+ duedate + "','','','R','ADVANCE LISCENSE SCHEME','NONE','"+cy.Country+"','"+cy.Port + "','"+ cy.DisCharge + "','" + cy.Destination + "','" + cy.FrightCharge + "','" + cy.Gross + "','"+cy.Net+"','"+cy.Narration+"','0','0','0','0','0','NO','"+cy.Terms+"','"+cy.Country+ "','Rupees','1','nil','"+ totqty +"','"+cy.Gross+"','"+ totdrums + ",'"+cy.Gross+"','"+totqty+"','','','0','','"+cy.Words+"','','0','0','"+cy.CCode+"','"+cy.Order+ "','Export Sales','INDIA','"+cy.Location+"','0','','','','"+cy.Discount+"','','','0','0','"+cy.igst+"','"+cy.CCode+"','"+cy.ByRoad + "','"+cy.Round+"','','"+cy.Shipment+"','"+cy.ShipDate+"','"+cy.Distance+"','-','"+cy.LadingDate+"','"+cy.Lading+"') RETURNING EDCBASICID INTO :LASTCID";
                                    command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    command.ExecuteNonQuery();
                                    string Pid = command.Parameters["LASTCID"].Value.ToString();

                                    command.Parameters.Clear();

                                    //string Pid = "0";
                                    if (cy.ID != null)
                                    {
                                        Pid = cy.ID;
                                    }
                                    //if (cy.ExportDCLst != null)
                                    //{
                                    //    if (cy.ID == null)
                                    //    {
                                    //        int r = 1;
                                    //        foreach (ExportDCItem cp in cy.ExportDCLst)
                                    //        {
                                    //            string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");
                                    //            DataTable item = datatrans.GetData("SELECT LOTYN,SERIALYN,ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId + "'");
                                    //            if (cp.saveItemId != "0")
                                    //            {
                                    //                string[] drumidlist = null;
                                    //                //double brate = Math.Round(acssamt / cp.quantity, 2);
                                    //                int totdrumscount = 0;
                                    //                double Crate1 = 0;
                                    //                double Camount = 0;
                                    //                string crate = "0";
                                    //                if (!string.IsNullOrEmpty(cp.DrumIds))
                                    //                {
                                    //                    string drumids = cp.DrumIds;
                                    //                    drumidlist = drumids.Split(",");
                                    //                    totdrumscount = drumidlist.Length;
                                    //                    crate = datatrans.GetDataString("SELECT ROUND((SUM(PL.Amount) / SUM(PL.QTY)),2) as CRATE  FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.plstockvalueID IN (" + cp.DrumIds + ")");

                                    //                    Crate1 = Convert.ToDouble(crate);
                                    //                    Camount = Crate1 * cp.quantity;
                                    //                }
                                    //                command.CommandText = "Insert into EDCDETAIL (EDCBASICID,ITEMID,PACKSPEC,UNIT,MQTY,ISSRATE,ISSAMT,LOTYN,SERIALYN,SEALNO,ORDQTY,CLSTOCK,DCQTY,QTY,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,DRUMDESC,CNO,TWT,VECHILENO,MATSUPP,M,CITEMID,EDCDETAILROW,SL,OPSTOCK,INVOICEDQTY,JODETAILID,TDRUM,PRIUNIT,UNNO) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.des + "','" + unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.Amount + "','" + item.Rows[0]["LOTYN"].ToString() + "','" + item.Rows[0]["SERIALYN"].ToString() + "','" + cp.Seal + "','" + cp.quantity + "','" + cp.Current + "','" + cp.quantity + "','" + cp.quantity + "','" + cp.discamt + "','" + cp.CashDisc + "','" + cp.Introduction + "','" + cp.Trade + "','" + cp.Addition + "','" + cp.Special + "','" + cp.Fright + "','" + cp.Drum + "','" + cp.Container + "','" + cp.Tare + "','" + cp.Vechile + "','OWN','Aluminium Powder Uncoated','" + item.Rows[0]["ITEMID"].ToString() + "','" + r + "','" + r + "','0','0','" + cp.jodetid + "','" + totdrums + "','0','0') RETURNING EDCDETAILID INTO :stkid";
                                    //                command.Parameters.Add("stkid", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                    //                command.ExecuteNonQuery();
                                    //                string did = command.Parameters["stkid"].Value.ToString();

                                    //                command.Parameters.Clear();

                                    //                if (!string.IsNullOrEmpty(cp.DrumIds))
                                    //                {
                                    //                    int n = 1;
                                    //                    foreach (string drumid in drumidlist)
                                    //                    {
                                    //                        long plstockvalue = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='plstockvalue'");
                                    //                        DataTable dt = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE as LOTRATE,ps.PLUSQTY,ps.DRUMNO,ps.RATE,ps.LOTNO,ps.STOCKVALUE,PL.Amount FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.plstockvalueID='" + drumid + "'");
                                    //                        // command.CommandText = "Insert into plstockvalue (T1SOURCEID,DOCID,DOCDATE,LOTNO,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,STOCKTRANSTYPE,APPROVAL,MAXAPPROVED,CANCEL,PLUSQTY,FROMLOCID) VALUES ('" + Pid + "','" + cy.InvNo + "','" + cy.InvDate + "','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + dt.Rows[0]["STOCKVALUE"].ToString() + "','" + cp.saveItemId + "','" + cy.Location + "','Ex.Invoice','0','0','F','0','0')";
                                    //                        //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                    //                        // command.ExecuteNonQuery();

                                    //                        //DataTable dt2 = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.PLSTOCKVALUEID='"+ plstockvalue + "'");
                                    //                        double amt = Convert.ToDouble(dt.Rows[0]["LOTRATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                    //                        double amt12 = Convert.ToDouble(dt.Rows[0]["RATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                    //                        command.CommandText = "Insert into EDCLOT (EDCBASICID,PARENTRECORDID,EDCLOTROW,DRUMNO,LSTOCK,LITEMID,PWT,BWT,LOTNO,LOTSTOCK,LQTY,LRATE,LAMOUNT,PARENTROW,LLOCID,LDNO,LBIN,LDOCDATE,LDOCID) VALUES ('" + Pid + "','" + did + "','" + n + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + cp.saveItemId + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','0','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + amt + "','" + r + "','" + cy.Location + "','1','0','" + cy.InvDate + "','" + cy.InvNo + "')";
                                    //                        //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                    //                        command.ExecuteNonQuery();

                                    //                        //datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (plstockvalue + 1).ToString() + "' where NAME='plstockvalue'");
                                    //                        n = n + 1;
                                    //                    }


                                    //                }
                                    //            }


                                    //            r++;
                                    //        }
                                    //    }

                                    //}

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

    }
}
