using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Arasan.Interface.Store_Management;
using Nest;

namespace Arasan.Services.Store_Management
{
    public class SubContractingMaterialReceiptService : ISubContractingMaterialReceipt
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SubContractingMaterialReceiptService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetItemDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,ITEMMASTER.LATPURPRICE,LOTYN from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID  Where ITEMMASTER.ITEMMASTERID='" + itemId + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            SvSql = "Select PARTYMAST.PARTYNAME,SUBCONTDCBASIC.PARTYID from SUBCONTDCBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SUBCONTDCBASIC.PARTYID  Where SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSubDelivItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBCONTDCDETAIL.ITEMID as item,ITEMMASTER.ITEMID,SUBCONTDCDETAIL.UNIT,SUBCONTDCDETAIL.QTY,SUBCONTDCDETAIL.RATE,SUBCONTDCDETAIL.AMOUNT,SUBCONTDCDETAILID from SUBCONTDCDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTDCDETAIL.ITEMID WHERE SUBCONTDCBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDC()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,SUBCONTDCBASICID from SUBCONTDCBASIC WHERE STATUS='Approve'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSubRecvItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,UNITMAST.UNITID,BALANCE_QTY,RATE,AMOUNT,TSOURCEID,THIREDPARTY_INVENTORY.ITEMID as item FROM THIREDPARTY_INVENTORY left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID =THIREDPARTY_INVENTORY.ITEMID LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID WHERE TSOURCEBASICID='" + id + "'";
            // SvSql = "Select SUBCONTEDET.RITEM,ITEMMASTER.ITEMID,RUNIT,ERATE,EAMOUNT,ERQTY,SUBCONTEDETID from SUBCONTEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTEDET.RITEM WHERE SUBCONTDCBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItems(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,ITEMMASTER.LATPURPRICE,LOTYN from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID  Where ITEMMASTER.ITEMMASTERID='" + itemId + "'";
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
        public DataTable GetDrum()
        {
            string SvSql = string.Empty;
            SvSql = "Select DRUMNO,DRUMMASTID from DRUMMAST";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWCDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT WCBASICID,WCID FROM WCBASIC WHERE ILOCATION='" + itemId + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllSubContractingMaterialItem(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DOCID,SUBMRBASICID,PARTYMAST.PARTYNAME,to_char(SUBMRBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,LOCDETAILS.LOCID from SUBMRBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SUBMRBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBMRBASIC.FROMLOCATION  WHERE SUBMRBASIC.IS_ACTIVE='Y'";
            }
            else
            {
                SvSql = "Select DOCID,SUBMRBASICID,PARTYMAST.PARTYNAME,to_char(SUBMRBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,LOCDETAILS.LOCID from SUBMRBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SUBMRBASIC.PARTYID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBMRBASIC.FROMLOCATION  WHERE SUBMRBASIC.IS_ACTIVE='N'";

            }
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SubContractingMaterialReceiptCRUD(SubContractingMaterialReceipt cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SCMR' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "SCMR", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SCMR' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocId = docid;
                }
                string WRKID = datatrans.GetDataString("Select WCBASICID from WCBASIC where WCID='" + cy.WorkCenter + "' ");
                string fromloc = datatrans.GetDataString("Select ILOCATION from WCBASIC where PARTYID='" + cy.Supplier + "' ");

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SUBMRBASICPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }


                    objCmd.Parameters.Add("Branch", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cy.DCNo;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;

                    objCmd.Parameters.Add("TOWCID", OracleDbType.NVarchar2).Value = WRKID;
                    objCmd.Parameters.Add("TOWCBASICID", OracleDbType.NVarchar2).Value = WRKID;
                    objCmd.Parameters.Add("FROMLOCATION", OracleDbType.NVarchar2).Value = fromloc;
                    objCmd.Parameters.Add("TOLOCATION", OracleDbType.NVarchar2).Value = cy.Location;
                     
                    objCmd.Parameters.Add("TOTRQTY", OracleDbType.NVarchar2).Value = cy.qtyrec;
                    objCmd.Parameters.Add("TOTRCQTY", OracleDbType.NVarchar2).Value = cy.TotRecqty;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.enterd;
                   
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        if (cy.SubMatlilst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (SubMaterialItem cp in cy.SubMatlilst)
                                {
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into SUBACTMRDET (SUBMRBASICID,MITEMID,MITEMMASTERID,MUNIT,MSUBQTY,MRQTY,MRRATE,MRAMOUNT,MLOTYN) VALUES ('" + Pid + "','" + cp.item + "','" + cp.item + "','" + cp.unit + "','" + cp.qty + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "','" + cp.lot + "') RETURNING SUBACTMRDETID INTO :LASTCID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                        objCmds.ExecuteNonQuery();

                                        string detid = objCmds.Parameters["LASTCID"].Value.ToString();

                                        string lot = datatrans.GetDataString("Select LOTYN  FROM ITEMMASTER where   ITEMMASTERID='" + cp.item + "'");

                                        if (lot == "YES")

                                        {


                                            string[] Ddrum = cp.drumno.Split('-');
                                            string[] Dqty = cp.dqty.Split('-');
                                            string[] Drate = cp.drate.Split('-');
                                            string[] Damount = cp.damount.Split('-');
                                            int l = 1;
                                            for (int i = 0; i < Ddrum.Length; i++)
                                            {

                                                string dddrum = Ddrum[i];
                                                string ddqty = Dqty[i];
                                                string ddrate = Drate[i];
                                                string ddamount = Damount[i];
                                                string itemname = datatrans.GetDataString("Select ITEMID  FROM ITEMMASTER where   ITEMMASTERID='" + cp.item + "'");

                                            
                                                    string item = itemname;

                                                 
                                                    string doc = cy.DocId;

                                                    string lotnumber = string.Format("{0}--{1}-{2}--{3} -{4}", item, dddrum, cy.Docdate, doc, l.ToString());


                                                    if (cp.Isvalid == "Y" && cp.drumno != "0")
                                                    {

                                                        svSQL = "Insert into SUBACTMRLOT (SUBMRBASICID,PARENTRECORDID,MLITEMID,MLITEMMASTERID,ACTUALDRUM,MLQTY,MLRATE,MLAMOUNT,MLDRUMNO,MLLOTNO,PKDRUMNO,LOTROWNO,DRUMTYPE,TPRODDRUM) VALUES ('" + Pid + "','" + detid + "','" + cp.item + "','" + cp.item + "','" + dddrum + "','" + ddqty + "','" + ddrate + "','" + ddamount + "','0','" + lotnumber + "','" + dddrum + "','" + l + "','PACKDRUM','0')";
                                                        objCmds = new OracleCommand(svSQL, objConn);
                                                        objCmds.ExecuteNonQuery();


                                                        string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='" + cy.Location + "' ");
                                                        OracleCommand objCmdIn = new OracleCommand("DRUMSTKPROC", objConn);
                                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdIn.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.item;
                                                        objCmdIn.Parameters.Add("DOC_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdIn.Parameters.Add("DRUM_ID", OracleDbType.NVarchar2).Value = dddrum;
                                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = dddrum;
                                                        objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdIn.Parameters.Add("STOCKTRANSTYPE", OracleDbType.NVarchar2).Value = "SUBMAT DC";
                                                        objCmdIn.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                                        objCmdIn.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;
                                                        objCmdIn.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = ddqty;
                                                        objCmdIn.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = ddqty;
                                                        objCmdIn.Parameters.Add("OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdIn.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                        objCmdIn.ExecuteNonQuery();
                                                        Object Pid1 = objCmdIn.Parameters["OUTID"].Value;




                                                        //if (cy.ID != null)
                                                        //{
                                                        //    Pid = cy.ID;
                                                        //}




                                                        string wc = datatrans.GetDataString("Select WCID from WCBASIC where WCBASICID='" + wcid + "' ");
                                                        string it = datatrans.GetDataString("Select ITEMID from ITEMMASTER where ITEMMASTERID='" + cp.item + "' ");


                                                        string wcenter = wc;
                                                        string docid = string.Format("{0}-{1}-{2}", it, wcenter, dddrum.ToString());


                                                        OracleCommand objCmdInp = new OracleCommand("DRUMSTKDETPROC", objConn);
                                                        objCmdInp.CommandType = CommandType.StoredProcedure;
                                                        objCmdInp.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                        objCmdInp.Parameters.Add("DRUMSTKID", OracleDbType.NVarchar2).Value = Pid1;
                                                        objCmdInp.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.item;
                                                        objCmdInp.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Now;
                                                        objCmdInp.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = dddrum;
                                                        objCmdInp.Parameters.Add("DRUM", OracleDbType.NVarchar2).Value = dddrum;
                                                        objCmdInp.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                        objCmdInp.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                        objCmdInp.Parameters.Add("SOURCETYPE", OracleDbType.NVarchar2).Value = "SUBMAT DC";
                                                        objCmdInp.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                                                        objCmdInp.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = wcid;

                                                        objCmdInp.Parameters.Add("PLUSQTY", OracleDbType.NVarchar2).Value = ddqty;
                                                        objCmdInp.Parameters.Add("MINSQTY", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdInp.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                        objCmdInp.Parameters.Add("LOTNO", OracleDbType.NVarchar2).Value = docid;
                                                        objCmdInp.Parameters.Add("SHEDNO", OracleDbType.NVarchar2).Value = "";

                                                        objCmdInp.ExecuteNonQuery();


                                                    }




                                                
                                                l++;
                                            }
                                        }
                                        else
                                        {
                                            using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                            {
                                                OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                objCmdI.CommandType = CommandType.StoredProcedure;
                                                objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.item;
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
                                                objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;

                                                objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                objConnI.Open();
                                                objCmdI.ExecuteNonQuery();
                                                Object Invid = objCmdI.Parameters["OUTID"].Value;



                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.item;
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detid;
                                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = Pid;
                                                objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "SUBMAT DC";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.qty;
                                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "SUBMAT DC ";
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
                        if (cy.Contlilst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (SubContractItem cp in cy.Contlilst)
                                {



                                    string itemname = datatrans.GetDataString("Select ITEMID  FROM ITEMMASTER where   ITEMMASTERID='" + cp.itemid + "'");
                                    string lotnumber = string.Format("{0} {1} ", itemname, "TO BE (MINUS) IN CLOSING STOCK");

                                    double qqqty = Convert.ToDouble(cp.qty);
                                    double tqqqty = Convert.ToDouble(cp.recqty);
                                    double penqty = qqqty - tqqqty;
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into SUBMRDETAIL (SUBMRBASICID,FGITEMID,FGITEMDESC,UNIT,RECQTY,DCQTY,PENDQTY,COSTRATE,AMOUNT,SUBCONTEDETID) VALUES ('" + Pid + "','" + cp.itemid + "','" + lotnumber + "','" + cp.unit + "','" + cp.recqty + "','" + cp.qty + "','" + penqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.detid + "') RETURNING SUBMRDETAILID INTO :LASTCID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                        objCmds.ExecuteNonQuery();

                                        string detlid = objCmds.Parameters["LASTCID"].Value.ToString();


                                        DataTable dt = datatrans.GetData("Select ITEMID,TSOURCEID,TSOURCEBASICID,PARTYID,DOCDATE,REC_GOOD_QTY,INVENTORY_ITEM_ID,BALANCE_QTY,FINANCIAL_YEAR,CREATED_BY,CREATED_ON,LOCATION_ID,BRANCH_ID,RATE,AMOUNT from THIREDPARTY_INVENTORY where TSOURCEID='"+ cp.detid+"'  ");
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                double ddqty = Convert.ToDouble(cp.recqty);
                                                double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                                                double bqty = rqty - ddqty;

                                                string Sql = string.Empty;
                                                Sql = "Update THIREDPARTY_INVENTORY SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                objCmds = new OracleCommand(Sql, objConn);
                                                objCmds.ExecuteNonQuery();

                                                OracleCommand objCmdIn = new OracleCommand("PARTYINVITEMTRANSPROC", objConn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.itemid;
                                                objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = detlid;
                                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                objCmdIn.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "SUBMAT DC";

                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = ddqty;
                                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "SUBMAT DC ";
                                                objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.Location;
                                                objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.Branch;

                                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                                objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.amount;
                                                objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objCmdIn.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    if(penqty==0)
                                    {
                                        DataTable baasic = datatrans.GetData("Select SUBMRBASICID  FROM SUBMRDETAIL where SUBCONTEDETID='" + cp.itemid + "'");
                                        if (baasic.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < baasic.Rows.Count; i++)
                                            {
                                                string Sqla = string.Empty;
                                                Sqla = "Update SUBMRBASIC SET  STATUS='Close'  WHERE SUBMRBASICID='" + baasic.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmdssa = new OracleCommand(Sqla, objConn);

                                                objCmdssa.ExecuteNonQuery();
                                            }
                                        }
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

        public DataTable GetSubContract(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BRANCHMAST.BRANCHID,SUBMRBASIC.DOCID,to_char(SUBMRBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,SUBMRBASIC.T1SOURCEID,PARTYMAST.PARTYNAME,WCBASIC.WCID,LOCDETAILS.LOCID,TOTRQTY,TOTRCQTY,ENTEREDBY,REFNO,REFDATE,NARRATION,SUBMRBASICID FROM SUBMRBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=SUBMRBASIC.PARTYID LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=SUBMRBASIC.BRANCH LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=SUBMRBASIC.FROMLOCATION LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=SUBMRBASIC.TOWCID WHERE SUBMRBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMaterialReceipt(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUBACTMRDETID,SUBMRBASICID,ITEMMASTER.ITEMID,MUNIT,MSUBQTY,MRRATE,MRAMOUNT FROM SUBACTMRDET  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBACTMRDET.MITEMID WHERE SUBMRBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetReceiptItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUBMRBASICID,ITEMMASTER.ITEMID,UNIT,RECQTY,COSTRATE,AMOUNT FROM SUBMRDETAIL  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBMRDETAIL.FGITEMID WHERE SUBMRBASICID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumdetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUBMRBASICID,PARENTRECORDID,SUBACTMRLOT.MLQTY,SUBACTMRLOT.MLRATE,SUBACTMRLOT.MLAMOUNT,SUBACTMRLOT.ACTUALDRUM,SUBACTMRLOT.MLLOTNO FROM SUBACTMRLOT WHERE PARENTRECORDID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
