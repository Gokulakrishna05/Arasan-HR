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

        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE ITEMGROUP='" + value  + "'";
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
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
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
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,ENQNO,to_char(ENQDATE,'dd-MON-yyyy') ENQDATE,EXCRATERATE,PARTYREFNO,CURRENCYID,PARTYRCODE.PARTY,PURENQID from PURENQ LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURENQ.BRANCHID LEFT OUTER JOIN  PARTYMAST on PURENQ.PARTYMASTID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')";
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
                            Supplier = rdr["PARTY"].ToString()



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
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURCHASEENQPROC";*/

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
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.NVarchar2).Value = cy.Enqdate;
                    objCmd.Parameters.Add("EXCRATERATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYREFNO", OracleDbType.NVarchar2).Value = cy.ParNo;
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = cy.Cur;
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = cy.Supplier;
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
        public IEnumerable<PurchaseFollowup> GetAllPurchaseFollowup()
        {
            List<PurchaseFollowup> cmpList = new List<PurchaseFollowup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ENQ_ID,FOLLOWED_BY,FOLLOW_DATE,NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,ENQ_FOLLOW_ID from ENQUIRY_FOLLOW_UP";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseFollowup cmp = new PurchaseFollowup
                        {
                            ID = rdr["ENQ_FOLLOW_ID"].ToString(),
                            Enqid = rdr["ENQ_ID"].ToString(),
                            Followby = rdr["FOLLOWED_BY"].ToString(),
                            Followdate = rdr["FOLLOW_DATE"].ToString(),
                            Nfdate = rdr["NEXT_FOLLOW_DATE"].ToString(),
                            Rmarks = rdr["REMARKS"].ToString(),
                            Enquiryst = rdr["FOLLOW_STATUS"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public PurchaseFollowup GetPurchaseFollowupById(string eid)
        {
            PurchaseFollowup PurchaseFollowup = new PurchaseFollowup();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ENQ_ID,FOLLOWED_BY,FOLLOW_STATUS,FOLLOW_DATE,NEXT_FOLLOW_DATE,REMARKS,FOLLOW_STATUS,ENQ_FOLLOW_ID  from ENQUIRY_FOLLOW_UP where ENQ_FOLLOW_ID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseFollowup cmp = new PurchaseFollowup
                        {
                            ID = rdr["ENQ_FOLLOW_ID"].ToString(),
                            Enqid = rdr["ENQ_ID"].ToString(),
                            Followby = rdr["FOLLOWED_BY"].ToString(),
                            Followdate = rdr["FOLLOW_DATE"].ToString(),
                            Nfdate = rdr["NEXT_FOLLOW_DATE"].ToString(),
                            Rmarks = rdr["REMARKS"].ToString(),
                            Enquiryst = rdr["FOLLOW_STATUS"].ToString(),


                        };

                        PurchaseFollowup = cmp;
                    }
                }
            }
            return PurchaseFollowup;
        }
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

                    objCmd.Parameters.Add("ENQ_ID", OracleDbType.NVarchar2).Value = cy.Enqid;
                    objCmd.Parameters.Add("FOLLOWED_BY", OracleDbType.NVarchar2).Value = cy.Followby;
                    objCmd.Parameters.Add("FOLLOW_DATE", OracleDbType.NVarchar2).Value = cy.Followdate;
                    objCmd.Parameters.Add("NEXT_FOLLOW_DATE", OracleDbType.NVarchar2).Value = cy.Nfdate;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Rmarks;
                    objCmd.Parameters.Add("FOLLOW_STATUS", OracleDbType.NVarchar2).Value = cy.Enquiryst;

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

        
    }
}
