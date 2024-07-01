using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Org.BouncyCastle.Bcpg;
namespace Arasan.Controllers
{
    public class PIApprovalController : Controller
    {
        IPurchaseIndent PurIndent;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public PIApprovalController(IPurchaseIndent _PurService, IConfiguration _configuratio)
        {
            PurIndent = _PurService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }
        public IActionResult List_PI_Approval()
        {
            return View();
        }
    }
}
