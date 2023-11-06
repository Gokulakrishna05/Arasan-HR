using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using DocumentFormat.OpenXml.Spreadsheet;
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
        public IActionResult DrumHistory(string id)
        {
           return View();
        }
        public JsonResult GetDrumJSON(string drumno)
        {
            try
            {
                DataTable dt = new DataTable();
                string locid = "";
                string wcid = "";
                string stkid = "";
                dt = datatrans.GetData("select LOCID,WCID,DRUM_STOCK_ID from DRUM_STOCK where DOC_DATE = (SELECT MAX(DOC_DATE) AS latest_effective_date FROM DRUM_STOCK) AND DRUM_NO='" + drumno + "'");
                if (dt.Rows.Count > 0)
                {

                    locid = dt.Rows[0]["LOCID"].ToString();
                    wcid = dt.Rows[0]["WCID"].ToString();
                    stkid = dt.Rows[0]["DRUM_STOCK_ID"].ToString();
                }



                var result = "";
                //var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
