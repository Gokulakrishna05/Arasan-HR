using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class PromotionMailService : IPromotionMailService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PromotionMailService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string PromotionMailCRUD(PromotionMail cy)
        {
            throw new NotImplementedException();
        }
    }
}
