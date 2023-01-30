using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services;

public class StoresReturnService : IStoresReturnService
{
    private readonly string _connectionString;
    public StoresReturnService(IConfiguration _configuratio)
    {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
    }
    public IEnumerable<StoresReturn> GetAllStoresReturn()
    {
        List<StoresReturn> cmpList = new List<StoresReturn>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "Select BRANCHID,FROMLOCID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(REFDATE,'dd-MON-yyyy')REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    StoresReturn cmp = new StoresReturn
                    {
                        ID = rdr["STORESRETBASICID"].ToString(),
                        Branch = rdr["BRANCHID"].ToString(),
                        Location = rdr["FROMLOCID"].ToString(),
                        DocId = rdr["DOCID"].ToString(),
                        Docdate = rdr["DOCDATE"].ToString(),
                        RefNo = rdr["REFNO"].ToString(),
                        RefDate = rdr["REFDATE"].ToString(),
                        Narr = rdr["NARRATION"].ToString(),

                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
    }
    public IEnumerable<StoreItem> GetAllStoresReturnItem(string id)
    {
        List<StoreItem> cmpList = new List<StoreItem>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())

            {
                con.Open();
                cmd.CommandText = "Select STORESRETDETAIL.QTY,STORESRETDETAIL.STORESRETDETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID from STORESRETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESRETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESRETDETAIL.STORESRETBASICID='" + id + "'";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    StoreItem cmp = new StoreItem
                    {
                        ItemId = rdr["ITEMID"].ToString(),
                        Unit = rdr["UNITID"].ToString(),
                        Quantity = Convert.ToDouble(rdr["QTY"].ToString())
                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
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
    public DataTable GetSRItemDetails(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select STORESRETDETAIL.QTY,STORESRETDETAIL.STORESRETDETAILID,STORESRETDETAIL.ITEMID,UNITMAST.UNITID,RATE,AMOUNT,TOTAMT,CF,FROMBINID,TOBINID from STORESRETDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESRETDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where STORESRETDETAIL.STORESRETBASICID='" + id + "'";
        DataTable dtt = new DataTable();
        OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        adapter.Fill(dtt);
        return dtt;
    }
    public DataTable GetStoresReturn(string id)
    {
        string SvSql = string.Empty;
        SvSql = "Select STORESRETBASIC.BRANCHID,STORESRETBASIC.DOCID,to_char(STORESRETBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,STORESRETBASIC.FROMLOCID,to_char(STORESRETBASIC.REFDATE,'dd-MON-yyyy')REFDATE,STORESRETBASIC.REFNO,STORESRETBASIC.NARRATION,STORESRETBASICID  from STORESRETBASIC where STORESRETBASIC.STORESRETBASICID=" + id + "";
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
    public string StoresReturnCRUD(StoresReturn cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty; 
            //string svSQL = "";

            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("STORESRETBASICPROC", objConn);
                /*objCmd.Connection = objConn;
                objCmd.CommandText = "STORESRETBASICPROC";*/

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
                objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = DateTime.Parse(cy.Docdate);
                objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                objCmd.Parameters.Add("REFDATE", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
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
                    foreach (StoreItem cp in cy.StrLst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("STORESRETDETAILPROC", objConns);
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
                                objCmds.Parameters.Add("STORESRETBASICID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("QTY", OracleDbType.NVarchar2).Value = cp.Quantity;
                                objCmds.Parameters.Add("UNIT", OracleDbType.NVarchar2).Value = cp.Unit;
                                objCmds.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = cp.rate;
                                objCmds.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = cp.Amount;
                                objCmds.Parameters.Add("TOTAMT", OracleDbType.NVarchar2).Value = cp.TotalAmount;
                                objCmds.Parameters.Add("CF", OracleDbType.NVarchar2).Value = cp.ConFac;
                                objCmds.Parameters.Add("FROMBINID", OracleDbType.NVarchar2).Value = cp.FromBin;
                                objCmds.Parameters.Add("TOBINID", OracleDbType.NVarchar2).Value = cp.ToBin;
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

}
