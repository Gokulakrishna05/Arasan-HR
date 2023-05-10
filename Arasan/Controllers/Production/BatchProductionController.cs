using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class BatchProductionController : Controller
    {
        IBatchProduction IProductionEntry;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public BatchProductionController(IBatchProduction _IProductionEntry, IConfiguration _configuratio)
        {
            IProductionEntry = _IProductionEntry;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchProduction(string id)
        {
            BatchProduction ca = new BatchProduction();
            ca.Brlst = BindBranch();
            ca.Loclst = BindWorkCenter();
            ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            ca.Processlst = BindProcess();
            ca.ETypelst = BindEType();
            ca.RecList = BindEmp();
            ca.ProdLoglst = BindProdLog();
            ca.ProdSchlst = BindProdSch();
            ca.Batchlst = BindBatch();
            List<ProInputItem> TData = new List<ProInputItem>();
            ProInputItem tda = new ProInputItem();

            List<BProInCons> TData1 = new List<BProInCons>();
            BProInCons tda1 = new BProInCons();

            List<Boutput> TData2 = new List<Boutput>();
            Boutput tda2 = new Boutput();

            List<Bwastage> TData3 = new List<Bwastage>();
            Bwastage tda3 = new Bwastage();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProInputItem();
                    // tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.drumlst = Binddrum();
                    //tda.outputlst = Bindoutput();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


                for (int i = 0; i < 1; i++)
                {
                    tda1 = new BProInCons();
                    tda1.Itemlst = BindItemlst("");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }


                for (int i = 0; i < 1; i++)
                {
                    tda2 = new Boutput();
                    tda2.Itemlst = BindItemlst("");
                    tda2.drumlst = Binddrum();
                    tda2.statuslst = BindStatus();
                    tda2.loclst = BindLocation();
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }


                for (int i = 0; i < 1; i++)
                {
                    tda3 = new Bwastage();
                    tda3.Itemlst = BindItemlst("");
                    tda3.loclst = BindLocation();
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
            }
            else
            { 
                DataTable dt = new DataTable();
                double total = 0;
                dt = IProductionEntry.GetBatchProduction(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ca.Shiftdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Location = dt.Rows[0]["WCID"].ToString();
                    ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                    ca.ID = id;
                    ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                    ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.ProdLogId = dt.Rows[0]["PRODLOGID"].ToString();
                    ca.ProdSchNo = dt.Rows[0]["PSCHNO"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ca.EntryType = dt.Rows[0]["ETYPE"].ToString();
                    ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());
                    ca.SchQty = Convert.ToDouble(dt.Rows[0]["SCHQTY"].ToString() == "" ? "0" : dt.Rows[0]["SCHQTY"].ToString());
                    ca.totalinqty = Convert.ToDouble(dt.Rows[0]["TOTALINPUT"].ToString() == "" ? "0" : dt.Rows[0]["TOTALINPUT"].ToString());
                    ca.totaloutqty = Convert.ToDouble(dt.Rows[0]["TOTALOUTPUT"].ToString() == "" ? "0" : dt.Rows[0]["TOTALOUTPUT"].ToString());
                    ca.wastageqty = Convert.ToDouble(dt.Rows[0]["TOTALWASTAGE"].ToString() == "" ? "0" : dt.Rows[0]["TOTALWASTAGE"].ToString());
                    ca.totalconsqty = Convert.ToDouble(dt.Rows[0]["TOTCONSQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTCONSQTY"].ToString());
                    ca.totaRmqty = Convert.ToDouble(dt.Rows[0]["TOTRMQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTRMQTY"].ToString());
                    ca.totalRmValue = Convert.ToDouble(dt.Rows[0]["TOTRMVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTRMVALUE"].ToString());
                    ca.CosValue = Convert.ToDouble(dt.Rows[0]["TOTCONSVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTCONSVALUE"].ToString());
                    ViewBag.entrytype = ca.EntryType;
                    ca.Machine = Convert.ToDouble(dt.Rows[0]["TOTMACHINEVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTMACHINEVALUE"].ToString());
                }
                DataTable dt2 = new DataTable();

                dt2 = IProductionEntry.GetBatchProInpDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ProInputItem();
                        tda.Itemlst = BindItemlst("");
                        
                        tda.drumlst = Binddrum();
                        tda.ItemId = dt2.Rows[i]["IITEMID"].ToString();
                        tda.drumno = dt2.Rows[i]["IDRUMNO"].ToString();
                        tda.BinId = dt2.Rows[i]["IBINID"].ToString();
                        tda.batchno = dt2.Rows[i]["IBATCHNO"].ToString();
                       
                        tda.batchqty = Convert.ToDouble(dt2.Rows[i]["IBATCHQTY"].ToString() == "" ? "0" : dt2.Rows[i]["IBATCHQTY"].ToString());
                        tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["IQTY"].ToString() == "" ? "0" : dt2.Rows[i]["IQTY"].ToString());
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = IProductionEntry.GetBatchProConsDet(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new BProInCons();
                        tda1.Itemlst = BindItemlst("");

                       
                        tda1.ItemId = dt3.Rows[i]["CITEMID"].ToString();
                      
                        tda1.BinId = dt3.Rows[i]["CBINID"].ToString();
                        tda1.consunit = dt3.Rows[i]["CUNIT"].ToString();
                        tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                        tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["CVALUE"].ToString() == "" ? "0" : dt3.Rows[i]["CVALUE"].ToString());
                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();

                dt4 = IProductionEntry.GetBatchProOutDet(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new Boutput();
                        tda2.Itemlst = BindItemlst("");

                        tda2.drumlst = Binddrum();
                        tda2.loclst = BindLocation();
                        tda2.statuslst = BindStatus();
                        tda2.ItemId = dt4.Rows[i]["OITEMID"].ToString();
                        tda2.drumno = dt4.Rows[i]["ODRUMNO"].ToString();
                        tda2.startdate = dt4.Rows[i]["DSDT"].ToString();
                        tda2.batchno = dt4.Rows[i]["OBATCHNO"].ToString();
                        tda2.enddate = dt4.Rows[i]["DEDT"].ToString();
                        tda2.starttime = dt4.Rows[i]["STIME"].ToString();
                        tda2.endtime = dt4.Rows[i]["ETIME"].ToString();
                        tda2.toloc = dt4.Rows[i]["TOLOCATION"].ToString();
                        tda2.status = dt4.Rows[i]["STATUS"].ToString();
                        tda2.OutStock = Convert.ToDouble(dt4.Rows[i]["OSTOCK"].ToString() == "" ? "0" : dt4.Rows[i]["OSTOCK"].ToString());
                       
                        tda2.ExcessQty = Convert.ToDouble(dt4.Rows[i]["OXQTY"].ToString() == "" ? "0" : dt4.Rows[i]["OXQTY"].ToString());
                        tda2.OutQty = Convert.ToDouble(dt4.Rows[i]["OQTY"].ToString() == "" ? "0" : dt4.Rows[i]["OQTY"].ToString());
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();

                dt5 = IProductionEntry.GetBatchProWasteDet(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new Bwastage();
                        tda3.Itemlst = BindItemlst("");
                        tda3.loclst = BindLocation();

                        tda3.ItemId = dt5.Rows[i]["WITEMID"].ToString();

                        tda3.BinId = dt5.Rows[i]["WBINID"].ToString();
                        tda3.batchno = dt5.Rows[i]["WBATCHNO"].ToString();
                        tda3.wastageQty = Convert.ToDouble(dt5.Rows[i]["WQTY"].ToString() == "" ? "0" : dt3.Rows[i]["WQTY"].ToString());
                        tda3.toloc = dt5.Rows[i]["WLOCATION"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }
            }
            ca.inplst = TData;
            ca.Binconslst = TData1;
            ca.Boutlst = TData2;
            ca.Bwastelst = TData3;
            return View(ca);
        }
        public IActionResult BatchProductionOUT(string PROID)
        {
            ProductionEntry ca = new ProductionEntry();
            DataTable dt = IProductionEntry.EditProEntry(PROID);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Shiftdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EntryType = dt.Rows[0]["ETYPE"].ToString();
                ca.OutEntryType = "OUTPUT";
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ca.startdate = dt.Rows[0]["STARTDATE"].ToString() + " & " + dt.Rows[0]["STARTTIME"].ToString();
                ca.enddate = dt.Rows[0]["ENDDATE"].ToString() + " & " + dt.Rows[0]["ENDTIME"].ToString();
                ca.totalinqty = dt.Rows[0]["TOTALINPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALINPUT"].ToString()) : 0;
                ca.totaloutqty = dt.Rows[0]["TOTALOUTPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALOUTPUT"].ToString()) : 0;
                ca.totalconsqty = dt.Rows[0]["TOTCONSQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSQTY"].ToString()) : 0;
                ca.wastageqty = dt.Rows[0]["TOTALWASTAGE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALWASTAGE"].ToString()) : 0;
                ca.totalRmValue = dt.Rows[0]["TOTRMVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMVALUE"].ToString()) : 0;
                ca.Machine = dt.Rows[0]["TOTMACHINEVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTMACHINEVALUE"].ToString()) : 0;
                ca.CosValue = dt.Rows[0]["TOTCONSVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSVALUE"].ToString()) : 0;
                ca.totaRmqty = dt.Rows[0]["TOTRMQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMQTY"].ToString()) : 0;
                ViewBag.entrytype = ca.EntryType;
                ViewBag.shift = ca.Shift;
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());
                ca.SchQty = Convert.ToDouble(dt.Rows[0]["SCHQTY"].ToString() == "" ? "0" : dt.Rows[0]["SCHQTY"].ToString());
                ca.ProdLogId = dt.Rows[0]["prodlog"].ToString();
                ca.ProdSchNo = dt.Rows[0]["Prodsch"].ToString();
                ca.BranchId = dt.Rows[0]["BRANCHMASTID"].ToString();
                ca.LOCID = datatrans.GetDataString("select ILOCATION from WCBASIC where WCBASICID='" + dt.Rows[0]["WCBASICID"].ToString() + "'");
                List<ProIn> TData = new List<ProIn>();
                ProIn tda = new ProIn();
                DataTable dtproin = IProductionEntry.ProIndetail(PROID);
                for (int i = 0; i < dtproin.Rows.Count; i++)
                {
                    tda = new ProIn();
                    tda.ItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.BinId = dtproin.Rows[i]["IBINID"].ToString();
                    tda.drumno = dtproin.Rows[i]["ICDRUMNO"].ToString();
                    tda.batchno = dtproin.Rows[i]["IBATCHNO"].ToString();
                    tda.batchqty = dtproin.Rows[i]["IBATCHQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IBATCHQTY"].ToString()) : 0;
                    tda.IssueQty = dtproin.Rows[i]["IQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IQTY"].ToString()) : 0;
                    //double stock = 0;
                    //string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtproin.Rows[i]["IITEMID"].ToString() + "'");
                    //if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    //{
                    //    stock = IProductionEntry.GetStockInQty(dtproin.Rows[i]["IITEMID"].ToString(), ca.BranchId, ca.LOCID);
                    //}
                    //tda.StockAvailable = stock;
                    TData.Add(tda);
                }

                List<ProInCons> TData1 = new List<ProInCons>();
                ProInCons tda1 = new ProInCons();
                DataTable dtProInCons = IProductionEntry.ProConsDetail(PROID);
                for (int i = 0; i < dtProInCons.Rows.Count; i++)
                {
                    tda1 = new ProInCons();
                    tda1.ItemId = dtProInCons.Rows[i]["ITEMID"].ToString();
                    tda1.BinId = dtProInCons.Rows[i]["CBINID"].ToString();
                    tda1.consunit = dtProInCons.Rows[i]["CUNIT"].ToString();
                    tda1.consQty = dtProInCons.Rows[i]["CONSQTY"].ToString() != "" ? Convert.ToDouble(dtProInCons.Rows[i]["CONSQTY"].ToString()) : 0;
                    //double stock = 0;
                    //string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtProInCons.Rows[i]["CITEMID"].ToString() + "'");
                    //if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    //{
                    //    stock = IProductionEntry.GetStockInQty(dtProInCons.Rows[i]["CITEMID"].ToString(), ca.BranchId, ca.LOCID);
                    //}
                    //tda1.ConsStock = stock;
                    TData1.Add(tda1);
                }

                List<output> TData2 = new List<output>();
                output tda2 = new output();
                DataTable dtproOut = IProductionEntry.ProOutDetail(PROID);
                for (int i = 0; i < dtproOut.Rows.Count; i++)
                {
                    tda2 = new output();
                    tda2.ItemId = dtproOut.Rows[i]["ITEMID"].ToString();
                    tda2.startdate = dtproOut.Rows[i]["DSDT"].ToString();
                    tda2.starttime = dtproOut.Rows[i]["STIME"].ToString();
                    tda2.enddate = dtproOut.Rows[i]["DEDT"].ToString();
                    tda2.endtime = dtproOut.Rows[i]["ETIME"].ToString();
                    tda2.batchno = dtproOut.Rows[i]["OBATCHNO"].ToString();
                    tda2.drumno = dtproOut.Rows[i]["DRUMNO"].ToString();
                    tda2.OutStock = dtproOut.Rows[i]["OSTOCK"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OSTOCK"].ToString()) : 0;
                    tda2.OutQty = dtproOut.Rows[i]["OQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OQTY"].ToString()) : 0;
                    tda2.ExcessQty = dtproOut.Rows[i]["OXQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OXQTY"].ToString()) : 0;
                    tda2.status = dtproOut.Rows[i]["STATUS"].ToString();
                    tda2.toloc = dtproOut.Rows[i]["LOCID"].ToString();
                    TData2.Add(tda2);
                }

                List<wastage> TData3 = new List<wastage>();
                wastage tda3 = new wastage();
                DataTable dtprowaste = IProductionEntry.ProwasteDetail(PROID);
                for (int i = 0; i < dtprowaste.Rows.Count; i++)
                {
                    tda3 = new wastage();
                    tda3.ItemId = dtprowaste.Rows[i]["ITEMID"].ToString();
                    tda3.BinId = dtprowaste.Rows[i]["WBINID"].ToString();
                    tda3.toloc = dtprowaste.Rows[i]["LOCID"].ToString();
                    tda3.wastageQty = dtprowaste.Rows[i]["WQTY"].ToString() != "" ? Convert.ToDouble(dtprowaste.Rows[i]["WQTY"].ToString()) : 0;
                    tda3.batchno = dtprowaste.Rows[i]["WBATCHNO"].ToString();
                }
                ca.inputlst = TData;
                ca.inconslst = TData1;
                ca.outlst = TData2;
                ca.wastelst = TData3;
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult BatchProduction(BatchProduction Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.BatchProductionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "BatchProduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "BatchProduction Updated Successfully...!";
                    }
                    return RedirectToAction("ListBatchProduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit BatchProduction";
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
        [HttpPost]
        public ActionResult ApproveProEntry(ProductionEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.BPRODStock(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                   TempData["notice"] = "BatchProduction Approved Successfully...!";
                   return RedirectToAction("ListBatchProduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit BatchProduction";
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
        public IActionResult ApproveProEntry(string PROID)
        {
            ProductionEntry ca = new ProductionEntry();
            DataTable dt = IProductionEntry.EditProEntry(PROID);
            if (dt.Rows.Count > 0)
            {
                ca.ID = PROID;
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Shiftdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EntryType = dt.Rows[0]["ETYPE"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ca.startdate = dt.Rows[0]["STARTDATE"].ToString() + " & " + dt.Rows[0]["STARTTIME"].ToString();
                ca.enddate = dt.Rows[0]["ENDDATE"].ToString() + " & " + dt.Rows[0]["ENDTIME"].ToString();
                ca.totalinqty = dt.Rows[0]["TOTALINPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALINPUT"].ToString()) : 0;
                ca.totaloutqty = dt.Rows[0]["TOTALOUTPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALOUTPUT"].ToString()) : 0;
                ca.totalconsqty = dt.Rows[0]["TOTCONSQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSQTY"].ToString()) : 0;
                ca.wastageqty = dt.Rows[0]["TOTALWASTAGE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALWASTAGE"].ToString()) : 0;
                ca.totalRmValue = dt.Rows[0]["TOTRMVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMVALUE"].ToString()) : 0;
                ca.Machine = dt.Rows[0]["TOTMACHINEVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTMACHINEVALUE"].ToString()) : 0;
                ca.CosValue = dt.Rows[0]["TOTCONSVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSVALUE"].ToString()) : 0;
                ca.totaRmqty = dt.Rows[0]["TOTRMQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMQTY"].ToString()) : 0;
                ViewBag.entrytype = ca.EntryType;
                ViewBag.shift = ca.Shift;
                ca.BatchNo= dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete= dt.Rows[0]["BATCHCOMP"].ToString();
                ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());
                ca.SchQty = Convert.ToDouble(dt.Rows[0]["SCHQTY"].ToString() == "" ? "0" : dt.Rows[0]["SCHQTY"].ToString());
                ca.ProdLogId= dt.Rows[0]["prodlog"].ToString();
                ca.ProdSchNo= dt.Rows[0]["Prodsch"].ToString();
                ca.BranchId= dt.Rows[0]["BRANCHMASTID"].ToString();
                ca.LOCID = datatrans.GetDataString("select ILOCATION from WCBASIC where WCBASICID='"+ dt.Rows[0]["WCBASICID"].ToString()  + "'");
                ca.WCID = dt.Rows[0]["WCBASICID"].ToString();
                List<ProIn> TData = new List<ProIn>();
                ProIn tda = new ProIn();
                DataTable dtproin = IProductionEntry.ProIndetail(PROID);
                for (int i = 0; i < dtproin.Rows.Count; i++)
                {
                    tda = new ProIn();
                    tda.Proinid= dtproin.Rows[i]["BPRODINPDETID"].ToString();
                    tda.ItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.saveitemId = dtproin.Rows[i]["IITEMID"].ToString();
                    tda.BinId = dtproin.Rows[i]["IBINID"].ToString();
                    tda.drumno = dtproin.Rows[i]["ICDRUMNO"].ToString();
                    tda.drumid= dtproin.Rows[i]["IDRUMNO"].ToString();
                    tda.batchno = dtproin.Rows[i]["IBATCHNO"].ToString();
                    tda.batchqty = dtproin.Rows[i]["IBATCHQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IBATCHQTY"].ToString()) : 0;
                    tda.IssueQty = dtproin.Rows[i]["IQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IQTY"].ToString()) : 0;
                    double stock = 0;
                    string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtproin.Rows[i]["IITEMID"].ToString() + "'");
                    if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    {
                       // stock = IProductionEntry.GetStockInQty(dtproin.Rows[i]["IITEMID"].ToString(), ca.BranchId, ca.LOCID);
                        tda.Purchasestock = "Yes";
                        tda.lotlist = BindLot(dtproin.Rows[i]["IITEMID"].ToString(), ca.BranchId, ca.LOCID);
                    }
                    tda.StockAvailable = stock;
                    TData.Add(tda);
                }

                List<ProInCons> TData1 = new List<ProInCons>();
                ProInCons tda1 = new ProInCons();
                DataTable dtProInCons = IProductionEntry.ProConsDetail(PROID);
                for (int i = 0; i < dtProInCons.Rows.Count; i++)
                {
                    tda1 = new ProInCons();
                    tda1.ItemId = dtProInCons.Rows[i]["ITEMID"].ToString();
                    tda1.Proinconsid = dtProInCons.Rows[i]["BPRODCONSDETID"].ToString();
                    tda1.saveitemId = dtProInCons.Rows[i]["CITEMID"].ToString();
                    tda1.BinId = dtProInCons.Rows[i]["CBINID"].ToString();
                    tda1.consunit = dtProInCons.Rows[i]["CUNIT"].ToString();
                    tda1.consQty = dtProInCons.Rows[i]["CONSQTY"].ToString() != "" ? Convert.ToDouble(dtProInCons.Rows[i]["CONSQTY"].ToString()) : 0;
                    double stock = 0;
                    string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtProInCons.Rows[i]["CITEMID"].ToString() + "'");
                    if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    {
                        stock = IProductionEntry.GetStockInQty(dtProInCons.Rows[i]["CITEMID"].ToString(), ca.BranchId, ca.LOCID);
                        tda1.Purchasestock = "Yes";
                    }
                    tda1.ConsStock = stock;
                    
                    TData1.Add(tda1);
                }

                List<output> TData2 = new List<output>();
                output tda2 = new output();
                DataTable dtproOut = IProductionEntry.ProOutDetail(PROID);
                for (int i = 0; i < dtproOut.Rows.Count; i++)
                {
                    tda2 = new output();
                    tda2.ItemId = dtproOut.Rows[i]["ITEMID"].ToString();
                    tda2.startdate = dtproOut.Rows[i]["DSDT"].ToString();
                    tda2.starttime = dtproOut.Rows[i]["STIME"].ToString();
                    tda2.enddate = dtproOut.Rows[i]["DEDT"].ToString();
                    tda2.endtime = dtproOut.Rows[i]["ETIME"].ToString();
                    tda2.batchno = dtproOut.Rows[i]["OBATCHNO"].ToString();
                    tda2.drumno = dtproOut.Rows[i]["DRUMNO"].ToString();
                    tda2.OutStock = dtproOut.Rows[i]["OSTOCK"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OSTOCK"].ToString()) : 0;
                    tda2.OutQty = dtproOut.Rows[i]["OQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OQTY"].ToString()) : 0;
                    tda2.ExcessQty = dtproOut.Rows[i]["OXQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OXQTY"].ToString()) : 0;
                    tda2.status = dtproOut.Rows[i]["STATUS"].ToString();
                    tda2.toloc = dtproOut.Rows[i]["LOCID"].ToString();
                    tda2.outid = dtproOut.Rows[i]["BPRODOUTDETID"].ToString(); 
                    tda2.saveitemId= dtproOut.Rows[i]["OITEMID"].ToString();
                    tda2.locid = dtproOut.Rows[i]["TOLOCATION"].ToString();
                    tda2.drumid = dtproOut.Rows[i]["ODRUMNO"].ToString();
                    TData2.Add(tda2);
                }

                List<wastage> TData3 = new List<wastage>();
                wastage tda3 = new wastage();
                DataTable dtprowaste = IProductionEntry.ProwasteDetail(PROID);
                for (int i = 0; i < dtprowaste.Rows.Count; i++)
                {
                    tda3 = new wastage();
                    tda3.ItemId = dtprowaste.Rows[i]["ITEMID"].ToString();
                    tda3.BinId = dtprowaste.Rows[i]["WBINID"].ToString();
                    tda3.toloc = dtprowaste.Rows[i]["LOCID"].ToString();
                    tda3.wastageQty = dtprowaste.Rows[i]["WQTY"].ToString() != "" ? Convert.ToDouble(dtprowaste.Rows[i]["WQTY"].ToString()) : 0;
                    tda3.batchno = dtprowaste.Rows[i]["WBATCHNO"].ToString();
                    tda3.wasteid= dtprowaste.Rows[i]["BPRODWASTEDETID"].ToString();
                    tda3.saveitemId = dtprowaste.Rows[i]["WITEMID"].ToString();
                    tda3.locid= dtprowaste.Rows[i]["WLOCATION"].ToString();
                }
                ca.inputlst = TData;
                ca.inconslst = TData1;
                ca.outlst = TData2;
                ca.wastelst = TData3;
            }
            return View(ca);
        }
        public IActionResult ViewProductionEntry(string PROID)
        {
            ProductionEntry ca = new ProductionEntry();
            DataTable dt = IProductionEntry.EditProEntry(PROID);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Shiftdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EntryType = dt.Rows[0]["ETYPE"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ca.startdate = dt.Rows[0]["STARTDATE"].ToString() + " & " + dt.Rows[0]["STARTTIME"].ToString();
                ca.enddate = dt.Rows[0]["ENDDATE"].ToString() + " & " + dt.Rows[0]["ENDTIME"].ToString();
                ca.totalinqty = dt.Rows[0]["TOTALINPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALINPUT"].ToString()) : 0;
                ca.totaloutqty = dt.Rows[0]["TOTALOUTPUT"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALOUTPUT"].ToString()) : 0;
                ca.totalconsqty = dt.Rows[0]["TOTCONSQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSQTY"].ToString()) : 0;
                ca.wastageqty = dt.Rows[0]["TOTALWASTAGE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTALWASTAGE"].ToString()) : 0;
                ca.totalRmValue = dt.Rows[0]["TOTRMVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMVALUE"].ToString()) : 0;
                ca.Machine = dt.Rows[0]["TOTMACHINEVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTMACHINEVALUE"].ToString()) : 0;
                ca.CosValue = dt.Rows[0]["TOTCONSVALUE"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTCONSVALUE"].ToString()) : 0;
                ca.totaRmqty = dt.Rows[0]["TOTRMQTY"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["TOTRMQTY"].ToString()) : 0;
                ViewBag.entrytype = ca.EntryType;
                ViewBag.shift = ca.Shift;
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHCOMP"].ToString();
                ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());
                ca.SchQty = Convert.ToDouble(dt.Rows[0]["SCHQTY"].ToString() == "" ? "0" : dt.Rows[0]["SCHQTY"].ToString());
                ca.ProdLogId = dt.Rows[0]["prodlog"].ToString();
                ca.ProdSchNo = dt.Rows[0]["Prodsch"].ToString();
                ca.BranchId = dt.Rows[0]["BRANCHMASTID"].ToString();
                ca.LOCID = datatrans.GetDataString("select ILOCATION from WCBASIC where WCBASICID='" + dt.Rows[0]["WCBASICID"].ToString() + "'");
                List<ProIn> TData = new List<ProIn>();
                ProIn tda = new ProIn();
                DataTable dtproin = IProductionEntry.ProIndetail(PROID);
                for (int i = 0; i < dtproin.Rows.Count; i++)
                {
                    tda = new ProIn();
                    tda.ItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.BinId = dtproin.Rows[i]["IBINID"].ToString();
                    tda.drumno = dtproin.Rows[i]["ICDRUMNO"].ToString();
                    tda.batchno = dtproin.Rows[i]["IBATCHNO"].ToString();
                    tda.batchqty = dtproin.Rows[i]["IBATCHQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IBATCHQTY"].ToString()) : 0;
                    tda.IssueQty = dtproin.Rows[i]["IQTY"].ToString() != "" ? Convert.ToDouble(dtproin.Rows[i]["IQTY"].ToString()) : 0;
                    //double stock = 0;
                    //string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtproin.Rows[i]["IITEMID"].ToString() + "'");
                    //if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    //{
                    //    stock = IProductionEntry.GetStockInQty(dtproin.Rows[i]["IITEMID"].ToString(), ca.BranchId, ca.LOCID);
                    //}
                    //tda.StockAvailable = stock;
                    TData.Add(tda);
                }

                List<ProInCons> TData1 = new List<ProInCons>();
                ProInCons tda1 = new ProInCons();
                DataTable dtProInCons = IProductionEntry.ProConsDetail(PROID);
                for (int i = 0; i < dtProInCons.Rows.Count; i++)
                {
                    tda1 = new ProInCons();
                    tda1.ItemId = dtProInCons.Rows[i]["ITEMID"].ToString();
                    tda1.BinId = dtProInCons.Rows[i]["CBINID"].ToString();
                    tda1.consunit = dtProInCons.Rows[i]["CUNIT"].ToString();
                    tda1.consQty = dtProInCons.Rows[i]["CONSQTY"].ToString() != "" ? Convert.ToDouble(dtProInCons.Rows[i]["CONSQTY"].ToString()) : 0;
                    //double stock = 0;
                    //string Itemgroup = datatrans.GetDataString("select ITEMGROUP.GROUPCODE from ITEMMASTER LEFT OUTER JOIN ITEMGROUP ON ITEMGROUP.ITEMGROUPID=ITEMMASTER.ITEMGROUP WHERE ITEMMASTERID='" + dtProInCons.Rows[i]["CITEMID"].ToString() + "'");
                    //if (Itemgroup == "RAW MATERIAL" || Itemgroup == "Consumables" || Itemgroup == "Other Consumables")
                    //{
                    //    stock = IProductionEntry.GetStockInQty(dtProInCons.Rows[i]["CITEMID"].ToString(), ca.BranchId, ca.LOCID);
                    //}
                    //tda1.ConsStock = stock;
                    TData1.Add(tda1);
                }

                List<output> TData2 = new List<output>();
                output tda2 = new output();
                DataTable dtproOut = IProductionEntry.ProOutDetail(PROID);
                for (int i = 0; i < dtproOut.Rows.Count; i++)
                {
                    tda2 = new output();
                    tda2.ItemId = dtproOut.Rows[i]["ITEMID"].ToString();
                    tda2.startdate = dtproOut.Rows[i]["DSDT"].ToString();
                    tda2.starttime = dtproOut.Rows[i]["STIME"].ToString();
                    tda2.enddate = dtproOut.Rows[i]["DEDT"].ToString();
                    tda2.endtime = dtproOut.Rows[i]["ETIME"].ToString();
                    tda2.batchno = dtproOut.Rows[i]["OBATCHNO"].ToString();
                    tda2.drumno = dtproOut.Rows[i]["DRUMNO"].ToString();
                    tda2.OutStock = dtproOut.Rows[i]["OSTOCK"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OSTOCK"].ToString()) : 0;
                    tda2.OutQty = dtproOut.Rows[i]["OQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OQTY"].ToString()) : 0;
                    tda2.ExcessQty = dtproOut.Rows[i]["OXQTY"].ToString() != "" ? Convert.ToDouble(dtproOut.Rows[i]["OXQTY"].ToString()) : 0;
                    tda2.status = dtproOut.Rows[i]["STATUS"].ToString();
                    tda2.toloc = dtproOut.Rows[i]["LOCID"].ToString();
                    TData2.Add(tda2);
                }

                List<wastage> TData3 = new List<wastage>();
                wastage tda3 = new wastage();
                DataTable dtprowaste = IProductionEntry.ProwasteDetail(PROID);
                for (int i = 0; i < dtprowaste.Rows.Count; i++)
                {
                    tda3 = new wastage();
                    tda3.ItemId = dtprowaste.Rows[i]["ITEMID"].ToString();
                    tda3.BinId = dtprowaste.Rows[i]["WBINID"].ToString();
                    tda3.toloc = dtprowaste.Rows[i]["LOCID"].ToString();
                    tda3.wastageQty = dtprowaste.Rows[i]["WQTY"].ToString() != "" ? Convert.ToDouble(dtprowaste.Rows[i]["WQTY"].ToString()) : 0;
                    tda3.batchno = dtprowaste.Rows[i]["WBATCHNO"].ToString();
                }
                ca.inputlst = TData;
                ca.inconslst = TData1;
                ca.outlst = TData2;
                ca.wastelst = TData3;
            }
            return View(ca);
        }
        public JsonResult GetItemJSON(string itemid)
        {
            EnqItem model = new EnqItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));
        }
        public JsonResult GetsearchItemJSON(string prefix)
        {
            return Json(BindSItemlst(prefix));
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(Binddrum());
        }
        public List<SelectListItem> BindProdLog()
        {
            try
            {
                DataTable dtDesg = datatrans.GetProdLog();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["LPRODBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLot(string Itemid,string branchid,string locid)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.Getlot(Itemid,branchid,locid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "--- Select ----", Value = "0" });
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOT_NO"].ToString(), Value = dtDesg.Rows[i]["LOT_NO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<SelectListItem> BindProdSch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetProdSch();
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
        public List<SelectListItem> BindBatch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBatch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
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
                DataTable dtDesg = datatrans.GetItem(value);
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
        public List<SelectListItem> BindSItemlst(string term)
        {
            try
            {
                DataTable dtDesg = IProductionEntry.SeacrhItem(term);
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetWorkCenter();
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
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
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
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.DrumDeatils();
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.ShiftDeatils();
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
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.BindProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSMASTID"].ToString() });
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
                lstdesg.Add(new SelectListItem() { Text = "BOTH", Value = "BOTH" });
                lstdesg.Add(new SelectListItem() { Text = "INPUT", Value = "INPUT" });
                //lstdesg.Add(new SelectListItem() { Text = "OUTPUT", Value = "OUTPUT" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PENDING", Value = "PENDING" });
                lstdesg.Add(new SelectListItem() { Text = "COMPLETED", Value = "COMPLETED" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindoutput()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "YES", Value = "YES" });
                lstdesg.Add(new SelectListItem() { Text = "NO", Value = "NO" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation()
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
        public ActionResult GetStkqty(string branch, string loc, string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string stkqty = "0";
                dt = IProductionEntry.Getstkqty(branch, loc, ItemId);
                if (dt.Rows.Count > 0)
                {
                    stkqty = dt.Rows[0]["QTY"].ToString();
                }
                if (stkqty == "")
                {
                    stkqty = "0";
                }
                var result = new { stkqty = stkqty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetLotStkqty(string branch, string loc, string ItemId,string Lotno)
        {
            try
            {
                DataTable dt = new DataTable();
                string stkqty = "0";
                dt = IProductionEntry.GetLotstkqty(branch, loc, ItemId, Lotno);
                if (dt.Rows.Count > 0)
                {
                    stkqty = dt.Rows[0]["QTY"].ToString();
                }
                if (stkqty == "")
                {
                    stkqty = "0";
                }
                var result = new { stkqty = stkqty };
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

        public IActionResult ListBatchProduction()
        {
            IEnumerable<BatchProduction> cmp = IProductionEntry.GetAllBatchProduction();
            return View(cmp);
        }

    }
}
