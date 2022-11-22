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
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,PARTYRCODE.PARTY, DOCID,  DOCDATE,VOUCHER, REFDT,LOCID,MAINCURRENCY,GROSS,NET,FREIGHT,OTHERCH,RNDOFF,OTHERDISC,LRCH,DELCH,NARR,DPBASICID from DPBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=DPBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on DPBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY DPBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectPurchase cmp = new DirectPurchase
                        {

                            DPId = rdr["DPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Voucher = rdr["VOUCHER"].ToString(),
                            RefDate = rdr["REFDT"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Currency = rdr["MAINCURRENCY"].ToString(),
                            Narration = rdr["NARR"].ToString()
                            // Gross = rdr["GROSS"].ToString(),
                            //// net = rdr["NET"].ToString(),
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
            SvSql = "Select DPDETAIL.QTY,DPDETAIL.DPDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from DPDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DPDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchase(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,PARTYID,DOCID,DOCDATE,VOUCHER,REFDT,LOCID,MAINCURRENCY,GROSS,NET,FREIGHT,OTHERCH,RNDOFF,OTHERDISC,LRCH,DELCH,NARR,DPBASICID  from DPBASIC where DPBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select SGCODE,ITEMSUBGROUPID FROM ITEMSUBGROUP";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DirectPurchase GetDirectPurById(string eid)
        {
            DirectPurchase DirectPurchase = new DirectPurchase();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,PARTYID,DOCID,DOCDATE,VOUCHER,REFDT,LOCID,MAINCURRENCY,GROSS,NET,FREIGHT,OTHERCH,RNDOFF,OTHERDISC,LRCH,DELCH,NARR,DPBASICID  from DPBASIC where DPBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectPurchase cmp = new DirectPurchase
                        {
                            DPId = rdr["DPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTYID"].ToString(),
                            DocNo = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Voucher = rdr["VOUCHER"].ToString(),
                            RefDate = rdr["REFDT"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Currency = rdr["MAINCURRENCY"].ToString(),
                            Narration = rdr["NARR"].ToString()
                            //  Gross = rdr["GROSS"].ToString(),
                            ////  net = rdr["NET"].ToString(),
                            //  Frig = rdr["FREIGHT"].ToString(),
                            //  Other = rdr["OTHERCH"].ToString(),
                            //  Round = rdr["RNDOFF"].ToString(),
                            //  SpDisc = rdr["OTHERDISC"].ToString(),
                            //  LRCha = rdr["LRCH"].ToString(),
                            //  DelCh = rdr["DELCH"].ToString(),


                        };

                        DirectPurchase = cmp;
                    }
                }
            }
            return DirectPurchase;
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
                    if (cy.DPId == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.DPId;

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
                        if (cy.DPId != null)
                        {
                            Pid = cy.DPId;
                        }



                        foreach (DirItem cp in cy.DirLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("DPDETAILPROC", objConns);
                                    if (cy.DPId == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }

                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("DPID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMIDS", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QUANTITY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNITP", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("Rat", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("Amou", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("TotAmo", OracleDbType.NVarchar2).Value = cp.TotalAmount;
                                    objCmds.Parameters.Add("Tariff", OracleDbType.NVarchar2).Value = cp.TariffId;
                                    objCmds.Parameters.Add("Cons", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("CostRa", OracleDbType.NVarchar2).Value = cp.CostRate;
                                    objCmds.Parameters.Add("Discount", OracleDbType.NVarchar2).Value = cp.Disc;
                                    objCmds.Parameters.Add("DiscountAmo", OracleDbType.NVarchar2).Value = cp.DiscAmount;
                                    objCmds.Parameters.Add("Assess", OracleDbType.NVarchar2).Value = cp.Assessable;
                                    objCmds.Parameters.Add("PType", OracleDbType.NVarchar2).Value = cp.PurType;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            }
                        }
                        foreach (DirItem cp in cy.DirLst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update DPDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.ConFac + "'  where DPBASICID='" + cy.DPId + "'  AND ITEMID='" + cp.saveItemId + "' ";
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

        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCurency()
        {
            string SvSql = string.Empty;
            SvSql = "Select MAINCURR || ' - ' || SYMBOL  as Cur,CURRENCYID from CURRENCY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE ITEMGROUP='" + value + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMGROUPID,GROUPCODE from itemgroup";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
