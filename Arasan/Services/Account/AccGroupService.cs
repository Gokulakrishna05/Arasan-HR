using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class AccGroupService : IAccGroup
    {
        private readonly string _connectionString;
        public AccGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<AccGroup> GetAllAccGroup()
        {
            List<AccGroup> cmpList = new List<AccGroup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCHMAST.BRANCHID,DOCID,PMNAME,UNIQUEID,CPMNAME,ACCGRBASICID from ACCGRBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=ACCGRBASIC.BRANCHID";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AccGroup cmp = new AccGroup
                        {
                            ID = rdr["ACCGRBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            CPMName = rdr["CPMNAME"].ToString(),
                            PmName = rdr["PMNAME"].ToString(),
                            Unique = rdr["UNIQUEID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                           

                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
    }
}
