using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using Arasan.Services;

using Arasan.Interface;
using Arasan.Services;
namespace Arasan.Controllers
{
    public class ActPlanningController : Controller
    {
        IActPlanning actPlanning;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ActPlanningController(IActPlanning _actPlanning, IConfiguration _configuratio)
        {
            actPlanning = _actPlanning;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        [HttpPost]

        public ActionResult ActPlanning(ActPlanning Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = actPlanning.ActPlanningCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Activity Planning Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Activity Planning Updated Successfully...!";
                    }
                    return RedirectToAction("ListActPlanning");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Activity Planning";
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
        public IActionResult ActPlanning(string id)
        {
            ActPlanning ca = new ActPlanning();

            ca.McTolst = BindMachine();
            ca.ActTyplst = BindActType();
            ca.DepTyplst = BindDepTyp();
            ca.MaiTyplst = BindMaiTyp();
            ca.PBlst = BindPB();
            ca.JTyplst = BindJobTyp();
            ca.AlTolst = BindAlloted();
            ca.BrDowlst = BindBrDown();


            DataTable dtv = datatrans.GetSequence("posch");

            if (dtv.Rows.Count > 0)
            {
                ca.Docno = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            ca.DocDate=DateTime.Now.ToString("dd-MMM-yyyy");
            Promst pr = new Promst();
            List<Promst> TData = new List<Promst>();

            Wcid wc = new Wcid();
            List<Wcid> TData1 = new List<Wcid>();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    pr = new Promst();
                    pr.ulst = BindItem();
                    pr.Isvalid = "Y";
                    TData.Add(pr);
                }
                for (int i = 0; i < 1; i++)
                {
                    wc = new Wcid();
                    wc.Isvalid1 = "Y";
                    TData1.Add(wc);
                }
            }

            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = actPlanning.GetEditActPlan(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Docno = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.MtId = dt.Rows[0]["MACNO"].ToString();
                    ca.McTolst = BindMachine();
                    ca.AcTyp = dt.Rows[0]["TYPE"].ToString();
                    ca.ActTyplst = BindActType();
                    ca.AcDat = dt.Rows[0]["ACTDATE"].ToString();
                    ca.AcFre = dt.Rows[0]["ACTFRE"].ToString();
                    ca.DeTyp = dt.Rows[0]["DTYPE"].ToString();
                    ca.DepTyplst = BindDepTyp();
                    ca.MaTyp = dt.Rows[0]["MTYPE"].ToString();
                    ca.MaiTyplst = BindMaiTyp();
                    ca.FrDat = dt.Rows[0]["FROMDATE"].ToString();
                    ca.ToDat = dt.Rows[0]["TODATE"].ToString();
                    ca.FrTim = dt.Rows[0]["FROMTIME"].ToString();
                    ca.ToTim = dt.Rows[0]["TOTIME"].ToString();
                    ca.PB = dt.Rows[0]["PREORBRE"].ToString();
                    ca.PBlst = BindPB();
                    ca.JTyp = dt.Rows[0]["JOBTYPE"].ToString();
                    ca.JTyplst = BindJobTyp();
                    ca.AlTo = dt.Rows[0]["ALLOTEDTO"].ToString();
                    ca.AlTolst = BindAlloted();
                    ca.BrDow = dt.Rows[0]["BDREF"].ToString();
                    ca.BrDowlst = BindBrDown();
                    ca.ADes = dt.Rows[0]["ACTDESC"].ToString();          
                    ca.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = actPlanning.GetEditTool(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    pr = new Promst();

                    pr.tools = dt2.Rows[i]["TOOLS"].ToString();
                    pr.ulst = BindItem();
                    pr.Isvalid = "Y";
                    TData.Add(pr);
                }
            }

            DataTable dt3 = new DataTable();
            dt3 = actPlanning.GetEditReason(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    wc = new Wcid();

                    wc.date = dt3.Rows[i]["RDATE"].ToString();
                    wc.rea = dt3.Rows[i]["REASON"].ToString();
                    wc.Isvalid = "Y";
                    TData1.Add(wc);
                }
            }

            ca.Prolst = TData;
            ca.wclst = TData1;
            return View(ca);
        }

        public List<SelectListItem> BindMachine()
        {
            try
            {
                DataTable dtDesg = actPlanning.GetMachine();
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

        public List<SelectListItem> BindActType()
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

        public List<SelectListItem> BindDepTyp()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "SELF", Value = "INTERNAL" });
                lstdesg.Add(new SelectListItem() { Text = "MECHANICAL", Value = "MECHANICAL" });
                lstdesg.Add(new SelectListItem() { Text = "ELECTRICAL", Value = "ELECTRICAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindMaiTyp()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MECHANICAL", Value = "MECHANICAL" });
                lstdesg.Add(new SelectListItem() { Text = "ELECTRICAL", Value = "ELECTRICAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindPB()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PREVENTIVE", Value = "PREVENTIVE" });
                lstdesg.Add(new SelectListItem() { Text = "BREAKDOWN", Value = "BREAKDOWN" });
                lstdesg.Add(new SelectListItem() { Text = "GENERAL", Value = "GENERAL" });
                lstdesg.Add(new SelectListItem() { Text = "ROUTINE", Value = "ROUTINE" });
                lstdesg.Add(new SelectListItem() { Text = "AMC", Value = "AMC" });
                lstdesg.Add(new SelectListItem() { Text = "ERACTION WORK", Value = "ERACTION WORK" });
                lstdesg.Add(new SelectListItem() { Text = "PREDICTION MAINTENANCE", Value = "PREDICTION MAINTENANCE" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindJobTyp()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "NONE" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindAlloted()
        {
            try
            {
                DataTable dtDesg = actPlanning.GetAlloted();
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

        public List<SelectListItem> BindBrDown()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "NONE" });
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
                DataTable dtDesg = actPlanning.GetItem();
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

        public JsonResult GetItemJSON()
        {
            Promst model = new Promst();
            return Json(BindItem());

        }
        public JsonResult GetItemJSON1()
        {
            Wcid model = new Wcid();
            return Json(model);

        }

        public IActionResult ListActPlanning()
        {
            return View();
        }

        public ActionResult MyListActPlanninggrid(string strStatus)
        {
            List<ListActPlanning> Reg = new List<ListActPlanning>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = actPlanning.GetAllActPlanning(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=ActPlanning?id=" + dtUsers.Rows[i]["SCHMAINBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SCHMAINBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["SCHMAINBASICID"].ToString() + "";
                }

                Reg.Add(new ListActPlanning
                {
                    dnum = dtUsers.Rows[i]["DOCID"].ToString(),
                    ddate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    mtid = dtUsers.Rows[i]["MCODE"].ToString(),
                    actyp = dtUsers.Rows[i]["TYPE"].ToString(),
                    editrow = EditRow,
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

            string flag = actPlanning.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListActPlanning");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListActPlanning");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = actPlanning.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListActPlanning");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListActPlanning");
            }
        }

    }
}
