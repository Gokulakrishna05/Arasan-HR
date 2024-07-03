using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class WorkCentersController : Controller
    {
        IWorkCentersService WorkCentersService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public WorkCentersController(IWorkCentersService _WorkCentersService, IConfiguration _configuratio)
        {
            WorkCentersService = _WorkCentersService;

            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WorkCenters(string id)
        {
            WorkCenters ca = new WorkCenters();

            ca.createby = Request.Cookies["UserId"];
            ca.Loc = BindLocation();
            ca.QCLst = BindQCLocation();
            ca.WIPLst = BindLocation();
            ca.CONLst = BindLocations();
            ca.Drumlst = BindLocation();
            ca.Contlst = BindContType();
            ca.Itemlst = BindItemlst();
            ca.ConItemlst = BindItemlst("");
            ca.Suplst = BindSupplier();
            ca.Typelst = BindType();
            ca.Bunker = "NO";
            ca.Mill = "NO";
            ca.ProcLot = "NO";
            ca.ProdSch = "NO";
            ca.Production = "NO";
            ca.Energy = "NO";
            List<WorkCentersDetail> TData = new List<WorkCentersDetail>();
            WorkCentersDetail tda = new WorkCentersDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new WorkCentersDetail();
                    tda.mlst = BindMeachine();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();

                dt = WorkCentersService.GetWorkCenters(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Wid = dt.Rows[0]["WCID"].ToString();
                    ca.WType = dt.Rows[0]["WCTYPE"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                    ca.QcLoc = dt.Rows[0]["QCLOCATION"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.WipItemid = dt.Rows[0]["WIPITEMID"].ToString();
                    ca.WipLocid = dt.Rows[0]["WIPLOCID"].ToString();
                    ca.ConvItem = dt.Rows[0]["CONVITEMID"].ToString();
                    ca.ConvLoc = dt.Rows[0]["CONVLOCID"].ToString();
                    ca.Bunker = dt.Rows[0]["BUNKERYN"].ToString();
                    ca.Opbbl = dt.Rows[0]["OPBBAL"].ToString();
                    ca.Mill = dt.Rows[0]["MLYN"].ToString();
                    ca.Opmlbal = dt.Rows[0]["OPMLBAL"].ToString();
                    ca.ProcLot = dt.Rows[0]["PROCLOTYN"].ToString();
                    ca.Cap = dt.Rows[0]["CAPACITY"].ToString();
                    ca.ProdSch = dt.Rows[0]["PRODSCHYN"].ToString();
                    ca.Uttl = dt.Rows[0]["PRODSCHYN"].ToString();
                    ca.Production = dt.Rows[0]["PRODYN"].ToString();
                    ca.DrumLoc = dt.Rows[0]["DRUMILOCATION"].ToString();
                    ca.Energy = dt.Rows[0]["ENRMETF"].ToString();
                    ca.Man = dt.Rows[0]["MANREQ"].ToString();
                    ca.Cost = dt.Rows[0]["COST"].ToString();
                    ca.Unit = dt.Rows[0]["COSTUNIT"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                    ca.rloc = dt.Rows[0]["RLOCATION"].ToString();
                    ca.rjloc = dt.Rows[0]["REJLOCATION"].ToString();
                    ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = WorkCentersService.GetWorkCentersDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new WorkCentersDetail();

                        tda.MId = dt2.Rows[i]["MACHINEID"].ToString();
                        tda.MCost = dt2.Rows[i]["MCOST"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
            }
            ca.WorkCenterlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult WorkCenters(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.WorkCentersCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "WorkCenters Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "WorkCenters Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
        public IActionResult ListWorkCenters()
        {
            return View();
        }
        public JsonResult GetItemGrpJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindMeachine());
        }
        public List<SelectListItem> BindType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "INTERNAL", Value = "INTERNAL" });
                lstdesg.Add(new SelectListItem() { Text = "EXTERNAL", Value = "EXTERNAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = WorkCentersService.GetSupplier();
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
        public List<SelectListItem> BindMeachine()
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
        public List<SelectListItem> BindContType()
        {
            try
            {
                DataTable dtDesg = WorkCentersService.GetContType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CONTTYPE"].ToString(), Value = dtDesg.Rows[i]["CONTRMASTID"].ToString() });
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
                DataTable dtDesg = datatrans.GetData("SELECT ITEMID ,ITEMMASTERID FROM ITEMMASTER UNION SELECT 'None',1 FROM DUAL ORDER BY ITEMMASTERID ASC");
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
        public List<SelectListItem> BindLocations()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT LOCID ,LOCDETAILSID FROM LOCDETAILS UNION SELECT 'None',1 FROM DUAL ORDER BY LOCDETAILSID ASC");
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
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
        public List<SelectListItem> BindQCLocation()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT LOCID,LOCDETAILSID FROM LOCDETAILS WHERE LOCATIONTYPE='QUALITY'");
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
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
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
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT PROCESSMASTID ,PROCESSID FROM PROCESSMAST");
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
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = WorkCentersService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWorkCenters");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWorkCenters");
            }
        }

        public ActionResult Remove(string tag, string id)
        {

            string flag = WorkCentersService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWorkCenters");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWorkCenters");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<WorkCentersgrid> Reg = new List<WorkCentersgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = WorkCentersService.GetAllWorkCenters(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ProductionRate = string.Empty;
                string rejdet = string.Empty;
                string ProdCap = string.Empty;
                string ProdCapday = string.Empty;
                string ApSive = string.Empty;
                string paste = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=WorkCenters?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ProductionRate = "<a href=ProductionRate?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + " ><img src='../Images/edit.png' alt='Edit' /></a>";
                    rejdet = "<a href=Rejdet?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ProdCap ="<a href=ProdCap?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ProdCapday = "<a href=ProdCapPerDay?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ApSive = "<a href=ApSive?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    paste = "<a href=PasteRun?id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["WCBASICID"].ToString() + "";
                }

                
                Reg.Add(new WorkCentersgrid
                {
                    id = dtUsers.Rows[i]["WCBASICID"].ToString(),
                    wid = dtUsers.Rows[i]["WCID"].ToString(),
                    wtype = dtUsers.Rows[i]["WCTYPE"].ToString(),
                    iloc = dtUsers.Rows[i]["LOCID"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,
                    productionrate = ProductionRate,
                    rejdet = rejdet,
                    prodcap = ProdCap,
                    apsive = ApSive,
                    prodcapday = ProdCapday,
                    paste = paste,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindInputtype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Primary", Value = "Primary" });
                lstdesg.Add(new SelectListItem() { Text = "Secondary", Value = "Secondary" });
 

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProductionRate(string id)
        {
            WorkCenters w = new WorkCenters();
            List<ProdRate> TData = new List<ProdRate>();
            ProdRate tda = new ProdRate();
            DataTable dt = new DataTable();
            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                w.ID = id;

            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("SELECT ITEMID,PRATE,ITEMTYPE FROM WCPRODDETAIL WHERE WCBASICID='"+ id +"'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProdRate();
                    tda.itemlst = BindItem();
                    tda.itemid = dt2.Rows[i]["ITEMID"].ToString();
                    tda.inputlst = BindInputtype();
                    tda.inputtype = dt2.Rows[i]["ITEMTYPE"].ToString();
                    tda.outputrate = dt2.Rows[i]["PRATE"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProdRate();
                    tda.itemlst = BindItem();
                    tda.inputlst = BindInputtype();
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
           

            w.ProdRatelst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult ProductionRate(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.ProductionRateCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionRate Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionRate Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ProductionRate";
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
        public JsonResult GetItemJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItem());
        }
        public JsonResult GetItemTypeJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindInputtype());
        }
        public IActionResult Rejdet(string id)
        {
            WorkCenters w = new WorkCenters();
            List<Rejdet> TData = new List<Rejdet>();
            Rejdet tda = new Rejdet();
            DataTable dt = new DataTable();
            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                w.ID = id;

            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("SELECT REJTYPE,REJPER FROM WCREJDETAIL WHERE WCBASICID='" + id + "'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new Rejdet();
                    tda.rejlst = Bindrejtype();
                    tda.rejtype = dt2.Rows[i]["REJTYPE"].ToString();
                    
                    tda.rejection = dt2.Rows[i]["REJPER"].ToString();
                
                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new Rejdet();
                    tda.rejlst = Bindrejtype();
                   
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            w.Rejdetlst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult Rejdet(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.RejdetCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Rejection Details Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Rejection Details Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
        public List<SelectListItem> Bindrejtype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "RECHARGE", Value = "RECHARGE" });
                lstdesg.Add(new SelectListItem() { Text = "GRADE CHANGE", Value = "GRADE CHANGE" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetRejJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindrejtype());
        }

        public IActionResult ProdCap(string id)
        {
            WorkCenters w = new WorkCenters();
            List<ProdCap> TData = new List<ProdCap>();
            ProdCap tda = new ProdCap();
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();

            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                w.ID = id;

            }
            dt2 = datatrans.GetData("SELECT PROCESSID,PITEMID,OUTPUTCAPACITY,CAPUNIQUE FROM WCPRODCAPDETAIL WHERE WCBASICID='" + id + "'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProdCap();
                    tda.itemlst = BindItem();
                    tda.itemid = dt2.Rows[i]["PITEMID"].ToString();
                    tda.prolst = BindProcess();
                    tda.process = dt2.Rows[i]["PROCESSID"].ToString();
                    tda.outputcap = dt2.Rows[i]["OUTPUTCAPACITY"].ToString();

                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProdCap();
                    tda.itemlst = BindItem();
                    tda.prolst = BindProcess();
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            w.ProdCaplst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult ProdCap(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.ProdCapCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Production Capacity Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Production Capacity Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
        public JsonResult GetcapItemJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItem());
        }
        public JsonResult GetprocessJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindProcess());
        }


        public IActionResult ProdCapPerDay(string id)
        {
            WorkCenters w = new WorkCenters();
            List<ProdCapPerDay> TData = new List<ProdCapPerDay>();
            ProdCapPerDay tda = new ProdCapPerDay();
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();

            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();

                w.ID = id;
            }
            dt2 = datatrans.GetData("SELECT CAPITEMID,CAPQTY FROM WCCAPDET WHERE WCBASICID='" + id + "'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProdCapPerDay();
                    tda.itemlst = BindItem();
                    tda.itemid = dt2.Rows[i]["CAPITEMID"].ToString();
                  
                    tda.Qty = dt2.Rows[i]["CAPQTY"].ToString();
                   

                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProdCapPerDay();
                    tda.itemlst = BindItem();
                    
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            w.ProdCapPerDaylst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult ProdCapPerDay(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.ProdCapPerCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Production Capacity Per Day Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Production Capacity Per Day Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
        public JsonResult GetcapperItemJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItem());
        }
        public IActionResult ApSive(string id)
        {
            WorkCenters w = new WorkCenters();
            List<ApSive> TData = new List<ApSive>();
            ApSive tda = new ApSive();
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();

            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                w.ID = id;


            }
            dt2 = datatrans.GetData("SELECT SIEVE,FUELQTY,METTQTY,MINSIEVE FROM WCAPSDETAIL WHERE WCBASICID='" + id + "'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ApSive();
                    tda.sivelst = BindSive();
                    tda.siveid = dt2.Rows[i]["SIEVE"].ToString();

                    tda.fuelqty = dt2.Rows[i]["FUELQTY"].ToString();
                    tda.mettqty = dt2.Rows[i]["METTQTY"].ToString();
                    tda.minsive = dt2.Rows[i]["MINSIEVE"].ToString();


                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ApSive();
                    tda.sivelst = BindSive();

                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            w.ApSivelst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult ApSive(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.ApSiveCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Ap Sieve Set Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Ap Sieve Set Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
        public JsonResult GetSiveJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindSive());
        }
        public List<SelectListItem> BindSive()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("Select SIEVE,SIEVEMASTID from SIEVEMAST");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SIEVE"].ToString(), Value = dtDesg.Rows[i]["SIEVE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult PasteRun(string id)
        {
            WorkCenters w = new WorkCenters();
            List<PasteRun> TData = new List<PasteRun>();
            PasteRun tda = new PasteRun();
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();

            dt = WorkCentersService.GetWorkCenters(id);
            if (dt.Rows.Count > 0)
            {
                w.Wid = dt.Rows[0]["WCID"].ToString();
                w.WType = dt.Rows[0]["WCTYPE"].ToString();
                w.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                w.Iloc = dt.Rows[0]["ILOCATION"].ToString();
                w.ID = id;


            }
            dt2 = datatrans.GetData("SELECT RUNITEM,RUNHRS,MTOLOSSPER,CAKEOP,APPOWKG,NOOFCHGSPDAY FROM PARUNDET WHERE WCBASICID='" + id + "'");
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PasteRun();
                    tda.itemlst = BindItem();
                    tda.itemid = dt2.Rows[i]["RUNITEM"].ToString();

                    tda.runhrs = dt2.Rows[i]["RUNHRS"].ToString();
                    tda.mtoloss = dt2.Rows[i]["MTOLOSSPER"].ToString();
                    tda.cake = dt2.Rows[i]["CAKEOP"].ToString();
                    tda.appowder = dt2.Rows[i]["APPOWKG"].ToString();
                    tda.noofchange = dt2.Rows[i]["NOOFCHGSPDAY"].ToString();


                    tda.ID = id;
                    TData.Add(tda);
                }

            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PasteRun();
                    tda.itemlst = BindItem();

                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }


            w.PasteRunlst = TData;
            return View(w);
        }
        [HttpPost]
        public ActionResult PasteRun(WorkCenters Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = WorkCentersService.PasteRunCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Paste Run Hr Detail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Paste Run Hr Detail Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkCenters");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WorkCenters";
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
         
    }
}
