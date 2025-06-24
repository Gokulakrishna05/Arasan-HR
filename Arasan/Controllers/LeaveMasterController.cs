using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class LeaveMasterController : Controller
    {
            ILeaveMaster LeaveM;

            IConfiguration? _configuratio;
            private string? _connectionString;

            DataTransactions datatrans;
            public LeaveMasterController(ILeaveMaster _LeaveM, IConfiguration _configuratio)
            {
                LeaveM = _LeaveM;
                _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
                datatrans = new DataTransactions(_connectionString);
            }
        public IActionResult LeaveMaster()
        {
            return View();
        }
    }
}
