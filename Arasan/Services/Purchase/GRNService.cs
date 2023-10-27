using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class GRNService : IGRN
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public GRNService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<GRN> GetAllGRN(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Yes";
            }
            List<GRN> cmpList = new List<GRN>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,GRNBLBASIC.DOCID,QCSTATUS,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,GRNBLBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,GRNBLBASIC.GRNBLBASICID,GRNBLBASIC.STATUS,POBASIC.DOCID as PONO,GRNBLBASIC.IS_ACTIVE from GRNBLBASIC LEFT OUTER JOIN POBASIC ON POBASIC.POBASICID=GRNBLBASIC.POBASICID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=GRNBLBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=GRNBLBASIC.MAINCURRENCY  ORDER BY GRNBLBASIC.GRNBLBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GRN cmp = new GRN
                        {
                            ID = rdr["GRNBLBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            PONo = rdr["PONO"].ToString(),
                            GRNNo = rdr["DOCID"].ToString(),
                            GRNdate = rdr["DOCDATE"].ToString(),
                            ExRate = rdr["EXRATE"].ToString(),
                            Cur = rdr["MAINCURR"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            Status = rdr["STATUS"].ToString(),
                            Active = rdr["IS_ACTIVE"].ToString(),
                            Qcstatus = rdr["QCSTATUS"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public IEnumerable<POItem> GetAllGRNItem(string id)
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

        public DataTable GetGRNbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,POBASIC.FREIGHT,POBASIC.GROSS,POBASIC.NET from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditGRNbyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNBLBASIC.BRANCHID,GRNBLBASIC.DOCID,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,GRNBLBASIC.EXRATE,GRNBLBASIC.MAINCURRENCY,PARTYMAST.PARTYNAME,GRNBLBASIC.PARTYID,GRNBLBASICID,GRNBLBASIC.STATUS,POBASIC.DOCID as PONO,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') PODate,GRNBLBASIC.PACKING_CHRAGES,GRNBLBASIC.OTHER_CHARGES,GRNBLBASIC.OTHER_DEDUCTION,GRNBLBASIC.ROUND_OFF_PLUS,GRNBLBASIC.ROUND_OFF_MINUS,GRNBLBASIC.NARRATION,GRNBLBASIC.REFNO,to_char(GRNBLBASIC.REFDT,'dd-MON-yyyy') REFDT,GRNBLBASIC.FREIGHT,GRNBLBASIC.GROSS,GRNBLBASIC.NET,DESPTHRU,LRNO,to_char(LRDT,'dd-MON-yyyy') LRDT,TRNSPNAME,truckno from GRNBLBASIC LEFT OUTER JOIN POBASIC ON POBASIC.POBASICID=GRNBLBASIC.POBASICID LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=GRNBLBASIC.PARTYID  Where  GRNBLBASIC.GRNBLBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNItembyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select GRNBLDETAIL.QTY,GRNBLDETAIL.GRNBLBASICID,GRNBLDETAIL.ITEMID,UNITMAST.UNITID,GRNBLDETAIL.RATE,CGSTP,CGST,SGSTP,SGST,IGSTP,IGST,TOTAMT,DISCPER,DISC,PURTYPE from GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GRNBLDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where GRNBLDETAIL.GRNBLBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable FetchAccountRec(string GRNId)
        {
            string SvSql = string.Empty;
            SvSql = "select PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,CGST,SGST,IGST,FREIGHT,GROSS,NET,TOT_DISC from GRNBLBASIC where GRNBLBASICID='" + GRNId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable LedgerList()
        {
            string SvSql = string.Empty;
            SvSql = "select MASTERID,DISPNAME from MASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GRNCRUD(GRN cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("GRNPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                   
                     StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.GRNID;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    if (!string.IsNullOrEmpty(cy.RefDate))
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    }
                    else
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DBNull.Value;
                    }
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("OTHER_CHARGES", OracleDbType.NVarchar2).Value = cy.Othercharges;
                    objCmd.Parameters.Add("ROUND_OFF_PLUS", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("PACKING_CHRAGES", OracleDbType.NVarchar2).Value = cy.Packingcharges;
                    objCmd.Parameters.Add("OTHER_DEDUCTION", OracleDbType.NVarchar2).Value = cy.otherdeduction;
                    objCmd.Parameters.Add("ROUND_OFF_MINUS", OracleDbType.NVarchar2).Value = cy.Roundminus;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("LRNO", OracleDbType.NVarchar2).Value = cy.LRno;
                    if (!string.IsNullOrEmpty(cy.LRdate)) {
                        objCmd.Parameters.Add("LRDT", OracleDbType.Date).Value = DateTime.Parse(cy.LRdate);
                    }
                    else
                    {
                        objCmd.Parameters.Add("LRDT", OracleDbType.Date).Value = DBNull.Value;
                    }
                    objCmd.Parameters.Add("TRNSPNAME", OracleDbType.NVarchar2).Value = cy.drivername;
                    objCmd.Parameters.Add("DESPTHRU", OracleDbType.NVarchar2).Value = cy.dispatchname;
                    objCmd.Parameters.Add("TRUCKNO", OracleDbType.NVarchar2).Value = cy.truckno;
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
                                        Sql = "Update GRNBLDETAIL SET  QTY= '" + cp.BillQty + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "',AMOUNT='" + cp.Amount + "',DISCPER='" + cp.DiscPer + "',DISC='" + cp.DiscAmt + "',PURTYPE='" + cp.Purtype + "',CGSTP='" + cp.CGSTPer + "',CGST='" + cp.CGSTAmt + "',SGSTP='" + cp.SGSTPer + "',SGST='" + cp.SGSTAmt + "',IGSTP='" + cp.IGSTPer + "',IGST='" + cp.IGSTAmt + "',TOTAMT='" + cp.TotalAmount + "',COSTRATE='" + cp.CostRate + "',ORDQTY='" + cp.Quantity + "',GOOD_QTY='" + cp.Goodqty + "',DAMAGE_QTY='" + cp.DamageQty + "',LOT_NO='"+ cp.Lotno +"' where GRNBLBASICID='" + cy.GRNID + "'  AND ITEMID='" + cp.saveItemId + "' ";
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

                                /////////////////////////Inventory details

                                string GRNITEMID = datatrans.GetDataString("Select GRNBLDETAILID from GRNBLDETAIL where GRNBLBASICID='" + cy.GRNID + "'  AND ITEMID='" + cp.saveItemId + "' ");
                                //string wcid = datatrans.GetDataString("Select WCBASICID from WCBASIC where ILOCATION='10001000000827' ");
                                using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                {  
                                    OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                    objCmdI.CommandType = CommandType.StoredProcedure;
                                    objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                    objCmdI.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = GRNITEMID;
                                    objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.GRNdate);
                                    objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = cp.BillQty;
                                    objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = cp.BillQty;
                                    objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = cp.DamageQty;
                                    objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = "10001000000827";
                                    objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchID;
                                  
                                    objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = cp.Lotno;
                                    objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    objConnI.Open();
                                    objCmdI.ExecuteNonQuery();
                                    Object Invid = objCmdI.Parameters["OUTID"].Value;

                                    using (OracleConnection objConnIn = new OracleConnection(_connectionString))
                                    {
                                        OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                        objCmdIn.CommandType = CommandType.StoredProcedure;
                                        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                        objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.saveItemId;
                                        objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = GRNITEMID;
                                        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                        objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "GRN";
                                        objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                        objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = cp.BillQty;
                                        objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "GRN";
                                        objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = "10001000000827";
                                        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchID;
                                        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                        objConnIn.Open();
                                        objCmdIn.ExecuteNonQuery();
                                        objConnIn.Close();

                                    }

                                        objConnI.Close();



                                }
                                string status = "GRN Completed";
                                if (cp.PendingQty  > 0)
                                {
                                    status= "Partially Completed";
                                }
                                bool result = datatrans.UpdateStatus("UPDATE GRNBLBASIC SET STATUS='"+ status  + "' Where GRNBLBASICID='" + cy.GRNID + "'");

                                /////////////////////////Inventory details


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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE GRNBLBASIC SET IS_ACTIVE ='No' WHERE GRNBLBASICID='" + id + "'";
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

    }
}
