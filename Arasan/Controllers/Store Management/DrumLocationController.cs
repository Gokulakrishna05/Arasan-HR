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
            Drumhistory ca = new Drumhistory();
            List<DrumhistoryDet> TData = new List<DrumhistoryDet>();
            DrumhistoryDet tda = new DrumhistoryDet();
            for (int i = 0; i < 1; i++)
            {
                tda = new DrumhistoryDet();
               
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.dumlst = TData;
            return View(ca);
        }
        public JsonResult GetDrumJSON(string drumno)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                string locid = "";
                string wcid = "";
                string stkid = "";
                string tsourid = "";
                string tsourbasicid = "";
                string drum = "";
                string drumid = "";
                string type = "";
                string item = "";
                var result = "";
                dt = datatrans.GetData("select LOCID,WCID,DRUMSTKID,T1SOURCEID,SOURCETYPE,ITEMID,TSOURCEBASICID,DRUM from DRUM_STOCKDET where DOCDATE = (SELECT MAX(DOCDATE) AS latest_effective_date FROM DRUM_STOCKDET) AND DRUM='" + drumno + "'");
                if (dt.Rows.Count > 0)
                {

                    locid = dt.Rows[0]["LOCID"].ToString();
                    wcid = dt.Rows[0]["WCID"].ToString();
                    stkid = dt.Rows[0]["DRUMSTKID"].ToString();
                    tsourbasicid = dt.Rows[0]["TSOURCEBASICID"].ToString();
                    drumid = dt.Rows[0]["DRUM"].ToString();
                    type = dt.Rows[0]["SOURCETYPE"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    //result = new { locid = locid, drumid = drumid };
                }
             
              
                dt2 = datatrans.GetData("select LOCID,WCID,DRUMSTKID,T1SOURCEID,TSOURCEBASICID,DRUM,ITEMID,LOCID,WCID,DRUMSTKID,SOURCETYPE from DRUM_STOCKDET where DRUM='" + drumid + "'  AND ITEMID='"+ item+"' AND TSOURCEBASICID NOT IN '" + tsourbasicid + "'");
                if (dt2.Rows.Count > 0)
                {
                    drum = dt2.Rows[0]["DRUM"].ToString();
                    string loc = dt2.Rows[0]["LOCID"].ToString();
                    string work = dt2.Rows[0]["WCID"].ToString();
                    type = dt2.Rows[0]["SOURCETYPE"].ToString();
                    tsourbasicid = dt2.Rows[0]["TSOURCEBASICID"].ToString();
                    item = dt2.Rows[0]["ITEMID"].ToString();

                }
                dt3 = datatrans.GetData("select LOCID,WCID,DRUMSTKID,T1SOURCEID,TSOURCEBASICID,DRUM,ITEMID,LOCID,WCID,DRUMSTKID,SOURCETYPE from DRUM_STOCKDET where TSOURCEBASICID='" + tsourbasicid + "'  AND SOURCETYPE NOT IN '" + type + "'");
                if (dt3.Rows.Count > 0)
                {
                    drum = dt3.Rows[0]["DRUM"].ToString();
                    string loc = dt3.Rows[0]["LOCID"].ToString();
                    string work = dt3.Rows[0]["WCID"].ToString();
                    type = dt3.Rows[0]["SOURCETYPE"].ToString();
                    item = dt3.Rows[0]["ITEMID"].ToString();

                }


               
                //var result = new { locid = locid, drumid = drumid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
