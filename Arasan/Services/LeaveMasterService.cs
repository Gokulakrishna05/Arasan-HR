using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.PortableExecutable;

namespace Arasan.Services
{
    public class LeaveMasterService : ILeaveMaster
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public LeaveMasterService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

    }
}
