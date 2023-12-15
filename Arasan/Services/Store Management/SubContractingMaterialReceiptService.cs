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
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,ITEMMASTER.LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID  Where ITEMMASTER.ITEMMASTERID='" + itemId + "'";
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
            SvSql = "Select DOCID,SUBCONTDCBASICID from SUBCONTDCBASIC";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSubRecvItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUBCONTEDET.RITEM,ITEMMASTER.ITEMID,RUNIT,ERATE,EAMOUNT,ERQTY,SUBCONTEDETID from SUBCONTEDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SUBCONTEDET.RITEM WHERE SUBCONTDCBASICID='" + id + "'";

            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItems(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,LOTYN,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,ITEMMASTER.LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID  Where ITEMMASTER.ITEMMASTERID='" + itemId + "'";
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
                    objCmd.Parameters.Add("FROMLOCATION", OracleDbType.NVarchar2).Value = cy.Location;
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

                                        svSQL = "Insert into SUBACTMRDET (SUBMRBASICID,MITEMID,MITEMMASTERID,MUNIT,MSUBQTY,MRQTY,MRRATE,MRAMOUNT) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.unit + "','" + cp.qty + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "') RETURNING SUBACTMRDETID INTO :LASTCID";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);

                                        objCmds.ExecuteNonQuery();

                                        string detid = objCmds.Parameters["LASTCID"].Value.ToString();



                                        string[] Ddrum = cp.drumno.Split('-');
                                        string[] Dqty = cp.dqty.Split('-');
                                        string[] Drate = cp.drate.Split('-');
                                        string[] Damount = cp.damount.Split('-');
                                        for (int i = 0; i < Ddrum.Length; i++)
                                        {

                                            string dddrum = Ddrum[i];
                                            string ddqty = Dqty[i];
                                            string ddrate = Drate[i];
                                            string ddamount = Damount[i];
                                            string itemname = datatrans.GetDataString("Select ITEMID  FROM ITEMMASTER where   ITEMMASTERID='" + cp.itemid + "'");

                                            string drumname = datatrans.GetDataString("Select DRUMNO  FROM DRUMMAST where DRUMMASTID='" + dddrum + "'");
                                            string partyname = datatrans.GetDataString("Select PARTYNAME  FROM PARTYMAST where PARTYMASTID='" + cy.Supplier + "'");

                                            string item = itemname;
                                            string sup = partyname;
                                            string drum = drumname;
                                            string doc = cy.DocId;

                                            string lotnumber = string.Format("{0}--{1}--{2}--{3}", item, sup, drum, doc);


                                            if (cp.Isvalid == "Y" && cp.drumno != "0")
                                            {

                                                svSQL = "Insert into SUBACTMRLOT (SUBMRBASICID,PARENTRECORDID,MLITEMID,MLITEMMASTERID,ACTUALDRUM,MLQTY,MLRATE,MLAMOUNT,MLDRUMNO,MLLOTNO) VALUES ('" + Pid + "','" + detid + "','" + cp.itemid + "','" + cp.itemid + "','" + dddrum + "','" + ddqty + "','" + ddrate + "','" + ddamount + "','" + dddrum + "','" + lotnumber + "')";
                                                objCmds = new OracleCommand(svSQL, objConn);
                                                objCmds.ExecuteNonQuery();

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


                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into SUBMRDETAIL (SUBMRBASICID,FGITEMID,FGITEMDESC,UNIT,RECQTY,COSTRATE,AMOUNT) VALUES ('" + Pid + "','" + cp.itemid + "','" + lotnumber + "','" + cp.unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        //if (cy.drumlst != null)
                        //{
                        //    if (cy.ID == null)
                        //    {
                        //        foreach (DrumItem cp in cy.drumlst)
                        //        {

                        //            string item = cp.item;
                        //            string sup = cy.Supplier;
                        //            string drum = cp.drumno;
                        //            string doc = cy.DocNo;

                        //            string lotnumber = string.Format("{0}--{1}--{2}--{3}", item, sup, drum, doc);


                        //            if (cp.Isvalid == "Y" && cp.itemid != "0")
                        //            {

                        //                svSQL = "Insert into RECFSUBBATCH (RECFSUBBASICID,PARENTRECORDID,BITEMID,BITEMMASTERID,DRUMMASTID,BQTY,BRATE,BAMOUNT,DRUMNO,LOTNO) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.itemid + "','" + cp.drumno + "','" + cp.qty + "','"+cp.rate+"','" + cp.amount + "','"+cp.drumno+"','"+ lotnumber+"')";
                        //                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        //                objCmds.ExecuteNonQuery();

                        //            }
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
            SvSql = "SELECT SUBMRBASICID,PARENTRECORDID,SUBACTMRLOT.MLQTY,SUBACTMRLOT.MLRATE,SUBACTMRLOT.MLAMOUNT,DRUMMAST.DRUMNO,SUBACTMRLOT.MLLOTNO FROM SUBACTMRLOT LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=SUBACTMRLOT.MLDRUMNO WHERE PARENTRECORDID='" + id + "'";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
