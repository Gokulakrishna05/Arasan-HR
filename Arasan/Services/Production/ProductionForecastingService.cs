using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Production
{
    public class ProductionForecastingService : IProductionForecastingService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductionForecastingService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<ProductionForecasting> GetAllProductionForecasting()
        {
            List<ProductionForecasting> cmpList = new List<ProductionForecasting>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select BRANCHMAST.BRANCHID,DOCID,PLANTYPE,PRODFCBASICID FROM PRODFCBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PRODFCBASIC.BRANCHID;";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProductionForecasting cmp = new ProductionForecasting
                        {
                            ID = rdr["PRODFCBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            PType = rdr["PLANTYPE"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetPFDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,DOCID,to_char(PRODFCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PLANTYPE,MONTH,INCDECPER,HD,to_char(PRODFCBASIC.FINYRPST,'dd-MON-yyyy')FINYRPST,to_char(PRODFCBASIC.FINYRPED,'dd-MON-yyyy')FINYRPED,ENTEREDBY from PRODFCBASIC    where PRODFCBASICID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProdForecastDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PTYPE,ITEMID,UNIT,PREVYQTY,PREVMQTY,PQTY from PRODFCDETAIL where PRODFCDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdForecastDGPasteDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PRODFCBASICID,PTYPE,ITEMID,UNIT,PREVYQTY,PREVMQTY,PQTY from PRODFCDETAIL where PRODFCDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ProductionForecastingCRUD(ProductionForecasting cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PRODFCBASICPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "PRODFCBASICPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = cy.Docdate;
                    objCmd.Parameters.Add("PLANTYPE", OracleDbType.NVarchar2).Value = cy.PType;
                    objCmd.Parameters.Add("MONTH", OracleDbType.NVarchar2).Value = cy.ForMonth;
                    objCmd.Parameters.Add("INCDECPER", OracleDbType.NVarchar2).Value = cy.Ins;
                    objCmd.Parameters.Add("HD", OracleDbType.NVarchar2).Value = cy.Hd;
                    objCmd.Parameters.Add("FINYRPST", OracleDbType.Date).Value = cy.Fordate;
                    objCmd.Parameters.Add("FINYRPED", OracleDbType.Date).Value = cy.Enddate;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Enterd;
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
}
