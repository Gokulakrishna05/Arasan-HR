using Arasan.Interface;
using Arasan.Models;
using Intuit.Ipp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales_Export
{
    public class ExportDCService : IExportDC
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ExportDCService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,LATPURPRICE,UNITMAST.UNITMASTID,CF from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListExportDC(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT EDCBASIC.DOCID,to_char(EDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,EDCBASIC.PARTYID,EDCBASICID,STATUS FROM EDCBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EDCBASIC.LOCID  WHERE EDCBASIC.STATUS='Y' ORDER BY to_date(EDCBASIC.DOCDATE) DESC , EDCBASIC.DOCID DESC";
            }
            else
            {
                SvSql = "SELECT EDCBASIC.DOCID,to_char(EDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,EDCBASIC.PARTYID,EDCBASICID,STATUS FROM EDCBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EDCBASIC.LOCID  WHERE EDCBASIC.STATUS='N' ORDER BY  EDCBASIC.EDCBASICID DESC";
            }
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
                    svSQL = "UPDATE EDCBASIC SET EDCBASIC.STATUS ='N' WHERE EDCBASICID='" + id + "'";
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
        public string ActStatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EDCBASICID SET EDCBASICID.STATUS ='Y' WHERE EDCBASICID='" + id + "'";
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
        public DataTable GetExportDC(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LOCID,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,PARTYID,JOBORDNO,RECDBY,DESPBY,INSPBY,DOCTHROUGH,CNAME,to_char(SMSDATE,'dd-MON-yyyy')SMSDATE,SENDSMS,DELIVAT,NARRATION FROM EDCBASIC    where EDCBASIC.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDCView(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,PARTYMAST.PARTYID,JOBORDNO,EMPMAST.EMPNAME,DESPBY,INSPBY,DOCTHROUGH,CNAME,to_char(SMSDATE,'dd-MON-yyyy')SMSDATE,SENDSMS,DELIVAT,NARRATION FROM EDCBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=EDCBASIC.LOCID LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=EDCBASIC.PARTYID LEFT OUTER JOIN EMPMAST ON EMPMAST.EMPMASTID=EDCBASIC.RECDBY where EDCBASIC.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDCItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EDCBASICID,ITEMID,PACKSPEC,UNITMAST.UNITID,MQTY,ISSRATE,ISSAMT,LOTYN,SERIALYN,SEALNO,ORDQTY,CLSTOCK,DCQTY,QTY,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,DRUMDESC,CNO,TWT,VECHILENO,MATSUPP FROM EDCDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EDCDETAIL.UNIT   where EDCDETAIL.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDCItemview(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EDCBASICID,ITEMMASTER.ITEMID,PACKSPEC,UNITMAST.UNITID,MQTY,ISSRATE,ISSAMT,EDCDETAIL.LOTYN,EDCDETAIL.SERIALYN,SEALNO,ORDQTY,CLSTOCK,DCQTY,QTY,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,DRUMDESC,CNO,TWT,VECHILENO,MATSUPP FROM EDCDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EDCDETAIL.UNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EDCDETAIL.ITEMID   where EDCDETAIL.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDCScrap(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EDCBASICID,RLOCID,RITEMID,UNITMAST.UNITID,RCF,SL1,RQTY,RCOSTRATE,BILLEDQTY FROM EDCREJDETAILS LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EDCREJDETAILS.RUNIT  where EDCREJDETAILS.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetExportDCScrapview(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT EDCBASICID,RLOCID,ITEMMASTER.ITEMID,UNITMAST.UNITID,RCF,SL1,RQTY,RCOSTRATE,BILLEDQTY FROM EDCREJDETAILS LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=EDCREJDETAILS.RUNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=EDCREJDETAILS.RITEMID  where EDCREJDETAILS.EDCBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ExportDCCRUD(ExportDC cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE TRANSTYPE = 'edelc' AND ACTIVESEQUENCE = 'T'");
                string DCNo = string.Format("{0}{1}", "Dec-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE TRANSTYPE = 'edelc' AND ACTIVESEQUENCE ='T'";
               
                cy.DCNo = DCNo;

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.ExportDCLst != null)
                    {
                        int totdrums = 0;
                        int totqty = 0;
                        double totdis = 0;
                        double totqtydis = 0;
                        double totcashdisc = 0;
                        foreach (ExportDCItem cp in cy.ExportDCLst)
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
                                    //            OracleCommand objCmd = new OracleCommand("EXPORTEDCPROC", objConn);
                                    ///*objCmd.Connection = objConn;
                                    //objCmd.CommandText = "DIRECTPURCHASEPROC";*/

                                    //objCmd.CommandType = CommandType.StoredProcedure;
                                    //if (cy.ID == null)
                                    //{
                                    //    StatementType = "Insert";
                                    //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //}
                                    //else
                                    //{
                                    //    StatementType = "Update";
                                    //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                    //}
                                    //objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                                    //objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DCNo;
                                    //objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DCdate;
                                    //objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                    //objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                                    //objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.Refdate;
                                    //objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Customer;
                                    //objCmd.Parameters.Add("JOBORDNO", OracleDbType.NVarchar2).Value = cy.Jobid;
                                    //objCmd.Parameters.Add("RECDBY", OracleDbType.NVarchar2).Value = cy.Recieved;

                                    //objCmd.Parameters.Add("DESPBY", OracleDbType.NVarchar2).Value = cy.Despatch;
                                    //objCmd.Parameters.Add("INSPBY", OracleDbType.NVarchar2).Value = cy.Inspected;
                                    //objCmd.Parameters.Add("DOCTHROUGH", OracleDbType.NVarchar2).Value = cy.Doc;
                                    //objCmd.Parameters.Add("CNAME", OracleDbType.NVarchar2).Value = cy.CName;


                                    //objCmd.Parameters.Add("SMSDATE", OracleDbType.NVarchar2).Value = cy.Emaildate;
                                    //objCmd.Parameters.Add("SENDSMS", OracleDbType.NVarchar2).Value = cy.Send;
                                    //objCmd.Parameters.Add("DELIVAT", OracleDbType.NVarchar2).Value = cy.Deliver;
                                    //objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;



                                    //objCmd.Parameters.Add("CREATED_BY", OracleDbType.Date).Value = DateTime.Now;
                                    //objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = cy.CreatedOn;
                                    //objCmd.Parameters.Add("UPDATED_BY", OracleDbType.Date).Value = DateTime.Now;
                                    //objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = cy.UpdatedOn;
                                    //objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Y";
                                    //objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    //objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    //try
                                    //{

                                    //    objConn.Open();
                                    //    objCmd.ExecuteNonQuery();
                                    //    Object Pid = objCmd.Parameters["OUTID"].Value;
                                    string loctype = datatrans.GetDataString("SELECT LOCATIONTYPE FROM LOCDETAILS WHERE LOCDETAILSID='" + cy.Location + "'");
                                    command.CommandText = "Insert into EDCBASIC (BRANCHID,DOCID,DOCDATE,LOCID,REFNO,REFDATE,PARTYID,JOBORDNO,RECDBY,DESPBY,INSPBY,DOCTHROUGH,CNAME,SMSDATE,SENDSMS,DELIVAT,NARRATION,APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,PARTYNAME,PARTYMASTID,LOCTYPE,JOBORDDT,PTERMS,COUNTRY,TGWT,TNWT) VALUES ('" + cy.Branch + "','" + cy.DCNo + "','" + cy.DCdate + "','" + cy.Location + "','" + cy.Ref + "','" + cy.Refdate + "','" + cy.Customerid + "','" + cy.Jobid + "','" + cy.Recieved + "','" + cy.Despatch + "','OWN','" + cy.Doc + "','" + cy.CName + "','" + cy.DCdate + "','NO','" + cy.Deliver + "','" + cy.Narration + "','0','0','F','0','0','" + cy.Customer + "','" + cy.Customerid + "','" + loctype + "','','','','','') RETURNING EDCBASICID INTO :LASTCID";
                                    command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    command.ExecuteNonQuery();
                                    string Pid = command.Parameters["LASTCID"].Value.ToString();

                                    command.Parameters.Clear();

                                    //string Pid = "0";
                                    if (cy.ID != null)
                                    {
                                        Pid = cy.ID;
                                    }
                                    if (cy.ExportDCLst != null)
                                    {
                                        if (cy.ID == null)
                                        {
                                            int r = 1;
                                            foreach (ExportDCItem cp in cy.ExportDCLst)
                                            {
                                                string unit = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");
                                                DataTable item = datatrans.GetData("SELECT LOTYN,SERIALYN,ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.saveItemId +"'");
                                                if (cp.saveItemId != "0")
                                                {
                                                    string[] drumidlist = null;
                                                    //double brate = Math.Round(acssamt / cp.quantity, 2);
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
                                                    command.CommandText = "Insert into EDCDETAIL (EDCBASICID,ITEMID,PACKSPEC,UNIT,MQTY,ISSRATE,ISSAMT,LOTYN,SERIALYN,SEALNO,ORDQTY,CLSTOCK,DCQTY,QTY,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,DRUMDESC,CNO,TWT,VECHILENO,MATSUPP,M,CITEMID,EDCDETAILROW,SL,OPSTOCK,INVOICEDQTY,JODETAILID,TDRUM,PRIUNIT,UNNO) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + cp.des + "','" + unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.Amount + "','" + item.Rows[0]["LOTYN"].ToString() + "','" + item.Rows[0]["SERIALYN"].ToString() + "','" + cp.Seal + "','" + cp.quantity + "','" + cp.Current + "','" + cp.quantity + "','" + cp.quantity + "','" + cp.discamt + "','" + cp.CashDisc + "','" + cp.Introduction + "','" + cp.Trade + "','" + cp.Addition + "','" + cp.Special + "','" + cp.Fright + "','" + cp.Drum + "','" + cp.Container + "','" + cp.Tare + "','" + cp.Vechile + "','OWN','Aluminium Powder Uncoated','" + item.Rows[0]["ITEMID"].ToString() + "','"+ r + "','"+ r + "','0','0','"+ cp.jodetid + "','"+ totdrums + "','0','0') RETURNING EDCDETAILID INTO :stkid";
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
                                                            command.CommandText = "Insert into EDCLOT (EDCBASICID,PARENTRECORDID,EDCLOTROW,DRUMNO,LSTOCK,LITEMID,PWT,BWT,LOTNO,LOTSTOCK,LQTY,LRATE,LAMOUNT,PARENTROW,LLOCID,LDNO,LBIN,LDOCDATE,LDOCID) VALUES ('" + Pid + "','" + did + "','"+n+"','" + dt.Rows[0]["DRUMNO"].ToString() + "','"+ dt.Rows[0]["PLUSQTY"].ToString() + "','" + cp.saveItemId + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','0','" + dt.Rows[0]["LOTNO"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["PLUSQTY"].ToString() + "','" + dt.Rows[0]["RATE"].ToString() + "','" + amt + "','" + r + "','" + cy.Location + "','1','0','"+cy.DCdate+"','"+cy.DCNo+"')";
                                                            //OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                                            command.ExecuteNonQuery();

                                                            //datatrans.UpdateStatus("UPDATE NEWSEQUENCE SET LASTNO ='" + (plstockvalue + 1).ToString() + "' where NAME='plstockvalue'");
                                                            n = n + 1;
                                                        }
                                                         
                                                       
                                                    }
                                                }


                                                r++;
                                            }
                                        }

                                    }

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

        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,QTY,RATE,LOTNO,EJODRUMALLOCATIONDETAILID,PLSTOCKID from EJODRUMALLOCATIONDETAIL D,EJODRUMALLOCATIONBASIC B where B.EJODRUMALLOCATIONBASICID=D.EJODRUMALLOCATIONBASICID AND B.EJOSCHEDULEID= '" + id + "' AND B.IS_ALLOCATE='Y' ORDER BY DRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
