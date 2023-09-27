using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;

using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccDescripController : Controller
    {

        IAccDescrip AccDescripService;

        IConfiguration? _configuration;
        private string? _connectionString;

        public AccDescripController(IAccDescrip _AccDescripService, IConfiguration _configuration)
        {
             AccDescripService = _AccDescripService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");

        }

        //public IActionResult AccDescrip(string id)
        //{
        //    AccountType AG = new AccountType();
        //    AG.CreatedBy = Request.Cookies["UserId"];


        //    //for edit & delete
        //    if (id != null)
        //    {
        //        DataTable dt = new DataTable();
        //        double total = 0;
        //        dt = AccDescripService.GetAccDescrip(id);
        //        if (dt.Rows.Count > 0)
        //        {
        //            AG.TransName = dt.Rows[0]["ADTRANSDESC"].ToString();
        //            AG.TransID = dt.Rows[0]["ADTRANSID"].ToString();
        //            AG.Scheme = dt.Rows[0]["ADSCHEME"].ToString();
        //            AG.Descrip = dt.Rows[0]["ADSCHEMEDESC"].ToString();
        //            AG.Status = dt.Rows[0]["STATUS"].ToString();
                    
        //            AG.ID = id;


        //        }
        //    }
        //    return View();
        //}

        //public IActionResult ListAccDescrip()
        //{
        //    IEnumerable<AccountType> cmp = AccountTypeService.GetAllAccDescrip(status);
        //    return View(cmp);
        //}
    }
}
