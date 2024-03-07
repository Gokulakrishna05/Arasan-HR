using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class ProFormaInvoiceService : IProFormaInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProFormaInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<ProFormaInvoice> GetAllProFormaInvoice(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }
            List<ProFormaInvoice> cmpList = new List<ProFormaInvoice>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,REFNO,PINVBASICID from PINVBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=PINVBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProFormaInvoice cmp = new ProFormaInvoice
                        {
                            ID = rdr["PINVBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            RefNo = rdr["REFNO"].ToString(),
                            //status = rdr["STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
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
        public DataTable EditProFormaInvoiceDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,TARIFFID,CGST,SGST,IGST,TOTEXAMT,SPINVBASICID from SPINVDETAIL left outer join ITEMMASTER on ITEMMASTERID =SPINVDETAIL.ITEMID left outer join UNITMAST on UNITMASTID=SPINVDETAIL.UNIT where SPINVBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ProFormaInvoiceCRUD(ProFormaInvoice cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                string currencys = datatrans.GetDataString("Select SYMBOL from CURRENCY where CURRENCYID='" + cy.Currency + "' ");
                 
                DataTable partyid = datatrans.GetData("Select PARTYMASTID,ACCOUNTNAME,MOBILE,GSTNO from PARTYMAST where PARTYNAME='" + cy.Party + "' ");
                
                string narr = "Pro-forma Invoice To " + cy.Party;
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PInv' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "PInv", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PInv' AND ACTIVESEQUENCE ='T'";
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
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PINVBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

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


                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    //objCmd.Parameters.Add("WORKORDER", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("SYMBOL", OracleDbType.NVarchar2).Value = currencys;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = partyid.Rows[0]["PARTYMASTID"].ToString(); ;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.Party;
                    //objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = party;
                    objCmd.Parameters.Add("SALVAL", OracleDbType.NVarchar2).Value = cy.SalesValue;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("AMTWORDS", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("BANK", OracleDbType.NVarchar2).Value = cy.BankName;
                    objCmd.Parameters.Add("ACNO", OracleDbType.NVarchar2).Value = cy.AcNo;
                    objCmd.Parameters.Add("SHIPADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = narr;
                    objCmd.Parameters.Add("JOPID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("CUSTACC", OracleDbType.NVarchar2).Value = partyid.Rows[0]["ACCOUNTNAME"].ToString(); ;
                    objCmd.Parameters.Add("RNDOFF", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("MOBILE", OracleDbType.NVarchar2).Value = partyid.Rows[0]["MOBILE"].ToString(); ;
                    objCmd.Parameters.Add("GSTNO", OracleDbType.NVarchar2).Value = partyid.Rows[0]["GSTNO"].ToString(); ;
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
                        if (cy.ProFormalst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProFormaInvoiceDetail cp in cy.ProFormalst)
                                {
                                    string UnitId = datatrans.GetDataString("Select  UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                    string ItemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.itemid + "' ");

                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into SPINVDETAIL (SPINVBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,TARIFFID,CGST,SGST,IGST,TOTEXAMT) VALUES ('" + Pid + "','" + ItemId + "','" + cp.itemdes + "','" + UnitId + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "','" + cp.discount + "','" + cp.itrodis + "''" + cp.cashdisc + "','" + cp.tradedis + "','" + cp.additionaldis + "','" + cp.dis + "','" + cp.frieght + "','" + cp.tariff + "','" + cp.cgst + "','" + cp.sgst + "','" + cp.igst + "','" + cp.totamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete SPINVDETAIL WHERE SPINVBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProFormaInvoiceDetail cp in cy.ProFormalst)
                                {
                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.unit + "' ");
                                    string ItemId = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + cp.itemid + "' ");

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into SPINVDETAIL (SPINVBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,TARIFFID,CGST,SGST,IGST,TOTEXAMT) VALUES ('" + Pid + "','" + ItemId + "','" + cp.itemdes + "','" + UnitId + "','" + cp.qty + "','" + cp.rate + "','" + cp.amount + "','" + cp.discount + "','" + cp.itrodis + "''" + cp.cashdisc + "','" + cp.tradedis + "','" + cp.additionaldis + "','" + cp.dis + "','" + cp.frieght + "','" + cp.tariff + "','" + cp.cgst + "','" + cp.sgst + "','" + cp.igst + "','" + cp.totamount + "')";
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
       
        public DataTable GetEditProFormaInvoice(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,PINVBASIC.DOCID,to_char(PINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(PINVBASIC.REFDATE,'dd-MON-yyyy')REFDATE,CURRENCY.MAINCURR,EXRATE,PARTYID,SALVAL,GROSS,NET,AMTWORDS,BANK,ACNO,SHIPADDRESS,NARRATION,PINVBASICID  from PINVBASIC LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PINVBASIC.MAINCURRENCY Where PINVBASIC.PINVBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
 
        public DataTable GetProFormaInvoiceDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT  PARTYMAST.PARTYNAME,JODRUMALLOCATIONBASICID from JODRUMALLOCATIONBASIC  LEFT OUTER JOIN  PARTYMAST on JODRUMALLOCATIONBASIC.CUSTOMERID=PARTYMAST.PARTYMASTID  Where PARTYMAST.TYPE IN ('Customer','BOTH') AND JOPID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
      
        //public DataTable GetProFormaInvoiceDetails(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT CURRENCY.MAINCURR,PARTYRCODE.PARTY,JOBASICID from JOBASIC LEFT OUTER JOIN CURRENCY ON JOBASIC.MAINCURRENCY=CURRENCY.CURRENCYID  LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND JOBASICID='" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public string StatusChange(string tag, string id)
 
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE PINVBASIC SET STATUS ='InActive' WHERE PINVBASICID='" + id + "'";
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
        public DataTable GetWorkOrderDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,ITEMMASTER.ITEMDESC,UNITMAST.UNITID,SUM(JODRUMALLOCATIONDETAIL.QTY) as qty,JODRUMALLOCATIONDETAIL.RATE from JODRUMALLOCATIONDETAIL left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID =JODRUMALLOCATIONDETAIL.ITEMID left outer join UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT where JODRUMALLOCATIONDETAIL.JODRUMALLOCATIONBASICID= '" + id + "' GROUP BY ITEMMASTER.ITEMID,ITEMMASTER.ITEMDESC,UNITMAST.UNITID,JODRUMALLOCATIONDETAIL.RATE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumAll(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,SUM(JODRUMALLOCATIONDETAIL.QTY) as totqty,SUM(JODRUMALLOCATIONDETAIL.RATE) as totrate,JODRUMALLOCATIONDETAIL.ITEMID as item,JODRUMALLOCATIONBASICID from JODRUMALLOCATIONDETAIL left outer join ITEMMASTER on ITEMMASTER.ITEMMASTERID =JODRUMALLOCATIONDETAIL.ITEMID  where JODRUMALLOCATIONDETAIL.JODRUMALLOCATIONBASICID= '" + id + "' GROUP BY ITEMMASTER.ITEMID , JODRUMALLOCATIONDETAIL.ITEMID,JODRUMALLOCATIONBASICID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CUSTOMERID from JODRUMALLOCATIONBASIC   where JODRUMALLOCATIONBASIC.JODRUMALLOCATIONBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrumDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMNO,QTY,RATE,LOTNO,JODRUMALLOCATIONDETAILID from JODRUMALLOCATIONDETAIL   where JODRUMALLOCATIONDETAIL.JODRUMALLOCATIONBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
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
        public DataTable GetAllListProFormaInvoiceItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(PINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,PINVBASIC.PARTYNAME,PINVBASICID from PINVBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=PINVBASIC.BRANCHID WHERE PINVBASIC.STATUS='Active' ORDER BY PINVBASIC.PINVBASICID DESC";
            }
            else
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,DOCID,to_char(PINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,PINVBASIC.PARTYNAME,PINVBASICID from PINVBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=PINVBASIC.BRANCHID WHERE PINVBASIC.STATUS='InActive' ORDER BY PINVBASIC.PINVBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
