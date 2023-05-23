using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccTreeViewController : Controller
    {
        IAccTreeView Accgroup;
        IConfiguration? _configuratio; 
        private string? _connectionString;

        DataTransactions datatrans;
        public AccTreeViewController(IAccTreeView _Accgroup, IConfiguration _configuratio)
        {
            Accgroup = _Accgroup;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccTreeView(string id)
        {
            return View();
        }

        }
}
