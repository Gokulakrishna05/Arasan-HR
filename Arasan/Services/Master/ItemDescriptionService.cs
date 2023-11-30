using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;


namespace Arasan.Services.Master
{
    public class ItemDescriptionService : IItemDescriptionService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ItemDescriptionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //public IEnumerable<ItemDescription> GetAllItemDescription()
        //{
        //    List<ItemDescription> brList = new List<ItemDescription>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "SELECT TESTDESCMASTERID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL FROM TESTDESCMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTDESCMASTER.UNIT  order by TESTDESCMASTER.TESTDESCMASTERID DESC ";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemDescription br = new ItemDescription
        //                {
        //                    ID = rdr["TESTDESCMASTERID"].ToString(),
        //                    Des = rdr["TESTDESC"].ToString(),
        //                    Unit = rdr["UNITID"].ToString(),
        //                    Value = rdr["VALUEORMANUAL"].ToString()
                         

        //                };
        //                brList.Add(br);
        //            }
        //        }
        //    }
        //    return brList;
        //}

        public DataTable GetEditItemDescription(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT TESTDESCMASTERID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL FROM TESTDESCMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTDESCMASTER.UNIT  where TESTDESCMASTERID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetUnit()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UNITMASTID,UNITID FROM UNITMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ItemDescriptionCRUD(ItemDescription cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.ID == null)
                    {       
                        svSQL = "Insert into TESTDESCMASTER (TESTDESC,UNIT,VALUEORMANUAL) VALUES ('" + cy.Des + "','" + cy.Unit + "','" + cy.Value + "')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = " UPDATE TESTDESCMASTER SET TESTDESC ='" + cy.Des + "', UNIT = '" + cy.Unit + "', VALUEORMANUAL = '" + cy.Value + "' Where TESTDESCMASTERID = '" + cy.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
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


        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE TESTDESCMASTER SET IS_ACTIVE ='N' WHERE TESTDESCMASTERID='" + id + "'";
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
        public DataTable GetAllItemDescription(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT TESTDESCMASTERID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL FROM TESTDESCMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTDESCMASTER.UNIT WHERE TESTDESCMASTER.IS_ACTIVE='Y' order by TESTDESCMASTER.TESTDESCMASTERID DESC ";

            }
            else
            {
                SvSql = "SELECT TESTDESCMASTERID,TESTDESC,UNITMAST.UNITID,VALUEORMANUAL FROM TESTDESCMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=TESTDESCMASTER.UNIT WHERE TESTDESCMASTER.IS_ACTIVE='N' order by TESTDESCMASTER.TESTDESCMASTERID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
