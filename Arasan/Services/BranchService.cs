using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services
{
    public class BranchService : IBranchService
    {
        private readonly string _connectionString;
        public BranchService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }  
   
    public IEnumerable<Branch> GetAllBranch()
    {
        List<Branch> branchList = new List<Branch>();
        using (OracleConnection con = new OracleConnection(_connectionString))
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.CommandText = "SELECT BRANCHID,ADDRESS1,ADDRESS2,ADDRESS3 FROM BRANCHMAST";
                OracleDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Branch stu = new Branch
                    {
                        BranchName = rdr["BRANCHID"].ToString(),
                        Addr1 = rdr["ADDRESS1"].ToString(),
                        Addr2 = rdr["ADDRESS2"].ToString(),
                        Addr3 = rdr["ADDRESS3"].ToString(),
                    };
                    branchList.Add(stu);
                }
            }
        }
        return branchList;
    }
        public void AddBranch(Branch _branch)
        {

        }
    }
}
