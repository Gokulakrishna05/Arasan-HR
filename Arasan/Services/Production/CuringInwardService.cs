using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services.Production
{
    public class CuringInwardService : ICuringInwardService
    {
        private readonly string _connectionString;
        public CuringInwardService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<CuringInward> GetAllCuringInward()
        {
            List<CuringInward> cmpList = new List<CuringInward>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCH,DOCID,DOCDATE,WCID,SHIFT,ENTEREDBY,REMARKS,CURINGESBASICID from CURINGESBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CuringInward cmp = new CuringInward
                        {
                            ID = rdr["CURINGESBASICID"].ToString(),
                            Branch = rdr["BRANCH"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            WorkCenter = rdr["WCID"].ToString(),
                            Shift = rdr["SHIFT"].ToString(),
                            RecevedBy = rdr["ENTEREDBY"].ToString(),
                            Remarks = rdr["REMARKS"].ToString(),

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string CuringInwardCRUD(CuringInward cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CURINGESBASICPROC",objConn);

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
                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.RecevedBy;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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
                        foreach (CIItem ca in cy.CILst)
                        {
                            if (ca.Isvalid == "Y" && ca.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("CURINGESDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("CURINGESBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ca.ItemId;
                                    objCmds.Parameters.Add("BATCHQTY", OracleDbType.NVarchar2).Value = ca.batchqty;
                                    objCmds.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = ca.drumno;
                                    objCmds.Parameters.Add("BATCHNO", OracleDbType.NVarchar2).Value = ca.batchno;
                                    //objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = ca.AccQty;
                                    //objCmds.Parameters.Add("COSTRATE", OracleDbType.NVarchar2).Va
                                    //lue = ca.CostRate;
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
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable DrumDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "select DRUMMASTID,DRUMNO from DRUMMAST where DRUMTYPE='PRODUCTION'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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

        public DataTable GetCuringInward(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select CURINGESBASIC.BRANCH,CURINGESBASIC.DOCID,to_char(CURINGESBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,CURINGESBASICID,CURINGESBASIC.WCID,CURINGESBASIC.SHIFT,CURINGESBASIC.ENTEREDBY,CURINGESBASIC.REMARKS  from CURINGESBASIC Where CURINGESBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCuringInwardDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CURINGESBASICID,ITEMID,BATCHQTY,DRUMNO,BATCHNO from CURINGESDETAIL where CURINGESDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
