using Arasan.Interface.Sales;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace Arasan.Services.Sales
{
    public class DebitNoteBillService : IDebitNoteBillService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DebitNoteBillService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<DebitNoteBill> GetAllDebitNoteBill()
        {
            List<DebitNoteBill> cmpList = new List<DebitNoteBill>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,DOCID,DOCDATE,DBNOTEBASICID from DBNOTEBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DBNOTEBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DebitNoteBill cmp = new DebitNoteBill
                        {
                            ID = rdr["DBNOTEBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string DebitNoteBillCRUD(DebitNoteBill cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DBNOTEBASICPROC", objConn);

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
                    objCmd.Parameters.Add("VTYPE", OracleDbType.NVarchar2).Value = cy.Vocher;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("AMTINWRD", OracleDbType.NVarchar2).Value = cy.Amount;
                    objCmd.Parameters.Add("BIGST", OracleDbType.NVarchar2).Value = cy.Bigst;
                    objCmd.Parameters.Add("BSGST", OracleDbType.NVarchar2).Value = cy.Bsgst;
                    objCmd.Parameters.Add("BCGST", OracleDbType.NVarchar2).Value = cy.Bcgst;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration;
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
                        if (cy.Depitlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DebitNote cp in cy.Depitlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.Item + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DBNOTEDETAIL WHERE DBNOTEBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DebitNote cp in cy.Depitlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DBNOTEDETAIL (DBNOTEBASICID,INVNO,ITEMID,CONVFACTOR,PRIUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.InvNo + "','" + cp.Item + "','" + cp.Cf + "','" + cp.Unit + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }



                        }

                    }
                    catch (Exception ex)
                    {

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

        public DataTable GetParty()
        {
            string SvSql = string.Empty;
            SvSql = "select GRNBLBASICID,PARTYNAME FROM GRNBLBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGrn()
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,GRNBLBASICID FROM  GRNBLBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemDetails(string itemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEMMASTER.ITEMID,CF,PUNIT,QTY,RATE,AMOUNT,CGST,SGST,IGST,TOTAMT FROM GRNBLDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID =GRNBLDETAIL.ITEMID WHERE GRNBLBASICID='" + itemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
