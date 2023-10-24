using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Arasan.Services.Qualitycontrol;
using Arasan.Services;
using Arasan.Interface;
using Arasan.Services.Sales;

namespace Arasan.Controllers
{
    public class PackingQCFinalValueEntryController : Controller
    {
        IPackingQCFinalValueEntry PackingQCFinalValueEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PackingQCFinalValueEntryController(IPackingQCFinalValueEntry _PackingQCFinalValueEntryService, IConfiguration _configuratio)
        {
            PackingQCFinalValueEntryService = _PackingQCFinalValueEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PackingQCFinalValueEntry (string id, string tag)
        {
            PackingQCFinalValueEntry ca = new PackingQCFinalValueEntry();
            ca.Doclst = BindDoc();
            ca.Entrylst = BindEntry();

            List<Packingitem> TData = new List<Packingitem>();
            Packingitem tda = new Packingitem();
            List<PackingGasitem> TData1 = new List<PackingGasitem>();
            PackingGasitem tda1 = new PackingGasitem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++) 
                {
                    tda = new Packingitem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new PackingGasitem();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }
            else
            {

            }
            ca.DrumLst = TData;
            ca.TimeLst = TData1;

            return View(ca);
        }

        [HttpPost]
        public ActionResult PackingQCFinalValueEntry(PackingQCFinalValueEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PackingQCFinalValueEntryService.PackingQCFinalValueEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = " PackingQCFinalValueEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " PackingQCFinalValueEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListPackingQCFinalValueEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PackingQCFinalValueEntry";
                    TempData["notice"] = Strout;
                    //return View();
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public IActionResult ListPackingQCFinalValueEntry (string st, string ed)
        {
            IEnumerable<PackingQCFinalValueEntry> cmp = PackingQCFinalValueEntryService.GetAllPackingQCFinalValueEntry(st, ed);
            return View(cmp);
        }
        //public IActionResult ListQCFinalValueEntry(string st, string ed)
        //{
        //    IEnumerable<QCFinalValueEntry> cmp = QCFinalValueEntryService.GetAllQCFinalValueEntry(st, ed);
        //    return View(cmp);
        //}

        public List<SelectListItem> BindDoc()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PQC - 301689", Value = "PQC - 301689" });
                

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEntry()
        {
            try
            {
                DataTable dtDesg = PackingQCFinalValueEntryService.GetPackingQCFinal();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PACKNOTEBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetDocJSON()
        {
            Packingitem model = new Packingitem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = PackingQCFinalValueEntryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPackingQCFinalValueEntry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPackingQCFinalValueEntry");
            }
        }

        public IActionResult ViewPackingQCFinalValueEntry(string id)
        {
            PackingQCFinalValueEntry ca = new PackingQCFinalValueEntry();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dtt1 = new DataTable();

            dt = PackingQCFinalValueEntryService.GetViewPacking(id);
            if (dt.Rows.Count > 0)
            {

                ca.Docid = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDT"].ToString();
                ca.PEntryid = dt.Rows[0]["PENTRYID"].ToString();
                ca.PEntrydt = dt.Rows[0]["PENTRYDT"].ToString();
                //ca.PNoteid = dt.Rows[0]["PACKNOTEID"].ToString();

                ca.Schedule = dt.Rows[0]["PSCHNO"].ToString();
                ca.PacNo = dt.Rows[0]["PACLOTNO"].ToString();
                ca.Item = dt.Rows[0]["ITEMID"].ToString();
                ca.TestReq = dt.Rows[0]["TESTREQ"].ToString();
                ca.drumnos = dt.Rows[0]["PKDRUMNOS"].ToString();
                ca.Same = dt.Rows[0]["SAMPLETAKENBY"].ToString();
                ca.Checked = dt.Rows[0]["CHECKEDBY"].ToString();
               
                ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                
                ca.ID = id;

                List<Packingitem> Data = new List<Packingitem>();
                Packingitem tda = new Packingitem();
                //double tot = 0;

                dtt = PackingQCFinalValueEntryService.GetViewPackingItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda.drum = dtt.Rows[0]["DRUMNO"].ToString();
                        tda.com = dtt.Rows[0]["COMBNO"].ToString();
                        tda.batch = dtt.Rows[0]["BATCHNO"].ToString();
                        tda.result = dtt.Rows[0]["FINALRESULT"].ToString();
                        tda.Isvalid = "Y";

                        Data.Add(tda);
                    }
                }

                List<PackingGasitem> DData = new List<PackingGasitem>();
                PackingGasitem tda1 = new PackingGasitem();
                //double tot = 0;

                dtt1 = PackingQCFinalValueEntryService.GetViewPackingGas(id);
                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tda1.Time = dtt1.Rows[0]["MINS"].ToString();
                        tda1.vol25 = dtt1.Rows[0]["VOL25C"].ToString();
                        tda1.vol35 = dtt1.Rows[0]["VOL35C"].ToString();
                        tda1.vol45 = dtt1.Rows[0]["VOL45C"].ToString();
                        tda1.vol = dtt1.Rows[0]["VOLSTP"].ToString();
                        tda1.Isvalid = "Y";

                        DData.Add(tda1);
                    }
                }
                ca.DrumLst = Data;
                ca.TimeLst = DData;

            }
            return View(ca);
        }
        public ActionResult GetPackingEntryDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string docdate = "";
                string entrydate = "";
                string schedule = "";
                string lot = "";
                string item = "";
                //string drumnos = "";


                dt = PackingQCFinalValueEntryService.GetPackingEntryDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    docdate = dt.Rows[0]["DOCDATE"].ToString(); 
                    entrydate = dt.Rows[0]["ENDDATE"].ToString(); 

                    schedule = dt.Rows[0]["SCHPLANTYPE"].ToString();
                    lot = dt.Rows[0]["PACLOTNO"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    //drumnos = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();

                   
                }

                var result = new { docdate = docdate,entrydate = entrydate, schedule = schedule, lot = lot, item = item/*, drumnos = drumnos*/ };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetPackingitemDetails(string id)   //10001000001122
        {
            PackingQCFinalValueEntry model = new PackingQCFinalValueEntry();
            DataTable dtt = new DataTable();
            List<Packingitem> Data = new List<Packingitem>();
            Packingitem tda = new Packingitem();
            dtt = PackingQCFinalValueEntryService.GetPackingitemDetail(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new Packingitem();

                    tda.drum = dtt.Rows[i]["DRUMNO"].ToString();
                    tda.batch = dtt.Rows[i]["IBATCHNO"].ToString();
                    
                    tda.com = dtt.Rows[i]["COMBNO"].ToString();
                    model.drumnos = tda.drum;
                    Data.Add(tda);
                }
            }
            model.DrumLst = Data;
            return Json(model.DrumLst);

        }


    }
}
