using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Production;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class PackingNoteController : Controller
    {
        IPackingNote Packing;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PackingNoteController(IPackingNote _Packing, IConfiguration _configuratio)
        {
            Packing = _Packing;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PackingNote(string id)
        {
            PackingNote ca = new PackingNote();
            ca.Brlst = BindBranch();
            ca.Worklst = BindWorkCenter();
            //ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Enterd = Request.Cookies["UserName"];
            ca.Shiftlst = BindShift();
            ca.RecList = BindEmp();
            ca.DrumLoclst = BindDrumLoc();
            ca.Schlst = BindSche();
            ca.Itemlst = BindItemlst("");
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PackN");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<DrumDetail> TData = new List<DrumDetail>();
            DrumDetail tda = new DrumDetail();
            if (id == null)
            {
                //for (int i = 0; i < 3; i++)
                //{
                //    tda = new DrumDetail();
                //    tda.DrumNolst = Binddrum("","");

                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}
            }
            else
            {
                DataTable dt = new DataTable();
                dt = Packing.GetPackingNote(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkId = dt.Rows[0]["TOLOCDETAILSID"].ToString();

                    ca.ID = id;
                    ca.DrumLoc = dt.Rows[0]["DRUMLOCATION"].ToString();
                    ca.Itemlst = BindItemlst(ca.DrumLoc);
                    ca.ItemId = dt.Rows[0]["OITEMID"].ToString();
                    //DataTable dt3 = new DataTable();
                    //dt3 = Packing.GetItemDet(dt.Rows[0]["OITEMID"].ToString());
                    //if (dt3.Rows.Count > 0)
                    //{
                    //    ca.DrumLoc = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                    //}

                    ca.ProdSchNo = dt.Rows[0]["PSCHNO"].ToString();

                    ca.startdate = dt.Rows[0]["STARTDATE"].ToString() + " - " + dt.Rows[0]["STARTTIME"].ToString();
                    ca.enddate = dt.Rows[0]["ENDDATE"].ToString() + " - " + dt.Rows[0]["ENDTIME"].ToString();

                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.LotNo = dt.Rows[0]["PACLOTNO"].ToString();
                    ca.PackYN = dt.Rows[0]["PACKCONSYN"].ToString();

                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();

                    ca.Remark = dt.Rows[0]["REMARKS"].ToString();

                }
                DataTable dt2 = new DataTable();

                dt2 = Packing.GetDrumItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DrumDetail();
                        //tda.DrumNolst = BindDrumNo(ca.DrumLoc);
                        tda.DrumNolst = Binddrum(ca.ItemId, ca.DrumLoc);
                        tda.drum = dt2.Rows[i]["IDRUMNO"].ToString();
                        //tda.Batchlst = BindBatch(tda.DrumNo);


                        tda.Isvalid = "Y";
                        tda.batch = dt2.Rows[i]["IBATCHNO"].ToString();
                        tda.qty = dt2.Rows[i]["IBATCHQTY"].ToString();
                        tda.comp = dt2.Rows[i]["COMBNO"].ToString();


                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
            }
            ca.DrumDetlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult PackingNote(PackingNote Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Packing.PackingNoteCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PackingNote Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PackingNote Updated Successfully...!";
                    }
                    return RedirectToAction("ListPackingNote");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PackingNote";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = Packing.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindDrumLoc()
        {
            try
            {
                DataTable dtDesg = Packing.GetDrumLocation();
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = Packing.ShiftDeatils();
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
        //public JsonResult GetDrumJSON(string id, string item)
        //{
        //    string DrumID = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + id + "' ");
        //    DrumIssueEntryItem model = new DrumIssueEntryItem();
        //    model.drumlst = Binddrum(DrumID, item);
        //    return Json(Binddrum(DrumID, item));
        //}
        public List<SelectListItem> Binddrum(string value, string item)
        {
            try
            {
                DataTable dtDesg = Packing.DrumDeatils(value, item);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUM_NO"].ToString(), Value = dtDesg.Rows[i]["DRUM_ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string supid)
        {
            PackingNote model = new PackingNote();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        //public JsonResult GetShedJSON(string supid)
        //{
        //    PackingNote model = new PackingNote();
        //    model.Itemlst = BindSche(supid);
        //    return Json(BindSche(supid));

        //}
        public List<SelectListItem> BindSche()
        {
            try
            {
                DataTable dtDesg = Packing.GetSchedule();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PSBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                if (value == "10044000011739")
                {
                    DataTable dtDesg = Packing.GetItembyId(value);
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });

                    }
                }
                else
                {
                    DataTable dtDesg = Packing.GetItem(value);
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });

                    }
                }

                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetDrumDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();


        //        string qty = "";

        //        dt = Packing.GetDrumDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {


        //            qty = dt.Rows[0]["QTY"].ToString();



        //        }

        //        var result = new { qty = qty };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult GetDrumDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();


        //        string qty = "";

        //        dt = Packing.GetDrumDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            qty = dt.Rows[0]["OQTY"].ToString();

        //        }

        //        var result = new {  qty = qty };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListPackingNote()
        {
            //IEnumerable<PackingNote> cmp = Packing.GetAllPackingNote(st, ed);
            return View();
        }
        public ActionResult MyListPackinggrid(string strStatus, string strfrom, string strTo)
        {
            List<PackingListItem> Reg = new List<PackingListItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Packing.GetAllPackingDeatils(strStatus, strfrom, strTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;
                string Edit = string.Empty;

                View = "<a href=ApprovePacking?id=" + dtUsers.Rows[i]["PACKNOTEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                Edit = "<a href=PackingNote?id=" + dtUsers.Rows[i]["PACKNOTEBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PACKNOTEBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new PackingListItem
                {
                    id = dtUsers.Rows[i]["PACKNOTEBASICID"].ToString(),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
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
        public IActionResult ApprovePacking(string NOTE)
        {
            PackingNote ca = new PackingNote();
            DataTable dt = Packing.EditNote(NOTE);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.WorkId = dt.Rows[0]["loc"].ToString();


                ca.DrumLoc = dt.Rows[0]["LOCID"].ToString();
                ca.startdate = dt.Rows[0]["STARTDATE"].ToString() + " - " + dt.Rows[0]["STARTTIME"].ToString();
                ca.enddate = dt.Rows[0]["ENDDATE"].ToString() + " - " + dt.Rows[0]["ENDTIME"].ToString();

                ca.ItemId = dt.Rows[0]["ITEMID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.LotNo = dt.Rows[0]["PACLOTNO"].ToString();
                ca.PackYN = dt.Rows[0]["PACKCONSYN"].ToString();
                ca.ProdSchNo = dt.Rows[0]["PS"].ToString();
                ca.Shift = dt.Rows[0]["SHIFTNO"].ToString();
                ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();

                ca.Remark = dt.Rows[0]["REMARKS"].ToString();

                //ViewBag.entrytype = ca.EntryType;
                List<DrumDetail> TData = new List<DrumDetail>();
                DrumDetail tda = new DrumDetail();
                DataTable dtDrum = Packing.EditDrumDetail(NOTE);
                for (int i = 0; i < dtDrum.Rows.Count; i++)
                {
                    tda = new DrumDetail();
                    //tda.DrumNolst = BindDrumNo(ca.DrumLoc);
                    tda.drum = dtDrum.Rows[i]["DRUMNO"].ToString();
                    //tda.Batchlst = BindBatch(tda.DrumNo);



                    tda.batch = dtDrum.Rows[i]["IBATCHNO"].ToString();
                    tda.qty = dtDrum.Rows[i]["IBATCHQTY"].ToString();
                    tda.comp = dtDrum.Rows[i]["COMBNO"].ToString();



                    TData.Add(tda);
                }


                ca.DrumDetlst = TData;
            }
            return View(ca);
        }
        public ActionResult GetshiftDetail(string Shiftid)
        {
            try
            {
                DataTable dt = new DataTable();
                string fromtime = "";
                string totime = "";
                string tothrs = "";
                dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTMASTID='" + Shiftid + "'");
                if (dt.Rows.Count > 0)
                {

                    fromtime = dt.Rows[0]["FROMTIME"].ToString();
                    totime = dt.Rows[0]["TOTIME"].ToString();
                    tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
                }

                var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = Packing.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPackingNote");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPackingNote");
            }
        }
        public ActionResult GetDrumStockDetails(string id, string item)
        {
            PackingNote model = new PackingNote();
            DataTable dtt = new DataTable();
            List<DrumDetail> Data = new List<DrumDetail>();
            DrumDetail tda = new DrumDetail();
            if (item == "10044000011739")
            {
                dtt = Packing.GetDrumDetails(id, item);
            }
            else
            {
                dtt = Packing.GetDrumDetailsdd(id, item);
            }
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new DrumDetail();

                    tda.drum = dtt.Rows[i]["DRUM_NO"].ToString();
                    tda.drumid = dtt.Rows[i]["DRUM_ID"].ToString();


                    tda.qty = dtt.Rows[i]["QTY"].ToString();
                    tda.batch = dtt.Rows[i]["LOTNO"].ToString();
                    tda.lotid = dtt.Rows[i]["LOTMASTID"].ToString();
                    tda.rate = dtt.Rows[i]["RATE"].ToString();

                    double bqty = Convert.ToDouble(tda.qty);
                    double brate = Convert.ToDouble(tda.rate);
                    double totamt = bqty * brate;
                    tda.amount = totamt.ToString();


                    tda.qty = dtt.Rows[i]["BALANCE_QTY"].ToString();

                    DataTable dtt1 = new DataTable();
                    dtt1 = Packing.GetDrumLot(id, item, tda.drumid);
                    for (int j = 0; j < dtt1.Rows.Count; j++)
                    {
                        tda.batch = dtt1.Rows[j]["LOTNO"].ToString();
                    }
                    tda.Isvalid = "Y";
                    Data.Add(tda);
                }
            }
            model.DrumDetlst = Data;
            return Json(model.DrumDetlst);

        }

        public ActionResult Getsch(string id, string item)
        {
            try
            {
                DataTable dt = new DataTable();
                

                dt = datatrans.GetData("select  LT.PSCHNO,P.PSBASICID,W.WCBASICID from PSBASIC P, LSTOCKVALUE L,LOTMAST LT  ,WCBASIC W   where LT.WCID=W.WCID AND LT.PSCHNO=P.DOCID AND L.ITEMID=LT.ITEMID AND LT.LOTNO=L.LOTNO AND  LT.INSFLAG='1' AND  L.ITEMID ='" + id + "' AND  L.LOCID ='" + item + "'  HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0 GROUP BY LT.PSCHNO,P.PSBASICID,W.WCBASICID");
                //dt1 = datatrans.GetData("select SUM(RQTY) as qty from PSINPDETAIL WHERE PSBASICID='" + schid + "'");

 
                string proc = "";
                string wcid = "";
                if (dt.Rows.Count > 0)
                {
                     
                    proc = dt.Rows[0]["PSBASICID"].ToString();
                    wcid = dt.Rows[0]["WCBASICID"].ToString();
                }

                var result = new {  proc = proc, wcid= wcid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
