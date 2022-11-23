using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Arasan.Services.Store_Management
{
    public class ItemTransferService : IItemTransferService
    {
        private readonly string _connectionString;
        public ItemTransferService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ItemTransfer> GetAllItemTransfer()
        {
            List<ItemTransfer> staList = new List<ItemTransfer>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID from ITEMTRANLOC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemTransfer sta = new ItemTransfer
                        {
                            ID = rdr["ITEMTRANLOCID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["FROMLOC"].ToString(),
                            Toloc = rdr["DESTLOC"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Reason = rdr["REASONCODE"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Net = rdr["NET"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),

                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        public ItemTransfer GetItemTransferById(string eid)
        {
            ItemTransfer ItemTransfer = new ItemTransfer();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID  from ITEMTRANLOC where ITEMTRANLOCID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemTransfer sta = new ItemTransfer
                        {

                            ID = rdr["ITEMTRANLOCID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["FROMLOC"].ToString(),
                            Toloc = rdr["DESTLOC"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Reason = rdr["REASONCODE"].ToString(),
                            Gro = rdr["GROSS"].ToString(),
                            Net = rdr["NET"].ToString(),
                            Narr = rdr["NARRATION"].ToString(),

                        };
                        ItemTransfer = sta;
                    }
                }
            }
            return ItemTransfer;
        }

        public string ItemTransferCRUD(ItemTransfer ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("STORESACCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STORESACCBASICPROC";*/

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
                    objCmd.Parameters.Add("FROMLOC", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DESTLOC", OracleDbType.NVarchar2).Value = ss.Toloc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = DateTime.Parse(ss.Docdate);
                    objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gro;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = ss.Net;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
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

        public DataTable GetItemTransferDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID  from ITEMTRANLOC where ITEMTRANLOCID=" + id + "";
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
