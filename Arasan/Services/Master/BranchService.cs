using Arasan.Interface;
using Arasan.Models;

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class BranchService : IBranchService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BranchService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetCompany()
        {
            string SvSql = string.Empty;
            SvSql = "select COMPANYID,COMPANYMASTID from  COMPANYMAST order by COMPANYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable Getcountry()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID from  STATEMAST  order by STATEMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCity(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from  CITYMASTER  where STATENAME='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public IEnumerable<Branch> GetAllBranch()
    {
        List<Branch> brList = new List<Branch>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {

            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "Select BRANCHMASTID,COMPANYMAST.COMPANYDESC,BRANCHID,ADDRESS1,STATE,CITY,PINCODE,CSTNO, CSTDATE,BRANCHMAST.STATUS from BRANCHMAST left outer join COMPANYMAST on COMPANYMASTID=BRANCHMAST.COMPANYID WHERE BRANCHMAST.STATUS = 'ACTIVE' ";
 
 
                //cmd.CommandText = "Select BRANCHMASTID,COMPANYMAST.COMPANYDESC,BRANCHID,ADDRESS1,STATE,CITY,PINCODE,CSTNO, CSTDATE,BRANCHMAST.STATUS from BRANCHMAST left outer join COMPANYMAST on COMPANYMASTID=BRANCHMAST.COMPANYID WHERE BRANCHMAST.STATUS = 'ACTIVE'  ";
 
 
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Branch br = new Branch
                    {
                        ID = rdr["BRANCHMASTID"].ToString(),
                        CompanyName = rdr["COMPANYDESC"].ToString(),
                        BranchName = rdr["BRANCHID"].ToString(),
                        Address = rdr["ADDRESS1"].ToString(),
                        StateName = rdr["STATE"].ToString(),
                        City = rdr["CITY"].ToString(),
                        PinCode = rdr["PINCODE"].ToString(),
                        GSTNo = rdr["CSTNO"].ToString(),
                        GSTDate = rdr["CSTDATE"].ToString()

                    };
                    brList.Add(br);
                }
            }
        }
        return brList;
    }
  public string BranchCRUD(Branch cy)
    {
        string msg = "";
        try
        {
            string StatementType = string.Empty;string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(BRANCHID) as cnt FROM BRANCHMAST WHERE BRANCHID =LTRIM(RTRIM('" + cy.BranchName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Branch Already Existed";
                        return msg;
                    }
                }
                else
                {
                    svSQL = " SELECT Count(BRANCHID) as cnt FROM BRANCHMAST WHERE BRANCHID =LTRIM(RTRIM('" + cy.BranchName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Branch Already Existed";
                        return msg;
                    }
                }
                string StaName = datatrans.GetDataString("Select STATE from STATEMAST where STATEMASTID='" + cy.StateName + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                OracleCommand objCmd = new OracleCommand("BRANCHPROC", objConn);


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

                objCmd.Parameters.Add("COMPANYID", OracleDbType.NVarchar2).Value = cy.CompanyName;
                objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.BranchName;
                objCmd.Parameters.Add("ADDRESS1", OracleDbType.NVarchar2).Value = cy.Address;
                objCmd.Parameters.Add("STATE", OracleDbType.NVarchar2).Value = StaName;
                objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                objCmd.Parameters.Add("CSTNO", OracleDbType.NVarchar2).Value = cy.GSTNo;
                objCmd.Parameters.Add("CSTDATE", OracleDbType.NVarchar2).Value = cy.GSTDate;
                objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
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
        public DataTable GetBranch(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMASTID,COMPANYMAST.COMPANYDESC,BRANCHID,ADDRESS1,STATE,CITY,PINCODE,CSTNO, to_char(CSTDATE,'dd-MON-yyyy')CSTDATE,BRANCHMAST.STATUS from BRANCHMAST left outer join COMPANYMAST on COMPANYMASTID=BRANCHMAST.COMPANYID  ";
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
                    svSQL = "UPDATE BRANCHMAST SET STATUS ='INACTIVE' WHERE BRANCHMASTID='" + id + "'";
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

        public DataTable GetEditBranch(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select COMPANYID,BRANCHMAST.BRANCHID,ADDRESS1,STATE,CITY,PINCODE,CSTNO,to_char(CSTDATE,'dd-MON-yyyy')CSTDATE,BRANCHMASTID from  BRANCHMAST  where BRANCHMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
   