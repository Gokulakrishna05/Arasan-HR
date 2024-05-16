using Arasan.Interface;
using Arasan.Models;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

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
                                        string inddt = datatrans.GetDataString("select DOCDATE from PINDBASIC where DOCID='" + cp.IndentId + "'");
                                        double cosa = cp.costrate * cp.ConvQty;
                                        DateTime del = DateTime.Parse(inddt);
                                        string duedate = del.ToString("dd-MMM-yyyy");
                                        svSQL = "Insert into DPDETAIL (DPBASICID,ITEMID,QTY,PUNIT,RATE,AMOUNT,TOTAMT,CF,DISC,DISCAMOUNT,IFREIGHTCH,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,RETQTY,TARIFFID,REJQTY,UNIT,PRIQTY,ITEMACC,QCYN,GRNQTY,ACCQTY,QCLOCID,COSTRATE,LOTYN,LOTQTY,BRATE,BAMOUNT,ACCDQTY,SERIALYN,INDENTNO,INDENTDT,INDENTQTY,PENDQTY,PINDDETAILID,EXPYN,DPDETAILROW,REJLOCID,BINID,ASSESSABLEVALUE,IBINYN,DRUMYN,MINRATE,MAXRATE,COSTAMOUNT,STKCTRLCONS,IPKNFDCH,ISPLDISCF,IOTHERCH,INDUNIT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.rate + "','" + cp.Amount + "','" + cp.TotalAmount + "','" + cp.ConFac + "','" + cp.Disc + "','" + cp.DiscAmount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','0','"+cp.gst+"','0','"+ lotnogen.Rows[0]["PRIUNIT"].ToString() +"','"+cp.ConvQty+"','"+ lotnogen.Rows[0]["ITEMACC"].ToString() + "','"+ lotnogen.Rows[0]["QCT"].ToString() + "','0','0','10035000000040','"+cp.costrate+"','"+ lotnogen.Rows[0]["LOTYN"].ToString() + "','0','"+cp.rate+"','"+cp.Amount+ "','0','"+ lotnogen.Rows[0]["SERIALYN"].ToString() + "','"+cp.IndentId+"','"+ duedate + "','"+cp.Indqty+"','0','"+cp.Inddetid+"','" + lotnogen.Rows[0]["EXPYN"].ToString() + "','"+drow+ "','10989000000581','" + lotnogen.Rows[0]["BINNO"].ToString() + "','"+cp.Amount+ "','YES','" + lotnogen.Rows[0]["DRUMYN"].ToString() + "','"+cp.rate+"','"+cp.rate+"','"+ cosa +"','F','"+ cp.packn+"','"+cp.spldic+"','"+cp.otherch+"','"+cp.Unit+"') RETURNING DPDETAILID INTO :LASTID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                        objCmds.ExecuteNonQuery();

                                        string dpdetid = objCmds.Parameters["LASTID"].Value.ToString();
                                        svSQL = "UPDATE PINDDETAIL SET POQTY='" + cp.ConvQty + "'  WHERE PINDDETAILID='" + cp.Inddetid + "' ";
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

                                                svSQL = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('0','0','F','" + dpdetid + "','p','" + cp.ItemId + "','" + cy.DocDate + "','" + cp.ConvQty + "','" + cp.Amount + "','" + loc + "','" + itemma.Rows[0]["BINNO"].ToString() + "','0','0','0','0','0','0','GRN','" + insflag + "') RETURNING STOCKVALUEID INTO :STKID";
                                                 objCmdss = new OracleCommand(svSQL, objConn);
                                                objCmdss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                                objCmdss.ExecuteNonQuery();
                                                string stkid = objCmdss.Parameters["STKID"].Value.ToString();
                                                string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION) VALUES ('" + stkid + "','" + cy.DocNo + "','" + cy.Narration + "') ";
                                                OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                                objCmddts.ExecuteNonQuery();
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
                                                svSQL = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,STOCKVALUE,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,STOCKTRANSTYPE,SINSFLAG) VALUES ('0','0','F','" + dpdetid + "','p','" + cp.ItemId + "','" + cy.DocDate + "','" + cp.ConvQty + "','" + cp.Amount + "','" + loc + "','" + itemma.Rows[0]["BINNO"].ToString() + "','0','0','0','0','0','0','GRN','" + insflag + "') RETURNING STOCKVALUEID INTO :STKID";
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
                                bool result = datatrans.UpdateStatus("UPDATE DPBASIC SET STATUS='" + status + "' Where DPBASICID='" + cy.ID + "'");




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
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,DPBASIC. DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.STATUS,DPBASICID,DPBASIC.GROSS,DPBASIC.NET from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where DPBASIC.IS_ACTIVE='Y' ORDER BY DPBASICID DESC";
            }
            else
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,PARTYMAST.PARTYNAME,DPBASIC. DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.STATUS,DPBASICID,DPBASIC.GROSS,DPBASIC.NET from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where DPBASIC.IS_ACTIVE='N' ORDER BY DPBASICID DESC ";
            }
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
    }
}
