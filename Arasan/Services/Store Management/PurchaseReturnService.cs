using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Store_Management;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Arasan.Interface;

namespace Arasan.Services
{
    public class PurchaseReturnService : IPurchaseReturn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<PurchaseReturn> GetAllPurReturn()
        {
            List<PurchaseReturn> cmpList = new List<PurchaseReturn>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,PRETBASIC.EXCHANGERATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,PRETBASIC.LOCID,PRETBASIC.REASONCODE,PRETBASIC.REJBY,PRETBASIC.TRANSITLOCID,PRETBASIC.TEMPFIELD,CURRENCY.MAINCURR,PARTYRCODE.PARTY,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID from PRETBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PRETBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on PRETBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=PRETBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY PRETBASIC.PRETBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseReturn cmp = new PurchaseReturn
                        {
                            ID = rdr["PRETBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Supplier = rdr["PARTY"].ToString(),
                            RetNo = rdr["DOCID"].ToString(),
                            RetDate = rdr["DOCDATE"].ToString(),
                            ExRate = rdr["EXCHANGERATE"].ToString(),
                            Currency = rdr["MAINCURR"].ToString(),
                          
                            Reason = rdr["REASONCODE"].ToString()

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetPurchaseReturn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETBASIC.BRANCHID,PRETBASIC.PARTYID,PRETBASIC.DOCID,to_char(PRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PRETBASIC.REFNO,to_char(PRETBASIC.REFDT,'dd-MON-yyyy')REFDT,PRETBASIC.LOCID,PRETBASIC.MAINCURRENCY,PRETBASIC.EXCHANGERATE,PRETBASIC.REASONCODE,PRETBASIC.REJBY,PRETBASIC.TRANSITLOCID,PRETBASIC.TEMPFIELD,PRETBASIC.RGRNNO,PRETBASIC.GROSS,PRETBASIC.NET,PRETBASIC.NARR,PRETBASICID  from PRETBASIC where PRETBASIC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseReturnDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETDETAIL.GRNNO,PRETDETAIL.ITEMID,PRETDETAIL.QTY,PRETDETAIL.UNIT,PRETDETAIL.RATE,PRETDETAIL.AMOUNT,PRETDETAIL.TOTAMT,PRETDETAIL.CF,PRETDETAIL.CGSTPER,PRETDETAIL.CGSTAMT,PRETDETAIL.SGSTPER,PRETDETAIL.SGSTAMT,PRETDETAIL.IGSTPER,PRETDETAIL.IGSTAMT,PRETBASICID,PRETDETAILID  from PRETDETAIL where PRETDETAIL.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseReturnDes(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PURRESADD.SADD1,PURRESADD.SCITY,PURRESADD.SSTATE,PURRESADD.SPINCODE,PURRESADD.SPHONE,PURRESADDID  from PURRESADD where PURRESADD.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchaseReturnReason(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PRETTANDC.REASON,PRETTANDCID  from PRETTANDC where PRETTANDC.PRETBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PurReturnCRUD(PurchaseReturn cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PURRETURNPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURRETURNPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.RetNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RetDate);
                    objCmd.Parameters.Add("EXCHANGERATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.ReqNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.ReqDate);
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = cy.Reason;
                    objCmd.Parameters.Add("REJBY", OracleDbType.NVarchar2).Value = cy.Rej;
                    objCmd.Parameters.Add("TRANSITLOCID", OracleDbType.NVarchar2).Value = cy.Trans;
                    objCmd.Parameters.Add("TEMPFIELD", OracleDbType.NVarchar2).Value = cy.Temp;
                    objCmd.Parameters.Add("RGRNNO", OracleDbType.NVarchar2).Value = cy.Grn;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
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

                        foreach (RetItem cp in cy.RetLst)
                        {
                           
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("PURRETURNDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("PRETBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cp.GRNNo;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.TotalAmount;
                                    objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("CGSTPER", OracleDbType.NVarchar2).Value = cp.CGSTPer;
                                    objCmds.Parameters.Add("CGSTAMT", OracleDbType.NVarchar2).Value = cp.CGSTAmt;
                                    objCmds.Parameters.Add("SGSTPER", OracleDbType.NVarchar2).Value = cp.SGSTPer;
                                    objCmds.Parameters.Add("SGSTAMT", OracleDbType.NVarchar2).Value = cp.SGSTAmt;
                                    objCmds.Parameters.Add("IGSTPER", OracleDbType.NVarchar2).Value = cp.IGSTPer;
                                    objCmds.Parameters.Add("IGSTAMT", OracleDbType.NVarchar2).Value = cp.IGSTAmt;


                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }


                        }
                        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        {
                            OracleCommand objCmds = new OracleCommand("DESPATCHRETURN", objConns);
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
                            objCmds.Parameters.Add("PRETBASICID", OracleDbType.NVarchar2).Value = Pid;
                            objCmds.Parameters.Add("SADD1", OracleDbType.NVarchar2).Value = cy.Addr;
                            objCmds.Parameters.Add("SCITY", OracleDbType.NVarchar2).Value = cy.City;
                            objCmds.Parameters.Add("SSTATE", OracleDbType.NVarchar2).Value = cy.State;
                            objCmds.Parameters.Add("SPINCODE", OracleDbType.NVarchar2).Value = cy.Pin;
                            objCmds.Parameters.Add("SPHONE", OracleDbType.NVarchar2).Value = cy.Phone;

                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                            objConns.Open();
                            objCmds.ExecuteNonQuery();
                            objConns.Close();
                        }
                        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        {
                            OracleCommand objCmds = new OracleCommand("PURRETURNTANDCPROC", objConns);
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
                            objCmds.Parameters.Add("PRETBASICID", OracleDbType.NVarchar2).Value = Pid;

                            objCmds.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cy.Reason;


                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                            objConns.Open();
                            objCmds.ExecuteNonQuery();
                            objConns.Close();
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
        public DataTable GetGRN()
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,GRNBLBASICID from GRNBLBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRNDetails(string POID)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UNITMAST.UNITID,CF,QTY,RATE,AMOUNT,ITEMMASTER.ITEMID,DISC,IFREIGHTCH,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,TOTAMT FROM GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GRNBLDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE GRNBLBASICID='" + POID + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "Select STATE,STATEMASTID from STATEMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCity(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER where STATEID='"+ ItemId+"'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,GRNBLBASICID,GRNBLDETAILID from GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=GRNBLDETAIL.ITEMID where GRNBLDETAIL.GRNBLBASICID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
