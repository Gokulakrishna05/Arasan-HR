using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services.Production;
using System.Collections.Specialized;
using Arasan.Interface;

namespace Arasan.Controllers 
{
    public class PyroProductionController : Controller
    {
        IPyroProduction Pyro;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PyroProductionController(IPyroProduction _Pyro, IConfiguration _configuratio)
        {
            Pyro = _Pyro;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PyroProduction(string id)
        {
            PyroProduction ca = new PyroProduction();
            ca.Eng = Request.Cookies["UserName"];
            ca.super = Request.Cookies["UserId"];
            DataTable dtv = datatrans.GetSequence("PYRO");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            ca.worklst = BindWork(ca.super);
            ca.Shiftlst = BindShift();
            ca.Branch = Request.Cookies["BranchId"];

            
            ca.Shift = "A";
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");


            List<PBreakDet> TData3 = new List<PBreakDet>();
            PBreakDet tda3 = new PBreakDet();
            List<PProInput> TData = new List<PProInput>();
            PProInput tda = new PProInput();
            List<PProOutput> TData4 = new List<PProOutput>();
            PProOutput tda4 = new PProOutput();
            List<PAPProInCons> TData1 = new List<PAPProInCons>();
            PAPProInCons tda1 = new PAPProInCons();
            List<PEmpDetails> TTData2 = new List<PEmpDetails>();
            PEmpDetails tda2 = new PEmpDetails();
            List<PLogDetails> TTData5 = new List<PLogDetails>();
            PLogDetails tda5 = new PLogDetails();
             
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new PBreakDet();

                    tda3.Machinelst = BindMachineID();
                    tda3.Emplst = BindEmp();
                    tda3.Isvalid = "Y";
                    tda3.APID = id;
                    TData3.Add(tda3);

                }
                for (int i = 0; i < 3; i++)
                {
                    tda = new PProInput();
                    tda.APID = id;
                    tda.Itemlst = BindItemlst();

                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda4 = new PProOutput();
                    tda4.APID = id;
                    tda4.Itemlst = BindOutItemlst();
                    tda4.drumlst = BindDrum();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new PAPProInCons();
                    tda1.Itemlst = BindItemlstCon();
                    tda1.Isvalid = "Y";
                    tda1.APID = id;
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new PEmpDetails();
                    tda2.APID = id;
                    tda2.Employeelst = BindEmp();
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda5 = new PLogDetails();
                    tda5.APID = id;
                    //string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                    //tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                    //tda5.StartTime = DateTime.Now.ToString("HH:mm");
                    //DateTime dateTime = DateTime.Parse(tda5.StartDate);
                    ////TimeSpan t1 = new TimeSpan(24,0,0);


                    //int hours = int.Parse(ShiftTime);
                    //TimeSpan t2 = new TimeSpan(hours, 0, 0);
                    //DateTime resultDateTime = dateTime + t2;
                    //tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                    //string[] sdateList = tda5.StartDate.Split(" ");
                    //string sdate = "";
                    //string stime = "";
                    //if (sdateList.Length > 0)
                    //{
                    //    sdate = sdateList[0];
                    //    stime = sdateList[1];
                    //}
                    //string[] edateList = tda5.EndDate.Split(" - ");
                    //string endate = "";
                    //string endtime = "";
                    //if (sdateList.Length > 0)
                    //{
                    //    endate = edateList[0];
                    //    endtime = edateList[1];
                    //}
                    //tda5.StartDate = sdate;
                    //tda5.EndDate = endate;

                    //tda5.EndTime = endtime;

                    tda5.Isvalid = "Y";
                    TTData5.Add(tda5);

                 
            }
   
            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;

            return View(ca);
        }

        [HttpPost]
        public ActionResult PyroProduction(PyroProduction Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Pyro.PyroProductionEntry(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PyroProduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PyroProduction Updated Successfully...!";
                    }
                    //return RedirectToAction("APProductionentryDetail", new { id = id });
                }

                else
                {
                    ViewBag.PageTitle = "Edit PyroProduction";
                    TempData["notice"] = Strout;
                    //return View();
                }
                return RedirectToAction("PyroProduction");
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public ActionResult GetMachineDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string name = "";
                string type = "";

                dt = Pyro.GetMachineDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    name = dt.Rows[0]["MNAME"].ToString();
                    type = dt.Rows[0]["MTYPE"].ToString();
                }
                var result = new { name = name, type = type };
                return Json(result);
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
                DataTable dtDesg = Pyro.GetItem();
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

        public List<SelectListItem> BindOutItemlst()
        {
            try
            {
                DataTable dtDesg = Pyro.GetOutItem();
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
        public List<SelectListItem> BindItemlstCon()
        {
            try
            {
                DataTable dtDesg = Pyro.GetItemCon();
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
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = Pyro.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
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
                DataTable dtDesg = Pyro.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetEmployeeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string code = "";


                dt = Pyro.GetEmployeeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    code = dt.Rows[0]["EMPID"].ToString();



                }

                var result = new { code = code };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetshiftDetail(string Shiftid)
        {
            try
            {
                DataTable dt = new DataTable();
                string fromtime = "";
                string totime = "";
                string tothrs = "";
                dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTNO='" + Shiftid + "'");
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
        public List<SelectListItem> BindMachineID()
        {
            try
            {
                DataTable dtDesg = datatrans.GetMachine();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MCODE"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString() });
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
                DataTable dtDesg = Pyro.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWork(string id)
        {
            try
            {
                DataTable dtDesg = Pyro.GetWork(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCATIONNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = Pyro.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();

                }

                var result = new { bin = bin, binid = binid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetOutItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = Pyro.GetOutItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();

                }

                var result = new { bin = bin, binid = binid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetConItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                string unit = "";
                string unitid = "";

                dt = Pyro.GetConItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    unitid = dt.Rows[0]["unit"].ToString();

                }

                var result = new { bin = bin, binid = binid, unit = unit, unitid = unitid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
