using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Services.Production;
using System.Collections.Specialized;
using Arasan.Interface;
using System.Xml.Linq;

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
            ca.worklst = BindWork(ca.super);
            ca.Shiftlst = BindShift();

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

            if (string.IsNullOrEmpty(id))
            {
                DataTable dtv = datatrans.GetSequence("PYRO");
                if (dtv.Rows.Count > 0)
                {
                    ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
                }
             
                ca.Branch = Request.Cookies["BranchId"];


                ca.Shift = "A";
                ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");


              

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
                    tda5.Isvalid = "Y";
                    TTData5.Add(tda5);


                }

               
            }
            else
            {
                /////////////////Edit/////////////
                string apID = id;

                     DataTable adt = new DataTable();
                    DataTable dt6 = new DataTable();
                    adt = Pyro.GetAPProd(apID);
                    if (adt.Rows.Count > 0)
                    {
                        ca.Location = adt.Rows[0]["LOCID"].ToString();
                        ca.Docdate = adt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = adt.Rows[0]["DOCID"].ToString();
                        //ca.Eng = adt.Rows[0]["EMPNAME"].ToString();
                        ca.Shift = adt.Rows[0]["SHIFT"].ToString();
                        ViewBag.shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.APID = apID;
                    }

                    DataTable adt2 = new DataTable();

                    adt2 = Pyro.GetInput(apID);
                    if (adt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt2.Rows.Count; i++)
                        {
                            tda = new PProInput();
                            tda.Itemlst = BindItemlst();
                            tda.ItemId = adt2.Rows[i]["ITEMID"].ToString();
                            tda.BinId = adt2.Rows[i]["BINID"].ToString();
                            tda.Time = adt2.Rows[i]["TIME"].ToString();
                            tda.batchno = adt2.Rows[i]["BATCHNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(adt2.Rows[i]["QTY"].ToString() == "" ? "0" : adt2.Rows[i]["QTY"].ToString());
                            tda.StockAvailable = Convert.ToDouble(adt2.Rows[i]["STOCK"].ToString() == "" ? "0" : adt2.Rows[i]["STOCK"].ToString());
                            tda.APID = apID;
                            TData.Add(tda);
                            tda.Isvalid = "Y";
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new PProInput();
                            tda.APID = apID;
                            tda.Itemlst = BindItemlst();

                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
                    }
                    DataTable adt3 = new DataTable();
                    adt3 = Pyro.GetCons(apID);
                    if (adt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt3.Rows.Count; i++)
                        {
                            tda1 = new PAPProInCons();
                            tda1.Itemlst = BindItemlstCon();
                            tda1.ItemId = adt3.Rows[i]["ITEMID"].ToString();
                            tda1.consunit = adt3.Rows[i]["UNITID"].ToString();
                            tda1.BinId = adt3.Rows[i]["BINID"].ToString();
                            tda1.Qty = Convert.ToDouble(adt3.Rows[i]["QTY"].ToString() == "" ? "0" : adt3.Rows[i]["QTY"].ToString());
                            tda1.consQty = Convert.ToDouble(adt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : adt3.Rows[i]["CONSQTY"].ToString());
                            tda1.ConsStock = Convert.ToDouble(adt3.Rows[i]["STOCK"].ToString() == "" ? "0" : adt3.Rows[i]["STOCK"].ToString());
                            tda1.APID = apID;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new PAPProInCons();
                            tda1.Itemlst = BindItemlstCon();
                            tda1.Isvalid = "Y";
                            tda1.APID = apID;
                            TData1.Add(tda1);
                        }
                    }

                    DataTable adt4 = new DataTable();
                    adt4 = Pyro.GetEmpdet(apID);
                    if (adt4.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt4.Rows.Count; i++)
                        {
                            tda2 = new PEmpDetails();
                            tda2.Employeelst = BindEmp();
                            tda2.Employee = adt4.Rows[i]["EMPID"].ToString();

                            tda2.EmpCode = adt4.Rows[i]["EMPCODE"].ToString();
                            tda2.Depart = adt4.Rows[i]["DEPARTMENT"].ToString();
                            tda2.StartDate = adt4.Rows[i]["STARTDATE"].ToString();
                            tda2.StartTime = adt4.Rows[i]["STARTTIME"].ToString();
                            tda2.EndDate = adt4.Rows[i]["ENDDATE"].ToString();
                            tda2.EndTime = adt4.Rows[i]["ENDTIME"].ToString();
                            tda2.OTHrs = adt4.Rows[i]["OTHOUR"].ToString();

                            tda2.ETOther = adt4.Rows[i]["ETOTHER"].ToString();
                            tda2.Normal = adt4.Rows[i]["NHOUR"].ToString();
                            tda2.NOW = adt4.Rows[i]["NATUREOFWORK"].ToString();
                            tda2.APID = apID;
                            TTData2.Add(tda2);
                            tda2.Isvalid = "Y";
                        }

                    }
                    else
                    {

                        for (int i = 0; i < 1; i++)
                        {
                            tda2 = new PEmpDetails();
                            tda2.APID = apID;
                            tda2.Employeelst = BindEmp();
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);
                        }
                    }
                    DataTable adt5 = new DataTable();
                    adt5 = Pyro.GetBreak(apID);
                    if (adt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt5.Rows.Count; i++)
                        {
                            tda3 = new PBreakDet();
                            tda3.Machinelst = BindMachineID();
                            tda3.MachineId = adt5.Rows[i]["MACHCODE"].ToString();
                            tda3.Emplst = BindEmp();
                            tda3.MachineDes = adt5.Rows[i]["MEACHDES"].ToString();
                            tda3.StartTime = adt5.Rows[i]["FROMTIME"].ToString();
                            tda3.EndTime = adt5.Rows[i]["TOTIME"].ToString();
                            tda3.PB = adt5.Rows[i]["PB"].ToString();
                            tda3.Isvalid = "Y";
                            tda3.Alloted = adt5.Rows[i]["ALLOTTEDTO"].ToString();
                            tda3.DType = adt5.Rows[i]["DTYPE"].ToString();
                            tda3.MType = adt5.Rows[i]["MTYPE"].ToString();
                            tda3.Reason = adt5.Rows[i]["REASON"].ToString();

                            tda3.APID = apID;
                            TData3.Add(tda3);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda3 = new PBreakDet();

                            tda3.Machinelst = BindMachineID();
                            tda3.Emplst = BindEmp();
                            tda3.Isvalid = "Y";
                            tda3.APID = apID;
                            TData3.Add(tda3);

                        }
                    }
                    DataTable adt6 = new DataTable();

                    adt6 = Pyro.GetOutput(apID);
                    if (adt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt6.Rows.Count; i++)
                        {
                            tda4 = new PProOutput();
                            tda4.Itemlst = BindOutItemlst();
                            tda4.ItemId = adt6.Rows[i]["ITEMID"].ToString();
                            tda4.saveitemId = adt6.Rows[i]["ITEMNAME"].ToString();
                            tda4.BinId = adt6.Rows[i]["BINID"].ToString();
                            tda4.drumlst = BindDrum();
                            tda4.drumno = adt6.Rows[i]["DRUMNO"].ToString();
                            tda4.FromTime = adt6.Rows[i]["FROMTIME"].ToString();
                            tda4.ToTime = adt6.Rows[i]["TOTIME"].ToString();
                            tda4.OutputQty = Convert.ToDouble(adt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : adt6.Rows[i]["OUTQTY"].ToString());
                            //tda4.Result = adt6.Rows[i]["TESTRESULT"].ToString();
                            //tda4.Status = adt6.Rows[i]["MOVETOQC"].ToString();
                            //DataTable dt7 = new DataTable();
                            //dt7 = IProductionEntry.GetResult(id);
                            //if (dt7.Rows.Count > 0)
                            //{
                            //    tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                            //    tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            //}
                            tda4.APID = adt6.Rows[i]["PYROPRODOUTDETID"].ToString();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new PProOutput();
                            tda4.APID = apID;
                            tda4.Itemlst = BindOutItemlst();
                            tda4.drumlst = BindDrum();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                    DataTable adt7 = new DataTable();

                    adt7 = Pyro.GetLogdetail(apID);
                    if (adt7.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt7.Rows.Count; i++)
                        {
                            tda5 = new PLogDetails();

                            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                            tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                            tda5.APID = apID;
                            TTData5.Add(tda5);
                            tda5.Isvalid = "Y";
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda5 = new PLogDetails();
                            tda5.APID = apID;
                            string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                            tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                            tda5.StartTime = DateTime.Now.ToString("HH:mm");
                            DateTime dateTime = DateTime.Parse(tda5.StartDate);
                            //TimeSpan t1 = new TimeSpan(24,0,0);


                            int hours = int.Parse(ShiftTime);
                            TimeSpan t2 = new TimeSpan(hours, 0, 0);
                            DateTime resultDateTime = dateTime + t2;
                            tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                            string[] sdateList = tda5.StartDate.Split(" ");
                            string sdate = "";
                            string stime = "";
                            if (sdateList.Length > 0)
                            {
                                sdate = sdateList[0];
                                stime = sdateList[1];
                            }
                            string[] edateList = tda5.EndDate.Split(" - ");
                            string endate = "";
                            string endtime = "";
                            if (sdateList.Length > 0)
                            {
                                endate = edateList[0];
                                endtime = edateList[1];
                            }
                            tda5.StartDate = sdate;
                            tda5.EndDate = endate;

                            tda5.EndTime = endtime;

                            tda5.Isvalid = "Y";
                            TTData5.Add(tda5);

                        }
                    }
          

                //////////////////Edit////////////////
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
        public JsonResult GetItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst());
        }
        public JsonResult GetconsItemJSON()
        {
            return Json(BindItemlstCon());
        }
        public JsonResult GetOutItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindOutItemlst());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(BindDrum());
        }
        public JsonResult GetWcRec(string wcid,string shift)
        {
            string res = "Yes";
            string id=datatrans.GetDataString("select PYROPRODBASICID from PYROPRODBASIC WHERE LOCID='" + wcid + "' AND IS_COMPLETE='No' AND SHIFT='"+ shift + "'");
            if(id==null || id == "0" || id=="")
            {
                res = "No";
            }
            var result = new { url = id , res = res };
            return Json(result);
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
