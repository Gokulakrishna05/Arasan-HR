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
    public class ActivitiesCOController : Controller
    {
        IActivitiesCO activitiesCO;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ActivitiesCOController(IActivitiesCO _activitiesCO, IConfiguration _configuratio)
        {
            activitiesCO = _activitiesCO;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ActivitiesCO(string id)
        {
            ActivitiesCO ca = new ActivitiesCO();

            ca.Brchlst = BindBranch();
            ca.LocLst = BindLocation();
            ca.Entlst = BindEntryTyp();
            ca.Actilst = BindActivity();
            ca.PBlst = BindPB();
            ca.McTolst = BindMachine();
            ca.DepTyplst = BindDepTyp();
            ca.MaiTyplst = BindMaiTyp();
            ca.MYNlst = BindMrYN();



            DataTable dtv = datatrans.GetSequence("SEntr");
            if (dtv.Rows.Count > 0)
            {
                ca.Docno = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

          

            Cons co = new Cons();
            List<Cons> TData1 = new List<Cons>();

            Emp em = new Emp();
            List<Emp> TData2 = new List<Emp>();

            Ser se = new Ser();
            List<Ser> TData3 = new List<Ser>();

            Chk ch = new Chk();
            List<Chk> TData4 = new List<Chk>();

            if (id == null)
            {
              
                for (int i = 0; i < 1; i++)
                {
                    co = new Cons();
                    co.Itemlst = BindItem();
                    co.Isvalid1 = "Y";
                    TData1.Add(co);
                }
                for (int i = 0; i < 1; i++)
                {
                    em = new Emp();
                    em.EmplIdlst = BindEmplId();
                    em.Isvalid2 = "Y";
                    TData2.Add(em);
                }
                for (int i = 0; i < 1; i++)
                {
                    se = new Ser();
                    se.Paridlst = BindPartyId();
                    se.Isvalid3 = "Y";
                    TData3.Add(se);
                }
                for (int i = 0; i < 1; i++)
                {
                    ch = new Chk();
                    ch.Isvalid4 = "Y";
                    TData4.Add(ch);
                }
            }


            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = activitiesCO.GetEditActivityCo(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Brc = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Brchlst = BindBranch();
                    ca.Loc = dt.Rows[0]["LOCID"].ToString();
                    ca.LocLst = BindLocation();
                    ca.EnTy = dt.Rows[0]["ENTRYTYPE"].ToString();
                    ca.Entlst = BindEntryTyp();
                    ca.Docno = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ActId = dt.Rows[0]["ACTIVITYID"].ToString();
                    ca.Actilst = BindActivity();
                    ca.AcDat = dt.Rows[0]["REFDATE"].ToString();
                    ca.FrTim = dt.Rows[0]["CFROMTIME"].ToString();
                    ca.ToTim = dt.Rows[0]["CTOTIME"].ToString();
                    ca.FrDat = dt.Rows[0]["CDATE"].ToString();
                    ca.FrTimTy = dt.Rows[0]["SFROMTIME"].ToString();
                    ca.CaHrs = dt.Rows[0]["CARRIEDHRS"].ToString();
                    ca.ToDat = dt.Rows[0]["CTODATE"].ToString();
                    ca.ToTimTy = dt.Rows[0]["STOTIME"].ToString();
                    ca.PB = dt.Rows[0]["PREORBER"].ToString();
                    ca.PBlst = BindPB();
                    ca.MtId = dt.Rows[0]["MCCODE"].ToString();
                    ca.McTolst = BindMachine();
                    ca.DeTyp = dt.Rows[0]["DTYPE"].ToString();
                    ca.DepTyplst = BindDepTyp();
                    ca.MaTyp = dt.Rows[0]["MTYPE"].ToString();
                    ca.MaiTyplst = BindMaiTyp();
                    ca.PrePB = dt.Rows[0]["MPREORBRE"].ToString();
                    ca.PBlst = BindPB();
                    ca.SfHr = dt.Rows[0]["MCSFRUNHR"].ToString();
                    ca.MhYN = dt.Rows[0]["RUNHRYN"].ToString();
                    ca.MYNlst = BindMrYN();
                    ca.Plac = dt.Rows[0]["ACTIVITY"].ToString();
                    ca.Acdo = dt.Rows[0]["ACTIVIYDONE"].ToString();

                    ca.Conam = dt.Rows[0]["CAMOUNT"].ToString();
                    ca.Oth = dt.Rows[0]["OTHERAMT"].ToString();
                    ca.Tamo = dt.Rows[0]["TOTALAMT"].ToString();
                    ca.Rem = dt.Rows[0]["REMARKS"].ToString();
                    ca.Camo = dt.Rows[0]["CAPAMT"].ToString();
                    ca.Samo = dt.Rows[0]["SAMOUNT"].ToString();
                    ca.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = activitiesCO.GetEditCons(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    co = new Cons();

                    co.Itm = dt2.Rows[i]["ITEMID"].ToString();
                    co.Itemlst = BindItem();
                    co.Unit = dt2.Rows[i]["UNIT"].ToString();
                    co.Active = dt2.Rows[i]["CONSYN"].ToString();
                    co.Curst = dt2.Rows[i]["CLSTK"].ToString();
                    co.Qty = dt2.Rows[i]["QTY"].ToString();
                    co.Rate = dt2.Rows[i]["RATE"].ToString();   
                    co.Amo = dt2.Rows[i]["AMOUNT"].ToString();
                    co.Isvalid1 = "Y";
                    TData1.Add(co);
                }
            }

            DataTable dt3 = new DataTable();
            dt3 = activitiesCO.GetEditEmp(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    em = new Emp();

                    em.EmplId = dt3.Rows[i]["EMPID"].ToString();
                    em.EmplIdlst = BindEmplId();
                    em.EmpName = dt3.Rows[i]["EMPNAME"].ToString();
                    em.Edes = dt3.Rows[i]["EMPDEPT"].ToString();
                    em.Nhr = dt3.Rows[i]["ENHRS"].ToString();
                    em.Ohr = dt3.Rows[i]["OTHRS"].ToString();
                    em.Whr = dt3.Rows[i]["EMPWHRS"].ToString();
                    em.Ecost = dt3.Rows[i]["EMPCOST"].ToString();
                    em.Isvalid2 = "Y";
                    TData2.Add(em);
                }
            }
            DataTable dt4 = new DataTable();
            dt4 = activitiesCO.GetEditSerdetail(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    se = new Ser();

                    se.Pid = dt4.Rows[i]["PARTYID"].ToString();
                    se.Paridlst = BindPartyId();
                    se.Sedec = dt4.Rows[i]["SERDESC"].ToString();
                    se.SQty = dt4.Rows[i]["SERQTY"].ToString();
                    se.SRate = dt4.Rows[i]["SERRATE"].ToString();
                    se.SAmo = dt4.Rows[i]["SERAMOUNT"].ToString();                 
                    se.Isvalid3 = "Y";
                    TData3.Add(se);
                }
            }

            DataTable dt5 = new DataTable();
            dt5 = activitiesCO.GetEditChkl(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    ch = new Chk();

                    ch.Ser = dt5.Rows[i]["SERVICE"].ToString();
                    ch.Isvalid4 = "Y";
                    TData4.Add(ch);
                }
            }
            ca.ConLst = TData1;
            ca.EmpLst = TData2;
            ca.SerLst = TData3;
            ca.ChkLst = TData4;

            return View(ca);
        }


        [HttpPost]

        public ActionResult ActivitiesCO(ActivitiesCO Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = activitiesCO.ActivitiesCOCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Activity Carried Out Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Activity Carried Out Updated Successfully...!";
                    }
                    return RedirectToAction("ListActivitiesCO");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Activity Carried Out";
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
                DataTable dtDesg = activitiesCO.GetBranch();
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

        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = activitiesCO.GetLocation();
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

        public List<SelectListItem> BindEntryTyp()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CURRENT", Value = "CURRENT" });
                lstdesg.Add(new SelectListItem() { Text = "OLDDATA", Value = "OLDDATA" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindActivity()
        {
            try
            {
                DataTable dtDesg = activitiesCO.GetActivity();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["SCHMAINBASICID"].ToString() });


                }
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

        public List<SelectListItem> BindMachine()
        {
            try
            {
                DataTable dtDesg = activitiesCO.GetMachine();
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
        public List<SelectListItem> BindMrYN()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "YES", Value = "Y" });
                lstdesg.Add(new SelectListItem() { Text = "NO", Value = "N" });
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
                DataTable dtDesg = activitiesCO.GetItem();
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

        public List<SelectListItem> BindEmplId()
        {
            try
            {
                DataTable dtDesg = activitiesCO.GetEmplId();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindPartyId()
        {
            try
            {
                DataTable dtDesg = activitiesCO.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetActDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string adat = "";
                string frti = "";
                string toti = "";
                string des = "";

                dt = datatrans.GetData("SELECT to_char(ACTDATE,'dd-MON-yyyy')ACTDATE,FROMTIME,TOTIME,ACTDESC FROM SCHMAINBASIC WHERE SCHMAINBASICID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    adat = dt.Rows[0]["ACTDATE"].ToString();
                    frti = dt.Rows[0]["FROMTIME"].ToString();
                    toti = dt.Rows[0]["TOTIME"].ToString();
                    des = dt.Rows[0]["ACTDESC"].ToString();

                }

                var result = new { adat = adat, frti = frti, toti = toti, des= des };

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetItemUnit(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string unit = "";


                dt = datatrans.GetData("SELECT UNITMAST.UNITID FROM ITEMMASTER  LEFT OUTER JOIN UNITMAST ON ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID WHERE ITEMMASTER.ITEMMASTERID='" + ItemId + "'");

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
        public ActionResult GetEMPDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string name = "";
                string dep = "";

                dt = datatrans.GetData("SELECT EMPMAST.EMPNAME,DDBASIC.DEPTNAME FROM EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASICID=EMPMAST.EMPDEPT WHERE EMPMASTID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    name = dt.Rows[0]["EMPNAME"].ToString();
                    dep = dt.Rows[0]["DEPTNAME"].ToString();

                }

                var result = new { name = name, dep = dep};

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult ListActivitiesCO()
        {
            return View();
        }

        public ActionResult MyListActivitiesCOgrid(string strStatus)
        {
            List<ListActivitiesCO> Reg = new List<ListActivitiesCO>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = activitiesCO.GetAllActivitiesCO(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=ActivitiesCO?id=" + dtUsers.Rows[i]["ACTCARRIEDHEADID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ACTCARRIEDHEADID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ACTCARRIEDHEADID"].ToString() + "";
                }

                Reg.Add(new ListActivitiesCO
                {
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    enty = dtUsers.Rows[i]["ENTRYTYPE"].ToString(),
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    actid = dtUsers.Rows[i]["ACTID"].ToString(),
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

            string flag = activitiesCO.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListActivitiesCO");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListActivitiesCO");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = activitiesCO.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListActivitiesCO");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListActivitiesCO");
            }
        }
        public JsonResult GetItemJSON1()
        {
            Cons model = new Cons();
            return Json(BindItem());

        }

        public JsonResult GetItemJSON2()
        {
            Emp model = new Emp();
            return Json(BindEmplId());

        }

        public JsonResult GetItemJSON3()
        {
            Ser model = new Ser();
            return Json(BindPartyId());

        }

        public JsonResult GetItemJSON4()
        {
            Chk model = new Chk();
            return Json(model);

        }
    }
}
