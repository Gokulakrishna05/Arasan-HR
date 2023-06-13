using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class WorkCentersService : IWorkCentersService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public WorkCentersService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public IEnumerable<WorkCenters> GetAllWorkCenters()
        {
            {
                List<WorkCenters> cmpList = new List<WorkCenters>();
                using (OracleConnection con = new OracleConnection(_connectionString))
                {

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select WCBASIC.WCID,WCTYPE,LOCDETAILS.LOCID,WCBASICID,WCBASIC.STATUS from WCBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID =WCBASIC.ILOCATION WHERE WCBASIC.STATUS= 'ACTIVE'";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            WorkCenters cmp = new WorkCenters
                            {
                                ID = rdr["WCBASICID"].ToString(),
                                Wid = rdr["WCID"].ToString(),
                                WType = rdr["WCTYPE"].ToString(),
                                Iloc = rdr["LOCID"].ToString(),
                                status = rdr["STATUS"].ToString()


                            };
                            cmpList.Add(cmp);
                        }
                    }
                }
                return cmpList;
            }
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetWorkCenters(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select WCID,WCTYPE,DOCDATE,ILOCATION,QCLOCATION,PARTYID,WIPITEMID,WIPLOCID,CONVITEMID,CONVLOCID,BUNKERYN,OPBBAL,MLYN,OPMLBAL,PROCLOTYN,CAPACITY,PRODSCHYN,UTILPERCENT,PRODYN,DRUMILOCATION,ENRMETF,MANREQ,COST,COSTUNIT,REMARKS,WCBASICID from WCBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetWorkCentersDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select WCBASICID,MACHINEID,MCOST,WCMDETAILID from WCMDETAIL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string WorkCentersCRUD(WorkCenters cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; 
                string svSQL = "";

                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM WCBASIC WHERE WCID = LTRIM(RTRIM('" + cy.Wid + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "HsnCode Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("WCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "WCBASICPROC";*/

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
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Wid;
                    objCmd.Parameters.Add("WCTYPE", OracleDbType.NVarchar2).Value = cy.WType;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                    objCmd.Parameters.Add("ILOCATION", OracleDbType.NVarchar2).Value = cy.Iloc;
                    objCmd.Parameters.Add("QCLOCATION", OracleDbType.NVarchar2).Value = cy.QcLoc;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("WIPITEMID", OracleDbType.NVarchar2).Value = cy.WipItemid;
                    objCmd.Parameters.Add("WIPLOCID", OracleDbType.NVarchar2).Value =cy.WipLocid;
                    objCmd.Parameters.Add("CONVITEMID", OracleDbType.NVarchar2).Value = cy.ConvItem;
                    objCmd.Parameters.Add("CONVLOCID", OracleDbType.NVarchar2).Value = cy.ConvLoc;
                    objCmd.Parameters.Add("BUNKERYN", OracleDbType.NVarchar2).Value = cy.Bunker;
                    objCmd.Parameters.Add("OPBBAL", OracleDbType.NVarchar2).Value = cy.Opbbl;
                    objCmd.Parameters.Add("MLYN", OracleDbType.NVarchar2).Value = cy.Mill;
                    objCmd.Parameters.Add("OPMLBAL", OracleDbType.NVarchar2).Value = cy.Opmlbal;
                    objCmd.Parameters.Add("PROCLOTYN", OracleDbType.NVarchar2).Value = cy.ProcLot;
                    objCmd.Parameters.Add("CAPACITY", OracleDbType.NVarchar2).Value = cy.Cap;
                    objCmd.Parameters.Add("PRODSCHYN", OracleDbType.NVarchar2).Value = cy.ProdSch;
                    objCmd.Parameters.Add("UTILPERCENT", OracleDbType.NVarchar2).Value = cy.Uttl;
                    objCmd.Parameters.Add("PRODYN", OracleDbType.NVarchar2).Value = cy.Production;
                    objCmd.Parameters.Add("DRUMILOCATION", OracleDbType.NVarchar2).Value = cy.DrumLoc;
                    objCmd.Parameters.Add("ENRMETF", OracleDbType.NVarchar2).Value = cy.Energy;
                    objCmd.Parameters.Add("MANREQ", OracleDbType.NVarchar2).Value = cy.Man;
                    objCmd.Parameters.Add("COST", OracleDbType.NVarchar2).Value = cy.Cost;
                    objCmd.Parameters.Add("COSTUNIT", OracleDbType.NVarchar2).Value = cy.Unit;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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
                        foreach (WorkCentersDetail cp in cy.WorkCenterlst)
                        {

                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("WCMDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("WCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("MACHINEID", OracleDbType.NVarchar2).Value = cp.MId;
                                objCmds.Parameters.Add("MCOST", OracleDbType.NVarchar2).Value = cp.MCost;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                                objConns.Open();
                                objCmds.ExecuteNonQuery();

                                objConns.Close();

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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE WCBASIC SET STATUS ='INACTIVE' WHERE WCBASICID='" + id + "'";
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
    }
}
