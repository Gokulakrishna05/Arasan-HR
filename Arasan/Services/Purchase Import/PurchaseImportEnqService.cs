﻿using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class PurchaseImportEnqService : IPurchaseImportEnqService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseImportEnqService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
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

        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
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
        public IEnumerable<PurchaseImportEnq> GetAllPurenquriy(string st, string ed)
        {


            List<PurchaseImportEnq> cmpList = new List<PurchaseImportEnq>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    if (st != null && ed != null)
                    {
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASICID,IPURENQBASIC.STATUS,IPURENQBASIC.ACTIVE from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') and IPURENQBASIC.ENQDATE BETWEEN '" + st + "'  AND ' " + ed + "'order by IPURENQBASICID DESC";

                    }
                    else
                    {
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASICID,IPURENQBASIC.STATUS,IPURENQBASIC.ACTIVE from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') and  IPURENQBASIC.ENQDATE > sysdate-30 order by IPURENQBASICID desc ";

                    }
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseImportEnq cmp = new PurchaseImportEnq
                        {
                            ID = rdr["IPURENQBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            RefNo = rdr["ENQNO"].ToString(),
                            Enqdate = rdr["ENQDATE"].ToString(),
                            ExRate = rdr["EXRATE"].ToString(),
                            ParNo = rdr["PARTYREFNO"].ToString(),
                            Cur = rdr["CURRENCYID"].ToString(),
                            Supplier = rdr["PARTYNAME"].ToString(),
                            Status = rdr["STATUS"].ToString(),
                            Active = rdr["ACTIVE"].ToString()


                        };
                        cmpList.Add(cmp);
                    }

                }

            }
            return cmpList;
        }

        public IEnumerable<ImportEnqItem> GetAllPurenquriyItem(string id)
        {
            List<ImportEnqItem> cmpList = new List<ImportEnqItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select IPURENQDETAIL.QTY,IPURENQDETAIL.IPURENQDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from IPURENQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURENQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where IPURENQDETAIL.IPURENQBASICID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ImportEnqItem cmp = new ImportEnqItem
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
        public DataTable GetPurchaseEnqByID(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASICID,IPURENQBASIC.STATUS from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID  Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnq(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASICID,IPURENQBASIC.STATUS from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID  Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURENQDETAIL.QTY,IPURENQDETAIL.IPURENQDETAILID,IPURENQDETAIL.ITEMID,IPURENQDETAIL.UNIT,UNITMAST.UNITID,IPURENQDETAIL.RATE,ITEMMASTER.ITEMID as ITEMNAME from IPURENQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURENQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=IPURENQDETAIL.UNIT  where IPURENQDETAIL.IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPURENQDETAIL.QTY,IPURENQDETAIL.IPURENQDETAILID,IPURENQDETAIL.ITEMID,IPURENQDETAIL.UNIT,UNITMAST.UNITID,IPURENQDETAIL.RATE,ITEMMASTER.ITEMID as ITEMNAME from IPURENQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPURENQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=IPURENQDETAIL.UNIT  where IPURENQDETAIL.IPURENQBASICID='" + id + "'";
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
        public DataTable GetRegenerateDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMASTID,IPURENQBASICID,IPURENQBASIC.STATUS,ENQREF,ASSIGNTO,ENQRECDBY  from IPURENQBASIC Where IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMASTID,IPURENQBASICID,IPURENQBASIC.STATUS,ENQREF,ASSIGNTO,ENQRECDBY  from IPURENQBASIC Where IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqFolwDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select IPURENQBASIC.ENQNO,PARTYMAST.PARTYID from IPURENQBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=IPURENQBASIC.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND IPURENQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public PurchaseEnquiry GetPurenqServiceById(string eid)
        //{
        //    PurchaseEnquiry PurchaseEnquiry = new PurchaseEnquiry();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select BRANCHID,ENQNO,ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYMASTID,PURENQID  from PURENQ where PURENQID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                PurchaseEnquiry cmp = new PurchaseEnquiry
        //                {
        //                    ID = rdr["PURENQID"].ToString(),
        //                    Branch = rdr["BRANCHID"].ToString(),
        //                    RefNo = rdr["ENQNO"].ToString(),
        //                    Enqdate = rdr["ENQDATE"].ToString(),
        //                    ExRate = rdr["EXCRATERATE"].ToString(),
        //                    ParNo = rdr["PARTYREFNO"].ToString(),
        //                    Cur = rdr["CURRENCYID"].ToString(),
        //                    Supplier = rdr["PARTYMASTID"].ToString()

        //                };

        //                PurchaseEnquiry = cmp;
        //            }
        //        }
        //    }
        //    return PurchaseEnquiry;
        //}

        public string PurenquriyCRUD(PurchaseImportEnq cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PENQ' AND ACTIVESEQUENCE = 'T'");
                    string EnqNo = string.Format("{0}{1}", "PENQ", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PENQ' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.EnqNo = EnqNo;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("IPURENQPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQREF", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.Enqdate;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYREFNO", OracleDbType.NVarchar2).Value = cy.ParNo;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Cur;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier;


                    objCmd.Parameters.Add("ENQRECDBY", OracleDbType.NVarchar2).Value = cy.EnqRecid;
                    objCmd.Parameters.Add("ASSIGNTO", OracleDbType.NVarchar2).Value = cy.Enqassignid;
                    objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.EnqRecid;
                    objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (ImportEnqItem cp in cy.EnqLst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update IPURENQDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "'  where IPURENQBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
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
        //public IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup()
        //{
        //    List<PurchaseFollowup> cmpList = new List<PurchaseFollowup>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select ENQ_ID,EMPMAST.EMPNAME,to_char(FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP LEFT OUTER JOIN EMPMAST ON EMPMASTID=ENQUIRY_FOLLOW_UP.FOLLOWED_BY";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                PurchaseFollowup cmp = new PurchaseFollowup
        //                {
        //                    FoID = rdr["ENQ_FOLLOW_ID"].ToString(),
        //                    Enqno = rdr["ENQ_ID"].ToString(),
        //                    Followby = rdr["EMPNAME"].ToString(),
        //                    Followdate = rdr["FOLLOW_DATE"].ToString(),
        //                    Nfdate = rdr["NEXT_FOLLOW_DATE"].ToString(),
        //                    Rmarks = rdr["REMARKS"].ToString(),
        //                    Enquiryst = rdr["FOLLOW_STATUS"].ToString()
        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        public DataTable GetFolowup(string enqid)
        {
            string SvSql = string.Empty;
            //SvSql = "Select ENQUIRY_FOLLOW_UP.ENQ_ID,EMPMAST.EMPNAME,to_char(ENQUIRY_FOLLOW_UP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(ENQUIRY_FOLLOW_UP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,ENQUIRY_FOLLOW_UP.REMARKS,ENQUIRY_FOLLOW_UP.FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP LEFT OUTER JOIN EMPMAST ON EMPNAME=ENQUIRY_FOLLOW_UP.FOLLOWED_BY  WHERE ENQ_ID='" + enqid + "'";
            SvSql = "Select ENQUIRY_FOLLOW_UP.ENQ_ID,FOLLOWED_BY,to_char(ENQUIRY_FOLLOW_UP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(ENQUIRY_FOLLOW_UP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,ENQUIRY_FOLLOW_UP.REMARKS,ENQUIRY_FOLLOW_UP.FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP WHERE ENQ_ID='" + enqid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        //public DataTable GetFollowupDetail(string enqid)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select ENQUIRY_FOLLOW_UP.ENQ_ID,EMPMAST.EMPNAME,to_char(ENQUIRY_FOLLOW_UP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(ENQUIRY_FOLLOW_UP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,ENQUIRY_FOLLOW_UP.REMARKS,ENQUIRY_FOLLOW_UP.FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP left outer join EMPMAST on EMPMASTID=ENQUIRY_FOLLOW_UP.FOLLOWED_BY  WHERE ENQ_ID='" + enqid + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;

        //}
        //public PurchaseFollowup GetPurchaseFollowupById(string eid)
        //{
        //    PurchaseFollowup PurchaseFollowup = new PurchaseFollowup();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select ENQ_ID,FOLLOWED_BY,FOLLOW_STATUS,FOLLOW_DATE,NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,ENQ_FOLLOW_ID  from ENQUIRY_FOLLOW_UP where ENQ_FOLLOW_ID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                PurchaseFollowup cmp = new PurchaseFollowup
        //                {
        //                    ID = rdr["ENQ_FOLLOW_ID"].ToString(),
        //                    Enqno = rdr["ENQ_ID"].ToString(),
        //                    Followby = rdr["FOLLOWED_BY"].ToString(),
        //                    Followdate = rdr["FOLLOW_DATE"].ToString(),
        //                    Nfdate = rdr["NEXT_FOLLOW_DATE"].ToString(),
        //                    Rmarks = rdr["REMARKS"].ToString(),
        //                    Enquiryst = rdr["FOLLOW_STATUS"].ToString(),


        //                };

        //                PurchaseFollowup = cmp;
        //            }
        //        }
        //    }
        //    return PurchaseFollowup;
        //}
        public string PurchaseFollowupCRUD(PurchaseImportFollowup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURCHASEFOLLOWPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURCHASEFOLLOWPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.FoID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.FoID;
                    }

                    objCmd.Parameters.Add("ENQ_ID", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("FOLLOWED_BY", OracleDbType.NVarchar2).Value = cy.Followby;
                    objCmd.Parameters.Add("FOLLOW_DATE", OracleDbType.NVarchar2).Value = cy.Followdate;
                    objCmd.Parameters.Add("NEXT_FOLLOW_DATE", OracleDbType.NVarchar2).Value = cy.Nfdate;
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


        public string EnquirytoQuote(string enqid)
        {


            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PQU1' AND ACTIVESEQUENCE = 'T'");
                string QuoNo = string.Format("{0}{1}", "PQU1", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PQU1' AND ACTIVESEQUENCE ='T'";
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
                    svSQL = "Insert into PURQUOTBASIC (ENQNO,BRANCHID,EXRATE,MAINCURRENCY,PARTYID,DOCID,DOCDATE,IS_ACTIVE,CREATED_BY,CREATED_ON) (Select IPURENQBASICID,BRANCHID,EXRATE,CURRENCYID,PARTYMASTID,'" + QuoNo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ,'Y','10001000000637','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' from IPURENQBASIC where IPURENQBASICID='" + enqid + "')";
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

                string quotid = datatrans.GetDataString("Select PURQUOTBASICID from PURQUOTBASIC Where ENQNO=" + enqid + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into PURQUOTDETAIL (PURQUOTBASICID,ITEMID,RATE,QTY,UNIT,CF) (Select '" + quotid + "',ITEMID,RATE,QTY,UNIT,CF from IPURENQDETAIL WHERE IPURENQBASICID=" + enqid + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    string Svql = "UPDATE IPURENQBASIC SET STATUS='Quote' where IPURENQBASICID='" + enqid + "'";
                    OracleCommand objCmds1 = new OracleCommand(Svql, objConnT);

                    objCmds1.ExecuteNonQuery();
                    objConnT.Close();
                }

                //using (OracleConnection objConnE = new OracleConnection(_connectionString))
                //{
                //    string Sql = "UPDATE PURENQBASIC SET STATUS=2 where PURENQBASICID='" + enqid + "'";
                //    OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                //    objConnE.Open();
                //    objCmds.ExecuteNonQuery();
                //    objConnE.Close();
                //}

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
                    //svSQL = "UPDATE PURENQ SET ACTIVE ='NO' WHERE PURENQID='" + id + "'";
                    svSQL = "UPDATE IPURENQBASIC SET IS_ACTIVE ='N' WHERE IPURENQBASICID='" + id + "'";
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



        public DataTable GetAllPurchaseEnquiryItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASIC.STATUS,IPURENQBASICID from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURENQBASIC.IS_ACTIVE='Y' order by IPURENQBASICID DESC";
            }
            else
            {
                SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXRATE,PARTYREFNO,CURRENCYID,PARTYMAST.PARTYNAME,IPURENQBASIC.STATUS,IPURENQBASICID from IPURENQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPURENQBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPURENQBASIC.PARTYMASTID=PARTYMAST.PARTYMASTID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPURENQBASIC.IS_ACTIVE='N' order by IPURENQBASICID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string RegenerateCRUD(PurchaseImportEnq cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PENQ' AND ACTIVESEQUENCE = 'T'");
                string EnqNo = string.Format("{0}{1}", "PENQ", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PENQ' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.EnqNo = EnqNo;
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURCHASEENQPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    StatementType = "Insert";
                    objCmd.Parameters.Add("IPURENQBASICID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("ENQREF", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.Enqdate;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYREFNO", OracleDbType.NVarchar2).Value = cy.ParNo;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Cur;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    //objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "Y";
                    objCmd.Parameters.Add("ENQRECDBY", OracleDbType.NVarchar2).Value = cy.EnqRecid;
                    objCmd.Parameters.Add("ASSIGNTO", OracleDbType.NVarchar2).Value = cy.Enqassignid;
                    //objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.EnqRecid;
                    //objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        //if (cy.ID != null)
                        //{
                        //    Pid = cy.ID;
                        //}
                        if (cy.EnqLst != null)
                        {
                            //if (cy.ID == null)
                            //{
                            foreach (ImportEnqItem cp in cy.EnqLst)
                            {
                                if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                                {
                                    string UnitID = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");


                                    svSQL = "Insert into IPURENQDETAIL (IPURENQBASICID,ITEMID,UNIT,CF,QTY,RATE) VALUES ('" + Pid + "','" + cp.saveItemId + "','" + UnitID + "','" + cp.Conversionfactor + "','" + cp.Quantity + "','" + cp.rate + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
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

        //public string RegenerateCRUD(PurchaseEnquiry cy)
        //{
        //    throw new NotImplementedException();
        //}
    }
}