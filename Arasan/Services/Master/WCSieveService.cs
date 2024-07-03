using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace Arasan.Services.Master
{
    public class WCSieveService : IWCSieveService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public WCSieveService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetSieve()
        {
            string SvSql = string.Empty;
            SvSql = "Select SIEVE,SIEVEMASTID from SIEVEMAST ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditWCSieve(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCSPRODDETAILID,BRANCHID,WCBASICID from WCSPRODDETAIL WHERE WCSPRODDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable WCSieveDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCSPRODDETAILID,SIEVEID,PRATE,ITEMTYPE from WCSPRODDETAIL WHERE WCSPRODDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetViewEditWCSieve(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCSPRODDETAILID,BRANCHMAST.BRANCHID,WCBASIC.WCID from WCSPRODDETAIL LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=WCSPRODDETAIL.WCBASICID LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=WCSPRODDETAIL.BRANCHID WHERE WCSPRODDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable WCSieveViewDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCSPRODDETAILID,PRATE,ITEMTYPE,SIEVEMAST.SIEVE from WCSPRODDETAIL LEFT OUTER JOIN SIEVEMAST ON SIEVEMAST.SIEVEMASTID=WCSPRODDETAIL.SIEVEID WHERE WCSPRODDETAILID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE WCSPRODDETAIL SET IS_ACTIVE ='N' WHERE WCSPRODDETAILID='" + id + "'";
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
                    svSQL = "UPDATE WCSPRODDETAIL SET IS_ACTIVE = 'Y' WHERE WCSPRODDETAILID='" + id + "'";
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
        public DataTable GetAllWCSieve(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT WCSPRODDETAILID,WCBASIC.WCID,SIEVEMAST.SIEVE,PRATE,ITEMTYPE FROM WCSPRODDETAIL LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=WCSPRODDETAIL.WCBASICID LEFT OUTER JOIN SIEVEMAST ON SIEVEMAST.SIEVEMASTID=WCSPRODDETAIL.SIEVEID WHERE WCSPRODDETAIL.IS_ACTIVE='Y' ORDER BY WCSPRODDETAILID DESC ";
            }
            else
            {
                SvSql = "SELECT WCSPRODDETAILID,WCBASIC.WCID,SIEVEMAST.SIEVE,PRATE,ITEMTYPE FROM WCSPRODDETAIL LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=WCSPRODDETAIL.WCBASICID LEFT OUTER JOIN SIEVEMAST ON SIEVEMAST.SIEVEMASTID=WCSPRODDETAIL.SIEVEID WHERE WCSPRODDETAIL.IS_ACTIVE='N' ORDER BY WCSPRODDETAILID DESC ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string WCSieveCRUD(WCSieve ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("WCSPRODDETAILPROC", objConn);
                    {
                        try
                        {
                            objConn.Open();

                            if (ss.WCSLst != null)
                            {
                                if (ss.ID == null)
                                {
                                    int r = 1;
                                    foreach (WCSItem cp in ss.WCSLst)
                                    {
                                        if (cp.Isvalid == "Y")
                                        {

                                            svSQL = "Insert into WCSPRODDETAIL (BRANCHID,WCBASICID,SIEVEID,PRATE,ITEMTYPE,WCPRODDETAILROW) VALUES ('" + ss.Branch + "','" + ss.WorkCenter + "','" + cp.Sieve + "','" + cp.Rate + "','" + cp.Type + "','" + r + "')";
                                            OracleCommand objCmds = new OracleCommand(svSQL,objConn);
                                            objCmds.ExecuteNonQuery();
                                        }
                                        r++;
                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete WCSPRODDETAIL WHERE WCSPRODDETAILID='" + ss.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                int r = 1;
                                foreach (WCSItem cp in ss.WCSLst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into WCSPRODDETAIL (BRANCHID,WCBASICID,SIEVEID,PRATE,ITEMTYPE,WCPRODDETAILROW) VALUES ('" + ss.Branch + "','" + ss.WorkCenter + "','" + cp.Sieve + "','" + cp.Rate + "','" + cp.Type + "','" + r + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                    r++;
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
