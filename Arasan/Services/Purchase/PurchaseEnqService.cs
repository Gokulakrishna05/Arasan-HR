 using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class PurchaseEnqService : IPurchaseEnqService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseEnqService(IConfiguration _configuratio)
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
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable();            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

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

        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBGROUPCODE='" + value  + "'";
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
        public IEnumerable<PurchaseEnquiry> GetAllPurenquriy()
        {
            List<PurchaseEnquiry> cmpList = new List<PurchaseEnquiry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYRCODE.PARTY,PURENQID,PURENQ.STATUS from PURENQ LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURENQ.BRANCHID LEFT OUTER JOIN  PARTYMAST on PURENQ.PARTYMASTID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY PURENQID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseEnquiry cmp = new PurchaseEnquiry
                        {
                            ID = rdr["PURENQID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            RefNo = rdr["ENQNO"].ToString(),
                            Enqdate = rdr["ENQDATE"].ToString(),
                            ExRate = rdr["EXCRATERATE"].ToString(),
                            ParNo = rdr["PARTYREFNO"].ToString(),
                            Cur = rdr["CURRENCYID"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            Status= rdr["STATUS"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public IEnumerable<EnqItem> GetAllPurenquriyItem(string id)
        {
            List<EnqItem> cmpList = new List<EnqItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select PURENQDETAIL.QTY,PURENQDETAIL.PURENQDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from PURENQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PURENQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where PURENQDETAIL.PURENQBASICID='"+ id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        EnqItem cmp = new EnqItem
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
            SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYRCODE.PARTY,PURENQID,PURENQ.STATUS from PURENQ LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURENQ.BRANCHID LEFT OUTER JOIN  PARTYMAST on PURENQ.PARTYMASTID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PURENQID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PURENQDETAIL.QTY,PURENQDETAIL.PURENQDETAILID,PURENQDETAIL.ITEMID,PURENQDETAIL.UNIT,UNITMAST.UNITID,PURENQDETAIL.RATE,ITEMMASTER.ITEMID as ITEMNAME from PURENQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PURENQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PURENQDETAIL.UNIT  where PURENQDETAIL.PURENQBASICID='" + id + "'";
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
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='"+ id +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseEnqDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYRCODE.PARTY,PURENQID,PURENQ.STATUS from PURENQ LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURENQ.BRANCHID LEFT OUTER JOIN  PARTYMAST on PURENQ.PARTYMASTID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')  AND PURENQID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public PurchaseEnquiry GetPurenqServiceById(string eid)
        {
            PurchaseEnquiry PurchaseEnquiry = new PurchaseEnquiry();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,ENQNO,ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYMASTID,PURENQID  from PURENQ where PURENQID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseEnquiry cmp = new PurchaseEnquiry
                        {
                            ID = rdr["PURENQID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            RefNo = rdr["ENQNO"].ToString(),
                            Enqdate = rdr["ENQDATE"].ToString(),
                            ExRate = rdr["EXCRATERATE"].ToString(),
                            ParNo = rdr["PARTYREFNO"].ToString(),
                            Cur = rdr["CURRENCYID"].ToString(),
                            Supplier = rdr["PARTYMASTID"].ToString()

                        };

                        PurchaseEnquiry = cmp;
                    }
                }
            }
            return PurchaseEnquiry;
        }

        public string PurenquriyCRUD(PurchaseEnquiry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURCHASEENQPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.EnqNo;
                    objCmd.Parameters.Add("RefNo", OracleDbType.NVarchar2).Value = cy.RefNo; 
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Enqdate);
                    objCmd.Parameters.Add("EXCRATERATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYREFNO", OracleDbType.NVarchar2).Value = cy.ParNo;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Cur;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (EnqItem cp in cy.EnqLst)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update PURENQDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "'  where PURENQBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
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
        public IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup()
        {
            List<PurchaseFollowup> cmpList = new List<PurchaseFollowup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ENQ_ID,EMPMAST.EMPNAME,to_char(FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP LEFT OUTER JOIN EMPMAST ON EMPMASTID=ENQUIRY_FOLLOW_UP.FOLLOWED_BY";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseFollowup cmp = new PurchaseFollowup
                        {
                            ID = rdr["ENQ_FOLLOW_ID"].ToString(),
                            Enqno = rdr["ENQ_ID"].ToString(),
                            Followby = rdr["EMPNAME"].ToString(),
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
            SvSql = "Select ENQUIRY_FOLLOW_UP.ENQ_ID,ENQUIRY_FOLLOW_UP.FOLLOWED_BY,to_char(ENQUIRY_FOLLOW_UP.FOLLOW_DATE,'dd-MON-yyyy')FOLLOW_DATE,to_char(ENQUIRY_FOLLOW_UP.NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE,ENQUIRY_FOLLOW_UP.REMARKS,ENQUIRY_FOLLOW_UP.FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP LEFT OUTER JOIN EMPMAST ON EMPNAME=ENQUIRY_FOLLOW_UP.FOLLOWED_BY  WHERE ENQ_ID='" + enqid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        
        }
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
        public string PurchaseFollowupCRUD(PurchaseFollowup cy)
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

                    objCmd.Parameters.Add("ENQ_ID", OracleDbType.NVarchar2).Value = cy.Enqno;
                    objCmd.Parameters.Add("FOLLOWED_BY", OracleDbType.NVarchar2).Value = cy.Followby;
                    objCmd.Parameters.Add("FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse(cy.Followdate);
                    objCmd.Parameters.Add("NEXT_FOLLOW_DATE", OracleDbType.Date).Value = DateTime.Parse( cy.Nfdate);
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
                DateTime theDate = DateTime.Now;
                DateTime todate; DateTime fromdate;
                string t; string f;
                if (DateTime.Now.Month >= 4)
                {
                    todate = theDate.AddYears(1);
                }
                else
                {
                    todate = theDate;
                }
                if (DateTime.Now.Month >= 4)
                {
                    fromdate = theDate;
                }
                else
                {
                    fromdate = theDate.AddYears(-1);
                }
                t = todate.ToString("yy");
                f = fromdate.ToString("yy");
                string disp = string.Format("{0}-{1}", f, t);

                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'QUO' AND IS_ACTIVE = 'Y'");
                string QuoNo = string.Format("{0} - {1} / {2}", "QUO", (idc + 1).ToString(), disp);

                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='QUO' AND IS_ACTIVE ='Y'";
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
                    svSQL = "Insert into PURQUOTBASIC (ENQID,BRANCHID,EXRATE,MAINCURRENCY,PARTYID,DOCID,DOCDATE) (Select PURENQID,BRANCHID,EXCRATERATE,CURRENCYID,PARTYMASTID,'" + QuoNo + "','"+ DateTime.Now.ToString("dd-MMM-yyyy") + "'  from PURENQ where PURENQID='" + enqid + "')";
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

                string quotid = datatrans.GetDataString("Select PURQUOTBASICID from PURQUOTBASIC Where ENQID="+ enqid + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into PURQUOTDETAIL (PURQUOTBASICID,ITEMID,RATE,QTY,UNIT,CF) (Select '"+ quotid  + "',ITEMID,RATE,QTY,UNIT,CF from PURENQDETAIL WHERE PURENQBASICID=" + enqid + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE PURENQ SET STATUS=2 where PURENQID='" + enqid + "'";
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

    }
}
