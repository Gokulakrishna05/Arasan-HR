using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
namespace Arasan.Services.Production
{
    public class CuringOutwardService :ICuringOutward
    {
        private readonly string _connectionString;
        public CuringOutwardService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
    }
}
