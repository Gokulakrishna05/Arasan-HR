using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace Arasan.Controllers 
{
    public class DrumChangeController : Controller
    {
        IDrumChange drumchange;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public DrumChangeController(IDrumChange _drumchange, IConfiguration _configuratio)
        {
            drumchange = _drumchange;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DrumChange(string id)
        {
            DrumChange ca = new DrumChange();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Enterd = Request.Cookies["UserName"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.shiftlst = BindShift();
            ca.Loclst = BindLoc();
            ca.typelst = BindEType();
            ca.itemlst = BindItem("");
            List<unpacking> TData = new List<unpacking>();
            unpacking tda = new unpacking();
            List<packeditem> TData1 = new List<packeditem>();
            packeditem tda1 = new packeditem();
            List<reuseitem> TData2 = new List<reuseitem>();
            reuseitem tda2 = new reuseitem();
            List<empitem> TData3 = new List<empitem>();
            empitem tda3 = new empitem();
            if (id==null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new unpacking();
                   
                    tda.drumlst = BindDrum("","");
                    tda.batchlst = Bindbatch("","","");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new packeditem();

                    tda1.drumlst = BindDrum();
                    //tda.batchlst = Bindbatch("", "", "");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda2 = new reuseitem();

                    tda2.Itemlst = BindreuseItem();
                    //tda.batchlst = Bindbatch("", "", "");
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda3 = new empitem();

                    tda3.employeelst = BindEmp();
                    //tda.batchlst = Bindbatch("", "", "");
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
            }
            ca.unpackLst=TData;
            ca.packLst=TData1;
            ca.reuseLst=TData2;
            ca.empLst=TData3;
            return View(ca);
        }

        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItem(string id)
        {
            try
            {
                DataTable dtDesg = drumchange.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindreuseItem()
        {
            try
            {
                DataTable dtDesg = drumchange.GetreuseItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindShift( )
        {
            try
            {
                DataTable dtDesg = datatrans.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "DRUM CHANGING", Value = "DRUM CHANGING" });
                lstdesg.Add(new SelectListItem() { Text = "UN PACKING", Value = "UN PACKING" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrum(string id,string loc)
        {
            try
            {
                DataTable dtDesg = drumchange.GetDrum(id, loc);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = drumchange.Getpackeddrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = drumchange.GetEmployee();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindbatch(string id,string item,string loc)
        {
            try
            {
                DataTable dtDesg = drumchange.GetBatch(id, item, loc);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOTNO"].ToString(), Value = dtDesg.Rows[i]["LOTNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Getdociddetails(string type )
        {
            try
            {
                string docid = "";
                 if(type== "DRUM CHANGING")
                {
                    docid = datatrans.GetDataString("select PREFIX || '' || LASTNO as doc from sequence where PREFIX = 'DCH#'");
                }
                else
                {
                    docid = datatrans.GetDataString("select PREFIX || '' || LASTNO as doc from sequence where PREFIX = 'UNP#'");

                }


                var result = new { docid = docid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindItem(itemid));

        }
        public JsonResult GetDrumJSON(string itemid,string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindDrum(itemid,loc));

        }
        public JsonResult GetoutDrumJSON( )
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindDrum());

        }
        public JsonResult GetBatchJSON(string itemid,string item,string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            // return Json(Bindbatch(itemid, item, loc));
            DataTable dt = new DataTable();
            string batch = "";
            string stock = "";
            string rate = "";
            //string tothrs = "";
            dt = datatrans.GetData("Select L.LOTNO,SUM(L.PLUSQTY-L.MINUSQTY) as qty,avg(L.RATE) as rate from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.LOTNO=LT.LOTNO AND L.ITEMID='" + item + "' AND L.LOCID='" + loc + "' AND L.DRUMNO='" + itemid + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.LOTNO");
            if (dt.Rows.Count > 0)
            {

                batch = dt.Rows[0]["LOTNO"].ToString();
                stock = dt.Rows[0]["qty"].ToString();
                rate = dt.Rows[0]["rate"].ToString();
                
            }

            var result = new { batch = batch, stock = stock  , rate = rate };
            return Json(result);

        }
        public JsonResult GetBatch(string itemid, string item, string loc,string doc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            // return Json(Bindbatch(itemid, item, loc));
            DataTable dt = new DataTable();
            string batch = "";
            string stock = "";
            string tbatch = "";
            string rate = "";
            string items = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE  ITEMMASTERID='"+ item + "'");
            DataTable drum = datatrans.GetData("Select L.DRUMNO from LSTOCKVALUE L , LOTMAST LT WHERE L.ITEMID = '" + item + "' AND L.LOCID = '" + loc + "' AND L.DRUMNO = '" + itemid + "'");
            if (drum.Rows.Count > 0)
            {


                dt = datatrans.GetData("Select L.LOTNO,SUM(L.PLUSQTY-L.MINUSQTY) as qty,avg(L.RATE) as rate from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.LOTNO=LT.LOTNO AND L.ITEMID='" + item + "' AND L.LOCID='" + loc + "' AND L.DRUMNO='" + itemid + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.LOTNO");
                if (dt.Rows.Count > 0)
                {

                    batch = dt.Rows[0]["LOTNO"].ToString();
                    tbatch = dt.Rows[0]["LOTNO"].ToString();
                    stock = dt.Rows[0]["qty"].ToString();
                    rate = dt.Rows[0]["rate"].ToString();

                }
            }
            else
            {
                batch = items + " - " + " - " + itemid + " - " + doc;
                tbatch = "None";
            }
            var result = new { batch = batch, stock = stock, tbatch= tbatch , rate=rate };
            return Json(result);

        }
        public JsonResult Getempdet(string emp )
        {
            
            DataTable dt = new DataTable();
            string code = "";
            string depart = "";
            string cost = "";
          
                dt = datatrans.GetData("Select EMPID,EMPNAME,EMPMASTID,EMPDEPT, DDBASIC.DEPTNAME,EMPCOST,OTPERHR from EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASICID=EMPMAST.EMPDEPT where EMPNAME='" + emp + "'");
                if (dt.Rows.Count > 0)
                {

                code = dt.Rows[0]["EMPID"].ToString();
                depart = dt.Rows[0]["DEPTNAME"].ToString();
                cost = dt.Rows[0]["EMPCOST"].ToString();

                }
           
            var result = new { code = code, depart = depart, cost = cost };
            return Json(result);

        }
        public IActionResult ListDrumChange()
        {
             return View();
        }
        public ActionResult MyListdrumchangegrid(string strStatus, string strfrom, string strTo)
        {
            List<PackingListItem> Reg = new List<PackingListItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = drumchange.GetAllDrumChangeDeatils(strStatus, strfrom, strTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;
                string Edit = string.Empty;

                View = "<a href=ApprovePacking?id=" + dtUsers.Rows[i]["UNPACKBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                Edit = "<a href=PackingNote?id=" + dtUsers.Rows[i]["UNPACKBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["UNPACKBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new PackingListItem
                {
                    id = dtUsers.Rows[i]["UNPACKBASICID"].ToString(),
                    doc = dtUsers.Rows[i]["DOCID"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    type = dtUsers.Rows[i]["ETYPE"].ToString(),
                    
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    viewrow = View,
                    editrow = Edit,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
