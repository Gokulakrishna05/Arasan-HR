using Arasan.Interface;
using Arasan.Models;
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
        public IEnumerable<DirectPurchase> GetAllDirectPur()
        {
            List<DirectPurchase> cmpList = new List<DirectPurchase>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,PARTYRCODE.PARTY,DPBASIC. DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER, to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,LOCID,CURRENCY.MAINCURR,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,OTHERCH,RNDOFF,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=DPBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY DPBASIC.DPBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectPurchase cmp = new DirectPurchase
                        {

                            ID = rdr["DPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Voucher = rdr["VOUCHER"].ToString(),
                            RefDate = rdr["REFDT"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Currency = rdr["MAINCURR"].ToString(),
                            Narration = rdr["NARR"].ToString()
                            // Gross = rdr["GROSS"].ToString(),
                            // net = rdr["NET"].ToString(),
                            // Frig = rdr["FREIGHT"].ToString(),
                            // Other = rdr["OTHERCH"].ToString(),
                            // Round = rdr["RNDOFF"].ToString(),
                            // SpDisc = rdr["OTHERDISC"].ToString(),
                            // LRCha = rdr["LRCH"].ToString(),
                            // DelCh = rdr["DELCH"].ToString()



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
            SvSql = "Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,DPDETAIL.ITEMID,UNITMAST.UNITID,RATE,TOTAMT,DISC,DISCAMOUNT,IFREIGHTCH,PURTYPE,AMOUNT,CF  from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchase(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DPBASIC.BRANCHID,DPBASIC.PARTYID,DPBASIC.DOCID,to_char(DPBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,DPBASIC.VOUCHER,to_char(DPBASIC.REFDT,'dd-MON-yyyy')REFDT,DPBASIC.LOCID,DPBASIC.MAINCURRENCY,DPBASIC.GROSS,DPBASIC.NET,DPBASIC.FREIGHT,DPBASIC.OTHERCH,DPBASIC.RNDOFF,DPBASIC.OTHERDISC,DPBASIC.LRCH,DPBASIC.DELCH,DPBASIC.NARR,DPBASICID  from DPBASIC where DPBASIC.DPBASICID=" + id + "";
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

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DIRECTPURCHASEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.DocDate);
                    objCmd.Parameters.Add("VOUCHER", OracleDbType.NVarchar2).Value = cy.Voucher;
                    objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frig;
                    objCmd.Parameters.Add("OTHERCH", OracleDbType.NVarchar2).Value = cy.Other;
                    objCmd.Parameters.Add("RNDOFF", OracleDbType.NVarchar2).Value = cy.Round;
                    objCmd.Parameters.Add("OTHERDISC", OracleDbType.NVarchar2).Value = cy.SpDisc;
                    objCmd.Parameters.Add("LRCH", OracleDbType.NVarchar2).Value = cy.LRCha;
                    objCmd.Parameters.Add("DELCH", OracleDbType.NVarchar2).Value = cy.DelCh;
                    objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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



                        foreach (DirItem cp in cy.DirLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("DPDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("DPBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("PUNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.TotalAmount;
                                    objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("DISC", OracleDbType.NVarchar2).Value = cp.Disc;
                                    objCmds.Parameters.Add("DISCAMOUNT", OracleDbType.NVarchar2).Value = cp.DiscAmount;
                                   
                                   
                                    objCmds.Parameters.Add("IFREIGHTCH", OracleDbType.NVarchar2).Value = cp.FrigCharge;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        //foreach (DirItem cp in cy.DirLst)
                        //{
                        //    if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                        //    {
                        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                        //        {
                        //            string Sql = string.Empty;
                        //            if (StatementType == "Update")
                        //            {
                        //                Sql = "Update DPDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.ConFac + "',AMOUNT='" + cp.Amount + "',DISCAMOUNT='" + cp.DiscAmount + "',PURTYPE='" + cp.PurType + "',IFREIGHTCH='" + cp.FrigCharge + "',TOTAMT='" + cp.TotalAmount + "'  where DPBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
                        //            }
                        //            else
                        //            {
                        //                Sql = "";
                        //            }
                        //            OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                        //            objConnT.Open();
                        //            objCmds.ExecuteNonQuery();
                        //            objConnT.Close();
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
    }
}
