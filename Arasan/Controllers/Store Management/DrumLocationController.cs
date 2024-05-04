using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
//using DocumentFormat.OpenXml.Spreadsheet;
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
            //IEnumerable<DrumLocation> cmp = drumlocation.GetAllDrumLocation();
            return View();
        }
        public ActionResult MyListDrumLocationGrid()
        {
            List<DrumItems> Reg = new List<DrumItems>();
            DataTable dtUsers = new DataTable();
            dtUsers = (DataTable)drumlocation.GetAllListDrumItem();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                //string Qc = string.Empty;
                //string GRNStatus = string.Empty;
                //string Account = string.Empty;
                //string View = string.Empty;
                //string EditRow = string.Empty;
                //string DeleteRow = string.Empty;

                //if (dtUsers.Rows[i]["STATUS"].ToString() == "GRN Completed")
                //{
                //    GRNStatus = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                //    Account = "<a href=GRNAccount?id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/profit.png' alt='View Details' width='20' /></a>";
                //    EditRow = "";
                //}
                //else
                //{
                //    GRNStatus = dtUsers.Rows[i]["STATUS"].ToString();
                //    //GRNStatus = "<a href=ViewQuote?id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                //    EditRow = "<a href=GRN?id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                //}
                //View = "<a href=ViewGRN?id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                //DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["GRNBLBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                Reg.Add(new DrumItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["LSTOCKVALUEID"].ToString()),
                    drum = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    //supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    //qcresult = dtUsers.Rows[i]["QCSTATUS"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

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
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();
                DataTable dt6 = new DataTable();
                DataTable dt7 = new DataTable();
                string locid = "";
                string locid1 = "";
                string locid2 = "";
                string locid3 = "";
                string locid4 = "";
                string loc = "";
                string loc1 = "";
                string loc2 = "";
                string loc3 = "";
                string loc4 = "";
                string wcid = "";
                string stkid = "";
                string tsourid = "";
                string tsourbasicid = "";
                string drum = "";
                string drum1 = "";
                string drum2 = "";
                string drum3 = "";
                string drumid = "";
                string drumid123 = "";
                string type = "";
                string type1 = "";
                string type2 = "";
                string type3 = "";
                string type4 = "";
                string item = "";
                string item1 = "";
                string item2 = "";
                string item3 = "";
                string item4 = "";
                string itemid = "";
                string itemid1 = "";
                string itemid2 = "";
                string itemid3 = "";
                string itemid4 = "";
                string Item123 = "";
                string loc123 = "";
                string initem = "";
                string inloc = "";
                string intype = "";
                string type123 = "";
                string grnid = "";
                string initem1 = "";
                string inloc1 = "";
                string intype1 = "";
                string party = "";
                dt = datatrans.GetData("select LOCDETAILS.LOCID,DRUM_STOCKDET.LOCID as loc,DRUM_STOCKDET.ITEMID as item,WCID,DRUMSTKID,DRUM_STOCKDET.T1SOURCEID,SOURCETYPE,ITEMMASTER.ITEMID,TSOURCEBASICID,DRUM from DRUM_STOCKDET LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUM_STOCKDET.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=DRUM_STOCKDET.ITEMID where DOCDATE = (SELECT MAX(DOCDATE) AS latest_effective_date FROM DRUM_STOCKDET) AND DRUM='" + drumno + "'");
                if (dt.Rows.Count > 0)
                {

                    locid = dt.Rows[0]["LOCID"].ToString();
                   
                    wcid = dt.Rows[0]["WCID"].ToString();
                    stkid = dt.Rows[0]["DRUMSTKID"].ToString();
                    tsourbasicid = dt.Rows[0]["TSOURCEBASICID"].ToString();
                    drumid = dt.Rows[0]["DRUM"].ToString();
                    drumid123 = dt.Rows[0]["DRUM"].ToString();
                    type = dt.Rows[0]["SOURCETYPE"].ToString();
                    type123 = dt.Rows[0]["SOURCETYPE"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    Item123 = dt.Rows[0]["item"].ToString();
                    itemid = dt.Rows[0]["item"].ToString();
                    loc = dt.Rows[0]["loc"].ToString();
                    loc123 = dt.Rows[0]["loc"].ToString();
                    //result = new { locid = locid, drumid = drumid };
                }
             
              
                dt2 = datatrans.GetData("select LOCDETAILS.LOCID,WCID,DRUM_STOCKDET.LOCID as loc,DRUM_STOCKDET.ITEMID as item,DRUM_STOCKDET.T1SOURCEID,TSOURCEBASICID,DRUM,ITEMMASTER.ITEMID,DRUMSTKID,SOURCETYPE from DRUM_STOCKDET LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUM_STOCKDET.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=DRUM_STOCKDET.ITEMID where DRUM='" + drumid123 + "'  AND DRUM_STOCKDET.ITEMID='" + Item123 + "' AND TSOURCEBASICID NOT IN '" + tsourbasicid + "'");
                if (dt2.Rows.Count > 0)
                {
                    drum = dt2.Rows[0]["DRUM"].ToString();
                    locid1 = dt2.Rows[0]["LOCID"].ToString();
                    string work = dt2.Rows[0]["WCID"].ToString();
                    type1 = dt2.Rows[0]["SOURCETYPE"].ToString();
                    tsourbasicid = dt2.Rows[0]["TSOURCEBASICID"].ToString();
                    item1 = dt2.Rows[0]["ITEMID"].ToString();
                    itemid1 = dt2.Rows[0]["item"].ToString();
                    loc1 = dt2.Rows[0]["loc"].ToString();
                    type123 = dt2.Rows[0]["SOURCETYPE"].ToString();
                    loc123 = dt2.Rows[0]["loc"].ToString();
                    Item123 = dt2.Rows[0]["item"].ToString();
                    drumid123 = dt.Rows[0]["DRUM"].ToString();
                }
                dt3 = datatrans.GetData("select LOCDETAILS.LOCID,WCID,DRUM_STOCKDET.LOCID as loc,DRUM_STOCKDET.ITEMID as item,DRUMSTKID,DRUM_STOCKDET.T1SOURCEID,TSOURCEBASICID,DRUM,ITEMMASTER.ITEMID,SOURCETYPE from DRUM_STOCKDET LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUM_STOCKDET.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=DRUM_STOCKDET.ITEMID where TSOURCEBASICID='" + tsourbasicid + "'  AND SOURCETYPE NOT IN '" + type123 + "'");
                if (dt3.Rows.Count > 0)
                {
                    drum1 = dt3.Rows[0]["DRUM"].ToString();
                    locid2 = dt3.Rows[0]["LOCID"].ToString();
                    string work = dt3.Rows[0]["WCID"].ToString();
                    type2 = dt3.Rows[0]["SOURCETYPE"].ToString();
                    item2 = dt3.Rows[0]["ITEMID"].ToString();
                    itemid2 = dt3.Rows[0]["item"].ToString();
                    loc2 = dt3.Rows[0]["loc"].ToString();
                    type123 = dt3.Rows[0]["SOURCETYPE"].ToString();
                    loc123 = dt3.Rows[0]["loc"].ToString();
                    Item123 = dt3.Rows[0]["item"].ToString();
                    drumid123 = dt3.Rows[0]["DRUM"].ToString();
                }
               
                dt4 = datatrans.GetData("select LOCDETAILS.LOCID,WCID,DRUM_STOCKDET.LOCID as loc,DRUM_STOCKDET.ITEMID as item,DRUMSTKID,DRUM_STOCKDET.T1SOURCEID,TSOURCEBASICID,DRUM,ITEMMASTER.ITEMID,SOURCETYPE from DRUM_STOCKDET LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUM_STOCKDET.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=DRUM_STOCKDET.ITEMID where DRUM='" + drumid123 + "'  AND  DRUM_STOCKDET.ITEMID = '" + Item123 + "' AND DRUM_STOCKDET.LOCID='" + loc123 + "'  AND SOURCETYPE NOT IN '" + type123 + "'");
                if (dt4.Rows.Count > 0)
                {
                    drum2 = dt4.Rows[0]["DRUM"].ToString();
                    locid3 = dt4.Rows[0]["LOCID"].ToString();
                    string work = dt4.Rows[0]["WCID"].ToString();
                    type3 = dt4.Rows[0]["SOURCETYPE"].ToString();
                    item3 = dt4.Rows[0]["ITEMID"].ToString();
                    itemid3 = dt4.Rows[0]["item"].ToString();
                    loc3 = dt4.Rows[0]["loc"].ToString();
                    type123 = dt4.Rows[0]["SOURCETYPE"].ToString();
                    loc123 = dt4.Rows[0]["loc"].ToString();
                    Item123 = dt4.Rows[0]["item"].ToString();
                    drumid123 = dt4.Rows[0]["DRUM"].ToString();
                }
                dt5 = datatrans.GetData("select LOCDETAILS.LOCID,WCID,DRUM_STOCKDET.LOCID as loc,DRUM_STOCKDET.ITEMID as item,DRUMSTKID,DRUM_STOCKDET.T1SOURCEID,TSOURCEBASICID,DRUM,ITEMMASTER.ITEMID,SOURCETYPE from DRUM_STOCKDET LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=DRUM_STOCKDET.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=DRUM_STOCKDET.ITEMID where DRUM='" + drumid123 + "'  AND  DRUM_STOCKDET.ITEMID = '" + Item123 + "' AND DRUM_STOCKDET.LOCID NOT IN'" + loc123 + "'  AND SOURCETYPE NOT IN '" + type123 + "'");
                if (dt5.Rows.Count > 0)
                {
                    drum3 = dt5.Rows[0]["DRUM"].ToString();
                    locid4 = dt5.Rows[0]["LOCID"].ToString();
                    string work = dt5.Rows[0]["WCID"].ToString();
                    type4 = dt5.Rows[0]["SOURCETYPE"].ToString();
                    item4 = dt5.Rows[0]["ITEMID"].ToString();
                    itemid4 = dt5.Rows[0]["item"].ToString();
                    loc4 = dt5.Rows[0]["loc"].ToString();
                    type123 = dt5.Rows[0]["SOURCETYPE"].ToString();
                    loc123 = dt5.Rows[0]["loc"].ToString();
                    Item123 = dt5.Rows[0]["item"].ToString();
                    drumid123 = dt5.Rows[0]["DRUM"].ToString();
                    tsourbasicid = dt5.Rows[0]["TSOURCEBASICID"].ToString();
                }
                if(type3== type)
                {
                    drum2 = "";
                    type3 = "";
                    item3 = "";
                    locid3 = "";
                }
                dt6 = datatrans.GetData("select INVENTORY_ITEM_TRANS.INVENTORY_ITEM_ID,INVENTORY_ITEM_TRANS.ITEM_ID as item,GRNID, LOCDETAILS.LOCID,TSOURCEBASICID,ITEMMASTER.ITEMID,INVENTORY_ITEM_TRANS.LOCATION_ID,INVENTORY_ITEM_TRANS.BRANCH_ID,INVENTORY_ITEM_ID,TSOURCEID,TRANS_DATE,TSOURCEID,TRANS_TYPE from INVENTORY_ITEM_TRANS LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=INVENTORY_ITEM_TRANS.LOCATION_ID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=INVENTORY_ITEM_TRANS.ITEM_ID where TSOURCEBASICID='" + tsourbasicid +"'");
                if (dt6.Rows.Count > 0)
                {
                    
                    inloc = dt6.Rows[0]["LOCID"].ToString();
                     
                    intype = dt6.Rows[0]["TRANS_TYPE"].ToString();
                    initem = dt6.Rows[0]["ITEMID"].ToString();
                    itemid4 = dt6.Rows[0]["item"].ToString();
                    loc4 = dt6.Rows[0]["LOCATION_ID"].ToString();
                    type123 = dt6.Rows[0]["TRANS_TYPE"].ToString();
                    loc123 = dt6.Rows[0]["LOCATION_ID"].ToString();
                    Item123 = dt6.Rows[0]["item"].ToString();
                    grnid = dt6.Rows[0]["GRNID"].ToString();
                    
                }
                dt6 = datatrans.GetData("select INVENTORY_ITEM_TRANS.INVENTORY_ITEM_ID,INVENTORY_ITEM_TRANS.ITEM_ID as item,GRNID, LOCDETAILS.LOCID,TSOURCEBASICID,ITEMMASTER.ITEMID,INVENTORY_ITEM_TRANS.LOCATION_ID,INVENTORY_ITEM_TRANS.BRANCH_ID,INVENTORY_ITEM_ID,TSOURCEID,TRANS_DATE,TSOURCEID,TRANS_TYPE from INVENTORY_ITEM_TRANS LEFT OUTER JOIN LOCDETAILS ON LOCDETAILSID=INVENTORY_ITEM_TRANS.LOCATION_ID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=INVENTORY_ITEM_TRANS.ITEM_ID where GRNID='" + grnid + "' AND INVENTORY_ITEM_TRANS.ITEM_ID='"+ Item123 + "' ");
                if (dt6.Rows.Count > 0)
                {

                    inloc1 = dt6.Rows[0]["LOCID"].ToString();

                    intype1 = dt6.Rows[0]["TRANS_TYPE"].ToString();
                    initem1 = dt6.Rows[0]["ITEMID"].ToString();
                    itemid4 = dt6.Rows[0]["item"].ToString();
                    loc4 = dt6.Rows[0]["LOCATION_ID"].ToString();
                    type123 = dt6.Rows[0]["TRANS_TYPE"].ToString();
                    loc123 = dt6.Rows[0]["LOCATION_ID"].ToString();
                    Item123 = dt6.Rows[0]["item"].ToString();
                    grnid = dt6.Rows[0]["GRNID"].ToString();

                }
                dt7 = datatrans.GetData("select GRNBLBASICID,PARTYMAST.PARTYNAME FROM GRNBLBASIC LEFT OUTER JOIN PARTYMAST ON PARTYMASTID=GRNBLBASIC.PARTYID where GRNBLBASICID='" + grnid + "' ");
                if (dt7.Rows.Count > 0)
                {

                    party= dt7.Rows[0]["PARTYNAME"].ToString();
                    grnid = dt7.Rows[0]["GRNBLBASICID"].ToString();

                }
                var result = new { locid = locid, drumid = drumid, type= type, drum= drum, locid1= locid1, type1= type1, item1= item1, drum1= drum1, locid2= locid2, type2= type2, item2= item2, drum2= drum2 , locid3 = locid3,item3 = item3 , drum3 = drum3, locid4= locid4, type4= type4, item4= item4, type3= type3,item=item, inloc= inloc , intype = intype, initem= initem, inloc1= inloc1 , intype1 = intype1, initem1= initem1, party= party };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
