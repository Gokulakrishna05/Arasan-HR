using Arasan.Interface;

using Arasan.Models;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Dapper;
namespace Arasan.Services
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllSalesInvoice(string status)
        {
            string SvSql = string.Empty;
            SvSql = @"SELECT A.DOCID INVNO,to_char(A.DOCDATE,'dd-MON-yyyy') INVDATE,P.PARTYID,C.MAINCURR,A.NET,A.GROSS,A.BCGST,A.BIGST,A.BSGST, A.EXINVBASICID
FROM EXINVBASIC A,PARTYMAST P,CURRENCY C
WHERE  C.CURRENCYID=A.MAINCURRENCY
AND P.PARTYMASTID=A.PARTYID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetSIDetails(string SIID)
        {
            string SvSql = string.Empty;
            SvSql = @"SELECT A.DOCID INVNO,to_char(A.DOCDATE,'dd-MON-yyyy') INVDATE,P.PARTYID,C.MAINCURR,A.EXRATE,P.ADD1,P.ADD2,P.CITY,P.PINCODE,P.EMAIL,P.PHONENO,P.GSTNO,S.SCODE,A.TDIST,A.VNO,A.TRANSP
,A.NET,A.GROSS,A.BCGST,A.BIGST,A.BSGST,A.ROFF,A.BDISCOUNT,A.BFREIGHT 
FROM EXINVBASIC A,PARTYMAST P,CURRENCY C,STATEDETAILS S
WHERE  C.CURRENCYID=A.MAINCURRENCY
AND P.PARTYMASTID=A.PARTYID AND P.STATE=S.STATENAME AND A.EXINVBASICID='" + SIID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSIITEMDetails(string SIID)
        {
            string SvSql = string.Empty;
            SvSql = @"SELECT B.EXINVDETAILID,I.ITEMID,B.ITEMSPEC,U.UNITID,I.ITEMMASTERID, B.Qty , B.DISCOUNT,B.RATE,B.HSN,B.AMOUNT,B.DRUMDESC,B.CGST,B.SGST,B.IGST,B.CGSTP,B.SGSTP,B.IGSTP,B.TOTAMT
FROM ITEMMASTER I,EXINVDETAIL B,ITEMMASTERPUNIT IP,UNITMAST U
WHERE  U.UNITMASTID = IP.UNIT
AND I.ITEMMASTERID = IP.ITEMMASTERID
AND I.ITEMMASTERID=B.ITEMID
AND IP.UNITTYPE = 'Sales' AND B.EXINVBASICID='" + SIID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesInvoiceDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,INVTYPE,DOCID,to_char(EXINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(EXINVBASIC.REFDATE,'dd-MON-yyyy')REFDATE,PARTYID,VTYPE,CUSTPO,SALVAL,RECDBY,DESPBY,INSPBY,TRANSMODE,VNO,INVDESC,TRANSP,TRANSLIMIT,DOCTHORUGH,RNDOFF,GROSS,NET,AMTWORDS,SERNO,NARRATION FROM EXINVBASIC Where EXINVBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DEPINVDETAIL.QTY,DEPINVDETAIL.DEPINVDETAILID,DEPINVDETAIL.ITEMID,UNITMAST.UNITID,CF,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT  from DEPINVDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEPINVDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=DEPINVDETAIL.UNIT  where DEPINVDETAIL.DEPINVBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getjobdetails(string jobid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT P.PARTYID, A.JOBASICID,L.LOCID FROM JOBASIC A,PARTYMAST P,LOCDETAILS L WHERE P.PARTYMASTID=A.PARTYID AND L.LOCDETAILSID=A.LOCID AND A.JOBASICID='" + jobid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTerms()
        {
            string SvSql = string.Empty;
            SvSql = "select TANDC,TANDCDETAILID from TANDCDETAIL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetFGItem(string locid)
        {
            string SvSql = string.Empty;
            SvSql = "select I.ITEMID,I.ITEMMASTERID,sum(S.Plusqty-S.Minusqty) Qty from ITEMMASTER I,PLSTOCKVALUE S,Locdetails L Where S.ItemID=I.ItemmasterID and S.locid=L.LocdetailsID and S.docdate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' and L.LocdetailsID='" + locid + "' group by I.ITEMID,I.ITEMMASTERID having sum(S.Plusqty-S.Minusqty)>0  UNION select 'Frieght Chrages',1,0 from dual";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetJob()
        {
            string SvSql = string.Empty;
            SvSql = "Select JOBASICID,DOCID From JOBASIC";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string Itemid, string locid)
        {
            string SvSql = string.Empty;
            SvSql = "select S.DRUMNO,SUM(PLUSQTY-MinusQty) QTY,S.lotno,M.rate,M.PLotmastID,M.Amount from plstockvalue s,Plotmast M where M.lotno=S.lotno and S.docdate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' and  S.ITEMID='" + Itemid + "' AND s.LOCID='" + locid + "' group by S.DRUMNO,S.lotno,M.rate,PlotmastID,M.Amount having sum(S.Plusqty-S.Minusqty)>0 order by S.DRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetDrumStock(string Itemid, string locid)
        {
            string SvSql = string.Empty;
            SvSql = "select sum(PLUSQTY-MINUSQTY) as QTY from plstockvalue where ITEMID='" + Itemid + "' AND LOCID='" + locid + "' having sum(PLUSQTY-MINUSQTY)>0";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            string stk = "0";
            if (dtt.Rows.Count > 0)
            {
                stk = dtt.Rows[0]["Qty"].ToString();
            }
            return stk;
        }
        public DataTable GetTname()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMASTID,PARTYID from PARTYMAST Where partycat in ('TRANSPORTER') union all select 1,'Own Vechicle' from dual union all select 2,'Customer Vechicle' from dual order by 2";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetArea(string custid)
        {
            string SvSql = string.Empty;
            SvSql = "select ADDBOOKTYPE,PARTYMASTADDRESSID from PARTYMASTADDRESS where PARTYMASTID='" + custid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAreaDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMASTADDRESSID,ADDBOOKCOMPANY,SPHONE,SFAX,SEMAIL,SSTATE,SCITY,SPINCODE,SADD1,SADD2,SADD3 from PARTYMASTADDRESS  where ADDBOOKTYPE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
        public string DirectPurCRUD(SalesInvoice cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                string Pid = "0";
                //datatrans.GetDataString("select ACCOUNTNAME from PARTYMAST where PARTYMASTID='" + cy.Party + "'");
                DataTable dtacc = new DataTable();
                //DataTable dtParty = datatrans.GetData("select CREDITDAYS,GSTNO,PARTYNAME,ACCOUNTNAME,PANNO from PARTYMAST where PARTYMASTID='" + cy.Party + "'");
                //string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();


                DataTable dtParty = datatrans.GetData("select distinct P.CREDITDAYS,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.ratecode,a.limit,P.PANNO,P.CREDITLIMIT,P.TRANSLMT from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and P.PARTYMASTID='" + cy.Party + "'");
                string mid = dtParty.Rows[0]["ACCOUNTNAME"].ToString();
                cy.partyarc = dtParty.Rows[0]["ratecode"].ToString();
                cy.PartyG = dtParty.Rows[0]["PartyGroup"].ToString();
                cy.limit = dtParty.Rows[0]["limit"].ToString() == "" ? 0 : (long)Convert.ToDouble(dtParty.Rows[0]["limit"].ToString());

                DataTable psaledt = datatrans.GetData("Select nvl(sum(net),0) nets from ( Select sum(net) net from exinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode ='" + cy.partyarc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID = '" + cy.Party + "' or(P.PARTYGROUP ='" + cy.PartyG + "' and 'None' <> '" + cy.PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE = 'ORDER' Union All Select sum(net) net from Depinvbasic e, partymast p,partyadvdisc d where e.docdate between D.SD and D.ED and e.RateCode = '" + cy.partyarc + "' and e.partyid = P.PARTYMASTID and(P.PARTYMASTID ='" + cy.Party + "' or(P.PARTYGROUP = '" + cy.PartyG + "' and 'None' <> '" + cy.PartyG + "')) and D.RATECODE = E.RATECODE and D.PARTYMASTID = P.PARTYMASTID  and e.EORDTYPE = 'ORDER')");
                cy.asale = (long)Convert.ToDouble(psaledt.Rows[0]["nets"].ToString());
                DataTable partybdt = datatrans.GetData("Select nvl(Sum(debit-Credit),0) bal from dailytrans where mid='" + mid + "' and t2vchdt<='" + cy.InvDate + "'");
                double PartyB = (long)Convert.ToDouble(partybdt.Rows[0]["bal"].ToString());
                DateTime duedate = DateTime.Now;
                if (dtParty.Rows[0]["CREDITDAYS"].ToString() != "")
                {
                    int creditdays = dtParty.Rows[0]["CREDITDAYS"].ToString() == "" ? 0 : Convert.ToInt32(dtParty.Rows[0]["CREDITDAYS"].ToString());
                    duedate = DateTime.Now.AddDays(creditdays);
                }

                double totacssamt = Convert.ToDouble(cy.Gross) - Convert.ToDouble(cy.Discount);

                string INvtype = "";
                if (cy.statetype == "GST")
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
                int totdrums = 0;
                int totqty = 0;
                double totdis = 0;
                double totqtydis = 0;
                double totcashdisc = 0;
                if (cy.SIlst != null)
                {
                    foreach (SalesInvoiceItem cp in cy.SIlst)
                    {
                        if (cp.Isvalid == "Y")
                        {
                            if (!string.IsNullOrEmpty(cp.DrumIds))
                            {
                                string drumids = cp.DrumIds;
                                string[] drumidlist = drumids.Split(",");
                                int tot = drumidlist.Length;
                                totdrums += tot;
                                totqty += Convert.ToInt32(cp.Quantity);
                                double totdic = Convert.ToDouble(cp.DiscAmount);
                                totdis += totdic;
                                totqtydis += (totdic * cp.Quantity);
                                totcashdisc += (cp.CashDiscount * cp.Quantity);
                            }

                        }
                    }
                }

                //string party = datatrans.GetDataString("Select ID from PARTYRCODE where PARTY='" + cy.Customer + "' ");
                //string partyid = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYNAME='" + party + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))

                {
                    objConn.Open();

                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            command.Transaction = transaction;

                            try
                            {
                                //////////////////////////////Begin transaction
                                string splrate = cy.limit > 0 ? "Yes" : "No";
                                double totaccamt = cy.Gross + cy.FrightCharge;
                                command.CommandText = "Insert into DEPINVOBASIC (LOCDETAILSID,BRANCHID,INVTYPE,DOCID,DOCDATE,REFNO,REFDATE,PARTYID,VTYPE,CUSTPO,SALVAL,RECDBY,DESPBY,INSPBY,TRANSMODE,VNO,INVDESC,TRANSP,TEMPID,TRANSLIMIT,DOCTHORUGH,RNDOFF,GROSS,NET,AMTWORDS,NARRATION,BSGST,BCGST,BIGST,BFREIGHT,BDISCOUNT,CANCEL,LOCID,MAINCURRENCY,SYMBOL,EXRATE,PARTYMASTID,PARTYNAME,ADSCHEME,CUSTACC,EORDTYPE,CREDITDAYS,GSTNO,TYPE,PANNO,TDIST,BDRUMS,BTOTQTY,RATECODE,ROFF,BQDISCA,BQDISC,PARTYGROUP,SALLIMIT,ASALVAL,TOTACCESAMOUNT,BTOTACCAMT,BTOTAMT,CREDITLIMIT,DUEDATE,CRATECODE,PARTYBALANCE) VALUES ('" + cy.Location + "','" + cy.Branch + "','" + INvtype + "','" + cy.InvNo + "','" + cy.InvDate + "','" + cy.RefNo + "','" + cy.RefDate + "','" + cy.Party + "','R','" + cy.Customer + "','" + cy.Sales + "','" + cy.RecBy + "','" + cy.Dis + "','" + cy.Inspect + "','" + cy.Trans + "','" + cy.Vno + "','" + cy.InvoiceD + "','" + cy.Tname + "','" + cy.Tname + "','" + dtParty.Rows[0]["TRANSLMT"].ToString() + "','" + cy.Doc + "','" + cy.Round + "','" + cy.Gross + "','" + cy.Net + "','" + cy.AinWords + "','" + cy.Narration + "','" + cy.sgst + "','" + cy.cgst + "','" + cy.igst + "','" + cy.FrightCharge + "','" + cy.Discount + "','T','" + cy.Location + "','1','Rs','1','" + cy.Party + "','" + dtParty.Rows[0]["PARTYNAME"].ToString() + "','" + dtacc.Rows[0]["ADCOMPHID"].ToString() + "','" + mid + "','" + cy.Ordsam + "','" + dtParty.Rows[0]["CREDITDAYS"].ToString() + "','" + dtParty.Rows[0]["GSTNO"].ToString() + "','Sales','" + dtParty.Rows[0]["PANNO"].ToString() + "','" + cy.Distance + "','" + totdrums + "','" + totqty + "','" + cy.arc + "','" + cy.Round + "','" + totcashdisc + "','" + totqtydis + "','" + cy.PartyG + "','" + cy.limit + "','" + cy.asale + "','" + totacssamt + "','" + totaccamt + "','" + cy.Gross + "','" + dtParty.Rows[0]["CREDITLIMIT"].ToString() + "','" + duedate.ToString("dd-MMM-yyyy") + "','" + cy.arc + "','" + PartyB + "')  RETURNING DEPINVOBASICID INTO :LASTCID";
                                command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                command.ExecuteNonQuery();
                                Pid = command.Parameters["LASTCID"].Value.ToString();

                                command.Parameters.Clear();


                                if (cy.SIlst != null)
                                {
                                    int rc = 1;
                                    int totrow = cy.SIlst.Count;
                                    foreach (SalesInvoiceItem cp in cy.SIlst)
                                    {
                                        string detailid = "0";
                                        int n = 1;
                                        string UnitId = "1";
                                        double asdqty = 0;
                                        if (cp.ItemId != "1")
                                        {
                                            UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");
                                            asdqty = cp.Quantity;
                                        }
                                        else
                                        {
                                            cp.Unit = "Nos";
                                            asdqty = 0;
                                        }


                                        if (cp.Isvalid == "Y")
                                        {
                                            double acssamt = (cp.rate * cp.Quantity) - Convert.ToDouble(cp.DiscountAmount) - Convert.ToDouble(cp.CashAmount);
                                            double totaltaxamt = Convert.ToDouble(cp.CGST) + Convert.ToDouble(cp.SGST) + Convert.ToDouble(cp.IGST);
                                            double totdisc = (cp.DiscAmount + cp.CashDiscount) * cp.Quantity;
                                            double cdisca = cp.CashDiscount * cp.Quantity;
                                            int totdrumscount = 0;
                                            double Crate1 = 0;
                                            double Camount = 0;
                                            string crate = "0";
                                            string[] drumidlist = null;
                                            double brate = Math.Round(acssamt / cp.Quantity, 2);
                                            if (!string.IsNullOrEmpty(cp.DrumIds))
                                            {
                                                string drumids = cp.DrumIds;
                                                drumidlist = drumids.Split(",");
                                                totdrumscount = drumidlist.Length;
                                                crate = datatrans.GetDataString("SELECT ROUND((SUM(PL.Amount) / SUM(PL.QTY)),2) as CRATE  FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND PL.PLotmastID IN (" + cp.DrumIds + ")");

                                                Crate1 = Convert.ToDouble(crate);
                                                Camount = Crate1 * cp.Quantity;
                                            }
                                            command.CommandText = "Insert into DEPINVODETAIL (DEPINVOBASICID,ITEMID,ITEMTYPE,ITEMSPEC,UNIT,SRATE,BINID,LOTYN,SERIALYN,TOSUBGRID,SUBQTY,EXCISEQTY,PRIUNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT,DRUMDESC,EXCISETYPE,TARIFFID,HSN,Accesamount,baserate,baseamount,unitc,Qdisca,Cdisca,DEPINVDETAILROW,SL,TOTEXAMT,Qdisc,SDNO,CRATE,CAMOUNT,ASDQTY,RCODE,SPLRATE) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.ItemType + "','" + cp.ItemSpec + "','" + UnitId + "','" + cp.rate + "','" + cp.binid + "','YES','NO','" + cp.Quantity + "','" + cp.Quantity + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.Quantity + "','" + cp.rate + "','" + (cp.Quantity * cp.rate) + "','" + totdisc + "','" + cp.IntroDiscount + "','" + cp.CashDiscount + "','" + cp.TradeDiscount + "','" + cp.AddDiscount + "','" + cp.SpecDiscount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.TotalAmount + "','" + cp.Drumsdesc + "','INPUTS','" + cp.tarrifid + "','" + cp.HSN + "','" + acssamt + "','" + brate + "','" + acssamt + "','" + cp.Unit + "','" + cp.DiscountAmount + "','" + cdisca + "','" + rc + "','" + rc + "','" + totaltaxamt + "','" + cp.DiscAmount + "','" + totdrumscount + "','" + crate + "','" + Camount + "','" + asdqty + "','" + cy.arc + "','" + splrate + "') RETURNING DEPINVODETAILID INTO :LASTDID";
                                            command.Parameters.Add("LASTDID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                            command.ExecuteNonQuery();
                                            detailid = command.Parameters["LASTDID"].Value.ToString();

                                            command.Parameters.Clear();
                                            //////////////////////////////////Inventory Update //////////////////////////

                                            //string[] drumidlist = drumids.Split(",");
                                            if (!string.IsNullOrEmpty(cp.DrumIds))
                                            {
                                                foreach (string drumid in drumidlist)
                                                {
                                                    long plstockvalue = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='plstockvalue'");
                                                    DataTable dt = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE as LOTRATE,ps.PLUSQTY,ps.DRUMNO,ps.RATE,ps.LOTNO,PL.Amount FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND PL.PLotmastID='" + drumid + "'");
                                                    //command.CommandText = "Insert into plstockvalue (T1SOURCEID,DOCID,DOCDATE,LOTNO,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,STOCKTRANSTYPE) VALUES ('" + Pid + "','" + cy.InvNo + "','" + cy.InvDate + "','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + cp.CurrentStock + "','" + cp.ItemId + "','" + cy.Location + "','Ex.Invoice')";
                                                    //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                    //command.ExecuteNonQuery();

                                                    //DataTable dt2 = datatrans.GetData("SELECT PL.PLOTMASTID,PL.RATE FROM plotmaSt PL,plstockvalue ps WHERE ps.lotno=pl.lotno AND ps.PLSTOCKVALUEID='"+ plstockvalue + "'");
                                                    double amt = Convert.ToDouble(dt.Rows[0]["LOTRATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                                    double amt12 = Convert.ToDouble(dt.Rows[0]["RATE"].ToString()) * Convert.ToDouble(dt.Rows[0]["PLUSQTY"].ToString());
                                                    command.CommandText = "Insert into DEPINVOLOT (DEPINVOBASICID,DEPINVOLOTROW,DRUMNO,LDNO,LITEMID,LITEMMASTERID,LOTNO,LOTSTOCK,LQTY,LRATE,SLRATE,LAMOUNT,PORL,TLOT,PARENTRECORDID) VALUES ('" + Pid + "','" + n + "','" + dt.Rows[0]["DRUMNO"].ToString() + "','1','" + cp.ItemId + "','" + cp.ItemId + "','" + dt.Rows[0]["PLOTMASTID"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["LOTRATE"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + amt + "','" + amt12 + "','" + dt.Rows[0]["LOTNO"].ToString() + "','" + detailid + "')";
                                                    //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                    command.ExecuteNonQuery();

                                                    //datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (plstockvalue + 1).ToString() + "' where NAME='plstockvalue'");
                                                    n = n + 1;
                                                }
                                                long STOCKVALUE = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='STOCKVALUE'");
                                                command.CommandText = "Insert into STOCKVALUE (STOCKVALUEID,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,MASTERID,LOCID,STOCKTRANSTYPE,BINID) VALUES ('" + STOCKVALUE + "','" + detailid + "','m','" + cp.ItemId + "','" + cy.InvDate + "','" + cp.Quantity + "','" + Camount + "','" + mid + "','" + cy.Location + "','SALES','" + cp.binid + "')";
                                                //OracleCommand objCmddt = new OracleCommand(svSQL, objConn);
                                                command.ExecuteNonQuery();
                                                datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (STOCKVALUE + 1).ToString() + "' where NAME='STOCKVALUE'");

                                                long STOCKVALUE2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='STOCKVALUE2'");
                                                command.CommandText = "Insert into STOCKVALUE2 (STOCKVALUE2ID,STOCKVALUEID,DOCID,RATE,NARRATION) VALUES ('" + STOCKVALUE2 + "','" + STOCKVALUE + "','" + cy.InvNo + "','" + crate + "','" + cy.Narration + "')";
                                                //OracleCommand objCmddts = new OracleCommand(svSQL, objConn);
                                                command.ExecuteNonQuery();
                                                datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (STOCKVALUE2 + 1).ToString() + "' where NAME='STOCKVALUE2'");

                                            }
                                            rc += 1;
                                            //////////////////////////////////Inventory Update //////////////////////////
                                        }

                                    }

                                }


                                if (cy.TermsItemlst != null)
                                {
                                    foreach (TermsItem cp in cy.TermsItemlst)
                                    {
                                        if (cp.Isvalid == "Y" && cp.Terms != "0")
                                        {
                                            command.CommandText = "Insert into DEPINVOTANDC (DEPINVOBASICID,TERMSANDCONDITION) VALUES ('" + Pid + "','" + cp.Terms + "')";
                                            //OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                            command.ExecuteNonQuery();

                                        }
                                    }
                                }


                                if (cy.AreaItemlst != null)
                                {
                                    foreach (AreaItem cp in cy.AreaItemlst)
                                    {
                                        if (cp.Isvalid == "Y" && cp.Areaid != "0")
                                        {
                                            command.CommandText = "Insert into DEPINVOSADD (DEPINVOBASICID,STYPE,SNAME,SADD1,SADD2,SADD3,SSTATE,SCITY,SPINCODE,SPHONE,SFAX,SEMAIL) VALUES ('" + Pid + "','" + cp.Areaid + "','" + cp.Receiver + "','" + cp.Add1 + "','" + cp.Add2 + "','" + cp.Add3 + "','" + cp.State + "','" + cp.City + "','" + cp.PinCode + "','" + cp.Phone + "','" + cp.Fax + "','" + cp.Email + "')";
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }

                                /////////////Count
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
                                if (cy.cgst > 0)
                                {
                                    t2cunt += 1;
                                    totgst = cy.cgst;
                                }
                                if (cy.sgst > 0)
                                {
                                    t2cunt += 1;
                                    totgst += cy.sgst;
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

                                /////////////Count


                                /////////////////////Accounts entry/////////////////////////////

                                long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
                                DataTable dtv = datatrans.GetData("select PREFIX,LASTNO from Sequence where TRANSTYPE='vchsl' and LOCID=('" + cy.Location + "')");
                                string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
                                string mno = DateTime.Now.ToString("yyyyMM");
                                command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID,TOTGST) VALUES ('" + TRANS1 + "','0','" + Pid + "','N','sa','" + vNo + "','1','1','" + cy.InvNo + "','" + cy.InvDate + "','" + mno + "','a','" + mid + "','" + cy.Net + "','" + Grossledger + "','" + cy.Gross + "','" + mid + "','" + cy.Net + "','vchsl','" + cy.Branch + "','" + cy.Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Narration + "','" + t2cunt + "','Y','0','R','T','" + cy.AinWords + "','" + cy.enterdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0','" + totgst + "')";
                                //OracleCommand objCmdA = new OracleCommand(svSQL, objConn);
                                command.ExecuteNonQuery();



                                int vchn = datatrans.GetDataId("select LASTNO from sequence where TRANSTYPE='vchsl' and LOCID=('" + cy.Location + "')");
                                string updateCMds = " UPDATE sequence SET LASTNO ='" + (vchn + 1).ToString() + "' where TRANSTYPE='vchsl' and LOCID=('" + cy.Location + "')";
                                datatrans.UpdateStatus(updateCMds);


                                string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
                                datatrans.UpdateStatus(updatetrans);


                                //////////////////Party Account
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
                                if (cy.cgst > 0)
                                {
                                    long TRANS2C = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2C + "','" + TRANS1 + "','Cr','" + cgstledger + "','0','0','" + cy.cgst + "','" + cy.cgst + "','1','1','0','" + cy.cgst + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                    //OracleCommand objCmdaf = new OracleCommand(svSQL, objConn);
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2C + 1).ToString() + "' where NAME='TRANS2'");
                                }
                                ///////////////////SGST
                                if (cy.sgst > 0)
                                {
                                    long TRANS2S = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2S + "','" + TRANS1 + "','Cr','" + sgstledger + "','0','0','" + cy.sgst + "','" + cy.sgst + "','1','1','0','" + cy.sgst + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','sa','N','T','" + mid + "')";
                                    //OracleCommand objCmdaf = new OracleCommand(svSQL, objConn);
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2S + 1).ToString() + "' where NAME='TRANS2'");
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

                                /////////////////////Accounts entry/////////////////////////////

                                int idc = datatrans.GetDataId("select LASTNO from sequence where TRANSTYPE = 'Deinv' and Locid = '" + cy.Location + "' and optionname='" + cy.Ordsam + "' ");
                                string updateCMd = " UPDATE sequence SET LASTNO ='" + (idc + 1).ToString() + "' where TRANSTYPE = 'Deinv' and Locid = '" + cy.Location + "' and optionname='" + cy.Ordsam + "'";
                                datatrans.UpdateStatus(updateCMd);

                                command.CommandText = "delete from dailytrans where t2vchdt='" + DateTime.Now.ToString("dd-MMM-yyyy") + "'";
                                command.ExecuteNonQuery();


                                command.CommandText = "delete from gstpri where ptype='Deinv'";
                                command.ExecuteNonQuery();

                                command.CommandText = "insert into gstpri(pdocid,ptype) values('" + cy.InvNo + "','Deinv')";
                                command.ExecuteNonQuery();

                                command.CommandText = @"insert into dailytrans 
Select t2.T2VCHDT, t2.MID, t1.VCHSTATUS EMODE, t1.MSTATUS MSTATUS, t1.MONTHNO MONTHNO,
   Sum(t2.DBAMOUNT) DEBIT,Sum(t2.CRAMOUNT) CREDIT, t1.BRANCHID, t1.vtype DTYPE
   FRom trans2 t2,trans1 t1 where t1.TRANS1ID = t2.TRANS1ID and t2.T2VCHDT ='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Group by t1.BRANCHID, t1.vtype,t2.T2VCHDT, t2.MID, t1.VCHSTATUS,t1.MSTATUS,t1.MONTHNO  ";
                                command.ExecuteNonQuery();
                                //////////////////////////////End transaction

                                transaction.Commit();
                            }
                            catch (DataException e)
                            {
                                transaction.Rollback();
                                Console.WriteLine(e.ToString());
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
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
        public string StatusChange(string tag, string id)
        {
            string svSQL = string.Empty;
            using (OracleConnection objConn = new OracleConnection(_connectionString))

            {
                objConn.Open();
                using (OracleCommand command = objConn.CreateCommand())
                {
                    using (OracleTransaction transaction = objConn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        command.Transaction = transaction;

                        try
                        {
                            string docid = datatrans.GetDataString("Select DOCID from DEPINVOBASIC where DEPINVOBASICID='" + id + "'");

                            // command.CommandText = "Delete from plstockvalue WHERE T1SOURCEID in (Select Depinvolotid from Depinvolot where depinvobasicid='" + id + "') AND STOCKTRANSTYPE='Ex.Invoice'";
                            command.CommandText = "Delete from plstockvalue WHERE docid='" + docid + "'";
                            command.ExecuteNonQuery();

                            // long STOCKVALUE = datatrans.GetDataIdlong("select STOCKVALUEID from STOCKVALUE WHERE T1SOURCEID='" + id + "' AND STOCKTRANSTYPE='SALES'");

                            command.CommandText = "Delete from STOCKVALUE WHERE stockvalueid in (select stockvalueid from stockvalue2 where docid='" + docid + "')";
                            command.ExecuteNonQuery();

                            command.CommandText = "Delete from STOCKVALUE2 WHERE docid='" + docid + "'";
                            command.ExecuteNonQuery();


                            command.CommandText = "Delete from DEPINVODETAIL WHERE DEPINVOBASICID='" + id + "'";
                            command.ExecuteNonQuery();

                            command.CommandText = "Delete from DEPINVOLOT WHERE DEPINVOBASICID='" + id + "'";
                            command.ExecuteNonQuery();

                            command.CommandText = "Delete from DEPINVOTANDC WHERE DEPINVOBASICID='" + id + "'";
                            command.ExecuteNonQuery();

                            command.CommandText = "Delete from DEPINVOSADD WHERE DEPINVOBASICID='" + id + "'";
                            command.ExecuteNonQuery();

                            long TRANS1 = datatrans.GetDataIdlong("select TRANS1ID from TRANS1 WHERE T1SOURCEID='" + id + "' AND TRANSID='vchsl'");

                            command.CommandText = "Delete from TRANS2 WHERE TRANS1ID='" + TRANS1 + "'";
                            command.ExecuteNonQuery();

                            command.CommandText = "Delete from TRANS1 WHERE T1SOURCEID='" + id + "' AND TRANSID='vchsl'";
                            command.ExecuteNonQuery();


                            command.CommandText = "Delete from DEPINVOBASIC WHERE DEPINVOBASICID='" + id + "'";
                            command.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            Console.WriteLine(e.ToString());
                            Console.WriteLine("Neither record was written to database.");
                        }
                    }
                }
            }

            return "";

        }
        public DataTable GetHsn(string id)
        {
            string SvSql = string.Empty;
            //  996519 -frieght
            SvSql = "select HSN,ITEMMASTERID from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,QTY,RATE,LOTNO,JODRUMALLOCATIONDETAILID from JODRUMALLOCATIONDETAIL D,JODRUMALLOCATIONBASIC B where B.JODRUMALLOCATIONBASICID=D.JODRUMALLOCATIONBASICID AND B.JOSCHEDULEID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSchedule(string id)
        {
            string SvSql = string.Empty;
            //  996519 -frieght
            SvSql = "Select SCHNO,JOSCHEDULEID from JOSCHEDULE S WHERE  S.JOSCHEDULEID NOT IN (select E.JOSCHEDULEID from EXINVDETAIL E,JOSCHEDULE S WHERE S.JOSCHEDULEID=E.JOSCHEDULEID AND S.JOBASICID='"+ id +"') AND S.JOBASICID='"+ id +"' AND S.IS_ALLOCATE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGSTDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select GSTP from HSNMAST WHERE HSCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTrefficDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSpecDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMTYPE,ITEMSPEC from DEPINVDETAIL WHERE ITEMID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ViewDepot(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  BRANCHMAST.BRANCHID,INVTYPE,DOCID,to_char(EXINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PARTYMAST.PARTYNAME,LOCDETAILS.LOCID,VTYPE,EORDTYPE,SALVAL,RECDBY,DESPBY,INSPBY,DOCTHORUGH,RNDOFF,GROSS,NET,AMTWORDS,SERNO,NARRATION,BSGST,BCGST,BIGST,BFREIGHT,BDISCOUNT ,TRANSMODE,VNO,INVDESC,TRANSP,TRANSLIMIT FROM EXINVBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=EXINVBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID =EXINVBASIC.LOCID LEFT OUTER JOIN  PARTYMAST on EXINVBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where  EXINVBASICID='" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Depotdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = " Select EXINVDETAIL.QTY,EXINVBASICID,EXINVDETAIL.EXINVDETAILID,ITEMMASTER.ITEMID,EXINVDETAIL.PRIUNIT,BINBASIC.BINID,EXINVDETAIL.RATE,EXINVDETAIL.AMOUNT,EXINVDETAIL.DISCOUNT,CDISC,EXINVDETAIL.FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,EXINVDETAIL.TOTAMT,ITEMTYPE,ITEMSPEC  from EXINVDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=EXINVDETAIL.ITEMID LEFT OUTER JOIN BINBASIC ON BINBASIC.BINBASICID=EXINVDETAIL.BINID    where EXINVDETAIL.EXINVBASICID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable TermsDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TANDCDETAIL.TANDC from EXINVTANDC left outer join TANDCDETAIL ON TANDCDETAIL.TANDCDETAILID=EXINVTANDC.TERMSANDCONDITION  WHERE EXINVBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable AreaDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EXINVBASICID,STYPE,SNAME,SADD1,SADD2,SADD3,SSTATE,SCITY,SPINCODE,SPHONE,SFAX,SEMAIL from EXINVSADD WHERE EXINVBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }



        public DataTable GetNarr(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYNAME from PARTYMAST WHERE PARTYMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListSalesInvoiceItems()
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCHMAST.BRANCHID,DEPINVOBASIC.DOCID,PARTYMAST.PARTYNAME,to_char(DEPINVOBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,VTYPE,DEPINVOBASICID,DEPINVOBASIC.NET from DEPINVOBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DEPINVOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DEPINVOBASIC.PARTYID=PARTYMAST.PARTYMASTID order by docdate desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public async Task<IEnumerable<ExinvBasicItem>> GetBasicItem(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvBasicItem>("SELECT EXINVBASICID, VTYPE, DOCID, DOCDATE, REFNO, REFDATE, MAINCURRENCY, SYMBOL, EXRATE,EXINVBASIC. PARTYID, EXINVBASIC.PARTYNAME, RECDBY, DESPBY, INSPBY, DOCTHORUGH, CUSTACC, TEMPID, ADTGMCONTROL, RNDOFF, EXINVBASIC.MOBILE, GROSS, NET, NARRATION, LOCID, EXINVBASIC.CREDITLIMIT, TRANSLIMIT, EXINVBASIC.PARTYBALANCE, EXINVBASIC.CREDITDAYS, DUEDATE, FBED, FCESS, FHSE, INVTYPE, BINYN, PACKRND, OTHCHAR, EORDTYPE, INVDESC, VNO, AMTWORDSBED, BFREIGHT, BDISCOUNT, FTAX, TOTACCESAMOUNT, BDRUMS, BTOTQTY, BTOTAMT, BTOTACCAMT, BEDTRCH, BEDTRAMT, BTOTPORLAMT, SERNO, DEPORECFLAG, FCFLAG, AMTWORDS, MCHELLAN, SCHELLAN, SCHALL2, BEDAMTW, TRANSITLOCID, PDOCID, EXSTATUS, STAXPER, ADSCHEME, SUBDITEMMID, SUBDLOCID, SUBDRATE, LOCDETAILSID, BSGST, BCGST, BIGST, pa.PARTYNAME as TRANSPORT,pa.GSTNO as TRANSPORTGST, TRANSMODE, CUSTPO, ROFF, TCS, BTCS, SALVAL, PARTYMAST.GSTNO, TDIST, TRANSCHGSP,PARTYMAST.ADD1||''||PARTYMAST.ADD2||''||PARTYMAST.ADD3||''||PARTYMAST.CITY||'-'||PARTYMAST.STATE||' '||PARTYMAST.PINCODE as ADDRESS,PARTYMAST.STATE FROM TAAIERP.EXINVBASIC  INNER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=EXINVBASIC.PARTYID LEFT OUTER JOIN PARTYMAST pa ON pa.PARTYMASTID=EXINVBASIC.TRANSP  WHERE EXINVBASIC.EXINVBASICID='" + id+"'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvDetailitem>(" SELECT EXINVDETAILID, EXINVBASICID, EXINVDETAILROW, JOBNO, ITEMMASTER.ITEMID, MATSUPP, UNIT, ORDQTY, OLDEIQTY, EXCISEQTY, EXINVDETAIL.PRIUNIT, QTY, RATE, AMOUNT, BASERATE, BASEAMOUNT, COSTRATE, RETURNEDQTY, JODETAILID, BEDPER, BEDAMT, CESSPER, CESSAMT, SHECESSAMT, AEDPER, AEDAMT, SEDPER, SEDAMT, BCDPER, BCDAMT, TOTAMT, TOTEXAMT, SL, PENDQTY, JOSCHEDULEID, SUBQTY, BINID, CRATE, CAMOUNT, ORDTYPE, TOSUBGRID, ITEMSPEC, ACCESAMOUNT, FREIGHT, QDISC, IDISC, CDISC, TDISC, ADISC, SDISC, FRECH, DISCOUNT, SRATE, SPORL, LLAMOUNT, PORLAMOUNT, SDNO, ASDQTY, DRUMDESC, ITEMTYPE, COMMTYPE, COMMVALUE, COMMAMOUNT, SCHDATE, SUBDCTRL, SGSTP, CGSTP, IGSTP, SGST, CGST, IGST, TCSCTRL, UNITC, EXINVDETAIL.HSN, TCSAMT, TCSAMTR, STKCTRL, SPLRATE, RCODE, FRQ FROM TAAIERP.EXINVDETAIL INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EXINVDETAIL.ITEMID where EXINVDETAIL.EXINVBASICID='" + id + "'", commandType: CommandType.Text);
            }
        }

    }
}
