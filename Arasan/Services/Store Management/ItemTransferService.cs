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
        DataTransactions datatrans;
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
                    cmd.CommandText = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID from ITEMTRANLOC";
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
                    cmd.CommandText = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID  from ITEMTRANLOC where ITEMTRANLOCID=" + eid + "";
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
                string StatementType = string.Empty; string svSQL = "";


                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ITEMTRANLOCPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "ITEMTRANLOCPROC";*/

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
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("REASONCODE", OracleDbType.NVarchar2).Value = ss.Reason;
                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = ss.Gro;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = ss.Net;
                    objCmd.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = ss.Narr;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                        foreach (Itemtran cp in ss.Itlst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("ITEMTRANLOTPROC", objConns);
                                    if (ss.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;

                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("ITEMTRANLOTID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cp.FromBinID;
                                    objCmds.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cp.ToBinID;
                                    objCmds.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = cp.Serial;
                                    objCmds.Parameters.Add("LSTOCK", OracleDbType.NVarchar2).Value = cp.Lot;
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

        public DataTable GetItemTransferDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,FROMLOC,DESTLOC,DOCID,DOCDATE,REASONCODE,GROSS,NET,NARRATION,ITEMTRANLOCID  from ITEMTRANLOC where ITEMTRANLOCID=" + id + "";
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
        public DataTable GetItem(string Value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER WHERE  ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemTransferItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMTRANLOT.QTY,ITEMTRANLOT.ITEMTRANLOTID,ITEMTRANLOT.ITEMID,UNITMAST.UNITID,RATE,AMOUNT,FROMBINID,TOBINID,SERIALYN,LSTOCK from ITEMTRANLOT LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMTRANLOT.UNIT  where ITEMTRANLOT.ITEMTRANLOTID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<Itemtran> GetAllItemTransferItem(string id)
        {
            List<Itemtran> cmpList = new List<Itemtran>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ITEMTRANLOT.QTY,ITEMTRANLOT.ITEMTRANLOTID,ITEMMASTER.ITEMID,UNITMAST.UNITID from ITEMTRANLOT LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMTRANLOTID=ITEMTRANLOT.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where ITEMTRANLOT.ITEMTRANLOTID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Itemtran cmp = new Itemtran
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITMASTID"].ToString(),
                            Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
