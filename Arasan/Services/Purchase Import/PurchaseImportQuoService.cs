using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace Arasan.Services
{
    public class PurchaseImportQuoService : IPurchaseImportQuo
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseImportQuoService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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


        public IEnumerable<ImportQoItem> GetAllPurQuotationItem(string id)
        {
            List<ImportQoItem> cmpList = new List<ImportQoItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select IPURQUOTDETAIL.QTY,IPURQUOTDETAIL.IPURQUOTDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from IPURQUOTDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURQUOTDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where IPURQUOTDETAIL.IPURQUOTBASICID='" + id + "' ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ImportQoItem cmp = new ImportQoItem
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

        public DataTable GetPurchaseQuoItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURQUOTDETAIL.QTY,IPURQUOTDETAIL.IPURQUOTDETAILID,IPURQUOTDETAIL.ITEMID,UNITMAST.UNITID,IPURQUOTDETAIL.RATE  from IPURQUOTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=IPURQUOTDETAIL.UNIT  where IPURQUOTDETAIL.IPURQUOTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurQuoteItem(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURQUOTDETAIL.QTY,IPURQUOTDETAIL.IPURQUOTDETAILID,ITEMMASTER.ITEMID,IPURQUOTDETAIL.UNIT,UNITMAST.UNITID,IPURQUOTDETAIL.RATE from IPURQUOTDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURQUOTDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=IPURQUOTDETAIL.UNIT  where IPURQUOTDETAIL.IPURQUOTBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurQuoteDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURQUOTDETAIL.QTY,IPURQUOTDETAIL.IPURQUOTDETAILID,ITEMMASTER.ITEMID,IPURQUOTDETAIL.UNIT,UNITMAST.UNITID,IPURQUOTDETAIL.RATE from IPURQUOTDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURQUOTDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=IPURQUOTDETAIL.UNIT  where IPURQUOTDETAIL.IPURQUOTBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseQuoDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,PARTYMAST.PARTYNAME from IPURQUOTBASIC LEFT OUTER JOIN  PARTYMAST on IPURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND IPURQUOTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseQuo(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURQUOTBASIC.BRANCHID, IPURQUOTBASIC.DOCID,to_char(IPURQUOTBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,IPURQUOTBASIC.PARTYID,IPURENQBASIC.ENQNO,to_char(IPURENQBASIC.ENQDATE,'dd-MON-yyyy')ENQDATE,IPURQUOTBASIC.MAINCURRENCY,IPURQUOTBASIC.ENQNO as enq,IPURQUOTBASIC.EXRATE,IPURQUOTBASICID from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC ON IPURENQBASIC.IPURENQBASICID=IPURQUOTBASIC.ENQNO where IPURQUOTBASIC.IPURQUOTBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<PurchaseQuo> GetAllPurQuotation(string st, string ed)
        {

            List<PurchaseQuo> cmpList = new List<PurchaseQuo>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {


                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    if (st != null && ed != null)
                    {
                        cmd.CommandText = "Select  BRANCHMAST.BRANCHID, DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,IPURENQBASIC.ENQDATE,MAINCURRENCY,EXRATE,IPURQUOTBASICID,IPURQUOTBASIC.STATUS,IPURQUOTBASIC.IS_ACTIVE from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURENQBASIC.IPURENQBASICID=IPURQUOTBASIC.ENQNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') and  IPURQUOTBASIC.DOCDATE BETWEEN '" + st + "'  AND ' " + ed + "' ORDER BY IPURQUOTBASIC.IPURQUOTBASICID DESC";

                    }
                    else
                    {
                        cmd.CommandText = "Select  BRANCHMAST.BRANCHID, DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,IPURENQBASIC.ENQDATE,MAINCURRENCY,EXRATE,IPURQUOTBASICID,IPURQUOTBASIC.STATUS,IPURQUOTBASIC.IS_ACTIVE from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURENQBASIC.IPURENQBASICID=IPURQUOTBASIC.ENQNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') and  IPURQUOTBASIC.DOCDATE  > sysdate-30 order by IPURQUOTBASICID desc ";

                    }
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseQuo cmp = new PurchaseQuo
                        {
                            ID = rdr["IPURQUOTBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            QuoId = rdr["DOCID"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            status = rdr["STATUS"].ToString(),
                            EnqNo = rdr["ENQNO"].ToString(),
                            EnqDate = rdr["ENQDATE"].ToString(),
                            Currency = rdr["MAINCURRENCY"].ToString(),
                            ExRate = rdr["EXRATE"].ToString(),
                            Active = rdr["IS_ACTIVE"].ToString(),

                        };
                        cmpList.Add(cmp);
                    }
                }
            }

            return cmpList;
        }


        //public PurchaseQuo GetPurQuotationById(string eid)
        //{
        //    PurchaseQuo Quotation = new PurchaseQuo();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select BRANCHID, DOCID,DOCDATE,PARTYID,ENQNO,ENQDATE,MAINCURRENCY,PURQUOTBASICID from PURQUOTBASIC where PURQUOTBASICID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                PurchaseQuo cmp = new PurchaseQuo
        //                {
        //                    ID = rdr["PURQUOTBASICID"].ToString(),
        //                    Branch = rdr["BRANCHID"].ToString(),
        //                    QuoId = rdr["DOCID"].ToString(),
        //                    DocDate = rdr["DOCDATE"].ToString(),
        //                    Supplier = rdr["PARTYID"].ToString(),

        //                    EnqNo = rdr["ENQNO"].ToString(),
        //                    EnqDate = rdr["ENQDATE"].ToString(),
        //                    Currency = rdr["MAINCURRENCY"].ToString()

        //                };

        //                Quotation = cmp;
        //            }
        //        }
        //    }
        //    return Quotation;
        //}
        public DataTable GetPurQuotationByName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCHMAST.BRANCHID, DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,to_char(IPURENQBASIC.ENQDATE,'dd-MON-yyyy') ENQDATE,MAINCURRENCY,IPURQUOTBASICID,IPURQUOTBASIC.STATUS from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURQUOTBASIC.ENQNO=IPURENQBASIC.IPURENQBASICID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURQUOTBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurQuotationName(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select  BRANCHMAST.BRANCHID, DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,to_char(IPURENQBASIC.ENQDATE,'dd-MON-yyyy') ENQDATE,MAINCURRENCY,IPURQUOTBASICID,IPURQUOTBASIC.STATUS from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURQUOTBASIC.ENQNO=IPURENQBASIC.IPURENQBASICID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURQUOTBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetItemSubGrp()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select SGCODE,ITEMSUBGROUPID FROM ITEMSUBGROUP";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetItemSubGroup(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public string PurQuotationCRUD(PurchaseImportQuo cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("IPURQUOPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;

                    StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.QuoId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.Enq;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.EnqDate;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.user;
                    objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = DateTime.Now;

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (ImportQoItem cp in cy.QoLst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update IPURQUOTDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.ConsFa + "'  where IPURQUOTBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
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
        public string QuotetoPO(string QuoteId)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'P.o-' AND ACTIVESEQUENCE = 'T'");
                string PONo = string.Format("{0}{1}", "P.o-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='P.o-' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into POBASIC (PARTYID,BRANCHID,QUOTNO,EXRATE,MAINCURRENCY,DOCID,DOCDATE,IS_ACTIVE) (Select PARTYID,BRANCHID,'" + QuoteId + "',EXRATE,MAINCURRENCY,'" + PONo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','Yes' from IPURQUOTBASIC where IPURQUOTBASICID='" + QuoteId + "')";
                    OracleCommand objCmd = new OracleCommand(svSQL, objConn);
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }

                string quotid = datatrans.GetDataString("Select POBASICID from POBASIC Where QUOTNO=" + QuoteId + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into PODETAIL (POBASICID,ITEMID,RATE,QTY,UNIT,CF) (Select '" + quotid + "',ITEMID,RATE,QTY,UNIT,CF FROM IPURQUOTDETAIL WHERE IPURQUOTBASICID=" + QuoteId + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE IPURQUOTBASIC SET STATUS='Generated' where IPURQUOTBASICID='" + QuoteId + "'";
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
        public IEnumerable<QuoFollowup> GetAllPurchaseFollowup()
        {
            List<QuoFollowup> cmpList = new List<QuoFollowup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select QUO_ID,EMPMAST.EMPNAME,to_char(FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,QUO_FOLLOW_ID from left outer join EMPMAST on FOLLOWED_BY =EMPMAST.EMPNAME ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QuoFollowup cmp = new QuoFollowup
                        {
                            FolID = rdr["QUO_FOLLOW_ID"].ToString(),
                            QuoNo = rdr["QUO_ID"].ToString(),
                            Followby = rdr["FOLLOWED_BY"].ToString(),
                            Followdate = rdr["FOLLOW_DATE"].ToString(),
                            Nfdate = rdr["NEXT_FOLLOW_DATE"].ToString(),
                            Rmarks = rdr["REMARKS"].ToString(),
                            Enquiryst = rdr["FOLLOW_STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetFolowup(string enqid)
        {
            string SvSql = string.Empty;
            SvSql = "Select QUOTATION_FOLLOW_UP.QUO_ID,FOLLOWED_BY,to_char(QUOTATION_FOLLOW_UP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(QUOTATION_FOLLOW_UP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,QUOTATION_FOLLOW_UP.REMARKS,QUOTATION_FOLLOW_UP.FOLLOW_STATUS,QUO_FOLLOW_ID from QUOTATION_FOLLOW_UP  WHERE QUO_ID='" + enqid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public string PurchaseFollowupCRUD(QuoFollowup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("QUOFOLLOWUPPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURCHASEFOLLOWPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.FolID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.FolID;
                    }

                    objCmd.Parameters.Add("QUO_ID", OracleDbType.NVarchar2).Value = cy.QuoNo;
                    objCmd.Parameters.Add("FOLLOWED_BY", OracleDbType.NVarchar2).Value = cy.Followby;
                    objCmd.Parameters.Add("FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.Followdate);
                    objCmd.Parameters.Add("NEXT_FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.Nfdate);
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Rmarks;
                    objCmd.Parameters.Add("FOLLOW_STATUS", OracleDbType.NVarchar2).Value = cy.Enquiryst;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE IPURQUOTBASIC SET IS_ACTIVE ='N' WHERE IPURQUOTBASICID='" + id + "'";
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
        public async Task<IEnumerable<PQuoItemDetail>> GetPQuoItem(string id)
        {
            using (OracleConnection db = new OracleConnection(_connectionString))
            {
                return await db.QueryAsync<PQuoItemDetail>("SELECT  TAAIERP.PARTYMAST.PARTYID, TAAIERP.PARTYMAST.PARTYNAME, TAAIERP.PARTYMAST.ADD1, TAAIERP.PARTYMAST.ADD2, TAAIERP.PARTYMAST.ADD3, TAAIERP.PARTYMAST.CITY, TAAIERP.PARTYMAST.PINCODE,TAAIERP.PARTYMAST.STATE, TAAIERP.PARTYMAST.CSTNO, TAAIERP.PARTYMAST.MOBILE, TAAIERP.IPURQUOTBASIC.BRANCHID,TAAIERP.IPURQUOTBASIC.DOCID, TAAIERP.IPURQUOTBASIC.DOCDATE,  TAAIERP.IPURQUOTBASIC.PARTYID AS EXPR1, TAAIERP.ITEMMASTER.ITEMID, TAAIERP.IPURQUOTDETAIL.ITEMDESC, TAAIERP.IPURQUOTDETAIL.RATE,TAAIERP.IPURQUOTDETAIL.QTY, TAAIERP.IPURQUOTDETAIL.UNIT FROM    TAAIERP.PARTYMAST INNER JOIN  TAAIERP.IPURQUOTBASIC ON TAAIERP.PARTYMAST.PARTYMASTID = TAAIERP.IPURQUOTBASIC.PARTYID INNER JOIN TAAIERP.IPURQUOTDETAIL ON TAAIERP.PURQUOTBASIC.PURQUOTBASICID=TAAIERP.IPURQUOTDETAIL.PURQUOTBASICID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=IPURQUOTDETAIL.ITEMID where IPURQUOTDETAIL.PURQUOTBASICID='" + id + "' and PURQUOTBASIC.PURQUOTBASICID ='" + id + "'", commandType: CommandType.Text);
            }
        }

        public DataTable GetAllPurchaseQuoItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,IPURQUOTBASIC.IPURQUOTBASICID, IPURQUOTBASIC.DOCID,to_char(IPURQUOTBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,IPURENQBASIC.ENQDATE,IPURQUOTBASIC.MAINCURRENCY,IPURQUOTBASIC.EXRATE,IPURQUOTBASIC.STATUS from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURENQBASIC.IPURENQBASICID=IPURQUOTBASIC.ENQNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=IPURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on PARTYMAST.PARTYMASTID =IPURQUOTBASIC.PARTYID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURENQBASIC.IS_ACTIVE='Y' ORDER BY IPURQUOTBASIC.IPURQUOTBASICID DESC";
            }
            else
            {
                SvSql = "Select  BRANCHMAST.BRANCHID,IPURQUOTBASIC.IPURQUOTBASICID, IPURQUOTBASIC.DOCID,to_char(IPURQUOTBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYMAST.PARTYNAME,IPURENQBASIC.ENQNO,IPURENQBASIC.ENQDATE,IPURQUOTBASIC.MAINCURRENCY,IPURQUOTBASIC.EXRATE,IPURQUOTBASIC.STATUS from IPURQUOTBASIC LEFT OUTER JOIN IPURENQBASIC on IPURENQBASIC.IPURENQBASICID=IPURQUOTBASIC.ENQNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=IPURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on PARTYMAST.PARTYMASTID =IPURQUOTBASIC.PARTYID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURENQBASIC.IS_ACTIVE='N' ORDER BY IPURQUOTBASIC.IPURQUOTBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
