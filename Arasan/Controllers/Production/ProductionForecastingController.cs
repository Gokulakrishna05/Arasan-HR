using System.Collections.Generic;
using Arasan.Interface.Master;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface.Production;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Arasan.Services.Store_Management;
using Arasan.Interface;

namespace Arasan.Controllers.Production
{
    public class ProductionForecastingController : Controller
    {
        IProductionForecastingService _ProdForecastServ;

        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ProductionForecastingController(IProductionForecastingService _ProductionForecastingService, IConfiguration _configuratio)
        {
            _ProdForecastServ = _ProductionForecastingService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionForecasting(string id)
        {
            ProductionForecasting ca = new ProductionForecasting();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.RecList = BindEmp();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Enterd = Request.Cookies["UserId"];
            ca.mnthlst = Bindmnth();
            DataTable dtv = datatrans.GetSequence("ProFc");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<PFCItem> TData = new List<PFCItem>();
            PFCItem tda = new PFCItem();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            List<PFCPYROItem> TData2 = new List<PFCPYROItem>();
            PFCPYROItem tda2 = new PFCPYROItem();
            List<PFCPOLIItem> TData3 = new List<PFCPOLIItem>();
            PFCPOLIItem tda3 = new PFCPOLIItem();
            List<PFCRVDItem> TData4 = new List<PFCRVDItem>();
            PFCRVDItem tda4 = new PFCRVDItem();
            List<PFCPASTEItem> TData5 = new List<PFCPASTEItem>();
            PFCPASTEItem tda5 = new PFCPASTEItem();
            List<PFCAPPRODItem> TData6 = new List<PFCAPPRODItem>();
            PFCAPPRODItem tda6 = new PFCAPPRODItem();
            List<PFCPACKItem> TData7 = new List<PFCPACKItem>();
            PFCPACKItem tda7 = new PFCPACKItem();

            List<ProdApItem> TData8 = new List<ProdApItem>();
            ProdApItem tda8 = new ProdApItem();

            List<ProdApReqItem> TData9 = new List<ProdApReqItem>();
            ProdApReqItem tda9 = new ProdApReqItem();
            

            if (id == null)
            {
                ca.plantype = "MONTHLY";
                for (int i = 0; i < 1; i++)
                {
                    tda = new PFCItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new PFCDGItem();
                    //tda1.PItemlst = BindItemlst("");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda2 = new PFCPYROItem();
                    tda2.Worklst = BindWorkCenter();
                    //tda2.PYItemlst = BindItemlst("");
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda3 = new PFCPOLIItem();
                    tda3.POWorklst = BindWorkCenter();
                    //tda3.POItemlst = BindItemlst("");
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda4 = new PFCRVDItem();
                    tda4.POWorklst = BindWorkCenter();
                    //tda3.POItemlst = BindItemlst("");
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda5 = new PFCPASTEItem();
                    tda5.Worklst = BindWorkCenter();
                    //tda3.POItemlst = BindItemlst("");
                    tda5.Isvalid = "Y";
                    TData5.Add(tda5);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda6 = new PFCAPPRODItem();
                    tda6.Worklst = BindWorkCenter();
                    //tda3.POItemlst = BindItemlst("");
                    tda6.Isvalid = "Y";
                    TData6.Add(tda6);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda7 = new PFCPACKItem();
                    tda7.partylst = BindParty();
                    //tda3.POItemlst = BindItemlst("");
                    tda7.Isvalid = "Y";
                    TData7.Add(tda7);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda8 = new ProdApItem();
 
                    tda8.Isvalid = "Y";
                    TData8.Add(tda8);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = _ProdForecastServ.GetPFDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.PType = dt.Rows[0]["PLANTYPE"].ToString();
                    ca.ForMonth = dt.Rows[0]["MONTH"].ToString();
                    ca.Ins = dt.Rows[0]["INCDECPER"].ToString();
                    ca.Hd = dt.Rows[0]["HD"].ToString();
                    ca.Fordate = dt.Rows[0]["FINYRPST"].ToString();
                    ca.Enddate = dt.Rows[0]["FINYRPED"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = _ProdForecastServ.GetProdForecastDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new PFCItem();
                        tda.Itemlst = BindItemlst(tda.ItemId);
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[0]["UNIT"].ToString();
                        tda.PType = dt2.Rows[0]["PTYPE"].ToString();
                        tda.PysQty = dt2.Rows[0]["PREVYQTY"].ToString();
                        tda.PtmQty = dt2.Rows[0]["PREVMQTY"].ToString();
                        tda.Fqty = dt2.Rows[0]["PQTY"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = _ProdForecastServ.GetProdForecastDGPasteDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new PFCDGItem();
                        tda1.PItemlst = BindItemlst(tda1.itemid);
                        tda1.itemid = dt3.Rows[0]["DGITEMID"].ToString();
                        tda1.target = dt3.Rows[0]["DGTARQTY"].ToString();
                        tda1.min = dt3.Rows[0]["DGMIN"].ToString();
                        tda1.stock = dt3.Rows[0]["DGSTOCK"].ToString();
                        tda1.required = dt3.Rows[0]["REQDG"].ToString();
                        tda1.dgaddit = dt3.Rows[0]["DGADDITID"].ToString();
                        tda1.reqadditive = dt3.Rows[0]["DGADDITREQ"].ToString();
                        tda1.rawmaterial = dt3.Rows[0]["DGRAWMAT"].ToString();
                        tda1.reqpyro = dt3.Rows[0]["DGREQAP"].ToString();
                        tda1.saveitemid = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();

                dt4 = _ProdForecastServ.GetProdForecastPyroDetail(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new PFCPYROItem();
                        tda2.PYItemlst = BindItemlst(tda2.itemid);
                        tda2.itemid = dt4.Rows[0]["PYITEMID"].ToString();

                        tda2.Worklst = BindWorkCenter();
                        tda2.WorkId = dt4.Rows[0]["PYWCID"].ToString();

                        tda2.CDays = dt4.Rows[0]["WCDAYS"].ToString();
                        tda2.minstock = dt4.Rows[0]["PYMINSTK"].ToString();
                        tda2.pasterej = dt4.Rows[0]["PYALLREJ"].ToString();
                        tda2.GradeChange = dt4.Rows[0]["PYGRCHG"].ToString();
                        tda2.rejqty = dt4.Rows[0]["PYREJQTY"].ToString();
                        tda2.required = dt4.Rows[0]["PYREQQTY"].ToString();
                        tda2.target = dt4.Rows[0]["PYTARQTY"].ToString();
                        tda2.proddays = dt4.Rows[0]["PYPRODCAPD"].ToString();
                        tda2.prodqty = dt4.Rows[0]["PYPRODQTY"].ToString();
                        tda2.rejmat = dt4.Rows[0]["PYRAWREJMAT"].ToString();
                        tda2.rejmatreq = dt4.Rows[0]["PYRAWREJMATPER"].ToString();
                        tda2.balanceqty = dt4.Rows[0]["PREBALQTY"].ToString();
                        tda2.additive = dt4.Rows[0]["PYADD1"].ToString();
                        tda2.per = dt4.Rows[0]["PYADDPER"].ToString();
                        tda2.allocadditive = dt4.Rows[0]["ALLOCADD"].ToString();
                        tda2.reqpowder = dt4.Rows[0]["PYREQAP"].ToString();
                        tda2.wstatus = dt4.Rows[0]["WSTATUS"].ToString();
                        tda2.powderrequired = dt4.Rows[0]["POWREQ"].ToString();
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();

                dt5 = _ProdForecastServ.GetProdForecastPolishDetail(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new PFCPOLIItem();
                    
                        tda3.itemid = dt5.Rows[0]["PIGWCID"].ToString();

                        tda3.POWorklst = BindWorkCenter();
                        tda3.workid = dt5.Rows[0]["PIGITEMID"].ToString();

                        tda3.wcdays = dt5.Rows[0]["PIGWCDAYS"].ToString();
                        tda3.target = dt5.Rows[0]["PIGTARGET"].ToString();
                        tda3.capacity = dt5.Rows[0]["PIGCAP"].ToString();
                        tda3.stock = dt5.Rows[0]["PIGSTOCK"].ToString();
                        tda3.minstock = dt5.Rows[0]["PIGMINSTK"].ToString();
                        tda3.required = dt5.Rows[0]["PIGRAWREQ"].ToString();
                        tda3.days = dt5.Rows[0]["PIGDAYS"].ToString();
                        tda3.additive = dt5.Rows[0]["PIGADDIT"].ToString();
                        tda3.add = dt5.Rows[0]["PIGADDPER"].ToString();
                        tda3.rejmat = dt5.Rows[0]["PIGRAWMAT"].ToString();
                        tda3.reqper = dt5.Rows[0]["PIGRAWREQPER"].ToString();
                        tda3.rvdqty = dt5.Rows[0]["PIGRVDQTY"].ToString();
                        tda3.pyropowder = dt5.Rows[0]["PIGPYPO"].ToString();
                        tda3.pyroqty = dt5.Rows[0]["PIGPYQTY"].ToString();
                        tda3.powderrequired = dt5.Rows[0]["PIGPOWREQ"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }

            }

            ca.PFCILst = TData;
            ca.PFCDGILst = TData1;
            ca.PFCPYROILst = TData2;
            ca.PFCPOLILst = TData3;
            ca.PFCRVDLst = TData4;
            ca.PFCPASTELst = TData5;
            ca.PFCAPPRODLst = TData6;
            ca.PFCPACKLst = TData7;
            ca.Aplst=TData8;
            ca.PFAPREFlst= TData9;
            return View(ca);
        }
        public IActionResult ProdForecasting(string id)
        {
            ProductionForecasting ca = new ProductionForecasting();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.RecList = BindEmp();
            List<PFCItem> TData = new List<PFCItem>();
            PFCItem tda = new PFCItem();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            List<PFCPYROItem> TData2 = new List<PFCPYROItem>();
            PFCPYROItem tda2 = new PFCPYROItem();
            List<PFCPOLIItem> TData3 = new List<PFCPOLIItem>();
            PFCPOLIItem tda3 = new PFCPOLIItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new PFCItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new PFCDGItem();
                    tda1.PItemlst = BindItemlst("");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new PFCPYROItem();
                    tda2.Worklst = BindWorkCenter();
                    tda2.PYItemlst = BindItemlst("");
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new PFCPOLIItem();
                    tda3.POWorklst = BindWorkCenter();
                    tda3.POItemlst = BindItemlst("");
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = _ProdForecastServ.GetPFDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.PType = dt.Rows[0]["PLANTYPE"].ToString();
                    ca.ForMonth = dt.Rows[0]["MONTH"].ToString();
                    ca.Ins = dt.Rows[0]["INCDECPER"].ToString();
                    ca.Hd = dt.Rows[0]["HD"].ToString();
                    ca.Fordate = dt.Rows[0]["FINYRPST"].ToString();
                    ca.Enddate = dt.Rows[0]["FINYRPED"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = _ProdForecastServ.GetProdForecastDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new PFCItem();
                        tda.Itemlst = BindItemlst(tda.ItemId);
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[0]["UNIT"].ToString();
                        tda.PType = dt2.Rows[0]["PTYPE"].ToString();
                        tda.PysQty = dt2.Rows[0]["PREVYQTY"].ToString();
                        tda.PtmQty = dt2.Rows[0]["PREVMQTY"].ToString();
                        tda.Fqty = dt2.Rows[0]["PQTY"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = _ProdForecastServ.GetProdForecastDGPasteDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new PFCDGItem();
                        tda1.PItemlst = BindItemlst(tda1.itemid);
                        tda1.itemid = dt3.Rows[0]["DGITEMID"].ToString();
                        tda1.target = dt3.Rows[0]["DGTARQTY"].ToString();
                        tda1.min = dt3.Rows[0]["DGMIN"].ToString();
                        tda1.stock = dt3.Rows[0]["DGSTOCK"].ToString();
                        tda1.required = dt3.Rows[0]["REQDG"].ToString();
                        tda1.dgaddit = dt3.Rows[0]["DGADDITID"].ToString();
                        tda1.reqadditive = dt3.Rows[0]["DGADDITREQ"].ToString();
                        tda1.rawmaterial = dt3.Rows[0]["DGRAWMAT"].ToString();
                        tda1.reqpyro = dt3.Rows[0]["DGREQAP"].ToString();
                        tda1.saveitemid = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();

                dt4 = _ProdForecastServ.GetProdForecastPyroDetail(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new PFCPYROItem();
                        tda2.PYItemlst = BindItemlst(tda2.itemid);
                        tda2.itemid = dt4.Rows[0]["PYITEMID"].ToString();

                        tda2.Worklst = BindWorkCenter();
                        tda2.WorkId = dt4.Rows[0]["PYWCID"].ToString();

                        tda2.CDays = dt4.Rows[0]["WCDAYS"].ToString();
                        tda2.minstock = dt4.Rows[0]["PYMINSTK"].ToString();
                        tda2.pasterej = dt4.Rows[0]["PYALLREJ"].ToString();
                        tda2.GradeChange = dt4.Rows[0]["PYGRCHG"].ToString();
                        tda2.rejqty = dt4.Rows[0]["PYREJQTY"].ToString();
                        tda2.required = dt4.Rows[0]["PYREQQTY"].ToString();
                        tda2.target = dt4.Rows[0]["PYTARQTY"].ToString();
                        tda2.proddays = dt4.Rows[0]["PYPRODCAPD"].ToString();
                        tda2.prodqty = dt4.Rows[0]["PYPRODQTY"].ToString();
                        tda2.rejmat = dt4.Rows[0]["PYRAWREJMAT"].ToString();
                        tda2.rejmatreq = dt4.Rows[0]["PYRAWREJMATPER"].ToString();
                        tda2.balanceqty = dt4.Rows[0]["PREBALQTY"].ToString();
                        tda2.additive = dt4.Rows[0]["PYADD1"].ToString();
                        tda2.per = dt4.Rows[0]["PYADDPER"].ToString();
                        tda2.allocadditive = dt4.Rows[0]["ALLOCADD"].ToString();
                        tda2.reqpowder = dt4.Rows[0]["PYREQAP"].ToString();
                        tda2.wstatus = dt4.Rows[0]["WSTATUS"].ToString();
                        tda2.powderrequired = dt4.Rows[0]["POWREQ"].ToString();
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();

                dt5 = _ProdForecastServ.GetProdForecastPolishDetail(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new PFCPOLIItem();

                        tda3.itemid = dt5.Rows[0]["PIGWCID"].ToString();

                        tda3.POWorklst = BindWorkCenter();
                        tda3.workid = dt5.Rows[0]["PIGITEMID"].ToString();

                        tda3.wcdays = dt5.Rows[0]["PIGWCDAYS"].ToString();
                        tda3.target = dt5.Rows[0]["PIGTARGET"].ToString();
                        tda3.capacity = dt5.Rows[0]["PIGCAP"].ToString();
                        tda3.stock = dt5.Rows[0]["PIGSTOCK"].ToString();
                        tda3.minstock = dt5.Rows[0]["PIGMINSTK"].ToString();
                        tda3.required = dt5.Rows[0]["PIGRAWREQ"].ToString();
                        tda3.days = dt5.Rows[0]["PIGDAYS"].ToString();
                        tda3.additive = dt5.Rows[0]["PIGADDIT"].ToString();
                        tda3.add = dt5.Rows[0]["PIGADDPER"].ToString();
                        tda3.rejmat = dt5.Rows[0]["PIGRAWMAT"].ToString();
                        tda3.reqper = dt5.Rows[0]["PIGRAWREQPER"].ToString();
                        tda3.rvdqty = dt5.Rows[0]["PIGRVDQTY"].ToString();
                        tda3.pyropowder = dt5.Rows[0]["PIGPYPO"].ToString();
                        tda3.pyroqty = dt5.Rows[0]["PIGPYQTY"].ToString();
                        tda3.powderrequired = dt5.Rows[0]["PIGPOWREQ"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }

            }

            ca.PFCILst = TData;
            ca.PFCDGILst = TData1;
            ca.PFCPYROILst = TData2;
            ca.PFCPOLILst = TData3;
            return View(ca);
        }

            [HttpPost]
        public ActionResult ProductionForecasting(ProductionForecasting Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout =  _ProdForecastServ.ProductionForecastingCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionForecasting Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionForecasting Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductionForecasting");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ProductionForecasting";
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
        public IActionResult ListProductionForecasting()
        {
            
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ProdFCList> Reg = new List<ProdFCList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = _ProdForecastServ.GetAllProdFC(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;
                string generate = string.Empty;

                View = "<a href=ViewProdFc?id=" + dtUsers.Rows[i]["PRODFCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                generate = "<a href=GenProdShe?id=" + dtUsers.Rows[i]["PRODFCBASICID"].ToString() + " ><img src='../Images/move_quote.png' width='20px' alt='View' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PRODFCBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new ProdFCList
                {
                    id = dtUsers.Rows[i]["PRODFCBASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    month = dtUsers.Rows[i]["MONTH"].ToString(),
                    plan = dtUsers.Rows[i]["PLANTYPE"].ToString(),
                    

                    viewrow = View,
                    generate = generate,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
               

                string unit = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    
                }

                var result = new { unit = unit };
                return Json(result);
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
        public JsonResult GetWCJSON()
        {
            return Json(BindPYROWC());

        }

        public JsonResult GetAPWCJSON()
        {
            return Json(BindAPWC());

        }

        public JsonResult GetPolishJSON()
        {
            return Json(BindPolishWC());

        }
        public JsonResult GetRVDJSON()
        {
            return Json(BindRVDWC());

        }
        public JsonResult GetPasteJSON()
        {
            return Json(BindPasteWC());

        }
        public JsonResult GetPartyJSON()
        {
            return Json(BindParty());

        }

        public List<SelectListItem> BindPYROWC()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetPYROWC();
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

        public List<SelectListItem> BindAPWC()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetAPWC();
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
        public List<SelectListItem> BindPolishWC()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetPolishWC();
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

        public List<SelectListItem> BindPasteWC()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetPasteWC();
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

        public List<SelectListItem> BindRVDWC()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetRVDWC();
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

        public List<SelectListItem> BindParty()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
                DataTable dtDesg = _ProdForecastServ.GetWorkCenter();
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

        public List<SelectListItem> Bindmnth()
        {
            try
            {
                DataTable dtDesg = _ProdForecastServ.GetMnth();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MONTH"].ToString(), Value = dtDesg.Rows[i]["MONTH"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetDGPaste(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            DataTable dtt = new DataTable();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            dtt = _ProdForecastServ.GetDGPaste(mnth, type);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda1 = new PFCDGItem();

                    tda1.itemid = dtt.Rows[i]["ITEMID"].ToString();
                    
                    tda1.target = dtt.Rows[i]["ORD"].ToString();
                    tda1.min = dtt.Rows[i]["MINSTK"].ToString();
                    tda1.stock = dtt.Rows[i]["STK"].ToString();
                    tda1.required = dtt.Rows[i]["REQ"].ToString();
                    tda1.saveitemid = datatrans.GetDataString("Select ITEMMASTERID From ITEMMASTER WHERE ITEMID='" + tda1.itemid + "'");
                    tda1.dgaddit= datatrans.GetDataString("SELECT   I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='"+ tda1.itemid + "' AND I1.ITEMMASTERID = I.ADD1");
                    tda1.dgadditid= datatrans.GetDataString("SELECT   I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID ='" + tda1.itemid + "' AND I1.ITEMMASTERID = I.ADD1");
                    string additive= datatrans.GetDataString("SELECT add1per FROM ITEMMASTER WHERE ITEMID='" + tda1.itemid + "'");
                    double addit = Math.Round(Convert.ToDouble(tda1.required) * (Convert.ToDouble(additive) / 100));
                    double reqpyro= Convert.ToDouble(tda1.required) - addit;
                    tda1.reqadditive = addit.ToString();
                    tda1.reqpyro = reqpyro.ToString();
                    tda1.rawmaterial = datatrans.GetDataString("SELECT I1.ItemID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID='"+ tda1.itemid + "' AND I1.ITEMMASTERID=I.ITEMFROM");
                    tda1.rawmaterialid = datatrans.GetDataString("SELECT I1.ITEMMASTERID FROM ITEMMASTER I, ITEMMASTER I1 WHERE I.ITEMID='" + tda1.itemid + "' AND I1.ITEMMASTERID=I.ITEMFROM");
                    TData1.Add(tda1);
                }
            }
            model.PFCDGILst = TData1;
            return Json(model.PFCDGILst);

        }

        public ActionResult GetPyroForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            model.PFCPYROILst = _ProdForecastServ.GetPyroForecast(mnth, type); 
            return Json(model.PFCPYROILst);

        }
        public ActionResult GetPolishForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            model.PFCPOLILst = _ProdForecastServ.GetPolishForecast(mnth, type);
            return Json(model.PFCPOLILst);

        }
       
        //public ActionResult GetRVDForecast(string mnth, string type)
        //{
        //    ProductionForecasting model = new ProductionForecasting();
        //    //model.PFCPYROILst = _ProdForecastServ.GetPyroForecast(mnth, type);
        //    if (model.PFCPYROILst == null)
        //    {
        //        List<PFCRVDItem> TData4 = new List<PFCRVDItem>();
        //        PFCRVDItem tda4 = new PFCRVDItem();
        //        for (int i = 0; i < 1; i++)
        //        {
        //            tda4 = new PFCRVDItem();
        //            tda4.POWorklst = BindWorkCenter();
        //            //tda3.POItemlst = BindItemlst("");
        //            tda4.Isvalid = "Y";
        //            TData4.Add(tda4);
        //        }
        //        model.PFCRVDLst = TData4;
        //    }
        //    return Json(model.PFCRVDLst);

        //}
        public ActionResult GetPasteForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            model.PFCPASTELst = _ProdForecastServ.GetPasteForecast(mnth, type);
            return Json(model.PFCPASTELst);

        }
        public ActionResult GetAPForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            //model.PFCPYROILst = _ProdForecastServ.GetPyroForecast(mnth, type);
            if (model.PFCAPPRODLst == null)
            {
                List<PFCAPPRODItem> TData6 = new List<PFCAPPRODItem>();
                PFCAPPRODItem tda6 = new PFCAPPRODItem();
                DataTable dt = new DataTable();
                dt = datatrans.GetData("select * from SIEVEMAST where IS_ACTIVE='Y'");
                if(dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tda6 = new PFCAPPRODItem();
                        tda6.sieve = dt.Rows[i]["SIEVE"].ToString();
                        tda6.sieveid = dt.Rows[i]["SIEVEMASTID"].ToString();
                        tda6.svalue = dt.Rows[i]["STARTVALUE"].ToString();
                        tda6.endvalue = dt.Rows[i]["ENDVALUE"].ToString();
                        TData6.Add(tda6);
                    }
                }


                //tda6 = new PFCAPPRODItem();
                //tda6.sieve = "< 40";
                //TData6.Add(tda6);

                //tda6 = new PFCAPPRODItem();
                //tda6.sieve = "40 - 50";
                //TData6.Add(tda6);

                //tda6 = new PFCAPPRODItem();
                //tda6.sieve = "50 - 65";
                //TData6.Add(tda6);

                //tda6 = new PFCAPPRODItem();
                //tda6.sieve = "65 - 85";
                //TData6.Add(tda6);

                //tda6 = new PFCAPPRODItem();
                //tda6.sieve = "85 above";
                //TData6.Add(tda6);

                model.PFCAPPRODLst = TData6;
            }
            return Json(model.PFCAPPRODLst);

        }
        public ActionResult GetPackingForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            model.PFCPACKLst = _ProdForecastServ.GetPackForecast(mnth, type);
            return Json(model.PFCPACKLst);

        }
        public ActionResult GetAPSForecast(string mnth, string type)
        {
            ProductionForecasting model = new ProductionForecasting();
            model.Aplst = _ProdForecastServ.GetAPSForecast(mnth, type);
            return Json(model.Aplst);

        }
        //public ActionResult GetAPForecast([FromBody] PyroItemArray[] model)
        //{
        //    ProductionForecasting m = new ProductionForecasting();
        //    List<ProdApItem> cmpList = new List<ProdApItem>();
        //    ProdApItem tda = new ProdApItem();
        //    DataTable dt =new DataTable();
        //    string itm = "";
        //    string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
        //    List<string> itemlist = new List<string>();
        //    foreach (PyroItemArray input in model)
        //    {
        //        //itemlist.Add(input.itemid);
        //        if (!string.IsNullOrEmpty(input.itemid))
        //        {
        //            itm += input.itemid + ",";
        //        }
                
        //    }
           
        //        m.Aplst = cmpList;
        //    return Json(m.PFAPREFlst);

        //}

        public ActionResult GetAPReqForecast([FromBody] PyroItemArray[] model)
        {
            ProductionForecasting m = new ProductionForecasting();
            var uniqueItemsList = model.Distinct().ToList();

            List<ProdApReqItem> cmpList = new List<ProdApReqItem>();
            ProdApReqItem tda = new ProdApReqItem();
            DataTable dt = new DataTable();
            string itm = "";
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<string> itemlist = new List<string>();
            string ingotssql = @"SELECT SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) stk
FROM StockValue S , ItemMaster I , LocDetails L 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <='" + Docdate + "' AND S.LocID = L.LocdetailsID ";
            ingotssql += @"AND i.SNCATEGORY IN ('ALUMINIUM INGOTS','REMELTED ALUMINIUM INGOTS','MOLTED INGOTS') AND i.QCCOMPFLAG='YES' AND L.LocationType IN ('AP MILL','STORES','REMELTING','QUALITY') 
HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 ORDER BY 1";
            string ingosstk = datatrans.GetDataString(ingotssql);

            foreach (PyroItemArray input in uniqueItemsList)
            {
                if (!string.IsNullOrEmpty(input.itemid))
                {
                    //itm += input.itemid + ",";
                   
                            tda = new ProdApReqItem();
                            tda.itemid = datatrans.GetDataString("select ITEMID from  ITEMMASTER Where ITEMMASTERID='" + input.itemid + "'");
                            tda.saveitemid = input.itemid;
                            tda.startvalue = input.StartValue;
                            tda.endvalue = input.EndValue;
                            string sql = @"SELECT Round(SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)),0) stk
FROM StockValue S , ItemMaster I , LocDetails L 
WHERE S.ItemID = I.ItemMasterID AND S.DocDate <='" + Docdate + "' AND S.LocID = L.LocdetailsID ";
                            sql += @" AND L.LocationType IN ('AP MILL','SIEVE & BLEND')
  AND I.ItemMasterID='" + tda.saveitemid + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0";
                            string avlstk = datatrans.GetDataString(sql);
                            tda.avlstk = avlstk == "" ? "0" : avlstk;
                            string ministk = datatrans.GetDataString("SELECT sum(MINSTK) FROM ITEMMASTER WHERE ITEMMASTERID ='" + tda.saveitemid + "'");
                            tda.ministk = ministk == "" ? "0" : ministk;
                            tda.ingotstock = ingosstk;
                            cmpList.Add(tda);
                      
                }

            }
            //itm = itm.Remove(itm.Length - 1);
        
//            dt = datatrans.GetData("select ITEMMASTERID,ITEMID from  ITEMMASTER Where ITEMMASTERID IN(" + itm + ") GROUP BY ITEMMASTERID,ITEMID");
//            if (dt.Rows.Count > 0)
//            {
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    tda = new ProdApReqItem();
//                    tda.itemid = dt.Rows[i]["ITEMID"].ToString();
//                    tda.saveitemid = dt.Rows[i]["ITEMMASTERID"].ToString();
//                    string sql = @"SELECT Round(SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)),0) stk
//FROM StockValue S , ItemMaster I , LocDetails L 
//WHERE S.ItemID = I.ItemMasterID AND S.DocDate <='" + Docdate + "' AND S.LocID = L.LocdetailsID ";
//                    sql += @" AND L.LocationType IN ('AP MILL','SIEVE & BLEND')
//  AND I.ItemMasterID='" + tda.saveitemid + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0";
//                    string avlstk = datatrans.GetDataString(sql);
//                    tda.avlstk = avlstk == "" ? "0" : avlstk;
//                    string ministk = datatrans.GetDataString("SELECT sum(MINSTK) FROM ITEMMASTER WHERE ITEMMASTERID ='" + tda.saveitemid + "'");
//                    tda.ministk = ministk == "" ? "0" : ministk;
//                    tda.ingotstock = ingosstk;
//                    cmpList.Add(tda);
//                }
//            }
            m.PFAPREFlst = cmpList;
            return Json(m.PFAPREFlst);

        }

        public ActionResult GetpyrowcDetail(string itemid,string wcid)
        {
            try
            {
                string tar = datatrans.GetDataString("Select Sum(tar) Tar from (SELECT SUM(WD.PRATE*22) TAR FROM WCBASIC W,WCPRODDETAIL WD,ITEMMASTER I WHERE W.WCBASICID=WD.WCBASICID AND W.WCBASICID='" + wcid + "' AND I.ITEMMASTERID=WD.ITEMID AND WD.ITEMTYPE='Primary' AND I.ITEMMASTERID='" + itemid + "' )");
                string powe = datatrans.GetDataString("Select EBCONSPERHR from wcbasic where wcbasicid='" + wcid + "'");
                var result = new { tar = tar , powe = powe };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetapwcDetail(string sieveid, string wcid)
        {
            try
            {
                string tar = datatrans.GetDataString("Select Sum(tar) Tar from (SELECT SUM(WD.PRATE*22) TAR FROM WCSPRODDETAIL WD WHERE  WD.WCBASICID='" + wcid + "' AND WD.ITEMTYPE='Primary' AND WD.SIEVEID='" + sieveid + "' )");
                string powe = datatrans.GetDataString("Select EBCONSPERHR from wcbasic where wcbasicid='" + wcid + "'");
                var result = new { tar = tar, powe = powe };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetpastewcDetail(string itemid, string wcid,string mnth)
        {
            try
            {
                string tar = datatrans.GetDataString("Select Sum(tar) Tar from (SELECT SUM(WD.PRATE*22) TAR FROM WCBASIC W,WCPRODDETAIL WD,ITEMMASTER I WHERE W.WCBASICID=WD.WCBASICID AND W.WCBASICID='" + wcid + "' AND I.ITEMMASTERID=WD.ITEMID AND WD.ITEMTYPE='Primary' AND I.ITEMMASTERID='" + itemid + "' )");
                string powe = datatrans.GetDataString("Select EBCONSPERHR from wcbasic where wcbasicid='" + wcid + "'");
                string appowe = datatrans.GetDataString("SELECT SUM(P.APPOWKG) ap FROM PARUNDET P WHERE  P.WCBASICID='" + wcid + "' AND P.RUNITEM='" + itemid + "'");
                string pamtoloss= datatrans.GetDataString("SELECT P.MTOLOSSPER LOSS FROM PARUNDET P where P.WCBASICID='" + wcid + "' AND P.RUNITEM='" + itemid + "'");
                string rvdloss= datatrans.GetDataString("Select Sum(D.RVDMTOLOS) Qty from ProdFcBasic B,ProdFcRvd D Where B.PRODFCBASICID=D.PRODFCBASICID And B.IS_ACTIVE='Y' AND B.PLANTYPE='MONTHLY' AND B.MONTH='" + mnth  + "'  And D.RVDRAWMAT='" + itemid + "' Group by D.RVDRAWMAT");
                var result = new { tar = tar, powe = powe , appowe = appowe , pamtoloss= pamtoloss , rvdloss = rvdloss };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetWcPower(string wcid)
        {
            try
            {
                string powe = datatrans.GetDataString("Select EBCONSPERHR from wcbasic where wcbasicid='" + wcid + "'");
                var result = new { powe = powe };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ViewProdFc(string id)
        {
            ProductionForecasting ca = new ProductionForecasting();
         
            List<PFCItem> TData = new List<PFCItem>();
            PFCItem tda = new PFCItem();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            List<PFCPYROItem> TData2 = new List<PFCPYROItem>();
            PFCPYROItem tda2 = new PFCPYROItem();
            List<PFCPOLIItem> TData3 = new List<PFCPOLIItem>();
            PFCPOLIItem tda3 = new PFCPOLIItem();
            List<PFCRVDItem> TData4 = new List<PFCRVDItem>();
            PFCRVDItem tda4 = new PFCRVDItem();
            List<PFCPASTEItem> TData5 = new List<PFCPASTEItem>();
            PFCPASTEItem tda5 = new PFCPASTEItem();
            List<PFCAPPRODItem> TData6 = new List<PFCAPPRODItem>();
            PFCAPPRODItem tda6 = new PFCAPPRODItem();
            List<PFCPACKItem> TData7 = new List<PFCPACKItem>();
            PFCPACKItem tda7 = new PFCPACKItem();
            List<ProdApItem> TData8 = new List<ProdApItem>();
            ProdApItem tda8 = new ProdApItem();

            DataTable dt = new DataTable();

                dt = _ProdForecastServ.GetPFDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.PType = dt.Rows[0]["PLANTYPE"].ToString();
                    ca.ForMonth = dt.Rows[0]["MONTH"].ToString();
                    ca.Ins = dt.Rows[0]["INCDECPER"].ToString();
                    ca.Hd = dt.Rows[0]["HD"].ToString();
                    ca.Fordate = dt.Rows[0]["FINYRPST"].ToString();
                    ca.Enddate = dt.Rows[0]["FINYRPED"].ToString();
                    
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = _ProdForecastServ.GetProdForecastDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new PFCItem();
                        tda.Itemlst = BindItemlst(tda.ItemId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.PType = dt2.Rows[i]["PTYPE"].ToString();
                        tda.PysQty = dt2.Rows[i]["PREVYQTY"].ToString();
                        tda.PtmQty = dt2.Rows[i]["PREVMQTY"].ToString();
                        tda.Fqty = dt2.Rows[i]["PQTY"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = _ProdForecastServ.GetProdForecastDGPasteDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new PFCDGItem();
                        tda1.PItemlst = BindItemlst(tda1.itemid);
                        tda1.itemid = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.target = dt3.Rows[i]["DGTARQTY"].ToString();
                        tda1.min = dt3.Rows[i]["DGMIN"].ToString();
                        tda1.stock = dt3.Rows[i]["DGSTOCK"].ToString();
                        tda1.required = dt3.Rows[i]["REQDG"].ToString();
                        tda1.dgaddit = dt3.Rows[i]["item"].ToString();
                        tda1.reqadditive = dt3.Rows[i]["DGADDITREQ"].ToString();
                        tda1.rawmaterial = dt3.Rows[i]["item1"].ToString();
                        tda1.reqpyro = dt3.Rows[i]["DGREQAP"].ToString();
                        tda1.saveitemid = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();

                dt4 = _ProdForecastServ.GetProdForecastPyroDetail(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new PFCPYROItem();
                        tda2.PYItemlst = BindItemlst(tda2.itemid);
                        tda2.itemid = dt4.Rows[i]["ITEMID"].ToString();

                        tda2.Worklst = BindWorkCenter();
                        tda2.WorkId = dt4.Rows[i]["WCID"].ToString();

                        tda2.CDays = dt4.Rows[i]["WCDAYS"].ToString();
                        tda2.minstock = dt4.Rows[i]["PYMINSTK"].ToString();
                        tda2.pasterej = dt4.Rows[i]["PYALLREJ"].ToString();
                        tda2.GradeChange = dt4.Rows[i]["PYGRCHG"].ToString();
                        tda2.rejqty = dt4.Rows[i]["PYREJQTY"].ToString();
                        tda2.required = dt4.Rows[i]["PYREQQTY"].ToString();
                        tda2.target = dt4.Rows[i]["PYTARQTY"].ToString();
                        tda2.proddays = dt4.Rows[i]["PYPRODCAPD"].ToString();
                        tda2.prodqty = dt4.Rows[i]["PYPRODQTY"].ToString();
                        tda2.rejmat = dt4.Rows[i]["PYRAWREJMAT"].ToString();
                        tda2.rejmatreq = dt4.Rows[i]["PYRAWREJMATPER"].ToString();
                        tda2.balanceqty = dt4.Rows[i]["PREBALQTY"].ToString();
                        tda2.additive = dt4.Rows[i]["item"].ToString();
                        tda2.per = dt4.Rows[i]["PYADDPER"].ToString();
                        tda2.allocadditive = dt4.Rows[i]["ALLOCADD"].ToString();
                        tda2.reqpowder = dt4.Rows[i]["PYREQAP"].ToString();
                        tda2.wstatus = dt4.Rows[i]["WSTATUS"].ToString();
                        tda2.powderrequired = dt4.Rows[i]["POWREQ"].ToString();
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();

                dt5 = _ProdForecastServ.GetProdForecastPolishDetail(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new PFCPOLIItem();

                        tda3.itemid = dt5.Rows[i]["ITEMID"].ToString();

                        tda3.POWorklst = BindWorkCenter();
                        tda3.workid = dt5.Rows[i]["WCID"].ToString();

                        tda3.wcdays = dt5.Rows[i]["PIGWCDAYS"].ToString();
                         
                        tda3.capacity = dt5.Rows[i]["PIGCAP"].ToString();
                        tda3.stock = dt5.Rows[i]["PIGAVAILQTY"].ToString();
                        tda3.minstock = dt5.Rows[i]["PIGMINSTK"].ToString();
                        tda3.required = dt5.Rows[i]["PIGRAWREQ"].ToString();
                        tda3.days = dt5.Rows[i]["PIGPRODD"].ToString();
                        tda3.additive = dt5.Rows[i]["item"].ToString();
                        tda3.add = dt5.Rows[i]["PIGADDPER"].ToString();
                        tda3.rejmat = dt5.Rows[i]["item1"].ToString();
                        tda3.reqper = dt5.Rows[i]["PIGRAWREQPER"].ToString();
                        tda3.rvdqty = dt5.Rows[i]["PIGREQQTY"].ToString();
                        tda3.pyropowder = dt5.Rows[i]["PIGRAWREQPY"].ToString();
                        tda3.pyroqty = dt5.Rows[i]["PIGRAWMATPY"].ToString();
                        tda3.powderrequired = dt5.Rows[i]["PIGPOWREQ"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }
            DataTable dt6 = new DataTable();

            dt6 = _ProdForecastServ.GetProdForecastRVDDetail(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new PFCRVDItem();

                    tda4.workid = dt6.Rows[i]["WCID"].ToString();
                    tda4.itemid = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.rawmat = dt6.Rows[i]["item"].ToString();
                    tda4.prodqty = dt6.Rows[i]["RVDPRODQTY"].ToString();
                    tda4.cons = dt6.Rows[i]["RVDCONS"].ToString();
                    tda4.consqty = dt6.Rows[i]["RVDCONSQTY"].ToString();
                    tda4.powderrequired = dt6.Rows[i]["RVDPOWREQ"].ToString();
                    tda4.wcdays = dt6.Rows[i]["RVDWCDAYS"].ToString();
                    tda4.mto = dt6.Rows[i]["RVDMTOREC"].ToString();
                    tda4.mtoloss = dt6.Rows[i]["RVDMTOLOS"].ToString();
                     

                   
                    tda4.ID = id;
                    TData4.Add(tda4);
                }

            }

            DataTable dt7 = new DataTable();

            dt7 = _ProdForecastServ.GetProdForecastPasteDetail(id);
            if (dt7.Rows.Count > 0)
            {
                for (int i = 0; i < dt7.Rows.Count; i++)
                {
                    tda5 = new PFCPASTEItem();

                    tda5.WorkId = dt7.Rows[i]["WCID"].ToString();
                    tda5.itemid = dt7.Rows[i]["ITEMID"].ToString();
                    tda5.charge = dt7.Rows[i]["PANOOFCHG"].ToString();
                    tda5.allocadditive = dt7.Rows[i]["PAALLADDIT"].ToString();
                    tda5.target = dt7.Rows[i]["PATARGQTY"].ToString();
                    tda5.stock = dt7.Rows[i]["PASTK"].ToString();
                    tda5.minstock = dt7.Rows[i]["PAMINSTK"].ToString();
                    tda5.production = dt7.Rows[i]["PAPROD"].ToString();
                    tda5.appowder = dt7.Rows[i]["PAAPPOW"].ToString();
                    tda5.balance = dt7.Rows[i]["PABALQTY"].ToString();
                    tda5.rvdloss = dt7.Rows[i]["RVDLOSTQTY"].ToString();
                    tda5.missmto = dt7.Rows[i]["MIXINGMTO"].ToString();
                    tda5.coarse = dt7.Rows[i]["PACOACONS"].ToString();
                    tda5.addcost = dt7.Rows[i]["item"].ToString();
                    tda5.powerrequired = dt7.Rows[i]["PAPOWREQ"].ToString();
                    
                     


                    tda5.ID = id;
                    TData5.Add(tda5);
                }

            }
            DataTable dt8 = new DataTable();

            dt8 = _ProdForecastServ.GetAPSDeatils(id);
            if (dt8.Rows.Count > 0)
            {
                for (int i = 0; i < dt8.Rows.Count; i++)
                {
                    tda8 = new ProdApItem();
                    tda8.reqappowder = dt8.Rows[i]["APPOWREQ"].ToString();
                    tda8.reqqty = dt8.Rows[i]["APREQ"].ToString();
                    tda8.avlstk = dt8.Rows[i]["APAVAILSTK"].ToString();
                    tda8.ministk = dt8.Rows[i]["APMINSTK"].ToString();
                    tda8.ordqty = dt8.Rows[i]["APREQQTY"].ToString();
                    tda8.itemid = dt8.Rows[i]["ITEMID"].ToString();
                    tda8.startvalue = dt8.Rows[i]["STARTVALUE"].ToString();
                    tda8.endvalue = dt8.Rows[i]["ENDVALUE"].ToString();
                    TData8.Add(tda8);
                }
                

            }
            DataTable dt9 = new DataTable();

            dt9 = _ProdForecastServ.GetAPReqDeatils(id);
            if (dt9.Rows.Count > 0)
            {

                ca.appyro = dt9.Rows[0]["REQAPPOWPY"].ToString();
                ca.appaste = dt9.Rows[0]["REQAPPOWPA"].ToString();
                ca.apfg = dt9.Rows[0]["REQAPPOWAP"].ToString();
                ca.reqappow = dt9.Rows[0]["REQAPPOW"].ToString();
                ca.apstk = dt9.Rows[0]["APPOWSTOCK"].ToString();
                ca.coarse = dt9.Rows[0]["MELTCOAW"].ToString();
                ca.power = dt9.Rows[0]["REQPOWQTY"].ToString();
                ca.ministk = dt9.Rows[0]["APPOWMIN"].ToString();




            }
            DataTable dt10 = new DataTable();

            dt10 = _ProdForecastServ.GetProdForecastAPProdDetail(id);
            if (dt10.Rows.Count > 0)
            {
                for (int i = 0; i < dt10.Rows.Count; i++)
                {
                    tda6 = new PFCAPPRODItem();

                    tda6.WorkId = dt10.Rows[i]["WCID"].ToString();
                    tda6.wdays = dt10.Rows[i]["APWCDAYS"].ToString();
                    tda6.capacity = dt10.Rows[i]["APPRODCAP"].ToString();
                    tda6.proddays = dt10.Rows[i]["APPRODD"].ToString();
                    tda6.production = dt10.Rows[i]["APPRODQTY"].ToString();
                    tda6.fuelreq = dt10.Rows[i]["FUELREQ"].ToString();
                    tda6.ingotreq = dt10.Rows[i]["RMREQ"].ToString();
                    tda6.powerrequired = dt10.Rows[i]["APPPOWREQ"].ToString();
                    tda6.target = dt10.Rows[i]["APTARPROD"].ToString();
                 



                    tda6.ID = id;
                    TData6.Add(tda6);
                }

            }
            DataTable dt11 = new DataTable();

            dt11 = _ProdForecastServ.GetProdForecastPackDetail(id);
            if (dt11.Rows.Count > 0)
            {
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    tda7 = new PFCPACKItem();

                    tda7.party = dt11.Rows[i]["PARTYID"].ToString();
                    tda7.targetitem = dt11.Rows[i]["ITEMID"].ToString();
                    tda7.packmat = dt11.Rows[i]["item"].ToString();
                    tda7.rawmat = dt11.Rows[i]["item1"].ToString();
                    tda7.targetqty = dt11.Rows[i]["TARQTY"].ToString();
                    tda7.packqty = dt11.Rows[i]["PACKQTY"].ToString();
                    tda7.reqmat = dt11.Rows[i]["PACKMATREQ"].ToString();
                  




                    tda7.ID = id;
                    TData7.Add(tda7);
                }

            }
            ca.PFCILst = TData;
            ca.PFCDGILst = TData1;
            ca.PFCPYROILst = TData2;
            ca.PFCPOLILst = TData3;
            ca.PFCRVDLst = TData4;
            ca.PFCPASTELst = TData5;
            ca.PFCAPPRODLst = TData6;
            ca.PFCPACKLst = TData7;
            ca.Aplst = TData8;
            return View(ca);
        }
        public IActionResult GenProdShe(string id)
        {
            ProductionForecasting ca = new ProductionForecasting();

            List<PFCItem> TData = new List<PFCItem>();
            PFCItem tda = new PFCItem();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            List<PFCPYROItem> TData2 = new List<PFCPYROItem>();
            PFCPYROItem tda2 = new PFCPYROItem();
            List<PFCPOLIItem> TData3 = new List<PFCPOLIItem>();
            PFCPOLIItem tda3 = new PFCPOLIItem();
            List<PFCRVDItem> TData4 = new List<PFCRVDItem>();
            PFCRVDItem tda4 = new PFCRVDItem();
            List<PFCPASTEItem> TData5 = new List<PFCPASTEItem>();
            PFCPASTEItem tda5 = new PFCPASTEItem();
            List<PFCAPPRODItem> TData6 = new List<PFCAPPRODItem>();
            PFCAPPRODItem tda6 = new PFCAPPRODItem();
            List<PFCPACKItem> TData7 = new List<PFCPACKItem>();
            PFCPACKItem tda7 = new PFCPACKItem();

            DataTable dt = new DataTable();

            dt = _ProdForecastServ.GetPFDeatils(id);
            if (dt.Rows.Count > 0)
            {

                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.PType = dt.Rows[0]["PLANTYPE"].ToString();
                ca.ForMonth = dt.Rows[0]["MONTH"].ToString();
                ca.Ins = dt.Rows[0]["INCDECPER"].ToString();
                ca.Hd = dt.Rows[0]["HD"].ToString();
                ca.Fordate = dt.Rows[0]["FINYRPST"].ToString();
                ca.Enddate = dt.Rows[0]["FINYRPED"].ToString();

                ca.ID = id;

            }
            DataTable dt2 = new DataTable();

            dt2 = _ProdForecastServ.GetProdForecastDetail(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PFCItem();
                    tda.Itemlst = BindItemlst(tda.ItemId);
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    tda.PType = dt2.Rows[i]["PTYPE"].ToString();
                    tda.PysQty = dt2.Rows[i]["PREVYQTY"].ToString();
                    tda.PtmQty = dt2.Rows[i]["PREVMQTY"].ToString();
                    tda.Fqty = dt2.Rows[i]["PQTY"].ToString();
                    tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            DataTable dt3 = new DataTable();

            dt3 = _ProdForecastServ.GetProdForecastDGPasteDetail(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new PFCDGItem();
                    tda1.PItemlst = BindItemlst(tda1.itemid);
                    tda1.itemid = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.target = dt3.Rows[i]["DGTARQTY"].ToString();
                    tda1.min = dt3.Rows[i]["DGMIN"].ToString();
                    tda1.stock = dt3.Rows[i]["DGSTOCK"].ToString();
                    tda1.required = dt3.Rows[i]["REQDG"].ToString();
                    tda1.dgaddit = dt3.Rows[i]["item"].ToString();
                    tda1.reqadditive = dt3.Rows[i]["DGADDITREQ"].ToString();
                    tda1.rawmaterial = dt3.Rows[i]["item1"].ToString();
                    tda1.reqpyro = dt3.Rows[i]["DGREQAP"].ToString();
                    tda1.saveitemid = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.ID = id;
                    TData1.Add(tda1);
                }

            }
            DataTable dt4 = new DataTable();

            dt4 = _ProdForecastServ.GetProdForecastPyroDetail(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new PFCPYROItem();
                    tda2.PYItemlst = BindItemlst(tda2.itemid);
                    tda2.itemid = dt4.Rows[i]["ITEMID"].ToString();
                    tda2.SchYN = dt4.Rows[i]["SCHEDULE"].ToString();


                    tda2.status = dt4.Rows[i]["STATUS"].ToString();


                    tda2.Worklst = BindWorkCenter();
                    tda2.WorkId = dt4.Rows[i]["WCID"].ToString();
                    tda2.detid = dt4.Rows[i]["PRODFCPYID"].ToString();

                    tda2.CDays = dt4.Rows[i]["WCDAYS"].ToString();
                    tda2.minstock = dt4.Rows[i]["PYMINSTK"].ToString();
                    tda2.pasterej = dt4.Rows[i]["PYALLREJ"].ToString();
                    tda2.GradeChange = dt4.Rows[i]["PYGRCHG"].ToString();
                    tda2.rejqty = dt4.Rows[i]["PYREJQTY"].ToString();
                    tda2.required = dt4.Rows[i]["PYREQQTY"].ToString();
                    tda2.target = dt4.Rows[i]["PYTARQTY"].ToString();
                    tda2.proddays = dt4.Rows[i]["PYPRODCAPD"].ToString();
                    tda2.prodqty = dt4.Rows[i]["PYPRODQTY"].ToString();
                    tda2.rejmat = dt4.Rows[i]["PYRAWREJMAT"].ToString();
                    tda2.rejmatreq = dt4.Rows[i]["PYRAWREJMATPER"].ToString();
                    tda2.balanceqty = dt4.Rows[i]["PREBALQTY"].ToString();
                    tda2.additive = dt4.Rows[i]["item"].ToString();
                    tda2.per = dt4.Rows[i]["PYADDPER"].ToString();
                    tda2.allocadditive = dt4.Rows[i]["ALLOCADD"].ToString();
                    tda2.reqpowder = dt4.Rows[i]["PYREQAP"].ToString();
                    tda2.wstatus = dt4.Rows[i]["WSTATUS"].ToString();
                    tda2.powderrequired = dt4.Rows[i]["POWREQ"].ToString();
                    tda2.ID = id;
                    TData2.Add(tda2);
                }

            }
            DataTable dt5 = new DataTable();

            dt5 = _ProdForecastServ.GetProdForecastPolishDetail(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new PFCPOLIItem();

                    tda3.itemid = dt5.Rows[i]["ITEMID"].ToString();

                    tda3.POWorklst = BindWorkCenter();
                    tda3.workid = dt5.Rows[i]["WCID"].ToString();
                    tda3.SchYN = dt5.Rows[i]["SCHEDULE"].ToString();

                    tda3.status = dt5.Rows[i]["STATUS"].ToString();


                    tda3.wcdays = dt5.Rows[i]["PIGWCDAYS"].ToString();
                    tda3.detid = dt5.Rows[i]["PRODFCPIGID"].ToString();

                    tda3.capacity = dt5.Rows[i]["PIGCAP"].ToString();
                    tda3.stock = dt5.Rows[i]["PIGAVAILQTY"].ToString();
                    tda3.minstock = dt5.Rows[i]["PIGMINSTK"].ToString();
                    tda3.required = dt5.Rows[i]["PIGRAWREQ"].ToString();
                    tda3.days = dt5.Rows[i]["PIGPRODD"].ToString();
                    tda3.additive = dt5.Rows[i]["item"].ToString();
                    tda3.add = dt5.Rows[i]["PIGADDPER"].ToString();
                    tda3.rejmat = dt5.Rows[i]["item1"].ToString();
                    tda3.reqper = dt5.Rows[i]["PIGRAWREQPER"].ToString();
                    tda3.rvdqty = dt5.Rows[i]["PIGREQQTY"].ToString();
                    tda3.pyropowder = dt5.Rows[i]["PIGRAWMATPY"].ToString();
                    tda3.pyroqty = dt5.Rows[i]["PIGRAWREQPY"].ToString();
                    tda3.powderrequired = dt5.Rows[i]["PIGPOWREQ"].ToString();
                    tda3.ID = id;
                    TData3.Add(tda3);
                }

            }
            DataTable dt6 = new DataTable();

            dt6 = _ProdForecastServ.GetProdForecastRVDDetail(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new PFCRVDItem();

                    tda4.workid = dt6.Rows[i]["WCID"].ToString();
                    tda4.detid = dt6.Rows[i]["PRODFCRVDID"].ToString();
                    tda4.SchYN = dt6.Rows[i]["SCHEDULE"].ToString();
                    tda4.itemid = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.rawmat = dt6.Rows[i]["item"].ToString();
                    tda4.prodqty = dt6.Rows[i]["RVDPRODQTY"].ToString();
                    tda4.cons = dt6.Rows[i]["RVDCONS"].ToString();
                    tda4.consqty = dt6.Rows[i]["RVDCONSQTY"].ToString();
                    tda4.powderrequired = dt6.Rows[i]["RVDPOWREQ"].ToString();
                    tda4.wcdays = dt6.Rows[i]["RVDWCDAYS"].ToString();
                    tda4.mto = dt6.Rows[i]["RVDMTOREC"].ToString();
                    tda4.mtoloss = dt6.Rows[i]["RVDMTOLOS"].ToString();
                    tda4.qty = dt6.Rows[i]["RVDRAWQTY"].ToString();
                    tda4.days = dt6.Rows[i]["RVDPRODD"].ToString();

                    tda4.status = dt6.Rows[i]["STATUS"].ToString();




                    tda4.ID = id;
                    TData4.Add(tda4);
                }

            }

            DataTable dt7 = new DataTable();

            dt7 = _ProdForecastServ.GetProdForecastPasteDetail(id);
            if (dt7.Rows.Count > 0)
            {
                for (int i = 0; i < dt7.Rows.Count; i++)
                {
                    tda5 = new PFCPASTEItem();

                    tda5.WorkId = dt7.Rows[i]["WCID"].ToString();
                    tda5.itemid = dt7.Rows[i]["ITEMID"].ToString();
                    tda5.detid = dt7.Rows[i]["PRODFCPAID"].ToString();
                    tda5.charge = dt7.Rows[i]["PANOOFCHG"].ToString();
                    tda5.SchYN = dt7.Rows[i]["SCHEDULE"].ToString();
                    tda5.allocadditive = dt7.Rows[i]["PAALLADDIT"].ToString();
                    tda5.target = dt7.Rows[i]["PATARGQTY"].ToString();
                    tda5.stock = dt7.Rows[i]["PASTK"].ToString();
                    tda5.minstock = dt7.Rows[i]["PAMINSTK"].ToString();
                    tda5.production = dt7.Rows[i]["PAPROD"].ToString();
                    tda5.appowder = dt7.Rows[i]["PAAPPOW"].ToString();
                    tda5.balance = dt7.Rows[i]["PABALQTY"].ToString();
                    tda5.rvdloss = dt7.Rows[i]["RVDLOSTQTY"].ToString();
                    tda5.missmto = dt7.Rows[i]["MIXINGMTO"].ToString();
                    tda5.coarse = dt7.Rows[i]["PACOACONS"].ToString();
                    tda5.additive = dt7.Rows[i]["item"].ToString();
                    tda5.powerrequired = dt7.Rows[i]["PAPOWREQ"].ToString();
                    tda5.proddays = dt7.Rows[i]["PAPRODD"].ToString();


                    tda5.status = dt7.Rows[i]["STATUS"].ToString();





                    tda5.ID = id;
                    TData5.Add(tda5);
                }

            }
            //DataTable dt8 = new DataTable();

            //dt8 = _ProdForecastServ.GetAPSDeatils(id);
            //if (dt8.Rows.Count > 0)
            //{

            //    ca.apspowder = dt8.Rows[0]["APPOWREQ"].ToString();
            //    ca.reqqty = dt8.Rows[0]["APREQ"].ToString();
            //    ca.avlstk = dt8.Rows[0]["APAVAILSTK"].ToString();
            //    ca.ministk = dt8.Rows[0]["APMINSTK"].ToString();
            //    ca.reqappowder = dt8.Rows[0]["APREQPOW"].ToString();




            //}
            DataTable dt9 = new DataTable();

            dt9 = _ProdForecastServ.GetAPReqDeatils(id);
            if (dt9.Rows.Count > 0)
            {

                ca.appyro = dt9.Rows[0]["REQAPPOWPY"].ToString();
                ca.appaste = dt9.Rows[0]["REQAPPOWPA"].ToString();
                ca.apfg = dt9.Rows[0]["REQAPPOWAP"].ToString();
                ca.reqappow = dt9.Rows[0]["REQAPPOW"].ToString();
                ca.apstk = dt9.Rows[0]["APPOWSTOCK"].ToString();
                ca.coarse = dt9.Rows[0]["MELTCOAW"].ToString();
                ca.power = dt9.Rows[0]["REQPOWQTY"].ToString();
                ca.ministk = dt9.Rows[0]["APPOWMIN"].ToString();




            }
            DataTable dt10 = new DataTable();

            dt10 = _ProdForecastServ.GetProdForecastAPProdDetail(id);
            if (dt10.Rows.Count > 0)
            {
                for (int i = 0; i < dt10.Rows.Count; i++)
                {
                    tda6 = new PFCAPPRODItem();

                    tda6.WorkId = dt10.Rows[i]["WCID"].ToString();
                    tda6.wdays = dt10.Rows[i]["APWCDAYS"].ToString();
                    tda6.capacity = dt10.Rows[i]["APPRODCAP"].ToString();
                    tda6.detid = dt10.Rows[i]["PRODFCAPPID"].ToString();
                    tda6.proddays = dt10.Rows[i]["APPRODD"].ToString();
                    tda6.production = dt10.Rows[i]["APPRODQTY"].ToString();
                    tda6.fuelreq = dt10.Rows[i]["FUELREQ"].ToString();
                    tda6.ingotreq = dt10.Rows[i]["RMREQ"].ToString();
                    tda6.powerrequired = dt10.Rows[i]["APPPOWREQ"].ToString();
                    tda6.target = dt10.Rows[i]["APTARPROD"].ToString();




                    tda6.ID = id;
                    TData6.Add(tda6);
                }

            }
            DataTable dt11 = new DataTable();

            dt11 = _ProdForecastServ.GetProdForecastPackDetail(id);
            if (dt11.Rows.Count > 0)
            {
                for (int i = 0; i < dt11.Rows.Count; i++)
                {
                    tda7 = new PFCPACKItem();

                    tda7.party = dt11.Rows[i]["PARTYID"].ToString();
                    tda7.targetitem = dt11.Rows[i]["ITEMID"].ToString();
                    tda7.packmat = dt11.Rows[i]["item"].ToString();
                    tda7.rawmat = dt11.Rows[i]["item1"].ToString();
                    tda7.targetqty = dt11.Rows[i]["TARQTY"].ToString();
                    tda7.packqty = dt11.Rows[i]["PACKQTY"].ToString();
                    tda7.reqmat = dt11.Rows[i]["PACKMATREQ"].ToString();





                    tda7.ID = id;
                    TData7.Add(tda7);
                }

            }
            ca.PFCILst = TData;
            ca.PFCDGILst = TData1;
            ca.PFCPYROILst = TData2;
            ca.PFCPOLILst = TData3;
            ca.PFCRVDLst = TData4;
            ca.PFCPASTELst = TData5;
            ca.PFCAPPRODLst = TData6;
            ca.PFCPACKLst = TData7;
            return View(ca);
        }
    }
}
