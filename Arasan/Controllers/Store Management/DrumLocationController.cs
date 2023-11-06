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
                DataTable dt2 = new DataTable();
                string locid = "";
                string wcid = "";
                string stkid = "";
                string tsourid = "";
                string tsourbasicid = "";
                string drum = "";
                string drumid = "";
                dt = datatrans.GetData("select LOCID,WCID,DRUMSTKID,T1SOURCEID,TSOURCEBASICID,DRUM from DRUM_STOCKDET where DOCDATE = (SELECT MAX(DOCDATE) AS latest_effective_date FROM DRUM_STOCKDET) AND DRUM='" + drumno + "'");
                if (dt.Rows.Count > 0)
                {

                    locid = dt.Rows[0]["LOCID"].ToString();
                    wcid = dt.Rows[0]["WCID"].ToString();
                    stkid = dt.Rows[0]["DRUMSTKID"].ToString();
                    tsourbasicid = dt.Rows[0]["TSOURCEBASICID"].ToString();
                    drumid = dt.Rows[0]["DRUM"].ToString();
                    dt2 = datatrans.GetData("select LOCID,WCID,DRUMSTKID,T1SOURCEID,TSOURCEBASICID,DRUM,LOCID,WCID,DRUMSTKID,SOURCETYPE from DRUM_STOCKDET where DRUM='" + drumid + "'  AND TSOURCEBASICID NOT IN '" + tsourbasicid + "'");
                    drum = dt2.Rows[0]["DRUM"].ToString();
                  string  loc = dt2.Rows[0]["LOCID"].ToString();
                  string  work = dt2.Rows[0]["WCID"].ToString();
                  string  type = dt2.Rows[0]["SOURCETYPE"].ToString();

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
