using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PurchaseIndentService : IPurchaseIndent
    {
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;

        public PurchaseIndentService(IConfiguration _configuratio)
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
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                OracleConnection objConn = new OracleConnection(_connectionString);
                OracleCommand objCmd = new OracleCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }
        public DataTable GetSLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS WHERE LOCATIONTYPE='STORES'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndent()
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,PINDBASICID,BRANCHMAST.BRANCHID from PINDBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PINDBASIC.BRANCHID Order by PINDBASICID DESC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetHistory(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select GRNBLDETAIL.RATE,PARTYRCODE.PARTY,GRNBLDETAIL.PUNIT,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy') as DOCDATE,GRNBLDETAIL.TOTAMT,GRNBLDETAIL.QTY from GRNBLDETAIL LEFT OUTER JOIN GRNBLBASIC on GRNBLBASIC.GRNBLBASICID=GRNBLDETAIL.GRNBLBASICID LEFT OUTER JOIN PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')    order by GRNBLDETAIL.GRNBLDETAILID DESC fetch  first 5 rows only";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLasttwoSupp(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  PARTYRCODE.PARTY,GRNBLBASIC.PARTYID from GRNBLDETAIL LEFT OUTER JOIN GRNBLBASIC on GRNBLBASIC.GRNBLBASICID=GRNBLDETAIL.GRNBLBASICID LEFT OUTER JOIN PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH')   group by PARTYRCODE.PARTY,GRNBLBASIC.PARTYID fetch first 2 rows only";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSuppPurchaseDetails(string Partyid,string itemid)
        {
            string SvSql = string.Empty;
            SvSql = "select GRNBLDETAIL.RATE,PARTYRCODE.PARTY,GRNBLDETAIL.PUNIT,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy') as DOCDATE,GRNBLDETAIL.TOTAMT,GRNBLDETAIL.QTY from GRNBLDETAIL LEFT OUTER JOIN GRNBLBASIC on GRNBLBASIC.GRNBLBASICID=GRNBLDETAIL.GRNBLBASICID LEFT OUTER JOIN PARTYMAST on GRNBLBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND  PARTYMAST.PARTYMASTID='"+ Partyid  + "'  order by GRNBLDETAIL.GRNBLDETAILID DESC fetch  first 1 rows only";
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
        public DataTable GetIndentItem(string PRID)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,UNITMAST.UNITID,PINDDETAIL.QTY,PINDDETAIL.PINDBASICID,LOCDETAILS.LOCID,PINDDETAIL.PINDDETAILID ,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINDDETAIL.UNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT ";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItemApprove()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,UNITMAST.UNITID,PINDDETAIL.QTY,PINDDETAIL.PINDBASICID,LOCDETAILS.LOCID,PINDDETAIL.PINDDETAILID ,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINDDETAIL.UNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT LEFT OUTER JOIN PINDBASIC ON PINDBASIC.PINDBASICID=PINDDETAIL.PINDBASICID WHERE PINDDETAIL.APPROVED1 IS NULL ";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItemApproved()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,UNITMAST.UNITID,PINDDETAIL.QTY,PINDDETAIL.PINDBASICID,LOCDETAILS.LOCID,PINDDETAIL.PINDDETAILID ,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINDDETAIL.UNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT LEFT OUTER JOIN PINDBASIC ON PINDBASIC.PINDBASICID=PINDDETAIL.PINDBASICID WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL ";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItemSuppDetail()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,PINDDETAIL.QTY,PINDDETAIL.PINDBASICID,LOCDETAILS.LOCID,PINDDETAIL.PINDDETAILID ,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy') DUEDATE,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINDDETAIL.UNIT LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PINDDETAIL.DEPARTMENT LEFT OUTER JOIN PINDBASIC ON PINDBASIC.PINDBASICID=PINDDETAIL.PINDBASICID WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL ";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItembyItemd(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select PINDDETAIL.ITEMID,PINDDETAIL.PINDBASICID,PINDDETAIL.PINDDETAILID  from PINDDETAIL  WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL AND PINDDETAIL.ITEMID='"+ ItemId  + "'";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItemSupp()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,SUM(PINDDETAIL.QTY) as QTY,ITEMMASTER.ITEMMASTERID from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID  WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL GROUP BY ITEMMASTER.ITEMID,ITEMMASTER.ITEMMASTERID  ";/*where PINDDETAIL.PINDBASICID='"+ PRID  + "'*/
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndentItemSuppEnq(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,SUM(PINDDETAIL.QTY) as QTY,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,UNITMAST.UNITMASTID from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL AND PINDDETAIL.ITEMID='" + id + "' GROUP BY ITEMMASTER.ITEMID,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,UNITMAST.UNITMASTID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //DataTable GetItemQty(string Itemid)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select ITEMMASTER.ITEMID,SUM(PINDDETAIL.QTY) as QTY,ITEMMASTER.ITEMMASTERID from PINDDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINDDETAIL.ITEMID  WHERE PINDDETAIL.APPROVED2 IS NULL AND PINDDETAIL.APPROVED1 IS NOT NULL AND PINDDETAIL.ITEMID='" + id + "'  GROUP BY ITEMMASTER.ITEMID,ITEMMASTER.ITEMMASTERID";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
    

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

        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE SUBGROUPCODE='" + value + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,UNITMAST.UNITMASTID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,QCYNTEMP from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndetnPlacedDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUM(PINDDETAIL.QTY) as QTY,PINDDETAIL.ITEMID from PINDDETAIL WHERE PINDDETAIL.APPROVED1 IS NULL AND PINDDETAIL.ITEMID='" + ItemId + "' GROUP BY PINDDETAIL.ITEMID ";
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

        public string GenerateEnquiry(string[] selectedRecord, string supid)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                
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

                    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'ENQ' AND IS_ACTIVE = 'Y'");
                    string EnqNo = string.Format("{0} - {1} / {2}", "ENQ", (idc + 1).ToString(), disp);

                    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='ENQ' AND IS_ACTIVE ='Y'";
                    try
                    {
                        UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
               


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURCHASEENQPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = "10061000000002";
                    objCmd.Parameters.Add("ENQNO", OracleDbType.NVarchar2).Value = EnqNo;
                    objCmd.Parameters.Add("ENQDATE", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("EXCRATERATE", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("PARTYREFNO", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("CURRENCYID", OracleDbType.NVarchar2).Value = "";
                    objCmd.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = supid;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        datatrans = new DataTransactions(_connectionString);
                        foreach (string itemid in selectedRecord)
                        {
                            string EnquiryQty = "";
                            string Unit = "";
                           
                            DataTable dr = new DataTable();
                            dr = GetIndentItemSuppEnq(itemid);
                            if (dr.Rows.Count > 0)
                            {
                                EnquiryQty = dr.Rows[0]["QTY"].ToString();
                                Unit = dr.Rows[0]["UNITMASTID"].ToString();
                            }
                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Insert")
                                    {
                                        Sql = "Insert into PURENQDETAIL (PURENQBASICID,ITEMID,QTY,UNIT) Values ('" + Pid + "','" + itemid + "','" + EnquiryQty + "','" + Unit + "') ";
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
                            DataTable dt = new DataTable();
                            dt = GetIndentItembyItemd(itemid);
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED2='YES',APPROVAL2U='SRRAJAN',APP2DT='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where PINDDETAILID='" + dt.Rows[i]["PINDDETAILID"].ToString() + "'");
                                }
                            }


                        }



                        //datatrans = new DataTransactions(_connectionString);
                        //bool result = datatrans.UpdateStatus("UPDATE PINDDETAIL SET APPROVED1='YES',APPROVAL1U='SRRAJAN',APP1DT='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' Where PINDDETAILID='" + id + "'");

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
        public string IndentCRUD(PurchaseIndent cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                if (cy.ID == null)
                {
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

                    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'PI' AND IS_ACTIVE = 'Y'");
                    cy.IndentId = string.Format("{0} - {1} / {2}", "IND", (idc + 1).ToString(), disp);

                    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='PI' AND IS_ACTIVE ='Y'";
                    try
                    {
                        UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PIPROC", objConn);

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

                    objCmd.Parameters.Add("Branch", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("Location", OracleDbType.NVarchar2).Value = cy.SLocation;
                    objCmd.Parameters.Add("IndentNo", OracleDbType.NVarchar2).Value = cy.IndentId;
                    objCmd.Parameters.Add("IndentDate", OracleDbType.Date).Value = DateTime.Parse(cy.IndentDate); 
                    objCmd.Parameters.Add("RefDate", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate); 
                    objCmd.Parameters.Add("Erecation", OracleDbType.NVarchar2).Value =cy.Erection;
                    objCmd.Parameters.Add("PurchaseType", OracleDbType.NVarchar2).Value = cy.Purtype;
                    objCmd.Parameters.Add("EnterBy", OracleDbType.NVarchar2).Value = "10032000091118";
                    objCmd.Parameters.Add("EnterDate", OracleDbType.Date).Value = DateTime.Now;
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

                        foreach (PIndentItem cp in cy.PILst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PIDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                   
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("PIID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMIDS", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QUANTITY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNITP", OracleDbType.NVarchar2).Value = cp.UnitID;
                                    objCmds.Parameters.Add("QC", OracleDbType.NVarchar2).Value = cp.QC;
                                    objCmds.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cp.Narration;
                                    objCmds.Parameters.Add("DUE_DATE", OracleDbType.Date).Value = DateTime.Parse(cp.Duedate);
                                    objCmds.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cp.LocId;
                                    objCmds.Parameters.Add("ITEMGROUPID", OracleDbType.NVarchar2).Value = cp.ItemGroupId;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                                }
                        }
                        foreach (PIndentTANDC cp in cy.TANDClst)
                        {
                            if (cp.Isvalid == "Y" && cp.TANDC != null)
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                   if (StatementType == "Insert")
                                    {
                                        Sql = "Insert into PINDTENDC (PINDBASICID,TERMNCDN) Values ('" + Pid + "','"+ cp.TANDC +"') ";
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
