using Arasan.Interface.Master;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services.Production
{
    public class ReasonCodeService : IReasonCodeService
    {
        private readonly string _connectionString;

        public ReasonCodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<ReasonCode> GetAllReasonCode()
        {
            throw new NotImplementedException();
        }

        public string ReasonCodeCRUD(ReasonCode cy)
        {
            throw new NotImplementedException();
        }
    }
}
