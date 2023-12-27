using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
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
            SvSql = "select BRANCHID,DOCID,to_char(PRODFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PLANTYPE,MONTH,INCDECPER,HD,to_char(PRODFCBASIC.FINYRPST,'dd-MON-yyyy')FINYRPST,to_char(PRODFCBASIC.FINYRPED,'dd-MON-yyyy')FINYRPED,ENTEREDBY from PRODFCBASIC    where PRODFCBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProdForecastDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PTYPE,ITEMID,UNIT,PREVYQTY,PREVMQTY,PQTY from PRODFCDETAIL where PRODFCDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastDGPasteDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODFCBASICID,DGITEMID,DGTARQTY,DGMIN,DGSTOCK,REQDG,DGADDITID,DGADDITREQ,DGRAWMAT,DGREQAP from PROFCDG where PROFCDGID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProdForecastPyroDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PYWCID,WCDAYS,PYITEMID,PYMINSTK,PYALLREJ,PYGRCHG,PYREJQTY,PYREQQTY,PYTARQTY, PYPRODCAPD,PYPRODQTY,PYRAWREJMAT,PYRAWREJMATPER,PREBALQTY,PYADD1,PYADDPER,ALLOCADD,PYREQAP,WSTATUS,POWREQ from PRODFCPY where PRODFCPYID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastPolishDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PIGWCID,PIGWCDAYS,PIGITEMID,PIGTARGET,PIGCAP,PIGSTOCK,PIGMINSTK,PIGRAWREQ,PIGDAYS,PIGADDIT,PIGADDPER,PIGRAWMAT,PIGRAWREQPER,PIGRVDQTY,PIGPYPO,PIGPYQTY,PIGPOWREQ from PRODFCPIG where PRODFCPIGID='" + id + "' ";
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
            SvSql = "SELECT W.WCBASICID, W.WCID FROM WCBASIC W,LOCDETAILS LD WHERE W.ILOCATION=LD.LOCDETAILSID AND LD.LOCATIONTYPE='BALL MILL'";
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
                        PFCPYROItem cmp = new PFCPYROItem
                        {
                            itemid = rdr["ITEMID"].ToString(),
                            required = rdr["REQ"].ToString(),
                            minstock = rdr["MINSTK"].ToString(),
                            stock = rdr["stk"].ToString(),
                            rejqty = rdr["REJ"].ToString(),
                            target = rdr["ORD"].ToString(),
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
      
        public string ProductionForecastingCRUD(ProductionForecasting cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = cy.Docdate;
                    objCmd.Parameters.Add("PLANTYPE", OracleDbType.NVarchar2).Value = cy.PType;
                    objCmd.Parameters.Add("MONTH", OracleDbType.NVarchar2).Value = cy.ForMonth;
                    objCmd.Parameters.Add("INCDECPER", OracleDbType.NVarchar2).Value = cy.Ins;;
                    objCmd.Parameters.Add("HD", OracleDbType.NVarchar2).Value = cy.Hd;
                    objCmd.Parameters.Add("FINYRPST", OracleDbType.Date).Value = cy.Fordate;
                    objCmd.Parameters.Add("FINYRPED", OracleDbType.Date).Value = cy.Enddate;
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
                        foreach (PFCItem ca in cy.PFCILst)
                        {
                            if (ca.Isvalid == "Y" && ca.PType != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODFCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PRODFCDETAILPROC", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("PTYPE", OracleDbType.NVarchar2).Value = ca.PType;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ca.ItemId;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = ca.Unit;
                                    objCmds.Parameters.Add("PREVYQTY", OracleDbType.NVarchar2).Value = ca.PysQty;
                                    objCmds.Parameters.Add("PREVMQTY", OracleDbType.NVarchar2).Value = ca.PtmQty;
                                    objCmds.Parameters.Add("PQTY", OracleDbType.NVarchar2).Value = ca.Fqty;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }
                        }
                        foreach (PFCDGItem cp in cy.PFCDGILst)
                        {
                            if (cp.Isvalid == "Y" && cp.itemid != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PROFCDGPROC", objConns);
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
                                    objCmds.Parameters.Add("DGITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                    objCmds.Parameters.Add("DGTARQTY", OracleDbType.NVarchar2).Value = cp.target;
                                    objCmds.Parameters.Add("DGMIN", OracleDbType.NVarchar2).Value = cp.min;
                                    objCmds.Parameters.Add("DGSTOCK", OracleDbType.NVarchar2).Value = cp.stock;
                                    objCmds.Parameters.Add("REQDG", OracleDbType.NVarchar2).Value = cp.required;
                                    objCmds.Parameters.Add("DGADDITID", OracleDbType.NVarchar2).Value = cp.dgaddit;
                                    objCmds.Parameters.Add("DGADDITREQ", OracleDbType.NVarchar2).Value = cp.reqadditive;
                                    objCmds.Parameters.Add("DGRAWMAT", OracleDbType.NVarchar2).Value = cp.rawmaterial;
                                    objCmds.Parameters.Add("DGREQAP", OracleDbType.NVarchar2).Value = cp.ReqPyro;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (PFCPYROItem cp in cy.PFCPYROILst)
                        {
                            if (cp.Isvalid == "Y" && cp.WorkId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PRODFCPYPROC", objConns);
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
                                    objCmds.Parameters.Add("PYITEMID", OracleDbType.NVarchar2).Value = cp.itemid;
                                    objCmds.Parameters.Add("PYMINSTK", OracleDbType.NVarchar2).Value = cp.minstock;
                                    objCmds.Parameters.Add("PYALLREJ", OracleDbType.NVarchar2).Value = cp.pasterej;
                                    objCmds.Parameters.Add("PYGRCHG", OracleDbType.NVarchar2).Value = cp.GradeChange;
                                    objCmds.Parameters.Add("PYREJQTY", OracleDbType.NVarchar2).Value = cp.rejqty;
                                    objCmds.Parameters.Add("PYREQQTY", OracleDbType.NVarchar2).Value = cp.required;
                                    objCmds.Parameters.Add("PYTARQTY", OracleDbType.NVarchar2).Value = cp.target;
                                    objCmds.Parameters.Add("PYPRODCAPD", OracleDbType.NVarchar2).Value = cp.ProdDays;
                                    objCmds.Parameters.Add("PYPRODQTY", OracleDbType.NVarchar2).Value = cp.ProdQty;
                                    objCmds.Parameters.Add("PYRAWREJMAT", OracleDbType.NVarchar2).Value = cp.RejMat;
                                    objCmds.Parameters.Add("PYRAWREJMATPER", OracleDbType.NVarchar2).Value = cp.RejMatReq;
                                    objCmds.Parameters.Add("PREBALQTY", OracleDbType.NVarchar2).Value = cp.BalanceQty;
                                    objCmds.Parameters.Add("PYADD1", OracleDbType.NVarchar2).Value = cp.Additive;
                                    objCmds.Parameters.Add("PYADDPER", OracleDbType.NVarchar2).Value = cp.Per;
                                    objCmds.Parameters.Add("ALLOCADD", OracleDbType.NVarchar2).Value = cp.AllocAdditive;
                                    objCmds.Parameters.Add("PYREQAP", OracleDbType.NVarchar2).Value = cp.ReqPowder;
                                    objCmds.Parameters.Add("WSTATUS", OracleDbType.NVarchar2).Value = cp.WStatus;
                                    objCmds.Parameters.Add("POWREQ", OracleDbType.NVarchar2).Value = cp.PowderRequired;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        //foreach (PFCPOLIItem cp in cy.PFCPOLILst)
                        //{
                        //    if (cp.Isvalid == "Y" && cp.WorkId != "0")
                        //    {
                        //        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        //        {
                        //            OracleCommand objCmds = new OracleCommand("PSPARAMDETAILPROC", objConns);
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
                        //            objCmds.Parameters.Add("PRODFCBASICID", OracleDbType.NVarchar2).Value = Pid;
                        //            objCmds.Parameters.Add("PYWCID", OracleDbType.NVarchar2).Value = cp.WorkId;
                        //            objCmds.Parameters.Add("WCDAYS", OracleDbType.NVarchar2).Value = cp.CDays;
                        //            objCmds.Parameters.Add("PYITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                        //            objCmds.Parameters.Add("PYMINSTK", OracleDbType.NVarchar2).Value = cp.MinStock;
                        //            objCmds.Parameters.Add("PYALLREJ", OracleDbType.NVarchar2).Value = cp.PasteRej;
                        //            objCmds.Parameters.Add("PYGRCHG", OracleDbType.NVarchar2).Value = cp.GradeChange;
                        //            objCmds.Parameters.Add("PYREJQTY", OracleDbType.NVarchar2).Value = cp.RejQty;
                        //            objCmds.Parameters.Add("PYREQQTY", OracleDbType.NVarchar2).Value = cp.Required;
                        //            objCmds.Parameters.Add("PYTARQTY", OracleDbType.NVarchar2).Value = cp.Target;
                        //            objCmds.Parameters.Add("PYPRODCAPD", OracleDbType.NVarchar2).Value = cp.ProdDays;
                        //            objCmds.Parameters.Add("PYPRODQTY", OracleDbType.NVarchar2).Value = cp.ProdQty;
                        //            objCmds.Parameters.Add("PYRAWREJMAT", OracleDbType.NVarchar2).Value = cp.RejMat;
                        //            objCmds.Parameters.Add("PYRAWREJMATPER", OracleDbType.NVarchar2).Value = cp.RejMatReq;
                        //            objCmds.Parameters.Add("PREBALQTY", OracleDbType.NVarchar2).Value = cp.BalanceQty;
                        //            objCmds.Parameters.Add("PYADD1", OracleDbType.NVarchar2).Value = cp.Additive;
                        //            objCmds.Parameters.Add("PYADDPER", OracleDbType.NVarchar2).Value = cp.Per;
                        //            objCmds.Parameters.Add("ALLOCADD", OracleDbType.NVarchar2).Value = cp.AllocAdditive;
                        //            objCmds.Parameters.Add("PYREQAP", OracleDbType.NVarchar2).Value = cp.ReqPowder;
                        //            objCmds.Parameters.Add("WSTATUS", OracleDbType.NVarchar2).Value = cp.WStatus;
                        //            objCmds.Parameters.Add("POWREQ", OracleDbType.NVarchar2).Value = cp.PowderRequired;
                        //            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                        //            objConns.Open();
                        //            objCmds.ExecuteNonQuery();
                        //            objConns.Close();
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
    }
}
