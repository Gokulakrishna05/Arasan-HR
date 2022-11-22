using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Store_Management
{
    public class DirectDeductionService : IDirectDeductionService
    {
        private readonly string _connectionString;
        public DirectDeductionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<DirectDeduction> GetAllDirectDeduction()
        {
            List<DirectDeduction> staList = new List<DirectDeduction>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID from DEDBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectDeduction sta = new DirectDeduction
                        {
                            ID = rdr["DEDBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Dcno = rdr["DCNO"].ToString(),
                            Reason = rdr["REASON"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Entered = rdr["ENTBY"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            NoDurms = rdr["NOOFD"].ToString(),




                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        public DirectDeduction GetDirectDeductionById(string eid)
        {
            DirectDeduction DirectDeduction = new DirectDeduction();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID  from DEDBASIC where DEDBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DirectDeduction sta = new DirectDeduction
                        {

                            ID = rdr["DEDBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Dcno = rdr["DCNO"].ToString(),
                            Reason = rdr["REASON"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Entered = rdr["ENTBY"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),
                            NoDurms = rdr["NOOFD"].ToString(),
                        };
                        DirectDeduction = sta;
                    }
                }
            }
            return DirectDeduction;
        }

        public string DirectDeductionCRUD(DirectDeduction ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DEDBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DEDBASICPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = ss.Branch;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("DCNO", OracleDbType.NVarchar2).Value = ss.Dcno;
                    objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gro;
                    objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = ss.Entered;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
                    objCmd.Parameters.Add("NOOFD", OracleDbType.NVarchar2).Value = ss.NoDurms;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public DataTable GetDirectDeductionDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,LOCID,DOCID,DOCDATE,DCNO,REASON,GROSS,ENTBY,NARRATION,NOOFD,DEDBASICID  from DEDBASIC where DEDBASICID=" + id + "";
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
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE  ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}

