using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class MaterialRequisitionService : IMaterialRequisition
    {
        private readonly string _connectionString;

        public MaterialRequisitionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
  public DataTable GetWorkCenter(string LocationId)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCBASIC.ILOCATION,WCID LOCDETAILS from WCBASIC LEFT OUTER JOIN LOCDETAILS on LOCDETAILS.LOCDETAILSID = WCBASIC.ILOCATION where LOCDETAILS.LOCDETAILSID = '" + LocationId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetmaterialReqDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID,FROMLOCID,PROCESSID,REQTYPE,DOCID,DOCDATE,STORESREQBASICID  from STORESREQBASIC where STORESREQBASICID=" + id + "";
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
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<MaterialRequisition> GetAllMaterial()
        {
            List<MaterialRequisition> cmpList = new List<MaterialRequisition>();
           using (OracleConnection con = new OracleConnection(_connectionString))
           {

                using (OracleCommand cmd = con.CreateCommand())
                {
                   con.Open();
                   cmd.CommandText = "Select BRANCHID,DOCID,DOCDATE,FROMLOCID,PROCESSID,REQTYPE,STORESREQBASICID from STORESREQBASIC";
                   OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                   {
                        MaterialRequisition cmp = new MaterialRequisition
                        {
                            ID = rdr["STORESREQBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Location = rdr["FROMLOCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            RequestType = rdr["REQTYPE"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            DocDa = rdr["DOCDATE"].ToString()
                           
                      

                        };
                        cmpList.Add(cmp);
                    }
               }
            }
           return cmpList;
        }


        public MaterialRequisition GetMaterialById(string eid)
        {
            MaterialRequisition Material = new MaterialRequisition();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHID,DOCID,DOCDATE,FROMLOCID,PROCESSID,REQTYPE,STORESREQBASICID from STORESREQBASIC where STORESREQBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MaterialRequisition cmp = new MaterialRequisition
                        {
                            ID = rdr["STORESREQBASICID"].ToString(),

                           Branch = rdr["BRANCHID"].ToString(),
                           Location = rdr["FROMLOCID"].ToString(),
                            Process = rdr["PROCESSID"].ToString(),
                            RequestType = rdr["REQTYPE"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            DocDa = rdr["DOCDATE"].ToString()


                       };
                         
                        Material = cmp;
                    }
                }
           } 
            return Material;
        }

        public string MaterialCRUD(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

               using (OracleConnection objConn = new OracleConnection(_connectionString))
               {
                  OracleCommand objCmd = new OracleCommand("MATERIALREQPROC", objConn);
                    /*objCmd.Connection = objConn;
                     objCmd.CommandText = "MATERIALREQPROC";*/

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
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.Int64).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDa;
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("REQTYPE", OracleDbType.NVarchar2).Value = cy.RequestType;

                     //objCmd.Parameters.Add("FaxNo", OracleDbType.NVarchar2).Value = cy.FaxNo;
                     //objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = cy.EmailId;
                     //objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Address;
          
                    // objCmd.Parameters.Add("Bin", OracleDbType.NVarchar2).Value = cy.Bin;
                    // objCmd.Parameters.Add("Trade", OracleDbType.NVarchar2).Value = cy.Trade;
                    // objCmd.Parameters.Add("FlowOrd", OracleDbType.Int64).Value = cy.FlowOrd;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

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
}
