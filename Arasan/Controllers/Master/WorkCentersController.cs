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
            ca.Loc = BindLocation();
            ca.QCLst = BindLocation();
            ca.WIPLst = BindLocation();
            ca.CONLst = BindLocation();
            ca.Drumlst = BindLocation(); 
            ca.Itemlst = BindItemlst("");
            ca.ConItemlst = BindItemlst("");
            ca.Suplst = BindSupplier();
            ca.Typelst = BindType();
            List<WorkCentersDetail> TData = new List<WorkCentersDetail>();
            WorkCentersDetail tda = new WorkCentersDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new WorkCentersDetail();

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
        public IActionResult ListWorkCenters(string status)
        {
            IEnumerable<WorkCenters> cmp = WorkCentersService.GetAllWorkCenters(status);
            return View(cmp);
        }
        public JsonResult GetItemGrpJSON()
        {
            //CIItem model = new CIItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst(""));
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

        public ActionResult DeleteMR(string tag, int id)
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
    }
}
