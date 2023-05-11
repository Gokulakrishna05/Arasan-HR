using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers 
{
    public class DrumLocationController : Controller
    {
        IDrumLocation drumlocation;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public DrumLocationController(IDrumLocation _drumlocation, IConfiguration _configuratio)
        {
            drumlocation = _drumlocation;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ListDrumLocation()
        {
            IEnumerable<DrumLocation> cmp = drumlocation.GetAllDrumLocation();
            return View(cmp);
        }
    }
}
