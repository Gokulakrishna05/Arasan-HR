using Arasan.Interface ;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services
{
    public class WorkOrderShortCloseService : IWorkOrderShortClose
    {

        private readonly string _connectionString;
        DataTransactions datatrans;

        public WorkOrderShortCloseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public DataTable GetWorkOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select JOBASIC.DOCID,JOBASIC.BRANCHID,JOBASIC.LOCID as Loc,LOCDETAILS.LOCID,JOBASIC.PARTYID as customerId,PARTYMAST.PARTYNAME as PARTY,ORDTYPE from JOBASIC left Outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=JOBASIC.LOCID  LEFT OUTER JOIN  PARTYMAST on JOBASIC.PARTYID=PARTYMAST.PARTYMASTID  Where PARTYMAST.TYPE IN ('Customer','BOTH')  And JOBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkOrderDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select  ITEMMASTER.ITEMID,JODETAIL.UNIT as units ,UNITMAST.UNITID,QTY,RATE from JODETAIL left outer join ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT where JOBASICID='" + id + "' ";
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
        public string WorkShortCRUD(WorkOrderShortClose cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                

                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'CSF-' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}-{1}",  "CSF", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='CSF-' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.DocId = docid;
                if(cy.ID!=null)
                {
                    cy.ID = null;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("WORKSHORTCLOSEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PURQUOPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                   
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    //objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LocationId;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cy.OrderType;

                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.CustomerId;


                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.Customer;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.Ref;
                    objCmd.Parameters.Add("REFDT", OracleDbType.NVarchar2).Value = cy.RefDate;

                    
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cy.Narr;

                 
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
                        if (cy.Closelst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (WorkCloseItem cp in cy.Closelst)
                                {

                                  

                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into JOCLDETAIL (JOCLBASICID,ORDQTY,PENDQTY,PRECLQTY,RATE,UNIT,ITEMID) VALUES ('" + Pid + "','" + cp.orderqty + "','" + cp.PendQty + "','" + cp.clQty + "','" + cp.rate + "','" + cp.UnitId + "','" + cp.ItemId + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete JOCLDETAIL WHERE JOCLBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (WorkCloseItem cp in cy.Closelst)
                                {

                                   
                                    if (cp.Isvalid == "Y" && cp.ItemId != "0")
                                    {
                                        svSQL = "Insert into JOCLDETAIL (JOCLBASICID,ORDQTY,PENDQTY,PRECLQTY,RATE,UNIT,ITEMID) VALUES ('" + Pid + "','" + cp.orderqty + "','" + cp.PendQty + "','" + cp.clQty + "','" + cp.rate + "','" + cp.UnitId + "','" + cp.ItemId + "')";
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
                    svSQL = "UPDATE JOBASIC SET STATUS ='IS_ACTIVE' WHERE DOCID='" + cy.JopId + "'";
                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);

                    objCmdss.ExecuteNonQuery();
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
