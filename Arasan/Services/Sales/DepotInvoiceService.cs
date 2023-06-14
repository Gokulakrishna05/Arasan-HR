using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services.Sales
{
    public class DepotInvoiceService : IDepotInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DepotInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string DirectPurCRUD(DepotInvoice cy)
        {
            throw new NotImplementedException();
        }
    }
}
