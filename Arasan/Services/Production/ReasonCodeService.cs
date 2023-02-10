using Arasan.Interface.Master;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Production
{
    public class ReasonCodeService : IReasonCodeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ReasonCodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<ReasonCode> GetAllReasonCode()
        {
            throw new NotImplementedException();
        }

        public DataTable GetReasonCode(string id)
        {
            throw new NotImplementedException();
        }

        public string ReasonCodeCRUD(ReasonCode cy)
        {
            throw new NotImplementedException();
        }
        
    }
}
