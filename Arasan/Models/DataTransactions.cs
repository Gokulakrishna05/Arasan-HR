using System.Data;
using Oracle.ManagedDataAccess.Client;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Arasan.Interface;
using Arasan.Models;
using Org.BouncyCastle.Crypto.Macs;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Arasan.Models
{
    public class DataTransactions
    {
        private readonly string _connectionString;
        public DataTransactions(string connectionString)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            _connectionString = connectionString;// _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }

      
        public int GetFinancialYear(DateTime Date1)
        {
            if (Date1.Year > 2000)
            {
                if (Date1.Month > 3)
                {
                    return new DateTime(Date1.Year, 4, 1).Year;
                }
                else
                {
                    return new DateTime(Date1.Year - 1, 4, 1).Year;
                }
            }
            return Date1.Year;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public long GetDataIdlong(String sql)
        {
            DataTable _dt = new DataTable();
            long Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt64(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS='ACTIVE'   order by BRANCHMASTID asc ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetRawItem()
        {
            string SvSql = string.Empty;
            //SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBGROUPCODE='" + value + "'";
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER where ACTIVE='Y' AND ITEMGROUP NOT IN (10044000014235,10044000011689)";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetconfigItem(string ConId)
        {
            string SvSql = string.Empty;
            SvSql = "select * from ADCOMPD where ADCOMPHID='" + ConId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNconfig()
        {
            string SvSql = string.Empty;
            SvSql = "select D.ADTYPE,D.ADNAME,D.ADACCOUNT,H.ADCOMPHID from ADCOMPH H ,ADCOMPD D where H.ADCOMPHID=D.ADCOMPHID AND H.ADSCHEME='GRN' AND H.IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequence(string vtype, string locid)
        {
            string SvSql = string.Empty;
            //SvSql = "select PREFIX,LASTNO from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            SvSql = " select PREFIX, LASTNO, PREFIX || '' || LASTNO  as doc from sequence where TRANSTYPE = 'Deinv' and Locid = '" + locid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable LedgerList()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER Where IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequence(string vtype)
        {
            string SvSql = string.Empty;
            SvSql = "select PREFIX,LASTNO+1 as last from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequence(string vtype, string locid, string ordtype)
        {
            string SvSql = string.Empty;
            //SvSql = "select PREFIX,LASTNO from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            SvSql = " select s.PREFIX, s.LASTNO, s.PREFIX || s.LASTNO  as doc from sequence s,locdetails l where s.TRANSTYPE = 'Deinv' and l.locdetailsid=s.locid  and l.locdetailsid = '" + locid + "' and s.optionname='" + ordtype + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequences(string vtype, string locid)
        {
            string SvSql = string.Empty;
            //SvSql = "select PREFIX,LASTNO from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            SvSql = " select s.PREFIX, s.LASTNO, s.PREFIX || s.LASTNO  as doc from sequence s,locdetails l where s.TRANSTYPE = 'dp' and l.locdetailsid=s.locid  and l.locdetailsid = '" + locid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequence1(string vtype, string locid)
        {
            string SvSql = string.Empty;
            //SvSql = "select PREFIX,LASTNO from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            SvSql = " select PREFIX, LASTNO, PREFIX || '' || LASTNO  as doc from sequence where TRANSTYPE = 'dp' and Locid = '" + locid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "select DISPLAY_NAME, LEDGERID from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBinMaster()
        {
            string SvSql = string.Empty;
            SvSql = "select * from BINBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMachine()
        {
            string SvSql = string.Empty;
            SvSql = "Select MNAME,MCODE,MACHINEINFOBASICID from MACHINEINFOBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPyroLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS where LOCATIONTYPE='BALL MILL' order by LOCDETAILSID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetcuringSet()
        {
            string SvSql = string.Empty;
            SvSql = "select * from BINBASIC WHERE ACTIVE='Y' AND ISCURINGSET='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmailConfig()
        {
            string SvSql = "Select EMAILCONFIG_ID,SMTP_HOST,PORT_NO,SIGNATURE,SSL,EMAIL_ID,PASSWORD from EMAIL_CONFIG where STATUS = 'ACTIVE'";
            DataTable dtCity = new DataTable();
            dtCity = GetData(SvSql);
            return dtCity;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCurency()
        {
            string SvSql = string.Empty;
            SvSql = "Select MAINCURR || ' - ' || SYMBOL  as Cur,CURRENCYID from CURRENCY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemSubGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select SGCODE,ITEMSUBGROUPID FROM ITEMSUBGROUP";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdLog()
        {
            string SvSql = string.Empty;
            SvSql = "select LPRODBASICID,DOCID from LPRODBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdLogsearch(string term)
        {
            string SvSql = string.Empty;
            SvSql = "select LPRODBASICID,DOCID from LPRODBASIC WHERE DOCID like '%"+ term + "%'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdSch()
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,DOCID from PSBASIC   WHERE PSCHSTATUS='ACTIVE'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPLot(string procid, string wcid)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCLOTNO,PROCLOTID from PROCLOT where PROCESSID='" + procid + "' AND WCID='"+ wcid + "' AND STATUS=1";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable BindProcess()
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSID,PROCESSMASTID from PROCESSMAST WHERE BATCHYN='N'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatch()
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,DOCID from BCPRODBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSchedule()
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,DOCID from PSBASIC WHERE PSCHSTATUS='ACTIVE' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            if(!string.IsNullOrEmpty(value) && value != "0")
            {
                SvSql += " Where SUBGROUPCODE='" + value + "'";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem( )
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
           
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVType()
        {
            string SvSql = string.Empty;
            SvSql = "select VCHTYPEID,DESCRIPTION from VCHTYPE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,VALMETHDES,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG,BINBASIC.BINID,ITEMMASTER.LOTYN,ITEMMASTER.VALMETHOD from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID LEFT OUTER JOIN BINBASIC ON BINBASICID=ITEMMASTER.BINNO Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPE,ACCOUNTCODE from ACCTYPE where ACCOUNTTYPEID='"+ id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCGROUP.GROUPCODE,ACCTYPE.ACCOUNTTYPE,ACCTYPE.ACCOUNTCODE from ACCGROUP LEFT OUTER JOIN ACCTYPE on ACCTYPE.ACCOUNTTYPEID=ACCGROUP.ACCTYPE  Where ACCGROUP.STATUS='Active' AND ACCGROUPID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCGROUP.GROUPCODE,ACCTYPE.ACCOUNTTYPE,ACCTYPE.ACCOUNTCODE,LEDGER.LEDNAME,LEDGER.DISPLAY_NAME,LEDGER.CATEGORY from LEDGER LEFT OUTER JOIN ACCGROUP ON LEDGER.ACCGROUP=ACCGROUP.ACCGROUPID LEFT OUTER JOIN ACCTYPE on ACCTYPE.ACCOUNTTYPEID=ACCGROUP.ACCTYPE  Where ACCGROUP.STATUS='Active' AND LEDGERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string getebcost()
        {
            string SvSql = string.Empty;
            string ebcost = string.Empty;
            SvSql = @"Select round(sum(amount)/sum(blunits),2) ebc from (
Select Det,Sum(blunits) blunits,Sum(amount) amount From (
Select 'EB Bill Units' det, Sum(d.EBUNITS) blunits, ROUND(SUM(d.ETAX+d.OM+d.METFWIND+d.PEAKPENALITY+d.RKV+d.EXTMD
+d.PENADDEDDMD+d.EXCESSUNITC+d.METRENT+d.DMDCHGS+d.INDUSCONSC+d.PEAKC-d.NHRREB+d.STAX+d.SOC+d.WHLCHGS+d.TRCHGS+d.BADJCHAGS),0) Amount
from EBBlbasic E,EbBlGendet D,EmasterBasic M where E.EBBLBASICID=D.EBBLBASICID and M.EMASTERBASICID=E.COMPANYID and M.COMPID='The Arasan Aluminium Industries Pvt Ltd' and E.DOCDATE= (Select max(eddt) from EBBLBASIC)
Union ALl
Select det,Sum(tot) tot,round(sum(tot*5.8),0) amount from (
SELECT 06 ord,'Wind Adj Units' det,SUM(peak) peak,SUM(norm) norm,SUM(offp) offp,SUM(peak+norm+offp) tot,sum(((peak*pk)+((peak+norm+offp)*uc))-((offp*uc*nh)/100)) cost FROM (
SELECT SUM(DECODE(SIGN(C2-WC2),-1,DECODE(SIGN(C1-WC1-(WC2-C2)),1,WC1+(WC2-C2),C1),DECODE(SIGN(c1),1,DECODE(SIGN(c1-wc1),1,wc1,c1),0))) c1,
SUM(DECODE(SIGN(C1-WC1),-1,DECODE(SIGN(C2-WC2-(WC1-C1)),1,WC2+(WC1-C1),C2),DECODE(SIGN(c2),1,DECODE(SIGN(c2-wc2),1,wc2,c2),0))) c2,
SUM(DECODE(SIGN(C1-WC1),-1,DECODE(SIGN(C3-WC3-(WC1-C1)),1,WC3+(WC1-C1),C3),DECODE(SIGN(C3),1,DECODE(SIGN(C3-wC3),1,wC3,C3),0))) C3,
SUM(DECODE(SIGN(C3-WC3),-1,DECODE(SIGN(C4-WC4-(WC3-C3)),1,WC4+(WC3-C3),C4),DECODE(SIGN(C4),1,DECODE(SIGN(C4-wC4),1,wC4,C4),0))) C4,
SUM(DECODE(SIGN(c5),1,DECODE(SIGN(c5-wc5),1,wc5,c5),0)) c5,ROUND(SUM(DECODE(SIGN(c1+c2),1,DECODE(SIGN((c1+c2)-(wc1+wc2)),1,wc1+wc2,c1+c2),0)),0) Peak,ROUND(SUM(DECODE(SIGN(c3+c4),1,DECODE(SIGN(c3+c4-wc4-wc3),1,wc3+wc4,c3+c4),0)),0) Norm,
ROUND(SUM(DECODE(SIGN(c5),1,DECODE(SIGN(c5-wc5),1,wc5,c5),0)),0) offp,ROUND(SUM(DECODE(SIGN(tot),1,DECODE(SIGN(tot-wtot),1,wtot,tot),0)),0) tot,max(uc) uc,max(peak) pk,max(nh) nh FROM (
SELECT SUM(c1) c1,SUM(c2) c2,SUM(c3) c3,SUM(c4) c4,SUM(c5) c5,SUM(tot) TOT,SUM(Wc1) Wc1,SUM(Wc2) Wc2,SUM(Wc3) Wc3,SUM(Wc4) Wc4,SUM(Wc5) Wc5,SUM(Wtot) WTOT,max(uc) uc,max(peak) peak,max(nh) nh FROM (
SELECT Decode(sign(SUM(c1)),1,SUM(c1),0) c1,Decode(sign(SUM(c2)),1,SUM(c2),0) c2,Decode(sign(SUM(c3)),1,SUM(c3),0) c3,Decode(sign(SUM(c4)),1,SUM(c4),0) c4,Decode(sign(SUM(c5)),1,SUM(c5),0) c5,SUM(tot) tot,0 WC1,0 WC2,0 WC3,0 WC4,0 WC5,0 WTOT,max(uc) uc,max(peak) peak,max(nh) nh FROM (
SELECT SUM(DECODE(PM,'p',c1,-c1)) c1,SUM(DECODE(PM,'p',c2,-c2)) c2,SUM(DECODE(PM,'p',c3,-c3)) c3,SUM(DECODE(PM,'p',c4,-c4)) c4,SUM(DECODE(PM,'p',c5,-c5)) c5,SUM(DECODE(PM,'p',tot,-tot)) tot,max(uc) uc,max(peak) peak,max(nh) nh FROM (
SELECT 'p' pm, SUM(C.CONSC1) C1,SUM(C.CONSC2) C2,SUM(C.CONSC3) C3,SUM(C.CONSC4) C4,SUM(C.CONSC5) C5,SUM(C.CONSTOT) TOT,max(M.WUP) uc,0 peak,0 nh
FROM EBBLBASIC E,EBBLCONSDET C,EMasterBasic M WHERE E.EBBLBASICID=C.EBBLBASICID AND E.DOCDATE=(Select max(eddt) from EBBLBASIC) AND E.COMPANYID=M.EmasterBasicID AND M.CompID='The Arasan Aluminium Industries Pvt Ltd' AND CONSTYPE='EB Units'
UNION ALL
SELECT 'm' PM, SUM(t.TC1) C1,SUM(t.tc2) C2,SUM(t.tc3) C3,SUM(t.tc4) C4,SUM(t.tc5) C5,SUM(t.TTOTAL) TOT,0 uc,0 peak,0 nh FROM EBBLBASIC E,EBBLthirdDET t,EMasterBasic M WHERE E.EBBLBASICID=t.EBBLBASICID AND E.DOCDATE=(Select max(eddt) from EBBLBASIC) AND E.COMPANYID=M.EmasterBasicID AND M.CompID='The Arasan Aluminium Industries Pvt Ltd'
))
UNION ALL
SELECT 0 C1,0 C2, 0 C3,0 C4,0 C5,0 TOT,SUM(C1) wC1,SUM(C2) wC2,SUM(C3) wC3,SUM(C4) wC4,SUM(C5) wC5,SUM(C1+C2+C3+C4+C5) wTOT,0 uc,0 peak,0 nh FROM (
SELECT CID,WID,PM,ROUND(SUM(C1-(C1*WLINEL/100)),2) C1,ROUND(SUM(C2-(C2*WLINEL/100)),2) C2,ROUND(SUM(C3-(C3*WLINEL/100)),2) C3,ROUND(SUM(C4-(C4*WLINEL/100)),2) C4
,ROUND(SUM(C5-(C5*WLINEL/100)),2) C5,bid,MAX(stax) stax FROM (
SELECT m1.EMASTERBASICID CID, M.WACCNAME WID,'W' PM, SUM(DECODE(W.WINDTYPE,'Export',WINDC1,-WINDC1)) C1,SUM(DECODE(W.WINDTYPE,'Export',WINDC2,-WINDC2)) C2
,SUM(DECODE(W.WINDTYPE,'Export',WINDC3,-WINDC3)) C3,SUM(DECODE(W.WINDTYPE,'Export',WINDC4,-WINDC4)) C4,SUM(DECODE(W.WINDTYPE,'Export',WINDC5,-WINDC5)) C5,M.WLINEL,b.EBBLBASICID bid,MAX(m.STAX) stax
FROM EBBLWINDDET W,WINDMaster M,EBBLBASIC B,EMasterBasic M1 WHERE M.WAccName=W.WINDID AND B.EBBLBASICID=W.EBBLBASICID AND m1.EMASTERBASICID=B.COMPANYID AND M1.COMPID='The Arasan Aluminium Industries Pvt Ltd' AND B.DOCDATE=(Select max(eddt) from EBBLBASIC)  GROUP BY M.WACCNAME,M.WLINEL, b.EBBLBASICID,m1.EMASTERBASICID
)GROUP BY WID,PM,bid,CID
)GROUP BY BID,CID
Union All
Select 0,0,0,0,0,0,-S.SC1,-S.SC2,-S.SC3,-S.SC4,-S.SC5,S.ST,0,0,0 from EBBLBASIC B,EBBlWindS S,WINDMaster W,EMasterBasic M,EMasterBasic M1 Where B.EBBLBASICID=S.EBBLBASICID AND M1.COMPID='The Arasan Aluminium Industries Pvt Ltd' AND B.DOCDATE=(Select max(eddt) from EBBLBASIC) and W.WACCNAME=S.WINDNAME
and M1.EMASTERBASICID=B.COMPANYID
and M.EMASTERBASICID=S.COMPNAME
))))group by det
Union ALl
Select Det,sum(tot) tot,sum(((peak*pk)+((peak+norm+offp)*uc))-((offp*uc*nh)/100)+(tot*badj)) cost From (
Select 08 ord,'Banking Adj Units' det, SUM(peak/2) C1,SUM(peak/2) C2,SUM(norm*0.1) C3,SUM(norm*0.9) C4,SUM(offp) C5,SUM(peak) peak,
SUM(norm) norm,SUM(offp) offp,SUM(peak+norm+offp) TOT,max(uc) uc,max(pk) pk,max(nh) nh,sum(badj) badj From (
select sum(peak) peak,sum(norm) norm,sum(offp) offp,max(uc) uc,max(pk) pk,max(nh) nh,sum(badj) badj from (
Select Decode(slot,'Peak',sum(wadj),0) peak,Decode(slot,'Offpeak',sum(wadj),0) Offp,Decode(slot,'Normal',sum(wadj),0) norm,max(uc) uc,max(peak) pk,max(nh) nh,sum(badj) badj From (
Select B.SLOT,B.WADJ,sum(M.WUP) uc,0 peak,0 nh,0 badj From EbBlBasic E, EBBilWindB B, EMasterBasic M
Where E.EBBLBASICID=B.EBBLBASICID and E.COMPANYID=m.EMASTERBASICID and M.COMPID='The Arasan Aluminium Industries Pvt Ltd' and E.DOCDATE=(Select max(eddt) from EBBLBASIC) Group by B.SLOT,B.WADJ
) Group by slot)))Group by det
Union All
Select Det,Sum(tot) tot,Sum(uc*tot) uc from (
SELECT 02 ord, 'Third Party Adjustment' det, SUM(t.TC1) C1,SUM(t.tc2) C2,SUM(t.tc3) C3,SUM(t.tc4) C4,SUM(t.tc5) C5
,SUM(t.tc1+t.tc2) peak,SUM(t.tc3+t.tc4) norm,SUM(t.tc5) offp, SUM(t.TTOTAL) TOT,Sum(t.TUNITC) uc 
FROM EBBLBASIC E,EBBLthirdDET t,EmasterBasic M WHERE E.EBBLBASICID=t.EBBLBASICID 
AND E.DOCDATE=(Select max(eddt) from EBBLBASIC) AND E.COMPANYID=M.EmasterBasicID AND M.CompID='The Arasan Aluminium Industries Pvt Ltd'
)group by det
)group by det Having sum(amount)>0
Union All
Select 'DG Set Cons' Det,Sum(unit) blunits,sum(amt) amount from (
SELECT SUM(G.gUnitCons) unit,0 amt
FROM PwrConsBasic B , PwrConsGenset G , WcBasic W
WHERE B.WcID = W.WcBasicID
AND B.PwrConsBasicID = G.PwrConsBasicID
AND B.DocDate between (Select max(stdt) from EBBLBASIC) and (Select max(eddt) from EBBLBASIC)
UNION ALL
SELECT 0 unit,SUM(g.FAMOUNT) amt
FROM PwrConsBasic B , PwrConsFuel G , WcBasic W,itemmaster i
WHERE B.WcID = W.WcBasicID
AND i.ITEMMASTERID=g.ITEMID
AND B.PwrConsBasicID = G.PwrConsBasicID
AND B.DocDate  between (Select max(stdt) from EBBLBASIC) and (Select max(eddt) from EBBLBASIC))
UNION ALL
Select 'ENERGY LOSS',0, Sum(amt) amt from (
SELECT SUM(-amt) amt from (
SELECT SUM(D.UNITCONS*EM.UNITCN) amt FROM EBCONSBASIC E,EBCONSDETAIL D,ENRMAST M,EMasterBasic EM 
WHERE E.EBCONSBASICID=D.EBCONSBASICID AND D.METERID=M.ENRMASTID AND E.DOCDATE Between (Select max(stdt) from EBBLBASIC) and (Select max(eddt) from EBBLBASIC)
and upper(m.meteid) not in ('LIGHTING','NEW HEAD OFFICE','FEEDER-1')  AND eM.CompID='The Arasan Aluminium Industries Pvt Ltd') 
Union All
Select Sum(unit) unit from (
Select 'EB Main Meter Reading' ID,Sum(E.EUNITCONS*em.UNITCN) Unit From PwrConsBasic B,WcBAsic W,PwrConsEB E,EMasterBasic EM  
Where B.PWRCONSBASICID=E.PWRCONSBASICID ANd B.WCID=W.WCBASICID And W.WCID='EB SUB SATION' 
ANd B.DOCDATE Between (Select max(stdt) from EBBLBASIC) and (Select max(eddt) from EBBLBASIC) AND eM.CompID='The Arasan Aluminium Industries Pvt Ltd'
Union ALl
Select 'Genset Reading' ID,Sum(E.GUNITCONS*EM.UNITCN) Unit From PwrConsBasic B,WcBAsic W,PwrConsGenset E,Locdetails LD,EMasterBasic EM  
Where B.PWRCONSBASICID=E.PWRCONSBASICID ANd B.WCID=W.WCBASICID And W.ILOCATION=LD.LOCDETAILSID And LD.LOCATIONTYPE='GENSET' 
ANd B.DOCDATE Between (Select max(stdt) from EBBLBASIC) 
and (Select max(eddt) from EBBLBASIC) AND eM.CompID='The Arasan Aluminium Industries Pvt Ltd')))";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            if(dtt.Rows.Count > 0)
            {
                ebcost = dtt.Rows[0]["EBC"].ToString();
            }
            return ebcost;
        }
        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCBASICID,WCID from WCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetDataString(String sql)
        {
            DataTable _dt = new DataTable();
            string str = string.Empty;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    str = _dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        public void sendemail(string Subject, string Message, string EmailID, string SenderID, string SenderPassword, string CompanySMTPPort, string SmtpEnableSsl, string sSmtpServer, string CompanyName)
        {
            string strerr = "";
            if (EmailID != "" && EmailID != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(sSmtpServer);
                    mail.From = new MailAddress(SenderID, CompanyName);
                    mail.To.Add(EmailID);
                    mail.Subject = Subject;
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append(Message);
                    mail.Body = sb3.ToString();
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(sb3.ToString(), null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(avHtml);
                    mail.IsBodyHtml = true;
                   
                    SmtpServer.Port = Convert.ToInt32(CompanySMTPPort);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(SenderID, SenderPassword);
                    SmtpServer.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
                    //SmtpServer.Timeout = 10000;
                    SmtpServer.Send(mail);
                    mail.Dispose();
                }
                catch (Exception ex)
                {
                    strerr = ex.Message;
                }
            }
        }
        public void sendemailpo(string Subject, string Message, string EmailID, string SenderID, string SenderPassword, string CompanySMTPPort, string SmtpEnableSsl, string sSmtpServer, string CompanyName)
        {
            string strerr = "";
            if (EmailID != "" && EmailID != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(sSmtpServer);
                    mail.From = new MailAddress(SenderID, CompanyName);
                    mail.To.Add(EmailID);
                    mail.Subject = Subject;
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append(Message);
                    mail.Body = sb3.ToString();
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(sb3.ToString(), null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(avHtml);
                    mail.IsBodyHtml = true;
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment("C:/Users/ace/Downloads/Basic.pdf");

                    mail.Attachments.Add(attachment);
                    SmtpServer.Port = Convert.ToInt32(CompanySMTPPort);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(SenderID, SenderPassword);
                    SmtpServer.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
                    //SmtpServer.Timeout = 10000;
                    SmtpServer.Send(mail);
                    mail.Dispose();
                }
                catch (Exception ex)
                {
                    strerr = ex.Message;
                }
            }
        }
        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                OracleConnection objConn = new OracleConnection(_connectionString);
                OracleCommand objCmd = new OracleCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }

    }
}
