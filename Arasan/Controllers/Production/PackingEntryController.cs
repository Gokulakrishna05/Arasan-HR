﻿using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;

using Arasan.Models;
using Arasan.Services ;
 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Arasan.Controllers 
{
    public class PackingEntryController : Controller
    {
        IPackingEntry Pack;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public PackingEntryController(IPackingEntry _Pack, IConfiguration _configuratio)
        {
            Pack = _Pack;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PackingEntry()
        {
            PackingEntry ca = new PackingEntry();
            ca.Brlst = BindBranch();
            ca.Worklst = BindWorkCenter();
            ca.Packlst = BindPackNote();
            ca.user = Request.Cookies["UserId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Pack");
            if (dtv.Rows.Count > 0)
            {
                ca.Docid = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<PackInp> Data = new List<PackInp>();
            PackInp tda = new PackInp();
            List<PackMat> Data1 = new List<PackMat>();
            PackMat tda1 = new PackMat();
            List<PackEmp> Data2 = new List<PackEmp>();
            PackEmp tda2 = new PackEmp();
            List<Packothcon> Data3 = new List<Packothcon>();
            Packothcon tda3 = new Packothcon();
            List<PackMach> Data4 = new List<PackMach>();
            PackMach tda4 = new PackMach();
           
            for (int i = 0; i < 1; i++)
            {
                tda1 = new PackMat();
               
                tda1.Itemlst = BindItemlst();
                tda1.Isvalid = "Y";
                Data1.Add(tda1);
            }
            for (int i = 0; i < 1; i++)
            {
                tda2 = new PackEmp();
              
                tda2.Emplst = BindEmplst();
                tda2.Isvalid = "Y";
                Data2.Add(tda2);
            }
            for (int i = 0; i < 1; i++)
            {
                tda3 = new Packothcon();
                tda3.Itemlst = BindItemlst();
                tda3.Isvalid = "Y";
                Data3.Add(tda3);
            }
            for (int i = 0; i < 1; i++)
            {
                tda4 = new PackMach();
                tda4.Machlst = BindMachine();
                tda4.Isvalid = "Y";
                Data4.Add(tda4);
            }
           
            ca.machlst = Data4;
            ca.oconlst = Data3;
            ca.Emplst = Data2;
            ca.Matlst = Data1;
            
            return View(ca);
        }

        [HttpPost]
        public ActionResult PackingEntry(PackingEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Pack.PackingEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PackingEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PackingEntry Updated Successfully...!";
                    }
                    return RedirectToAction("PackingEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PackingEntry";
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = datatrans.GetWorkCenter();
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
        public List<SelectListItem> BindPackNote()
        {
            try
            {
                DataTable dtDesg = Pack.GetPackNote();
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = Pack.GetItem();
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
        public List<SelectListItem> BindEmplst()
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
        public List<SelectListItem> BindMachine()
        {
            try
            {
                DataTable dtDesg = Pack.GetMachine();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDetail(string Note)
        {
            try
            {
                DataTable dt = new DataTable();
                string work = "";
                string workid = "";
                string shedule = "";
                string sheduleid = "";
                string item = "";
                string itemid = "";
                string padate = "";
                string packyn = "";
                string shift = "";
                string shiftid = "";
                string basic = "";
                string qty = "";
                string startda = "";
                string endda = "";
                string starttime = "";
                string endtime = "";
                string stdatime = "";
                string enddatime = "";
                string toloc = "";
                string tothrs = "";
                double totamt = '0';
                double totrate = '0';
                dt = Pack.GetNoteDetail(Note);
                if (dt.Rows.Count > 0)
                {

                    work = dt.Rows[0]["WCID"].ToString();
                    workid = dt.Rows[0]["work"].ToString();
                    shedule = dt.Rows[0]["DOCID"].ToString();
                    sheduleid = dt.Rows[0]["PSCHNO"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    itemid = dt.Rows[0]["OITEMID"].ToString();
                    padate = dt.Rows[0]["DOCDATE"].ToString();
                    packyn = dt.Rows[0]["PACKCONSYN"].ToString();
                    shift = dt.Rows[0]["SHIFTNO"].ToString();
                    shiftid = dt.Rows[0]["SHIFT"].ToString();
                    basic = dt.Rows[0]["PACKNOTEBASICID"].ToString();
                    startda = dt.Rows[0]["STARTDATE"].ToString();
                    endda = dt.Rows[0]["ENDDATE"].ToString();
                    starttime = dt.Rows[0]["STARTTIME"].ToString();
                    endtime = dt.Rows[0]["ENDTIME"].ToString();
                    toloc = dt.Rows[0]["TOLOCDETAILSID"].ToString();
                    tothrs = dt.Rows[0]["TOTHRS"].ToString();
                  
                    DataTable ap = datatrans.GetData("Select SUM(IQTY) as qty,SUM(IRATE) as rate from PACKNOTEINPDETAIL where PACKNOTEBASICID='" + basic + "'  ");
                    qty = ap.Rows[0]["qty"].ToString();
                    totrate = Convert.ToDouble(ap.Rows[0]["rate"].ToString());
                    int totqty = Convert.ToInt32(qty);
                     
                    totamt = totqty * totrate;
                    stdatime = startda + "-" + starttime;
                    enddatime = endda + "-" + endtime;
                }

                var result = new { work = work, workid = workid, shedule = shedule, sheduleid = sheduleid, item = item, itemid = itemid, padate = padate, packyn = packyn, shift = shift, shiftid = shiftid , qty = qty, basic= basic , stdatime = stdatime , enddatime = enddatime , toloc = toloc, tothrs= tothrs, totamt= totamt };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDrumStockDetails(string Note)
        {
            PackingEntry model = new PackingEntry();
            DataTable dtt = new DataTable();
            List<PackInp> Data = new List<PackInp>();
            PackInp tda = new PackInp();
            dtt = Pack.GetPackDetails(Note);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PackInp();

                    tda.drum = dtt.Rows[i]["DRUMNO"].ToString();
                    tda.drumid = dtt.Rows[i]["IDRUMNO"].ToString();


                    tda.bqty = dtt.Rows[i]["IBATCHQTY"].ToString();
                    tda.batch = dtt.Rows[i]["IBATCHNO"].ToString();
                    tda.batchno = dtt.Rows[i]["IBATCHNO"].ToString();
                    tda.iqty = Convert.ToDouble(dtt.Rows[i]["IQTY"].ToString());
                    tda.comp = dtt.Rows[i]["COMBNO"].ToString();
                    tda.packid = dtt.Rows[i]["PACKNOTEINPDETAILID"].ToString();
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["IRATE"].ToString());
                    tda.amount = tda.rate * tda.iqty;


                    tda.Isvalid = "Y";
                    Data.Add(tda);
                }
            }
            model.Inplst = Data;
            return Json(model.Inplst);

        }
        public ActionResult GetOutputDrumDetails(string Note)
        {
            PackingEntry model = new PackingEntry();
            DataTable dtt = new DataTable();
            List<PackDet> Data = new List<PackDet>();
            PackDet tda = new PackDet();
            string loc = datatrans.GetDataString("Select WCID from PACKNOTEBASIC where PACKNOTEBASICID='" + Note + "'  ");
            string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + loc + "'  ");
            string item = datatrans.GetDataString("Select WCID from WCBASIC where WCBASICID='" + loc + "'  ");
            string pdabasic = datatrans.GetDataString("Select PDABASICID from PDABASIC where PACKLOCID='" + locid + "' and IS_ACTIVE='Y' ORDER BY PDABASICID DESC fetch  first rows only ");
            string prefix = datatrans.GetDataString("Select SPREFIX from PDABASIC where PDABASICID='" + pdabasic + "' and IS_ACTIVE='Y' ");
            dtt = Pack.GetPackOutDetails(pdabasic);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PackDet();

                    tda.drum = dtt.Rows[i]["DRUMNO"].ToString();
                    tda.drumid = dtt.Rows[i]["TDRUMNO"].ToString();
                    


                   
                   
                    tda.loc = item;
                    tda.prefix = prefix;
                  
                    tda.pdaid = dtt.Rows[i]["PDADETAILID"].ToString();
                    //tda.rate = dtt.Rows[i]["IRATE"].ToString();
                    //tda.amount = dtt.Rows[i]["IAMOUNT"].ToString();


                    tda.Isvalid = "Y";
                    Data.Add(tda);
                }
            }
            model.Packdetlst = Data;
            return Json(model.Packdetlst);

        }
        public ActionResult GetPackItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    
                    
                }

                var result = new { unit = unit , price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetConsItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string val = "";
                string price = "";
                string stk = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    val = dt.Rows[0]["VALMETHDES"].ToString();
                    //dt1 = Pack.GetConstkqty(ItemId);
                    //if (dt1.Rows.Count > 0)
                    //{
                    //    stk = dt1.Rows[0]["QTY"].ToString();
                    //}
                    //if (stk == "")
                    //{
                    //    stk = "0";
                    //}
                }

                var result = new { unit = unit, val = val, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListPackingentry()
        {
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<PackingList> Reg = new List<PackingList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Pack.GetAllPackingentry(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;

                View = "<a href=ViewPackDrum?id=" + dtUsers.Rows[i]["PACKBASICID"].ToString() + "><img src='../Images/view_icon.png' alt='View' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PACKBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new PackingList
                {
                    id = dtUsers.Rows[i]["PACKBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),

                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    packing = dtUsers.Rows[i]["pack"].ToString(),
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                   
                    
                    shift = dtUsers.Rows[i]["SHIFTNO"].ToString(),

                    viewrow = View,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, string id)
        {
            string flag = Pack.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPackDrum");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPackDrum");
            }

        }
    }
}