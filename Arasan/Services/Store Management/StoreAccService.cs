using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Cryptography;

namespace Arasan.Interface.Master
{
    public class StoreAccService : IStoreAccService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StoreAccService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<StoreAcc> GetAllStoreAcc()
        {
            List<StoreAcc> staList = new List<StoreAcc>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID, LOCDETAILS.LOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,RETNO,to_char(RETDATE,'dd-MON-yyyy')RETDATE,NARRATION,STORESACCBASICID from STORESACCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESACCBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=STORESACCBASIC.TOLOCID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoreAcc sta = new StoreAcc
                        {
                            ID = rdr["STORESACCBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            Docid = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Refno = rdr["REFNO"].ToString(),
                            Refdate = rdr["REFDATE"].ToString(),
                            Retno = rdr["RETNO"].ToString(),
                            Retdate = rdr["RETDATE"].ToString(),
                            Narr = rdr["NARRATION"].ToString()

                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }

        //public StoreAcc GetStoreAccById(string eid)
        //{
        //    StoreAcc StoreAcc = new StoreAcc();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select BRANCHID,LOCATIONID,DOCID,DOCDATE,REFNO,REFDATE,RETNO,RETDATE,NARRATION,STORESACCBASICID  from STORESACCBASIC where STORESACCBASICID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                StoreAcc sta = new StoreAcc
        //                {

        //                    ID = rdr["STORESACCBASICID"].ToString(),
        //                    Branch = rdr["BRANCHID"].ToString(),
        //                    Location = rdr["LOCATIONID"].ToString(),
        //                    Docid = rdr["DOCID"].ToString(),
        //                    Docdate = rdr["DOCDATE"].ToString(),
        //                    Refno = rdr["REFNO"].ToString(),
        //                    Refdate = rdr["REFDATE"].ToString(),
        //                    Retno = rdr["RETNO"].ToString(),
        //                    Retdate = rdr["RETDATE"].ToString(),
        //                    Narr = rdr["NARRATION"].ToString()
        //                };
        //                StoreAcc = sta;
        //            }
        //        }
        //    }
        //    return StoreAcc;
        //}

        public string StoreAccCRUD(StoreAcc ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";


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
                    objCmd.Parameters.Add("LOCATIONID", OracleDbType.NVarchar2).Value = ss.Location;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = ss.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = ss.Docdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = ss.Refno;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = ss.Refdate;
                    objCmd.Parameters.Add("RETNO", OracleDbType.NVarchar2).Value = ss.Retno;
                    objCmd.Parameters.Add("RETDATE", OracleDbType.NVarchar2).Value = ss.Retdate;
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
                        foreach (StoItem cp in ss.Stolst)
                        {
                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("STORESACCDETAILPROC", objConns);
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
                                    objCmds.Parameters.Add("STORESACCBASICID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                    objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                    objCmds.Parameters.Add("CONVFACTOR", OracleDbType.NVarchar2).Value = cp.ConFac;
                                    objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.Rate;
                                    objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                    objCmds.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cp.FromBinID;
                                    objCmds.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cp.ToBinID;
                                    objCmds.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = cp.Serial;
                                    objCmds.Parameters.Add("PENDQTY", OracleDbType.NVarchar2).Value = cp.PendQty;
                                    objCmds.Parameters.Add("REJQTY", OracleDbType.NVarchar2).Value = cp.RejQty;
                                    objCmds.Parameters.Add("ACCQTY", OracleDbType.NVarchar2).Value = cp.AccQty;
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

        public DataTable GetStoreAccDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,LOCATIONID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,RETNO,to_char(RETDATE,'dd-MON-yyyy')RETDATE,NARRATION,STORESACCBASICID  from STORESACCBASIC where STORESACCBASICID=" + id + "";
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
   
        public DataTable GetItemCF(string ItemId, string Unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + Unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStoreAccItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESACCDETAIL.QTY,STORESACCDETAIL.STORESACCDETAILID,STORESACCDETAIL.ITEMID,UNITMAST.UNITID,CONVFACTOR,RATE,AMOUNT,FROMBINID,TOBINID,SERIALYN,PENDQTY,REJQTY,ACCQTY from STORESACCDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESACCDETAIL.UNIT  where STORESACCDETAIL.STORESACCBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<StoItem> GetAllStoreAccItem(string id)
        {
            List<StoItem> cmpList = new List<StoItem>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select STORESACCDETAIL.QTY,STORESACCDETAIL.STORESACCDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNIT from STORESACCDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESACCDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESACCDETAIL.STORESACCDETAILID='" + id + "'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StoItem cmp = new StoItem
                        {
                            ItemId = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNIT"].ToString(),
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
