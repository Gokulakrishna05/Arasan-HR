using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class ProFormaInvoiceService : IProFormaInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProFormaInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<ProFormaInvoice> GetAllProFormaInvoice(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "Active";
            }
            List<ProFormaInvoice> cmpList = new List<ProFormaInvoice>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHID,DOCID,REFNO,STATUS,SPINVBASICID from SPINVBASIC WHERE STATUS= 'Active'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProFormaInvoice cmp = new ProFormaInvoice
                        {
                            ID = rdr["SPINVBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            RefNo = rdr["REFNO"].ToString(),
                            status = rdr["STATUS"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
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
        public DataTable GetJob()
        {
            string SvSql = string.Empty;
            SvSql = "Select JOBASICID,DOCID From JOBASIC";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ProFormaInvoiceCRUD(ProFormaInvoice cy)
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

                    int idc = GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'SE' AND IS_ACTIVE = 'Y'");
                    cy.DocId = string.Format("{0}/{3}/{1} - {2} ", "TAAI", "SE", (idc + 1).ToString(), disp);

                    string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='SE' AND IS_ACTIVE ='Y'";
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
                    OracleCommand objCmd = new OracleCommand("SPINVBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

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


                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("WORKORDER", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Currency;
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("SALESVALUE", OracleDbType.NVarchar2).Value = cy.SalesValue;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("AMTWORDS", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("BANKNAME", OracleDbType.NVarchar2).Value = cy.BankName;
                    objCmd.Parameters.Add("ACNO", OracleDbType.NVarchar2).Value = cy.AcNo;
                    objCmd.Parameters.Add("SHIPADDRESS", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "Active";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    //objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
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
                        if (cy.ProFormalst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProFormaInvoiceDetail cp in cy.ProFormalst)
                                {
                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {

                                        svSQL = "Insert into SPINVDETAIL (SPINVBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,TARIFFID,CGST,SGST,IGST,TOTEXAMT) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemdes + "','" + cp.unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.discount + "','" + cp.amount + "','" + cp.cashdisc + "','" + cp.tradedis + "','" + cp.additionaldis + "','" + cp.dis + "','" + cp.frieght + "','" + cp.tariff + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.totamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete SPINVDETAIL WHERE SPINVBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProFormaInvoiceDetail cp in cy.ProFormalst)
                                {

                                    if (cp.Isvalid == "Y" && cp.itemid != "0")
                                    {
                                        svSQL = "Insert into SPINVDETAIL (SPINVBASICID,ITEMID,ITEMSPEC,UNIT,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,TARIFFID,CGST,SGST,IGST,TOTEXAMT) VALUES ('" + Pid + "','" + cp.itemid + "','" + cp.itemdes + "','" + cp.unit + "','" + cp.qty + "','" + cp.rate + "','" + cp.discount + "','" + cp.amount + "','" + cp.cashdisc + "','" + cp.tradedis + "','" + cp.additionaldis + "','" + cp.dis + "','" + cp.frieght + "','" + cp.tariff + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.totamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
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
        public DataTable GetEditProFormaInvoice(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,WORKORDER,DOCID,DOCDATE,REFNO,REFDATE,MAINCURRENCY,EXRATE,PARTYID,SALESVALUE,GROSS,NET,AMTWORDS,BANKNAME,ACNO,SHIPADDRESS,NARRATION,SPINVBASICID  from SPINVBASIC where SPINVBASIC.SPINVBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProFormaInvoiceDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURRENCY.MAINCURR,PARTYRCODE.PARTY,JOBASICID from JOBASIC LEFT OUTER JOIN CURRENCY ON JOBASIC.MAINCURRENCY=CURRENCY.CURRENCYID  LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Customer','BOTH') AND JOBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE SPINVBASIC SET STATUS ='InActive' WHERE SPINVBASICID='" + id + "'";
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

        public DataTable GetWorkOrderDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMMASTER.ITEMID,ITEMSPEC,UNITMAST.UNITID,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,JOBASICID from JODETAIL left outer join ITEMMASTER on ITEMMASTERID =JODETAIL.ITEMID left outer join UNITMAST on UNITMASTID=JODETAIL.UNIT where JOBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
