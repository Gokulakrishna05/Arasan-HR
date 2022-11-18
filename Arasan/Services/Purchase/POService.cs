using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class POService : IPO
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public POService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<PO> GetAllPO()
        {
            List<PO> cmpList = new List<PO>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYRCODE.PARTY,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY POBASIC.POBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PO cmp = new PO
                        {
                            ID = rdr["POBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoteNo = rdr["Quotno"].ToString(),
                            PONo = rdr["DOCID"].ToString(),
                            POdate = rdr["DOCDATE"].ToString(),
                            ExRate = rdr["EXRATE"].ToString(),
                            Cur = rdr["MAINCURR"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            Status = rdr["STATUS"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public IEnumerable<POItem> GetAllPOItem(string id)
        {
            List<POItem> cmpList = new List<POItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select PODETAIL.QTY,PODETAIL.PODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PODETAIL.POBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        POItem cmp = new POItem
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

        public DataTable GetPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYRCODE.PARTY,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable EditPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select POBASIC.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,POBASIC.MAINCURRENCY,POBASIC.PARTYID,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,NARRATION,PAYTERMS,DELTERMS,DESP,WARRTERMS,POBASIC.REFNO,to_char(POBASIC.REFDT,'dd-MON-yyyy') REFDT,POBASIC.FREIGHT,POBASIC.GROSS,POBASIC.NET from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO  Where  POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItembyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select PODETAIL.QTY,PODETAIL.PODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID,PODETAIL.RATE from PODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PODETAIL.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select PODETAIL.QTY,PODETAIL.PODETAILID,PODETAIL.ITEMID,UNITMAST.UNITID,PODETAIL.RATE,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTALAMT,DISCPER,DISCAMT,FREIGHTCHGS,PURTYPE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE from PODETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PODETAIL.UNIT  where PODETAIL.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PurOrderCRUD(PO cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("POPROC", objConn);
                   
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.POID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.POID;

                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("OTHER_CHARGES", OracleDbType.NVarchar2).Value = cy.Othercharges;
                    objCmd.Parameters.Add("ROUND_OFF_PLUS", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("PACKING_CHRAGES", OracleDbType.NVarchar2).Value = cy.Packingcharges;
                    objCmd.Parameters.Add("OTHER_DEDUCTION", OracleDbType.NVarchar2).Value = cy.otherdeduction;
                    objCmd.Parameters.Add("ROUND_OFF_MINUS", OracleDbType.NVarchar2).Value = cy.Roundminus;
                    objCmd.Parameters.Add("PAYTERMS", OracleDbType.NVarchar2).Value = cy.Paymentterms;
                    objCmd.Parameters.Add("DELTERMS", OracleDbType.NVarchar2).Value = cy.delterms;
                    objCmd.Parameters.Add("DESP", OracleDbType.NVarchar2).Value = cy.desp;
                    objCmd.Parameters.Add("WARRTERMS", OracleDbType.NVarchar2).Value = cy.warrantyterms;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.Recid;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        foreach (POItem cp in cy.PoItem)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update PODETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "',AMOUNT='" + cp.Amount + "',DISCPER='" + cp.DiscPer + "',DISCAMT='" + cp.DiscAmt + "',PURTYPE='" + cp.Purtype + "',FREIGHTCHGS='" + cp.FrieghtAmt + "',CGSTPER='" + cp.CGSTPer + "',CGSTAMT='" + cp.CGSTAmt + "',SGSTPER='" + cp.SGSTPer + "',SGSTAMT='" + cp.SGSTAmt + "',IGSTPER='" + cp.IGSTPer + "',IGSTAMT='" + cp.IGSTAmt + "',TOTALAMT='" + cp.TotalAmount + "' where POBASICID='" + cy.POID + "'  AND ITEMID='" + cp.saveItemId + "' ";
                                    }
                                    else
                                    {
                                        Sql = "";
                                    }
                                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                    objConnT.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConnT.Close();
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


    }
}
