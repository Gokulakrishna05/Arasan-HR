using Arasan.Interface;

using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccDescripService : IAccDescrip
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccDescripService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<AccDescrip> GetAllAccDescrip(string Active)
        {
            if (string.IsNullOrEmpty(Active))
            {
                Active = "Yes";
            }

            List<AccDescrip> cmpList = new List<AccDescrip>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,ADCOMPHID,ADTRANSID,ADTRANSDESC,ADSCHEME,ADSCHEMEDESC,ACTIVE from ADCOMPH LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ADCOMPH.BRANCHID WHERE ADCOMPH.ACTIVE='" + Active + "' order by ADCOMPH.ADCOMPHID DESC  ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccDescrip cmp = new AccDescrip
                        {
                            ID = rdr["ADCOMPHID"].ToString(),
                           
                          TransactionName = rdr["ADTRANSDESC"].ToString(),
                          TransactionID = rdr["ADTRANSID"].ToString(),
                          SchemeName = rdr["ADSCHEME"].ToString(),
                          Description = rdr["ADSCHEMEDESC"].ToString(),
                          Branch = rdr["BRANCHID"].ToString(),
                          Active = rdr["ACTIVE"].ToString()

                        };
                     cmpList.Add(cmp);
                    }
                }
            }
                return cmpList;
         }

        public string AccDescripCRUD(AccDescrip cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM ADCOMPH WHERE ADTRANSDESC =LTRIM(RTRIM('" + cy.TransactionName + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "AccDescrip Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ADCOMPH_PROC", objConn);

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

                    objCmd.Parameters.Add("ADTRANSDESC", OracleDbType.NVarchar2).Value = cy.TransactionName;
                    objCmd.Parameters.Add("ADTRANSID", OracleDbType.NVarchar2).Value = cy.TransactionID;
                    objCmd.Parameters.Add("ADSCHEME", OracleDbType.NVarchar2).Value = cy.SchemeName;
                    objCmd.Parameters.Add("ADSCHEMEDESC", OracleDbType.NVarchar2).Value = cy.Description;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.Createdby;
                    objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("CURRENT_DATE", OracleDbType.Date).Value = DateTime.Now;
                    
                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = "Yes";
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

        public DataTable GetAccDescrip(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ADCOMPHID,ADTRANSDESC,ADTRANSID,ADSCHEME,ADSCHEMEDESC FROM ADCOMPH WHERE ADCOMPHID ='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable GetBranch()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS='ACTIVE' order by BRANCHMASTID asc ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADCOMPH SET ACTIVE ='No' WHERE ADCOMPHID='" + id + "'";
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

        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ADCOMPH SET ACTIVE ='Yes' WHERE ADCOMPHID='" + id + "'";
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
