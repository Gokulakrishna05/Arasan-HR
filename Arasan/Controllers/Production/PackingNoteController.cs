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
            ca.Shiftlst = BindShift();
            ca.RecList = BindEmp();
            ca.DrumLoclst = BindDrumLoc();
            ca.Schlst = BindSche( );
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DrumDetail> TData = new List<DrumDetail>();
            DrumDetail tda = new DrumDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DrumDetail();
                    tda.DrumNolst = Binddrum("","");
                    
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
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
                        tda.DrumNo = dt2.Rows[i]["IDRUMNO"].ToString();
                        //tda.Batchlst = BindBatch(tda.DrumNo);


                        tda.Isvalid = "Y";
                        tda.BatchNo = dt2.Rows[i]["IBATCHNO"].ToString();
                        tda.BatchQty = dt2.Rows[i]["IBATCHQTY"].ToString();
                        tda.Comp = dt2.Rows[i]["COMBNO"].ToString();
                        

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
        public JsonResult GetDrumJSON(string id, string item)
        {
            string DrumID = datatrans.GetDataString("Select ITEMMASTERID from ITEMMASTER where ITEMID='" + id + "' ");
            DrumIssueEntryItem model = new DrumIssueEntryItem();
            model.drumlst = Binddrum(DrumID, item);
            return Json(Binddrum(DrumID, item));
        }
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
            model.Itemlst = BindItemlst(supid );
            return Json(BindItemlst(supid));

        }
        //public JsonResult GetShedJSON(string supid)
        //{
        //    PackingNote model = new PackingNote();
        //    model.Itemlst = BindSche(supid);
        //    return Json(BindSche(supid));

        //}
        public List<SelectListItem> BindSche( )
        {
            try
            {
                DataTable dtDesg = Packing.GetSchedule( );
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
      
        public List<SelectListItem> BindItemlst(string value )
        {
            try
            {
                DataTable dtDesg = Packing.GetItembyId(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });

                }


                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDrumDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string batch = "";
                string qty = "";

                dt = Packing.GetDrumDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    batch = dt.Rows[0]["BATCHNO"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();



                }

                var result = new { batch = batch, qty = qty };
                return Json(result);
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
            IEnumerable<PackingNote> cmp = Packing.GetAllPackingNote();
            return View(cmp);
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
                    tda.DrumNo = dtDrum.Rows[i]["DRUMNO"].ToString();
                    //tda.Batchlst = BindBatch(tda.DrumNo);



                    tda.BatchNo = dtDrum.Rows[i]["IBATCHNO"].ToString();
                    tda.BatchQty = dtDrum.Rows[i]["IBATCHQTY"].ToString();
                    tda.Comp = dtDrum.Rows[i]["COMBNO"].ToString();


                   
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
    }
}
