using Arasan.Interface;
using Arasan.Models;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.CodeAnalysis.Operations;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace Arasan.Services
{
    public class DirectPurchaseService: IDirectPurchase
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DirectPurchaseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<DirectPurchase> GetAllDirectPur(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Yes";
            }

            List<DirectPurchase> cmpList = new List<DirectPurchase>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,DPBASIC. DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER, to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCID,CURRENCY.MAINCURR,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,OTHERCH,RNDOFF,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID,DPBASIC.IS_ACTIVE from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=DPBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') and DPBASIC.IS_ACTIVE= '"+ status +"' ORDER BY DPBASIC.DPBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectPurchase cmp = new DirectPurchase
                        {

                            ID = rdr["DPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Voucher = rdr["VOUCHER"].ToString(),
                            RefDate = rdr["REFDT"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Currency = rdr["MAINCURR"].ToString(),
                            Narration = rdr["NARR"].ToString(),
                            status = rdr["IS_ACTIVE"].ToString()



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public IEnumerable<DirItem> GetAllDirectPurItem(string id)
        {
            List<DirItem> cmpList = new List<DirItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DPDETAIL.DPBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirItem cmp = new DirItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable getindent()
        {
            string SvSql = string.Empty;
            SvSql= @"SELECT ID.PINDDETAILID,' ' AS MARK,IB.DOCID AS INDENT_NO , to_char(IB.DOCDATE,'dd-MON-yyyy') AS  INDENT_DATE, A.ITEMID,U.UNITID,  ID.QTY AS INDENT_QTY, A.ITEMDESC, ((ID.QTY+ID.RETQTY)-(ID.POQTY+ID.SHCLQTY+ID.GRNQTY)) AS ORD_QTY , IB.PurType,ID.mailto
FROM ITEMMASTER A ,PINDBASIC IB,PINDDETAIL ID , BRANCHMAST BM , Unitmast U
WHERE ID.APPROVED1 = 'YES'   AND ID.APPROVED2 = 'YES' 
AND ((ID.QTY+ID.RETQTY)-(ID.POQTY+ID.SHCLQTY+ID.GRNQTY))>0
AND ID.UNIT=U.UNITMASTID
AND IB.PINDBASICID=ID.PINDBASICID
AND A.ITEMMASTERID=ID.ITEMID 
AND IB.BRANCHID = BM.BRANCHMASTID
AND 'AGAINST PURCHASE INDENT' = 'AGAINST PURCHASE INDENT' ";
            //AND IB.PINDBASICID = ID.PINDBASICID
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchaseItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = " Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,DPDETAIL.ITEMID,UNITMAST.UNITID,RATE,TOTAMT,DISC,DISCAMOUNT,IFREIGHTCH,PURTYPE,AMOUNT,CF,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST  from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT   where DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchase(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DPBASIC.BRANCHID,DPBASIC.PARTYID,DPBASIC.DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER,to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,DPBASIC.LOCID,DPBASIC.MAINCURRENCY,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,DPBASIC.OTHERCH,DPBASIC.ROUNDM,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID  from DPBASIC where DPBASIC.DPBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchaseItemDetailsView(string id)
        {
            string SvSql = string.Empty;
            SvSql = " Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,ITEMMASTER.ITEMID,INDENTDT,INDENTNO,UNITMAST.UNITID,RATE,TOTAMT,DISC,DISCAMOUNT,IFREIGHTCH,PURTYPE,AMOUNT,CF,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST  from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT   where DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchaseView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,PARTYMAST.PARTYID,DPBASIC.DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER,to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCDETAILS.LOCID,CURRENCY.MAINCURR,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,DPBASIC.OTHERCH,DPBASIC.ROUNDM,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID  from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=DPBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DPBASIC.LOCID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=DPBASIC.MAINCURRENCY  where DPBASIC.DPBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string DPACC(GRN cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string Location = "12418000000423";
                DataTable dtt = new DataTable();
                dtt = datatrans.GetData("select DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,BRANCHID,PARTYID from DPBASIC where DPBASICID='" + cy.GRNID + "'");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();

                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                command.Transaction = transaction;

                                ////////////////////transaction
                                int t2cunt = 0;
                                double grossamt = 0;
                                string Grossledger = "";
                                double totgst = 0;
                                double netamt = 0;
                                foreach (GRNAccount cp in cy.Acclst)
                                {
                                    t2cunt += 1;
                                    if (cp.TypeName == "GROSS")
                                    {
                                        grossamt += cp.CRAmount;
                                        Grossledger = cp.Ledgername;
                                    }
                                    if (cp.TypeName == "CGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "SGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "IGST")
                                    {
                                        totgst += cp.CRAmount;
                                    }
                                    if (cp.TypeName == "NET")
                                    {
                                        netamt = cp.DRAmount;
                                    }
                                }
                                DataTable dtv = datatrans.GetData("select PREFIX,LASTNO from Sequence where TRANSTYPE='vchpr' AND ACTIVESEQUENCE='T' ");
                                string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
                                string mno = DateTime.Now.ToString("yyyyMM");
                                long TRANS1 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS1'");
                                command.CommandText = "Insert into TRANS1 (TRANS1ID,APPROVAL,T1SOURCEID,VCHSTATUS,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,MSTATUS,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,TRANSID,BRANCHID,LOCID,CANCEL,MAXAPPROVED,LATEMPLATEID,T1VCHDT,T1NARR,T2COUNT,POSTYN,BRANCHMASTID,VTYPE,GENERATED,AMTWD,USERNAME,MODIFIEDON,T1SID,TOTGST) VALUES " +
                                    "('" + TRANS1 + "','0','" + cy.GRNID + "','N','pu','" + vNo + "','1','1','" + dtt.Rows[0]["DOCID"].ToString() + "','" + dtt.Rows[0]["DOCDATE"].ToString() + "','" + mno + "','a','" + Grossledger + "','" + grossamt + "','" + cy.mid + "','" + netamt + "','" + dtt.Rows[0]["PARTYID"].ToString() + "','" + netamt + "','vchpr','" + dtt.Rows[0]["BRANCHID"].ToString() + "','" + Location + "','F','0','0','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Vmemo + "','" + t2cunt + "','Y','0','R','T','" + cy.Amtinwords + "','" + cy.createdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','0','" + totgst + "')";
                                command.ExecuteNonQuery();
                                foreach (GRNAccount cp in cy.Acclst)
                                {
                                    string mledger = "";
                                    if (cp.TypeName == "NET")
                                    {
                                        mledger = Grossledger;
                                    }
                                    else
                                    {
                                        mledger = cy.mid;
                                    }
                                    long TRANS2 = datatrans.GetDataIdlong("select LASTNO from NEWSEQUENCE where NAME='TRANS2'");
                                    command.CommandText = "Insert into TRANS2 (TRANS2ID,TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,SLN,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS,RCTRLFLD,CMID) VALUES ('" + TRANS2 + "','" + TRANS1 + "','" + cp.CRDR + "','" + cp.Ledgername + "','" + cp.DRAmount + "','" + cp.DRAmount + "','" + cp.CRAmount + "','" + cp.CRAmount + "','1','1','" + cp.DRAmount + "','" + cp.CRAmount + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','pu','N','F','" + mledger + "')";
                                    command.ExecuteNonQuery();
                                    datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS2 + 1).ToString() + "' where NAME='TRANS2'");

                                }
                                string updatetrans = " UPDATE NEWSEQUENCE SET LASTNO ='" + (TRANS1 + 1).ToString() + "' where NAME='TRANS1'";
                                datatrans.UpdateStatus(updatetrans);


                                command.CommandText = "UPDATE DPBASIC SET ADSCHEME ='" + cy.ADCOMPHID + "',IS_ACCOUNT='Y' WHERE DPBASICID='" + cy.GRNID + "'";
                                command.ExecuteNonQuery();

                                ///////////////////transaction
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

        public string DirectPurCRUD(DirectPurchase cy)
        {
            string msg = "";
            try
            { 
                string StatementType = string.Empty; string svSQL = "";
                string updateCMd = ""; string DocNo = "";
               datatrans = new DataTransactions(_connectionString);
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);
                    if (cy.Location == "10001000000827")
                    {
                        int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE  PREFIX = 'Dpu-'");
                         DocNo = string.Format("{0}{1}", "Dpu-", (idc + 1).ToString());

                         updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'dp'  and PREFIX = 'Dpu-'";
                    }
                    else
                    {
                        int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE  TRANSTYPE = 'dp'  and LOCID = '" + cy.Location + "'");
                        string pre = datatrans.GetDataString(" SELECT PRIFIX FROM SEQUENCE WHERE  TRANSTYPE = 'dp'  and LOCID = '" + cy.Location + "'");
                         DocNo = string.Format("{0}{1}", pre, (idc + 1).ToString());

                         updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'dp'  and LOCID = '" + cy.Location + "'";
                    }
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocNo = DocNo;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DIRECTPURCHASEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/
                    DataTable party  = datatrans.GetData("SELECT PARTYNAME,ACCOUNTNAME,TYPE FROM PARTYMAST WHERE PARTYMASTID='"+cy.Supplier+"'");
                    string entat = DateTime.Now.ToString("dd\\/MM\\/yyyy hh:mm:ss tt");
                    string loc = "";
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
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("VOUCHER", OracleDbType.NVarchar2).Value = cy.Voucher;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("OTHERCH", OracleDbType.NVarchar2).Value = cy.Other;
                    objCmd.Parameters.Add("ROUNDM", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("OTHERDISC", OracleDbType.NVarchar2).Value = cy.Disc;
                    objCmd.Parameters.Add("LRCH", OracleDbType.NVarchar2).Value = cy.LRCha;
                    objCmd.Parameters.Add("DELCH", OracleDbType.NVarchar2).Value = cy.DelCh;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = "1";
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = party.Rows[0]["PARTYNAME"].ToString();
                    objCmd.Parameters.Add("CRACC", OracleDbType.NVarchar2).Value = party.Rows[0]["ACCOUNTNAME"].ToString();
                    objCmd.Parameters.Add("QCFLAG", OracleDbType.NVarchar2).Value = "0";
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = cy.user;
                    objCmd.Parameters.Add("ETYPE", OracleDbType.NVarchar2).Value = "New";
                    objCmd.Parameters.Add("PARTYCAT", OracleDbType.NVarchar2).Value = party.Rows[0]["TYPE"].ToString();
                    objCmd.Parameters.Add("PURCHTYPE", OracleDbType.NVarchar2).Value = cy.Purtype;
                    objCmd.Parameters.Add("PKNFDCH", OracleDbType.NVarchar2).Value = cy.Packingcharges;
                    objCmd.Parameters.Add("ENTAT", OracleDbType.NVarchar2).Value = entat;
                    objCmd.Parameters.Add("AMTINWORDS", OracleDbType.NVarchar2).Value = cy.Amountinwords;

                    //objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
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
                        if (cy.DirLst != null)
                        {
                            if (cy.ID == null)
                            {
                                int l = 1;
                                int drow = 1;
                                foreach (DirItem cp in cy.DirLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        DataTable lotnogen = datatrans.GetData("Select LOTYN ,QCT,EXPYN,SERIALYN,SUBCATEGORY,DRUMYN,BINNO,PRIUNIT,ITEMACC FROM ITEMMASTER where   ITEMMASTERID='" + cp.ItemId + "'");
                                       // string inddt = datatrans.GetDataString("select DOCDATE from PINDBASIC where DOCID='" + cp.IndentId + "'");
                                        double cosa = cp.costrate * cp.ConvQty;
                                        //DateTime del = DateTime.Parse(inddt);
                                        //string duedate = del.ToString("dd-MMM-yyyy");
                                        svSQL = "Insert into DPDETAIL (DPBASICID,ITEMID,QTY,PUNIT,RATE,AMOUNT,TOTAMT,CF,DISC,DISCAMOUNT,IFREIGHTCH,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,RETQTY,TARIFFID,REJQTY,UNIT,PRIQTY,ITEMACC,QCYN,GRNQTY,ACCQTY,QCLOCID,COSTRATE,LOTYN,LOTQTY,BRATE,BAMOUNT,ACCDQTY,SERIALYN,INDENTNO,INDENTDT,INDENTQTY,PENDQTY,PINDDETAILID,EXPYN,DPDETAILROW,REJLOCID,BINID,ASSESSABLEVALUE,IBINYN,DRUMYN,MINRATE,MAXRATE,COSTAMOUNT,STKCTRLCONS,IPKNFDCH,ISPLDISCF,IOTHERCH,INDUNIT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.rate + "','" + cp.Amount + "','" + cp.TotalAmount + "','" + cp.ConFac + "','" + cp.Disc + "','" + cp.DiscAmount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','0','"+cp.gst+"','0','"+ lotnogen.Rows[0]["PRIUNIT"].ToString() +"','"+cp.ConvQty+"','"+ lotnogen.Rows[0]["ITEMACC"].ToString() + "','"+ lotnogen.Rows[0]["QCT"].ToString() + "','0','0','10035000000040','"+cp.costrate+"','"+ lotnogen.Rows[0]["LOTYN"].ToString() + "','0','"+cp.rate+"','"+cp.Amount+ "','0','"+ lotnogen.Rows[0]["SERIALYN"].ToString() + "','"+cp.indentno + "','"+ cp.indentdate + "','"+cp.Indqty+"','0','"+cp.inddetid + "','" + lotnogen.Rows[0]["EXPYN"].ToString() + "','"+drow+ "','10989000000581','" + lotnogen.Rows[0]["BINNO"].ToString() + "','"+cp.Amount+ "','YES','" + lotnogen.Rows[0]["DRUMYN"].ToString() + "','"+cp.rate+"','"+cp.rate+"','"+ cosa +"','F','"+ cp.packn+"','"+cp.spldic+"','"+cp.otherch+"','"+cp.Unit+"') RETURNING DPDETAILID INTO :LASTID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmds.ExecuteNonQuery();

                                        string dpdetid = objCmds.Parameters["LASTID"].Value.ToString();
                                        svSQL = "UPDATE PINDDETAIL SET POQTY='" + cp.Quantity + "'  WHERE PINDDETAILID='" + cp.inddetid + "' ";
                                        OracleCommand objCmdsin = new OracleCommand(svSQL, objConn);
                                        objCmdsin.ExecuteNonQuery();
                                        string itemname = datatrans.GetDataString("select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "'");
                                        string lotnumber = "";
                                        string slotno = "";
                                        string ins = "";
                                        if (lotnogen.Rows[0]["QCT"].ToString() == "YES")
                                        {
                                            ins = "0";
                                        }
                                        else { ins = "1"; }
                                        if (lotnogen.Rows[0]["SUBCATEGORY"].ToString() == "INGOTS")
                                        {
                                            loc = "10036000012390";
                                        }
                                        else
                                        {
                                            loc = "10001000000827";
                                        }

                                        svSQL = "UPDATE ITEMMASTER SET LATPURPRICE='" + cp.rate + "',LATPURDT='" + cy.DocDate + "' WHERE ITEMMASTERID='" + cp.ItemId + "' ";
                                        OracleCommand objCmdsi = new OracleCommand(svSQL, objConn);
                                        objCmdsi.ExecuteNonQuery();

                                        if (lotnogen.Rows[0]["LOTYN"].ToString() == "YES")
                                        {
                                            string item = itemname;
                                            string Docid = cy.DocNo;
                                            string DocDate = cy.DocDate;

                                            lotnumber = string.Format("{0} -- {1} -- {2} -- {3}", item, DocDate, Docid, l.ToString());
                                            l++;
                                        }
                                        else
                                        {
                                            string item = itemname;
                                            string Docid = cy.DocNo;
                                            string DocDate = cy.DocDate;
                                            slotno = string.Format("{0} -- {1} -- {2}", item, DocDate, Docid);
                                        }
                                        DataTable itemma = datatrans.GetData("SELECT LOTYN,QCT,BINBASIC.BINID,ITEMMASTER.BINNO FROM ITEMMASTER LEFT OUTER JOIN BINBASIC on BINBASICID=ITEMMASTER.BINNO WHERE ITEMMASTERID='" + cp.ItemId + "'");
                                        string insflag = "";
                                        if (itemma.Rows.Count > 0)
                                        {
                                            if (itemma.Rows[0]["QCT"].ToString() == "YES")
                                            {
                                                insflag = "0";
                                            }
                                            else
                                            {
                                                insflag = "1";
                                            }
                                            if (itemma.Rows[0]["LOTYN"].ToString() == "YES")
                                            {
                                                svSQL = "Insert into LOTMAST (T1SOURCEID,ITEMID,PARTYID,RATE,DOCID,DOCDATE,QTY,LOTNO,LOCATION,INSFLAG,RCFLAG,PRODTYPE,QCRELASEFLAG,ESTATUS,COMPFLAG,AMOUNT,PACKFLAG,CURINWFLAG,CUROUTFLAG,PACKINSFLAG,MATCOST,MCCOST,EMPCOST,OTHERCOST,ADMINCOST,GENSETCOST,EBCOST,EBUNITRATE,DIESELRATE,TESTINSFLAG,BINNO,FIDRMS) VALUES ('" + dpdetid + "','" + cp.ItemId + "','" + cy.Supplier + "','" + cp.rate + "','" + cy.DocNo + "','" + cy.DocDate + "','" + cp.ConvQty + "','" + lotnumber + "','" + loc + "','" + insflag + "','0','GRN','0','0','0','" + cp.Amount + "','0','0','0','0','0','0','0','0','0','0','0','0','0','0','" + itemma.Rows[0]["BINNO"].ToString() + "','0')";
                                                OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                                objCmdss.ExecuteNonQuery();
                                                svSQL = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID) VALUES ('0','0','F','" + dpdetid + "','0','" + cy.DocNo + "','" + cy.DocDate + "','" + lotnumber + "' ,'" + cp.ConvQty + "','0','" + cp.rate + "','" + cp.Amount + "','" + cp.ItemId + "','" + loc + "','" + itemma.Rows[0]["BINNO"].ToString() + "','0')";
                                                OracleCommand objCmdsss = new OracleCommand(svSQL, objConn);
                                                objCmdsss.ExecuteNonQuery();

                                                //svSQL = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('0','0','F','" + dpdetid + "','p','" + cp.ItemId + "','" + cy.DocDate + "','" + cp.ConvQty + "','" + cp.Amount + "','" + loc + "','" + itemma.Rows[0]["BINNO"].ToString() + "','0','0','0','0','0','0','GRN','" + insflag + "') RETURNING STOCKVALUEID INTO :STKID";
                                                // objCmdss = new OracleCommand(svSQL, objConn);
                                                //objCmdss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                //objCmdss.ExecuteNonQuery();
                                                //string stkid = objCmdss.Parameters["STKID"].Value.ToString();
                                                //string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION) VALUES ('" + stkid + "','" + cy.DocNo + "','" + cy.Narration + "') ";
                                                //OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                                //objCmddts.ExecuteNonQuery();
                                            }
                                            //else
                                            //{
                                            //    svSQL = "Insert into SLOTMAST (T1SOURCEID,ITEMMASTERID,TYPE,LOTNO,MDATE,EDATE,DOCDATE,DOCID,BINNO,RATE,QTY,LOCID) VALUES ('" + dpdetid + "','" + cp.saveItemId + "','GRN','" + slotno + "','" + cp.mdate + "','" + cp.edate + "','" + cy.GRNdate + "','" + cy.GRNNo + "','" + itemma.Rows[0]["BINNO"].ToString() + "','" + cp.rate + "','" + cp.Quantity + "','" + loc + "')";
                                            //    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                            //    objCmdss.ExecuteNonQuery();
                                            //}
                                            DataTable lstock = datatrans.GetData("SELECT ITEMID,T1SOURCEID,QTY FROM STOCKVALUE WHERE ITEMID='" + cp.ItemId + "' and T1SOURCEID='" + cy.ID + "'");
                                            if (lstock.Rows.Count > 0)
                                            {
                                                double sqty = Convert.ToDouble(lstock.Rows[0]["QTY"].ToString());
                                                double totqty = sqty + cp.ConvQty;
                                                svSQL = "UPDATE STOCKVALUE SET QTY='" + totqty + "' WHERE ITEMID='" + cp.ItemId + "' and T1SOURCEID='" + cy.ID + "'";
                                                OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                                objCmdss.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                svSQL = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('0','0','F','" + dpdetid + "','p','" + cp.ItemId + "','" + cy.DocDate + "','" + cp.ConvQty + "','" + cp.Amount + "','" + loc + "','" + itemma.Rows[0]["BINNO"].ToString() + "','0','0','0','0','0','0','','" + insflag + "') RETURNING STOCKVALUEID INTO :STKID";
                                                OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                                objCmdss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                objCmdss.ExecuteNonQuery();
                                                string stkid = objCmdss.Parameters["STKID"].Value.ToString();
                                                string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION) VALUES ('" + stkid + "','" + cy.DocNo + "','" + cy.Narration + "') ";
                                                OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                                objCmddts.ExecuteNonQuery();
                                            }
                                        }

                                    }
                                    drow++;
                                }
                                string status = " Completed";
                                //if (cp.PendingQty > 0)
                                //{
                                //    status = "Partially Completed";
                                //}
                                bool result = datatrans.UpdateStatus("UPDATE DPBASIC SET STATUS='" + status + "' Where DPBASICID='" + Pid + "'");




                            }

                            else
                            {
                                svSQL = "Delete DPDETAIL WHERE DPBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DirItem cp in cy.DirLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into DPDETAIL (DPBASICID,ITEMID,QTY,PUNIT,RATE,AMOUNT,TOTAMT,CF,DISC,DISCAMOUNT,IFREIGHTCH,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.rate + "','" + cp.Amount + "','" + cp.TotalAmount + "','" + cp.ConFac + "','" + cp.Disc + "','" + cp.DiscAmount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            } }
                        
                       
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DPBASIC SET IS_ACTIVE ='N' WHERE DPBASICID='" + id + "'";
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

        public DataTable GetVocher()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT VCHTYPEID,DESCRIPTION FROM VCHTYPE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllDirectPurchases(string strStatus)
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,DPBASIC. DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.STATUS,DPBASICID,DPBASIC.GROSS,DPBASIC.NET,IS_ACCOUNT from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID ";
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql += " Where DPBASIC.IS_ACTIVE='Y'";
            }
            else
            {
                SvSql += "Where DPBASIC.IS_ACTIVE='N'";
            }
            SvSql += " ORDER BY DPBASIC.DOCDATE DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string DirectPurchasetoGRN(string id)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE DPBASIC SET STATUS='GRN Generated' where DPBASICID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public DataTable GetDirectPurchaseGrn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,DPBASIC.DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER,to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCDETAILS.LOCID,CURRENCY.MAINCURR,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,DPBASIC.OTHERCH,DPBASIC.ROUNDM,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=DPBASIC.LOCID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=DPBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND DPBASIC.DPBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDirectPurchaseItemGRN(string id)
        {
            string SvSql = string.Empty;
            SvSql = " Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID,RATE,TOTAMT,DISC,DISCAMOUNT,IFREIGHTCH,PURTYPE,AMOUNT,CF,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST  from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT   where DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
        public DataTable GetgstDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID from HSNROW WHERE HSNCODEID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GethsnDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select HSNCODEID from HSNCODE WHERE HSNCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetTariff(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFMASTER.TARIFFID,HSNROW.TARIFFID as tariff from HSNROW left outer join TARIFFMASTER on TARIFFMASTERID=HSNROW.TARIFFID where  HSNCODEID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public async Task<IEnumerable<DpItemDetail>> GetdpItem(string id )
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<DpItemDetail>(" SELECT DOCID, to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, REFDT, DPBASIC.PARTYNAME, CRACC, ADTGMCONTROL, RNDOFF, LOCDETAILID, EXCISEAPP, APPDATE, GROSS, NET, NARR, QCFLAG, VTYPE, TERMSTEMPLATE, DPBASIC.USERID, DPBASICID, EXCHANGERATE, MAINCURRENCY, ETYPE, RPCTRL, DUEDAYS, DUEDATE, MID, DELCH, BINYN, VOUCHER, TEMPDESC, TAXABLEAMOUNT, FREIGHT, QCCHECK, LRNO, LRDT, LRCH, TRNSPNAME, EXINVNO, EXINVDT, EXINVBASICID, PURCHTYPE, FPFLAG, CSTCH, PURBLSTATUS, MCATEGORY, RPAMOUNT, MDCTRL, TRANSITLOCID, DECLID, DECLYN, DESPTHRU, EXSTATUS, PURCHASEYN, ENTBY, CSTVATCH, TRUCKNO, PKNFDCH, SPLDISCF, ROUNDM, OTHERCH, PARTYBILL, OTHERDISC, REFNO, FINYR, AMTINWORDS,PARTYMAST.STATE,PARTYMAST.MOBILE,PARTYMAST.GSTNO,PARTYMAST.ADD1||''||PARTYMAST.ADD2||''||PARTYMAST.ADD2||''||PARTYMAST.PINCODE as ADDRESS FROM DPBASIC INNER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=DPBASIC.PARTYID where DPBASIC.DPBASICID='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<DpDetItemDetail>> GetdpdetItem(string supid )
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<DpDetItemDetail>(" SELECT ITEMMASTER.ITEMID, DPDETAIL.RETQTY, DPDETAIL.QTY, RATE, AMOUNT, SHECESSAMT, SEDPER, SEDAMT, AEDPER, AEDAMT, BCDPER, BCDAMT, TOTAMT, TOTEXAMT, REJQTY, DPDETAIL.UNIT, PRIQTY, DPBASICID, DPDETAILID, CF, DPDETAIL.GRNQTY, ACCQTY, PUNIT, COSTRATE, LOTQTY, BRATE, BAMOUNT, ACCDQTY, INDENTNO, to_char(INDENTDT,'dd-MON-yyyy')INDENTDT, INDENTQTY, PENDQTY, DPDETAIL.PINDDETAILID, DPDETAILROW, REJLOCID, QCTESTFLAG, BINID, DISC, DISCAMOUNT, ASSESSABLEVALUE, BEDPER, CESSPER, IBINYN, BEDAMT, CESSAMT, PURTYPE, MINRATE, MAXRATE, IFREIGHTCH, IDELCH, ILRCH, COSTAMOUNT, STKCTRLCONS, ICSTVATCH, IPKNFDCH, ISPLDISCF, ICSTCH, IOTHERCH, ISPCDISC, INDUNIT, IFREIGHT, CGSTP, SGSTP, IGSTP, SGST, CGST, IGST,PINDDETAIL.NARRATION,LOCDETAILS.LOCID FROM DPDETAIL INNER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID INNER JOIN PINDDETAIL ON PINDDETAIL.PINDDETAILID=DPDETAIL.PINDDETAILID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT  where DPDETAIL.DPBASICID='" + supid + "'", commandType: CommandType.Text);
            }
        }
    }
}
