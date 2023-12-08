using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class RetNonRetDcService : IRetNonRetDc
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RetNonRetDcService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }


        public string RetNonRetDcCRUD(RetNonRetDc cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);

                if (cy.ID != null)
                {
                    cy.ID = null;
                }

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Rdc-' AND ACTIVESEQUENCE = 'T'  ");
                string Did = string.Format("{0}{1}", "Rdc-", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Rdc-' AND ACTIVESEQUENCE ='T'  ";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //string PARTY = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYID='" + cy.Party + "' ");
                //string WID = datatrans.GetDataString("Select WCBASICID from WCBASIC where WCID='" + cy.work + "' ");
                
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("RDELPROC", objConn);
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
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Did;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DDate;
                    objCmd.Parameters.Add("DELTYPE", OracleDbType.NVarchar2).Value = cy.DcType;
                    objCmd.Parameters.Add("THROUGH", OracleDbType.NVarchar2).Value = cy.Through;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("STKTYPE", OracleDbType.NVarchar2).Value = cy.Stock;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("DELDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Delivery); 
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narration; 
                    objCmd.Parameters.Add("APPBY", OracleDbType.NVarchar2).Value = cy.Approved; 
                    objCmd.Parameters.Add("APPBY2", OracleDbType.NVarchar2).Value = cy.Approval2; 
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = 'Y'; 

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
                        foreach (RetNonRetDcItem cp in cy.RetLst)
                        {
                            if (cp.Isvalid == "Y" && cp.item != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("RDELDETAILPROC", objConns);
                                    if (cy.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("RDELBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.item;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("CLSTOCK", OracleDbType.NVarchar2).Value = cp.Current;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Qty;
                                    objCmds.Parameters.Add("PURFTRN", OracleDbType.NVarchar2).Value = cp.Transaction;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;

                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
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

        public DataTable GetParty()
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,PARTYMASTID from PARTYMAST order by PARTYMASTID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPartyDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYID,ADD1,ADD2,CITY FROM PARTYMAST WHERE PARTYMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetRetItemDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select UNITMAST.UNITID,ITEMID,LATPURPRICE from ITEMMASTER LEFT OUTER JOIN UNITMAST on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID where ITEMMASTER.ITEMMASTERID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetReturnable(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT FROMLOCID,DOCID,DOCDATE,DELTYPE,THROUGH,PARTYNAME,STKTYPE,REFNO,REFDATE,DELDATE,NARRATION,APPBY,APPBY2 FROM RDELBASIC WHERE RDELBASIC.RDELBASICID = '" + id + "' ";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetReturnableItems(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,UNIT,CLSTOCK,QTY,PURFTRN,RATE,AMOUNT from RDELDETAIL WHERE RDELDETAIL.RDELBASICID = '" + id + "' ";

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
                    svSQL = "UPDATE RDELBASIC SET IS_ACTIVE ='N' WHERE RDELBASICID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE RDELBASIC SET IS_ACTIVE ='Y' WHERE RDELBASICID = '" + id + "'";
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
        public DataTable GetAllReturn(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " select RDELBASICID,IS_ACTIVE,DOCID,DOCDATE,DELTYPE,PARTYNAME from RDELBASIC where IS_ACTIVE = 'Y' ORDER BY RDELBASICID DESC";

            }
            else
            {
                SvSql = " select RDELBASICID,IS_ACTIVE,DOCID,DOCDATE,DELTYPE,PARTYNAME from RDELBASIC where IS_ACTIVE = 'N' ORDER BY RDELBASICID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
