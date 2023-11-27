using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;
using Arasan.Services.Master;
using Arasan.Interface.Account;

namespace Arasan.Controllers
{
    public class CreditorDebitNoteController : Controller
    {

        ICreditorDebitNote CreditorDebitNote;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public CreditorDebitNoteController(ICreditorDebitNote _CreditorDebitNote, IConfiguration _configuratio)
        {
            CreditorDebitNote = _CreditorDebitNote;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CreditorDebitNotes()
        {
            return View();
        }
    }
}
