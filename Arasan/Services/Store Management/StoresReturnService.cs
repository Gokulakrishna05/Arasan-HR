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
                cmd.CommandText = "Select BRANCHID,FROMLOCID,DOCID,DOCDATE,REFNO,REFDATE,NARRATION,STORESRETBASICID from STORESRETBASIC";
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
                        Narr = rdr["NARRATION"].ToString()

                    };
                    cmpList.Add(cmp);
                }
            }
        }
        return cmpList;
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
            string StatementType = string.Empty; string svSQL = "";

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
                objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.RefDate;
                objCmd.Parameters.Add("NARRATION", OracleDbType.Int64).Value = cy.Narr;
                objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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
