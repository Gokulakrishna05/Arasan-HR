using Arasan.Interface;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services.Store_Management
{
    public class MaterialRequisitionService :IMaterialRequisition
    {
        private readonly string _connectionString;

        public MaterialRequisitionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public DataTable GetItem(string value)
        {
            throw new NotImplementedException();
        }

        public DataTable GetItemGrp()
        {
            throw new NotImplementedException();
        }

        public DataTable GetLocation()
        {
            throw new NotImplementedException();
        }

        public DataTable GetWorkCenter()
        {
            throw new NotImplementedException();
        }
    }
}
