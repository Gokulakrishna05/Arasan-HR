using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Arasan.Services.Production
{
    public class ProductionForecastingService : IProductionForecastingService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductionForecastingService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<ProductionForecasting> GetAllProductionForecasting()
        {
            List<ProductionForecasting> cmpList = new List<ProductionForecasting>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select BRANCHMAST.BRANCHID,DOCID,PLANTYPE,PRODFCBASICID FROM PRODFCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PRODFCBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductionForecasting cmp = new ProductionForecasting
                        {
                            ID = rdr["PRODFCBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            PType = rdr["PLANTYPE"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetPFDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  DOCID,to_char(PRODFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PLANTYPE,MONTH,INCDECPER,HD,to_char(PRODFCBASIC.FINYRPST,'dd-MON-yyyy')FINYRPST,to_char(PRODFCBASIC.FINYRPED,'dd-MON-yyyy')FINYRPED,ENTEREDBY from PRODFCBASIC    where PRODFCBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProdForecastDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PTYPE,ITEMMASTER.ITEMID,UNITMAST.UNITID,PREVYQTY,PREVMQTY,PQTY from PRODFCDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PRODFCDETAIL.ITEMID LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID=PRODFCDETAIL.UNIT where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastDGPasteDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODFCBASICID,ITEMMASTER.ITEMID,DGITEMID,DGTARQTY,DGMIN,DGSTOCK,REQDG,it.ITEMID as item,DGADDITID,DGADDITREQ,it1.ITEMID as item1,DGRAWMAT,DGREQAP from PROFCDG  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PROFCDG.DGITEMID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PROFCDG.DGADDITID LEFT OUTER JOIN ITEMMASTER it1 on it1.ITEMMASTERID=PROFCDG.DGRAWMAT where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProdForecastPyroDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PYWCID,WCDAYS,ITEMMASTER.ITEMID,PYITEMID,PYMINSTK,PYALLREJ,PYGRCHG,PYREJQTY,PYREQQTY,PYTARQTY, PYPRODCAPD,PYPRODQTY,PYRAWREJMAT,PYRAWREJMATPER,PREBALQTY,it.ITEMID as item,PYADD1,PYADDPER,ALLOCADD,PYREQAP,WSTATUS,POWREQ from PRODFCPY LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PRODFCPY.PYITEMID LEFT OUTER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPY.PYWCID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PRODFCPY.PYADD1 where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastPolishDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PIGWCID,PIGWCDAYS,ITEMMASTER.ITEMID,  it.ITEMID as item,it1.ITEMID as item1 ,PIGCAP,PIGAVAILQTY,PIGMINSTK,PIGRAWREQ,PIGPRODD,PIGADDIT,PIGADDPER,PIGRAWMAT,PIGRAWREQPER,PIGREQQTY,PIGRAWMATPY,PIGRAWREQPY,PIGPOWREQ from PRODFCPIG LEFT OUTER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPIG.PIGWCID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PRODFCPIG.PIGITEMID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PRODFCPIG.PIGADDIT LEFT OUTER JOIN ITEMMASTER it1 on it1.ITEMMASTERID=PRODFCPIG.PIGRAWMAT where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastRVDDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,RVDWCID,ITEMMASTER.ITEMID,RVDITEMID,RVDPRODQTY,RVDCONS,RVDCONSQTY,it.ITEMID as item,RVDRAWMAT,RVDPOWREQ,RVDWCDAYS,RVDMTOREC,RVDMTOLOS from PRODFCRVD LEFT OUTER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCRVD.RVDWCID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PRODFCRVD.RVDITEMID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PRODFCRVD.RVDRAWMAT where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastPasteDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,PAWCID,ITEMMASTER.ITEMID,PAITEMID,PANOOFCHG,PAALLADDIT,PATARGQTY,PASTK,PAMINSTK,PAPROD,PAAPPOW,PABALQTY,RVDLOSTQTY,MIXINGMTO,PACOACONS,PAMTOC,it.ITEMID as item,PAADD1,PAPOWREQ from PRODFCPA LEFT OUTER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCPA.PAWCID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PRODFCPA.PAITEMID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PRODFCPA.PAADD1 where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAPSDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PRODFCBASICID,APPOWREQ,APREQ,APAVAILSTK,APMINSTK,APREQPOW from PRODFCAP    where PRODFCBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAPReqDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PRODFCBASICID,REQAPPOWPY,REQAPPOWPA,REQAPPOWAP,REQAPPOW,APPOWSTOCK,MELTCOAW,REQPOWQTY,APPOWMIN from PRODREQBASIC    where PRODFCBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastAPProdDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,WCBASIC.WCID,APWCID,APWCDAYS,APPRODCAP,APPRODD,APPRODQTY,FUELREQ,RMREQ,APPPOWREQ,APTARPROD from PRODFCAPP LEFT OUTER JOIN WCBASIC on WCBASIC.WCBASICID=PRODFCAPP.APWCID where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastPackDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PARTYMAST.PARTYID,ITEMMASTER.ITEMID,TARITEMID,TARQTY,it.ITEMID as item,PACKMAT,PACKQTY,PACKMATREQ,it1.ITEMID as item1,PACKMATPRI from PROFCPACK LEFT OUTER JOIN PARTYMAST on PARTYMAST.PARTYMASTID=PROFCPACK.PARTYID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PROFCPACK.TARITEMID LEFT OUTER JOIN ITEMMASTER it on it.ITEMMASTERID=PROFCPACK.PACKMAT LEFT OUTER JOIN ITEMMASTER it1 on it1.ITEMMASTERID=PROFCPACK.TARFROM   where PRODFCBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPYROWC()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='BALL MILL' order by 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAPWC()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='AP MILL' order by 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPolishWC()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='POLISH' order by 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetRVDWC()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='RVD' order by 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPasteWC()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='PASTE' order by 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMnth()
        {
            string SvSql = string.Empty;
            SvSql = "select MONTH from SALFCBASIC where IS_ACTIVE='Y' AND FCTYPE='MONTHLY'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDGPaste(string mnth, string type)
        {
            string SvSql = string.Empty;
            SvSql = @"SELECT ITEMID,SUM(stk) stk,SUM(QTY) REQ,SUM(MINSTK) MINSTK,Decode(Sign(SUM(QTY+MINSTK-STK)),1,SUM(QTY+MINSTK-STK),0) ORD FROM (
SELECT ITEMID,SUM(QTY) QTY,SUM(STK) STK,SUM(MINSTK) MINSTK FROM (
SELECT I.ITEMID,0 qty, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,0 MINSTK
FROM StockValue S , ItemMaster I , LocDetails L 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <='"+ DateTime.Now.ToString("dd-MMM-yyyy") + "' AND S.LocID = L.LocdetailsID  ";
            SvSql += @"AND i.SUBCATEGORY IN ('DG PASTE') AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('FG GODOWN') 
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I.ITEMID
UNION ALL
SELECT  I.ITEMID,SUM(SD.QTY) QTY,0 stk,I.MINSTK
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER I
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=I.ITEMMASTERID
AND I.SUBCATEGORY IN ('DG PASTE') 
AND ((sb.MONTH='" + mnth + "' And  SB.FCTYPE='"+ type + "') Or (Sb.FCTYPE='YEARLY' And 'YEARLY'='"+ type + "')) ";
            SvSql += @"GROUP BY I.ITEMID,I.MINSTK,I.ITEMID
)GROUP BY ITEMID)GROUP BY ITEMID
ORDER BY ORD DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public List<PFCPYROItem> GetPyroForecast(string mnth, string type)
        {
            List<PFCPYROItem> cmpList = new List<PFCPYROItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DateTime now = DateTime.Now;
            var sDate = new DateTime(now.Year, now.Month, 1);
            var eDate = sDate.AddMonths(1).AddDays(-1);
            string startDate = sDate.ToString("dd-MMM-yyyy");
            string endDate = eDate.ToString("dd-MMM-yyyy");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT ITEMID,SUM(QTY) REQ,SUM(MINSTK) MINSTK,SUM(stk) stk,SUM(REJ) REJ,Decode(Sign(SUM(QTY+MINSTK-STK)),1,SUM(QTY+MINSTK-STK),0) ORD FROM (
SELECT ITEMID,SUM(QTY) QTY,SUM(STK) STK,SUM(MINSTK) MINSTK,REJ FROM (
SELECT I2.ITEMID,0 qty, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,0 MINSTK,i2.REJRAWMATPER REJ
FROM StockValue S , ItemMaster I , LocDetails L,ITEMMASTER I2 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND i.SUBCATEGORY IN ('PYRO POWDER','PYRO DF','PYRO POLISHED') AND i.QCCOMPFLAG='YES' AND L.LocationType in ('FG GODOWN') 
AND I2.ITEMMASTERID=I.ITEMFROM
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I2.ITEMID,i2.REJRAWMATPER
UNION ALL
SELECT I.ITEMID,0 qty, Decode(L.RCFLAG,0,SUM(S.PLUSQTY-S.MINUSQTY)) stk,0 MINSTK, 0 rejper
FROM LStockValue S , ItemMaster I , LocDetails L,LotMast L 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID And L.LOTNO=S.LOTNO 
AND i.SUBCATEGORY IN ('PYRO POWDER') AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('CURING','MIXING','POLISH','PACKING') And I.ISUBGROUP='SFG'
HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0 
GROUP BY I.ITEMID,i.REJRAWMATPER,L.RCFLAG
Union All
SELECT I2.ITEMID,0 qty, Decode(L.RCFLAG,0,SUM(S.PLUSQTY-S.MINUSQTY)) stk,0 MINSTK, 0 rejper
FROM LStockValue S , ItemMaster I , LocDetails L,LotMast L,Itemmaster I2 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID And L.LOTNO=S.LOTNO And I.ITEMFROM=i2.ITEMMASTERID
AND i2.SUBCATEGORY IN ('PYRO POWDER') AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('CURING','MIXING','POLISH','PACKING') And I.ISUBGROUP='FG-PYRO'
HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0 
GROUP BY I2.ITEMID,i.REJRAWMATPER,L.RCFLAG
UNION ALL
SELECT  I2.ITEMID,SUM(SD.QTY) QTY,0 stk,IM.MINSTK,I2.REJRAWMATPER REJ
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I2
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I2.ITEMMASTERID
AND IM.SUBCATEGORY IN ('PYRO POWDER') 
AND ((sb.MONTH=:MONTH And SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:PlanType))
GROUP BY I2.ITEMID,IM.MINSTK,IM.ITEMID,I2.REJRAWMATPER
UNION ALL
SELECT  I1.ITEMID,SUM(SD.QTY*IM.RAWMATPER/100) QTY,0 stk,IM.MINSTK,I1.REJRAWMATPER REJ 
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I1,ITEMMASTER I2
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I1.ITEMMASTERID
AND I1.ITEMFROM=I2.ITEMMASTERID
AND IM.SUBCATEGORY IN ('PYRO DF','PYRO POLISHED') 
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:PlanType))
GROUP BY I1.ITEMID,IM.MINSTK,I1.REJRAWMATPER
UNION ALL
SELECT ITEMID,ROUND(SUM(QTY*RAWP/100),0) QTY,0 STK,0 MINSTK,SUM(REJ) REJ FROM (
SELECT FG,ITEMID,SUM(QTY+MINSTK-STK) QTY,SUM(REJ) REJ,SUM(RAWP)RAWP FROM (
SELECT IM.ITEMID FG, I1.ITEMID ,SUM(SD.QTY) QTY,0 stk,IM.MINSTK,I1.REJRAWMATPER REJ,IM.RAWMATPER RAWP
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I1
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I1.ITEMMASTERID
AND IM.SUBCATEGORY IN ('DG PASTE') 
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:PlanType))
GROUP BY IM.ITEMID,I1.ITEMID,IM.MINSTK,I1.REJRAWMATPER,IM.RAWMATPER,IM.MINSTK
UNION ALL
SELECT I.ITEMID,I2.ITEMID,0 qty, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,0 MINSTK,i2.REJRAWMATPER REJ,0 RAWM
FROM StockValue S , ItemMaster I , LocDetails L,ITEMMASTER I2 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND i.SUBCATEGORY IN ('DG PASTE') AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('FG GODOWN') 
AND I2.ITEMMASTERID=I.ITEMFROM
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I.ITEMID,I2.ITEMID,i2.REJRAWMATPER
) GROUP BY ITEMID,FG
) GROUP BY ITEMID,RAWP
ORDER BY 2 DESC
)GROUP BY ITEMID,REJ
)GROUP BY ITEMID
ORDER BY ORD DESC";
                    cmd.Parameters.Add("Docdate", Docdate);
                    cmd.Parameters.Add("PlanType", type);
                    cmd.Parameters.Add("MONTH",mnth);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                  
                    while (rdr.Read())
                    {
                        string Additive = datatrans.GetDataString("SELECT   I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1");
                        string Additiveid = datatrans.GetDataString("SELECT   I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1");
                        string Per = datatrans.GetDataString("SELECT add1per FROM ITEMMASTER WHERE ITEMID='" + rdr["ITEMID"].ToString() + "'");

                        PFCPYROItem cmp = new PFCPYROItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            saveitemid = datatrans.GetDataString("SELECT   ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["ITEMID"].ToString() + "'"),
                            
                            required = rdr["REQ"].ToString(),
                           // balanceqty= rdr["REQ"].ToString(),
                            minstock = rdr["MINSTK"].ToString(),
                            stock = rdr["stk"].ToString(),
                            rejqty = rdr["REJ"].ToString(),
                            target = rdr["ORD"].ToString(),
                            additive = Additive,
                            additiveid = Additiveid,
                            per = Per,
                            wstatus="Pending"
                        };
                        //cmp.pasterej = datatrans.GetDataString("Select Decode(sign(3-round(sum(rc/(oq+1)*100),2)),1,round(sum(rc/(oq+1)*100),2),3) rcper from (Select nvl(sum(oq),1) oq,nvl(sum(rc),1) rc from (Select 0 oq,Sum(B.OQTY) rc from FQTVEBAsic B,Itemmaster I Where B.DOCDATE between '"+ startDate + "' and '" + endDate + "' And B.Finalresult='NOT OK' and I.ITEMID='" + cmp.saveitemid + "' and B.RESULTTYPE='RECHARGE' And I.ITEMMASTERID=B.ITEMID Union All Select Sum(D.OQTY) Oq,0 From Nprodbasic B,Nprodoutdet D,Itemmaster I where I.ITEMMASTERID=D.OITEMID and B.NPRODBASICID=D.NPRODBASICID and B.DOCDATE between '" + startDate + "' and '"+ endDate + "' and I.ITEMID='" + cmp.saveitemid + "'))");
                        cmp.pasterej = "0.16";
                        double rem = Convert.ToDouble(cmp.target) + Convert.ToDouble(cmp.minstock) - Convert.ToDouble(cmp.stock);
                        double regqty= Math.Floor(rem * (Convert.ToDouble(cmp.pasterej) / 100));
                        cmp.rejqty= regqty.ToString();
                        double required=Math.Round(rem - regqty);
                        cmp.required = required.ToString();
                        cmp.balanceqty= required.ToString();
                        cmp.rejmat = "2";
                        DataTable rawdt = new DataTable();
                        rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ITEMFROM = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + cmp.saveitemid + "'");

                        if (rawdt.Rows.Count > 0)
                        {
                            cmp.rawmat = rawdt.Rows[0]["ITEMID"].ToString();
                            cmp.rawmatid = rawdt.Rows[0]["ITEMFROM"].ToString();
                           
                        }
                        //cmp.targethrs = datatrans.GetDataString("Select Sum(tar) Tar from (SELECT SUM(WD.PRATE*22) TAR FROM WCBASIC W,WCPRODDETAIL WD,ITEMMASTER I WHERE W.WCBASICID=WD.WCBASICID AND W.WCID=:PYWCID AND I.ITEMMASTERID=WD.ITEMID \r\nAND WD.ITEMTYPE='Primary' AND I.ITEMID=:PYITEMID\r\nUnion All\r\nSELECT SUM(WD.PRATE*22) TAR FROM NMPC.WCBASIC W,NMPC.WCPRODDETAIL WD,ITEMMASTER I WHERE W.WCBASICID=WD.WCBASICID AND W.WCID=:PYWCID AND I.ITEMMASTERID=WD.ITEMID \r\nAND WD.ITEMTYPE='Primary' AND I.ITEMID=:PYITEMID\r\n)");
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public List<PFCPOLIItem> GetPolishForecast(string mnth, string type)
        {
            List<PFCPOLIItem> cmpList = new List<PFCPOLIItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT ITEMID,SUM(QTY) Tar,SUM(MINSTK) MINSTK,SUM(stk) stk,Decode(Sign(SUM(QTY+MINSTK-STK)),1,SUM(QTY+MINSTK-STK),0) ORD FROM (
SELECT ITEMID,SUM(QTY) QTY,SUM(STK) STK,SUM(MINSTK) MINSTK FROM (
SELECT I2.ITEMID,0 qty, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,0 MINSTK
FROM StockValue S , ItemMaster I , LocDetails L,ITEMMASTER I2 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND (I.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED') or I2.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED'))  
AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('FG GODOWN') 
AND I2.ITEMMASTERID=I.ITEMMASTERID
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I2.ITEMID
UNION ALL
SELECT I.ITEMID,0 qty, SUM(DECODE(LM.RCFLAG,0,(S.PLUSQTY-S.MINUSQTY),0)) stk,0 MINSTK
FROM LStockValue S , ItemMaster I , LocDetails L,LotMast LM,ItemMaster I2 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND (I.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED') or I2.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED'))
And I2.ITEMMASTERID=I.ITEMMASTERID
AND Lm.lotno=S.lotno 
AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('CURING') 
HAVING SUM(DECODE(LM.RCFLAG,0,(S.PLUSQTY-S.MINUSQTY),0)) > 0 
GROUP BY I.ITEMID
UNION ALL
SELECT  IM.ITEMID,SUM(SD.QTY) QTY,0 stk,IM.MINSTK
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I2
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I2.ITEMMASTERID
AND IM.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED')   
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:Plantype))
GROUP BY I2.ITEMID,IM.MINSTK,IM.ITEMID
Union All
SELECT  I2.ITEMID,SUM((SD.QTY*IM.RAWMATPER)/100) QTY,0 stk,IM.MINSTK
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I2
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I2.ITEMMASTERID
AND I2.SUBCATEGORY IN ('PIGMENT POWDER','PYRO DF','PYRO POLISHED')  
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:Plantype))
GROUP BY I2.ITEMID,IM.MINSTK
)GROUP BY ITEMID
)GROUP BY ITEMID 
ORDER BY ord desc";
                    cmd.Parameters.Add("Docdate", Docdate);
                    cmd.Parameters.Add("PlanType", type);
                    cmd.Parameters.Add("MONTH", mnth);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        string Per = datatrans.GetDataString("SELECT add1per FROM ITEMMASTER WHERE ITEMID='" + rdr["ITEMID"].ToString() + "'");
                        PFCPOLIItem cmp = new PFCPOLIItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            saveitemid = datatrans.GetDataString("SELECT   ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["ITEMID"].ToString() + "'"),
                            required = rdr["Tar"].ToString(),
                            minstock = rdr["MINSTK"].ToString(),
                            stock = rdr["stk"].ToString(),
                          add= Per,
                            target = rdr["ORD"].ToString(),
                            additive = datatrans.GetDataString("SELECT   I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1"),
                            additiveid = datatrans.GetDataString("SELECT   I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1"),
                            rawmat = datatrans.GetDataString("SELECT I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID=I.ITEMFROM"),
                            rawmatid = datatrans.GetDataString("SELECT I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID=I.ITEMFROM"),
                            reqper="100",
                            rvdqty= rdr["Tar"].ToString(),
                            pyroqty = rdr["Tar"].ToString(),
                            // itemid = rdr["ITEMID"].ToString(),
                            //Branch = rdr["BRANCHID"].ToString(),

                            //InvNo = rdr["DOCID"].ToString(),

                            //InvDate = rdr["DOCDATE"].ToString(),
                            //Party = rdr["PARTYNAME"].ToString(),
                            //Net = Convert.ToDouble(rdr["NET"].ToString()),

                        };
                        if (cmp.required != "0")
                        {
                            DataTable consdt = new DataTable();
                            consdt = datatrans.GetData("SELECT I.ADD1,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ADD1 = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + cmp.rawmatid + "'");
                           
                            if (consdt.Rows.Count > 0)
                            {
                                cmp.consmat = consdt.Rows[0]["ITEMID"].ToString();
                                cmp.consmatid= consdt.Rows[0]["ADD1"].ToString();
                                string per = datatrans.GetDataString("SELECT ADD1PER FROM ITEMMASTER WHERE  ITEMMASTERID= '" + consdt.Rows[0]["ADD1"].ToString() + "'");
                                double consqty = Math.Round(Convert.ToDouble(cmp.required) * Convert.ToDouble(per), 0);
                                cmp.consqty = consqty.ToString();
                            }
                           DataTable rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I,ITEMMASTER I2 WHERE  I.ITEMMASTERID='" + cmp.rawmatid + "' AND I2.ITEMMASTERID=I.ITEMFROM");
                            if(rawdt.Rows.Count > 0)
                            {
                                cmp.rvdrawmatid = rawdt.Rows[0]["ITEMFROM"].ToString();
                                cmp.rvdrawmat = rawdt.Rows[0]["ITEMID"].ToString();
                                string rawper = datatrans.GetDataString("SELECT RAWMATPER FROM ITEMMASTER WHERE  ITEMMASTERID= '" + cmp.rawmatid + "'");
                                double rawqty = Math.Round((Convert.ToDouble(cmp.required) * Convert.ToDouble(rawper))/100, 0);
                                cmp.rvdrawmatqty = rawqty.ToString();
                                double rvdmtor = Math.Round((rawqty * 20) / 100, 0);
                                cmp.rvdmtorec = rvdmtor.ToString();
                                double rvdmtol = Math.Round(rawqty - rvdmtor - Convert.ToDouble(cmp.required), 0);
                                cmp.rvdmtoloss = rvdmtol.ToString();
                            }

                        }


                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public List<PFCPASTEItem> GetPasteForecast(string mnth, string type)
        {
            List<PFCPASTEItem> cmpList = new List<PFCPASTEItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"Select Itemid,Decode(sign(Sum(qty-stk+min)),1,Sum(qty-stk+min) ,0) qty,Sum(qty) tar,Sum(stk) stk,Sum(min) min from (
SELECT I2.ITEMID,SUM(SD.QTY*0.9) QTY,0 stk, 0 min 
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER IM,ITEMMASTER I2
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=IM.ITEMMASTERID
AND IM.ITEMFROM=I2.ITEMMASTERID
AND IM.SUBCATEGORY IN ('NON LEAFING PASTE','LEAFING PASTE') 
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:Plantype))
GROUP BY SB.DOCID,I2.ITEMID
Union ALL
Select I1.itemid,0,Sum(x.stk) stk,Sum(x.minstk) minstk From (
SELECT I.ITEMID,0,SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,I.MINSTK,I.ITEmFrom iid
FROM StockValue S , ItemMaster I , LocDetails L   
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND i.SUBCATEGORY IN ('NON LEAFING PASTE','LEAFING PASTE') AND i.QCCOMPFLAG='YES'
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I.ITEMID,I.MINSTK,I.ITEmFrom
) X,Itemmaster I1 where I1.itemMasterID=X.iid
Group By I1.itemid
Union ALL
SELECT I.ITEMID,0, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,I.MINSTK
FROM StockValue S , ItemMaster I , LocDetails L   
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
AND i.SUBCATEGORY IN ('NON LEAFING CAKE','LEAFING CAKE') AND i.QCCOMPFLAG='YES'
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I.ITEMID,I.MINSTK) Group by itemid 
UNION ALL
Select I.ITEMID,Sum(D.RVDPRODQTY) Qty,0,0,0 from ProdFcBasic B,ProdFcRvd D,Itemmaster I Where B.PRODFCBASICID=D.PRODFCBASICID  And I.ITEMMASTERID=D.RVDRAWMAT Group by I.ITEMID
Order by 2 Desc";
                    cmd.Parameters.Add("Docdate", Docdate);
                    cmd.Parameters.Add("PlanType", type);
                    cmd.Parameters.Add("MONTH", mnth);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PFCPASTEItem cmp = new PFCPASTEItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            saveitemid = datatrans.GetDataString("SELECT   ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["ITEMID"].ToString() + "'"),
                            required = rdr["Tar"].ToString(),
                            minstock = rdr["min"].ToString(),
                            stock = rdr["stk"].ToString(),

                            target = rdr["qty"].ToString(),
                            additive = datatrans.GetDataString("SELECT   I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1"),
                            additiveid = datatrans.GetDataString("SELECT   I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + rdr["ITEMID"].ToString() + "' AND I1.ITEMMASTERID = I.ADD1"),
                            allocadditive= datatrans.GetDataString("SELECT ADD1PER FROM ITEMMASTER WHERE  ITEMID='" + rdr["ITEMID"].ToString() + "'"),
                          
                            // itemid = rdr["ITEMID"].ToString(),
                            //Branch = rdr["BRANCHID"].ToString(),

                            //InvNo = rdr["DOCID"].ToString(),

                            //InvDate = rdr["DOCDATE"].ToString(),
                            //Party = rdr["PARTYNAME"].ToString(),
                            //Net = Convert.ToDouble(rdr["NET"].ToString()),

                        };
                        cmp.paaddpurpri = datatrans.GetDataString("SELECT LATPURPRICE FROM ITEMMASTER WHERE ITEMMASTERID='" + cmp.additiveid + "'");
                        cmp.mtopurpri = datatrans.GetDataString("SELECT LATPURPRICE FROM ITEMMASTER WHERE ITEMID='DISTILLED MINERAL TURPENTINE'");
                        DataTable rawdt = new DataTable();
                        rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ITEMFROM = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + cmp.saveitemid + "'");

                        if (rawdt.Rows.Count > 0)
                        {
                            cmp.rawmat = rawdt.Rows[0]["ITEMID"].ToString();
                            cmp.rawmatid = rawdt.Rows[0]["ITEMFROM"].ToString();

                        }
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public List<PFCPACKItem> GetPackForecast(string mnth, string type)
        {
            List<PFCPACKItem> cmpList = new List<PFCPACKItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT TYPES, PARTYID,ITEM,SUM(TAR) TAR,RAWM,DRUM,PACKID,PACK,QTY,LATPURPRICE,SUM(PKG) PKG FROM (
SELECT '01 LOCAL' TYPES, PARTYID,ITEM,SUM(TAR) TAR,RAWM,DRUM,PACKID,CI.ITEMID PACK,QTY,CI.LATPURPRICE,ROUND(SUM(CI.LATPURPRICE/QTY),2) PKG FROM (
SELECT PARTYID,ITEMID ITEM,RAWM,DRUM,PB.PACKBASICID PACKID,PD.PDRUMQTY QTY,TAR FROM (
SELECT P.PARTYID, I.ITEMID,I2.ITEMID RAWM,MAX(L.DRUMNO) DRUM,SD.QTY TAR
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER I,PARTYMAST P,ITEMMASTER I2,EXINVBASIC E,EXINVLOT L
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND I2.ITEMMASTERID=I.ITEMFROM
AND P.PARTYMASTID=SD.PARTYID
AND SD.ITEMID=I.ITEMMASTERID
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:Plantype))
AND P.PARTYMASTID=E.PARTYID 
AND E.EXINVBASICID=L.EXINVBASICID 
AND I.ITEMMASTERID=L.LITEMID
GROUP BY I.ITEMID,I2.ITEMID,P.PARTYID,SD.QTY) X,PACKBASIC PB,PACKPDETAIL PD
WHERE PB.PACKBASICID=PD.PACKBASICID AND PD.PDRUMNO=X.DRUM
) Y,PACKCONSDETAIL CD,ITEMMASTER CI
WHERE CD.PACKBASICID=Y.PACKID AND CI.ITEMMASTERID=CD.CITEMID
AND CD.CONSQTY=(SELECT MAX(CONSQTY) FROM PACKCONSDETAIL WHERE PACKBASICID=Y.PACKID)
AND CD.CONSRATE=(SELECT MAX(CONSRATE) FROM PACKCONSDETAIL WHERE PACKBASICID=Y.PACKID)
GROUP BY PARTYID,ITEM,RAWM,DRUM,PACKID,CI.ITEMID,QTY,CI.LATPURPRICE
UNION ALL
SELECT'02 EXPORT' TYPES, PARTYID,ITEM,SUM(TAR) TAR,RAWM,DRUM,PACKID,CI.ITEMID PACK,QTY,CI.LATPURPRICE,ROUND(SUM(CI.LATPURPRICE/QTY),2) PKG FROM (
SELECT PARTYID,ITEMID ITEM,RAWM,DRUM,PB.PACKBASICID PACKID,PD.PDRUMQTY QTY,TAR FROM (
SELECT P.PARTYID, I.ITEMID,I2.ITEMID RAWM,MAX(L.DRUMNO) DRUM,SD.QTY TAR
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER I,PARTYMAST P,ITEMMASTER I2,EEXINVBASIC E,EEXINVLOT L
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND I2.ITEMMASTERID=I.ITEMFROM
AND P.PARTYMASTID=SD.PARTYID
AND SD.ITEMID=I.ITEMMASTERID
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:Plantype))
AND P.PARTYMASTID=E.PARTYID 
AND E.EEXINVBASICID=L.EEXINVBASICID 
AND I.ITEMMASTERID=L.LITEMID
GROUP BY I.ITEMID,I2.ITEMID,P.PARTYID,SD.QTY) X,PACKBASIC PB,PACKPDETAIL PD
WHERE PB.PACKBASICID=PD.PACKBASICID AND PD.PDRUMNO=X.DRUM
) Y,PACKCONSDETAIL CD,ITEMMASTER CI
WHERE CD.PACKBASICID=Y.PACKID AND CI.ITEMMASTERID=CD.CITEMID
AND CD.CONSQTY=(SELECT MAX(CONSQTY) FROM PACKCONSDETAIL WHERE PACKBASICID=Y.PACKID)
AND CD.CONSRATE=(SELECT MAX(CONSRATE) FROM PACKCONSDETAIL WHERE PACKBASICID=Y.PACKID)
GROUP BY PARTYID,ITEM,RAWM,DRUM,PACKID,CI.ITEMID,QTY,CI.LATPURPRICE)
GROUP BY PARTYID,ITEM,RAWM,DRUM,PACKID,PACK,QTY,LATPURPRICE,TYPES
ORDER BY 1,2";
                    cmd.Parameters.Add("Docdate", Docdate);
                    cmd.Parameters.Add("PlanType", type);
                    cmd.Parameters.Add("MONTH", mnth);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PFCPACKItem cmp = new PFCPACKItem
                        {
                            targetitem = rdr["item"].ToString(),
                            saveitemid = datatrans.GetDataString("SELECT ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["item"].ToString() + "'"),
                            packmat = rdr["PACK"].ToString(),
                            packmatid = datatrans.GetDataString("SELECT   ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["PACK"].ToString() + "'"),
                            packqty = rdr["QTY"].ToString(),
                            rawmat = rdr["RAWM"].ToString(),
                            rawmatid = datatrans.GetDataString("SELECT   ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["RAWM"].ToString() + "'"),
                            party = rdr["PARTYID"].ToString(),
                            partyid = datatrans.GetDataString("SELECT   PARTYMASTID FROM PARTYMAST WHERE  PARTYID ='" + rdr["PARTYID"].ToString() + "'"),
                            targetqty = rdr["TAR"].ToString(),
                            //reqmat = (Convert.ToDouble(rdr["TAR"].ToString()) - Convert.ToDouble(rdr["RAWM"].ToString())).ToString,
                            // itemid = rdr["ITEMID"].ToString(),
                            //Branch = rdr["BRANCHID"].ToString(),

                            //InvNo = rdr["DOCID"].ToString(),

                            //InvDate = rdr["DOCDATE"].ToString(),
                            //Party = rdr["PARTYNAME"].ToString(),
                            //Net = Convert.ToDouble(rdr["NET"].ToString()),

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public List<ProdApItem> GetAPSForecast(string mnth, string type)
        {
            List<ProdApItem> cmpList = new List<ProdApItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"SELECT ITEMID,SUM(stk) stk,SUM(QTY) REQ,SUM(MINSTK) MINSTK,Decode(Sign(SUM(QTY+MINSTK-STK)),1,SUM(QTY+MINSTK-STK),0) ORD,STARTVALUE,ENDVALUE FROM (
SELECT ITEMID,SUM(QTY) QTY,SUM(STK) STK,SUM(MINSTK) MINSTK,STARTVALUE,ENDVALUE FROM (
SELECT I.ITEMID,0 qty, SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk,0 MINSTK,D.STARTVALUE,D.ENDVALUE
FROM StockValue S , ItemMaster I , LocDetails L,TestTBasic B,TestTDetail D 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <=:Docdate AND S.LocID = L.LocdetailsID 
and I.TEMPLATEID=B.TESTTBASICID and B.TESTTBASICID=D.TESTTBASICID and D.TESTDESC='PAN'
AND I.ISUBGROUP IN ('FG-AP')  AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('FG GODOWN') 
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 
GROUP BY I.ITEMID,D.STARTVALUE,D.ENDVALUE
UNION ALL
SELECT  I.ITEMID,SUM(SD.QTY) QTY,0 stk,I.MINSTK,D.STARTVALUE,D.ENDVALUE
FROM SALFCBASIC SB,SALFCDETAIL SD,ITEMMASTER I,TestTBasic B,TestTDetail D
WHERE SB.SALFCBASICID=SD.SALFCBASICID
AND SD.ITEMID=I.ITEMMASTERID and D.TESTDESC='PAN'
AND I.ISUBGROUP IN ('FG-AP') and I.TEMPLATEID=B.TESTTBASICID and B.TESTTBASICID=D.TESTTBASICID
AND ((sb.MONTH=:MONTH And  SB.FCTYPE=:PlanType) Or (Sb.FCTYPE='YEARLY' And 'YEARLY'=:PlanType))
GROUP BY I.ITEMID,I.MINSTK,I.ITEMID,D.STARTVALUE,D.ENDVALUE
)GROUP BY ITEMID,STARTVALUE,ENDVALUE)GROUP BY ITEMID,STARTVALUE,ENDVALUE
ORDER BY ORD DESC";
                    cmd.Parameters.Add("Docdate", Docdate);
                    cmd.Parameters.Add("PlanType", type);
                    cmd.Parameters.Add("MONTH", mnth);
                    cmd.BindByName = true;
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProdApItem cmp = new ProdApItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            saveitemid = datatrans.GetDataString("SELECT ITEMMASTERID FROM ITEMMASTER WHERE  ITEMID ='" + rdr["ITEMID"].ToString() + "'"),
                            avlstk = rdr["stk"].ToString(),
                            ministk = rdr["MINSTK"].ToString(),
                            reqqty = rdr["REQ"].ToString(),
                            ordqty = rdr["ORD"].ToString(),
                            startvalue = rdr["STARTVALUE"].ToString(),
                            endvalue = rdr["ENDVALUE"].ToString(),
                            reqappowder= rdr["REQ"].ToString(),

                        };
                        DataTable rawdt = new DataTable();
                        rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ITEMFROM = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + cmp.saveitemid + "'");

                        if (rawdt.Rows.Count > 0)
                        {
                            cmp.rawmat = rawdt.Rows[0]["ITEMID"].ToString();
                            cmp.rawmatid = rawdt.Rows[0]["ITEMFROM"].ToString();

                        }
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public List<ProdApReqItem> GetAPReqForecast(string mnth, string type)
        {
            List<ProdApReqItem> cmpList = new List<ProdApReqItem>();
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            
            return cmpList;
        }

        public string ProductionForecastingCRUD(ProductionForecasting cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PFc-' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "PFc-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PFc-' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = docid;
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PRODFCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PRODFCBASICPROC";*/

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
                    objCmd.Parameters.Add("PLANTYPE", OracleDbType.NVarchar2).Value = cy.plantype;
                    objCmd.Parameters.Add("MONTH", OracleDbType.NVarchar2).Value = cy.ForMonth;
                    objCmd.Parameters.Add("INCDECPER", OracleDbType.NVarchar2).Value = cy.Ins;;
                    objCmd.Parameters.Add("HD", OracleDbType.NVarchar2).Value = cy.Hd;
                    objCmd.Parameters.Add("FINYRPST", OracleDbType.NVarchar2).Value = cy.Fordate;
                    objCmd.Parameters.Add("FINYRPED", OracleDbType.NVarchar2).Value = cy.Enddate;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
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
                        //foreach (PFCItem ca in cy.PFCILst)
                        //{
                        //    if (ca.Isvalid == "Y" && ca.PType != null)
                        //    {
                                 
                                     
                        //            OracleCommand objCmds = new OracleCommand("PRODFCDETAILPROC", objConn);
                        //            if (cy.ID == null)
                        //            {
                        //                StatementType = "Insert";
                        //                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                        //            }
                        //            else
                        //            {
                        //                StatementType = "Update";
                        //                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                        //            }
                        //            objCmds.CommandType = CommandType.StoredProcedure;
                        //            objCmds.Parameters.Add("PRODFCDETAILPROC", OracleDbType.NVarchar2).Value = Pid;
                        //            objCmds.Parameters.Add("PTYPE", OracleDbType.NVarchar2).Value = ca.PType;
                        //            objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ca.ItemId;
                        //            objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = ca.Unit;
                        //            objCmds.Parameters.Add("PREVYQTY", OracleDbType.NVarchar2).Value = ca.PysQty;
                        //            objCmds.Parameters.Add("PREVMQTY", OracleDbType.NVarchar2).Value = ca.PtmQty;
                        //            objCmds.Parameters.Add("PQTY", OracleDbType.NVarchar2).Value = ca.Fqty;
                        //            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                        //            objCmds.ExecuteNonQuery();



                        //        }
                        //    }
                            foreach (PFCDGItem cp in cy.PFCDGILst)
                            {
                                if ( cp.saveitemid != null)
                                {

                                    OracleCommand objCmds = new OracleCommand("PROFCDGPROC", objConn);
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
                                    objCmds.Parameters.Add("PRODFCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("DGITEMID", OracleDbType.NVarchar2).Value = cp.saveitemid;
                                    objCmds.Parameters.Add("DGTARQTY", OracleDbType.NVarchar2).Value = cp.target;
                                    objCmds.Parameters.Add("DGMIN", OracleDbType.NVarchar2).Value = cp.min;
                                    objCmds.Parameters.Add("DGSTOCK", OracleDbType.NVarchar2).Value = cp.stock;
                                    objCmds.Parameters.Add("REQDG", OracleDbType.NVarchar2).Value = cp.required;
                                    objCmds.Parameters.Add("DGADDITID", OracleDbType.NVarchar2).Value = cp.dgadditid;
                                    objCmds.Parameters.Add("DGADDITREQ", OracleDbType.NVarchar2).Value = cp.reqadditive;
                                    objCmds.Parameters.Add("DGRAWMAT", OracleDbType.NVarchar2).Value = cp.rawmaterialid;
                                    objCmds.Parameters.Add("DGREQAP", OracleDbType.NVarchar2).Value = cp.reqpyro;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objCmds.ExecuteNonQuery();





                                }
                            }
                            foreach (PFCPYROItem cp in cy.PFCPYROILst)
                            {
                                if ( cp.WorkId != null)
                                {

                                    OracleCommand objCmds = new OracleCommand("PRODFCPYPROC", objConn);
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
                                    objCmds.Parameters.Add("PRODFCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PYWCID", OracleDbType.NVarchar2).Value = cp.WorkId;
                                    objCmds.Parameters.Add("WCDAYS", OracleDbType.NVarchar2).Value = cp.CDays;
                                    objCmds.Parameters.Add("PYITEMID", OracleDbType.NVarchar2).Value = cp.saveitemid;
                                    objCmds.Parameters.Add("PYMINSTK", OracleDbType.NVarchar2).Value = cp.minstock;
                                    objCmds.Parameters.Add("PYALLREJ", OracleDbType.NVarchar2).Value = cp.pasterej;
                                    objCmds.Parameters.Add("PYGRCHG", OracleDbType.NVarchar2).Value = cp.GradeChange;
                                    objCmds.Parameters.Add("PYREJQTY", OracleDbType.NVarchar2).Value = cp.rejqty;
                                    objCmds.Parameters.Add("PYREQQTY", OracleDbType.NVarchar2).Value = cp.required;
                                    objCmds.Parameters.Add("PYTARQTY", OracleDbType.NVarchar2).Value = cp.target;
                                    objCmds.Parameters.Add("PYPRODCAPD", OracleDbType.NVarchar2).Value = cp.proddays;
                                    objCmds.Parameters.Add("PYPRODQTY", OracleDbType.NVarchar2).Value = cp.prodqty;
                                    objCmds.Parameters.Add("PYRAWREJMAT", OracleDbType.NVarchar2).Value = cp.rejmat;
                                    objCmds.Parameters.Add("PYRAWREJMATPER", OracleDbType.NVarchar2).Value = cp.rejmatreq;
                                    objCmds.Parameters.Add("PREBALQTY", OracleDbType.NVarchar2).Value = cp.balanceqty;
                                    objCmds.Parameters.Add("PYADD1", OracleDbType.NVarchar2).Value = cp.additiveid;
                                    objCmds.Parameters.Add("PYADDPER", OracleDbType.NVarchar2).Value = cp.per;
                                    objCmds.Parameters.Add("ALLOCADD", OracleDbType.NVarchar2).Value = cp.allocadditive;
                                    objCmds.Parameters.Add("PYREQAP", OracleDbType.NVarchar2).Value = cp.reqpowder;
                                    objCmds.Parameters.Add("WSTATUS", OracleDbType.NVarchar2).Value = cp.wstatus;
                                    objCmds.Parameters.Add("POWREQ", OracleDbType.NVarchar2).Value = cp.powderrequired;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objCmds.ExecuteNonQuery();





                                }
                            }
                            foreach (PFCPOLIItem cp in cy.PFCPOLILst)
                            {
                                if ( cp.workid != null)
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODFCPIGPROC", objConn);
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
                                    objCmds.Parameters.Add("PRODFCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PIGWCID", OracleDbType.NVarchar2).Value = cp.workid;
                                    objCmds.Parameters.Add("PIGWCDAYS", OracleDbType.NVarchar2).Value = cp.wcdays;
                                    objCmds.Parameters.Add("PIGITEMID", OracleDbType.NVarchar2).Value = cp.saveitemid;
                                    objCmds.Parameters.Add("PYMINSTK", OracleDbType.NVarchar2).Value = cp.target;
                                    objCmds.Parameters.Add("PIGCAP", OracleDbType.NVarchar2).Value = cp.capacity;
                                    objCmds.Parameters.Add("PIGAVAILQTY", OracleDbType.NVarchar2).Value = cp.stock;
                                    objCmds.Parameters.Add("PIGMINSTK", OracleDbType.NVarchar2).Value = cp.minstock;
                                    objCmds.Parameters.Add("PIGRAWREQ", OracleDbType.NVarchar2).Value = cp.required;
                                    objCmds.Parameters.Add("PIGPRODD", OracleDbType.NVarchar2).Value = cp.days;
                                    objCmds.Parameters.Add("PIGADDIT", OracleDbType.NVarchar2).Value = cp.additiveid;
                                    objCmds.Parameters.Add("PIGALLOCADD", OracleDbType.NVarchar2).Value = cp.add;
                                    objCmds.Parameters.Add("PIGRAWMAT", OracleDbType.NVarchar2).Value = cp.rawmatid;
                                    objCmds.Parameters.Add("PIGRAWREQPER", OracleDbType.NVarchar2).Value = cp.reqper;
                                    objCmds.Parameters.Add("PIGREQQTY", OracleDbType.NVarchar2).Value = cp.rvdqty;
                                    objCmds.Parameters.Add("PIGRAWMATPY", OracleDbType.NVarchar2).Value = cp.pyropowder;
                                    objCmds.Parameters.Add("PIGRAWREQPY", OracleDbType.NVarchar2).Value = cp.pyroqty;
                                    objCmds.Parameters.Add("PIGPOWREQ", OracleDbType.NVarchar2).Value = cp.powderrequired;

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                    objCmds.ExecuteNonQuery();





                                }
                            }
                        if (cy.PFCRVDLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PFCRVDItem cp in cy.PFCRVDLst)
                                {
                                    if (cp.workid != null)
                                    {
                                        svSQL = "Insert into PRODFCRVD (PRODFCBASICID,RVDWCID,RVDITEMID,RVDPRODQTY,RVDCONS,RVDCONSQTY,RVDRAWMAT,RVDPOWREQ,RVDWCDAYS,RVDMTOREC,RVDMTOLOS,RVDPRODD,RVDRAWQTY) VALUES ('" + Pid + "','"+ cp.workid+"','" + cp.saveitemid + "','" + cp.prodqty + "','" + cp.consmatid + "','"+cp.consqty+"','" + cp.rawmat + "','" + cp.powderrequired + "','" + cp.wcdays + "','"+cp.mto+"','"+cp.mtoloss+"','"+cp.days+"','"+cp.qty+"')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.PFCPASTELst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PFCPASTEItem cp in cy.PFCPASTELst)
                                {
                                    if (cp.WorkId != null)
                                    {
                                        svSQL = "Insert into PRODFCPA (PRODFCBASICID,PAWCID,PAITEMID,PANOOFCHG,PAALLADDIT,PATARGQTY,PASTK,PAMINSTK,PAPROD,PAAPPOW,PABALQTY,RVDLOSTQTY,MIXINGMTO,PACOACONS,PAMTOC,PAADD1,PAPOWREQ,PAPRODD) VALUES ('" + Pid + "','" + cp.WorkId + "','" + cp.saveitemid + "','" + cp.charge + "','" + cp.allocadditive + "','" + cp.target + "','" + cp.stock + "','" + cp.minstock + "','" + cp.production + "','" + cp.appowder + "','" + cp.balance + "','"+cp.rvdloss+"','"+cp.missmto+"','"+cp.coarse+"','"+cp.mtocost+"','"+cp.additiveid+"','"+cp.powerrequired+"','"+cp.proddays+"')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.ID == null)
                        {

                            svSQL = "Insert into PRODFCAP (PRODFCBASICID,APPOWREQ,APREQ,APAVAILSTK,APMINSTK,APREQPOW) VALUES ('" + Pid + "','" + cy.apspowder + "','" + cy.reqqty + "','" + cy.avlstk + "','" + cy.ministk + "','" + cy.reqappowder + "')";
                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                            objCmds.ExecuteNonQuery();



                        }
                        if (cy.ID == null)
                        {

                            svSQL = "Insert into PRODREQBASIC (PRODFCBASICID,REQAPPOWPY,REQAPPOWPA,REQAPPOWAP,REQAPPOW,APPOWSTOCK,MELTCOAW,REQPOWQTY,APPOWMIN) VALUES ('" + Pid + "','" + cy.appyro + "','" + cy.appaste + "','" + cy.apfg + "','" + cy.reqappow + "','" + cy.apstk + "','"+cy.coarse+"','"+cy.power+"','"+cy.ministk+"')";
                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                            objCmds.ExecuteNonQuery();



                        }
                        if (cy.PFCAPPRODLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PFCAPPRODItem cp in cy.PFCAPPRODLst)
                                {
                                    if (cp.WorkId != null)
                                    {
                                        svSQL = "Insert into PRODFCAPP (PRODFCBASICID,APWCID,APWCDAYS,APPRODCAP,APPRODD,APPRODQTY,FUELREQ,RMREQ,APPPOWREQ,APTARPROD) VALUES ('" + Pid + "','" + cp.WorkId + "','" + cp.wdays + "','" + cp.capacity + "','" + cp.proddays + "','" + cp.production + "','" + cp.fuelreq + "','" + cp.ingotreq + "','" + cp.powerrequired + "','" + cp.target + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.PFCPACKLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PFCPACKItem cp in cy.PFCPACKLst)
                                {
                                    if (cp.partyid != null)
                                    {
                                        svSQL = "Insert into PROFCPACK (PRODFCBASICID,PARTYID,TARITEMID,TARQTY,PACKMAT,PACKQTY,PACKMATREQ,TARFROM) VALUES ('" + Pid + "','" + cp.partyid + "','" + cp.saveitemid + "','" + cp.targetqty + "','" + cp.packmatid + "','" + cp.packqty + "','" + cp.reqmat + "','" + cp.rawmatid + "')";
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

        public DataTable GetAllProdFC(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select  DOCID,PLANTYPE,MONTH,to_char(DOCDATE,'dd-MM-yy')DOCDATE,PRODFCBASICID FROM PRODFCBASIC  WHERE PRODFCBASIC.IS_ACTIVE='Y' ORDER BY  PRODFCBASICID DESC";
            }
            else
            {
                SvSql = "select  DOCID,PLANTYPE,MONTH,to_char(DOCDATE,'dd-MM-yy')DOCDATE,PRODFCBASICID FROM PRODFCBASIC  WHERE PRODFCBASIC.IS_ACTIVE='N' ORDER BY  PRODFCBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
