using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class BonusMasterController : Controller
    {
        IBonusMaster BonusM;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public BonusMasterController(IBonusMaster _BonusM, IConfiguration _configuratio)
        {
            BonusM = _BonusM;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BonusMaster(String id)
        {
            BonusMaster ic = new BonusMaster();
            //ic.LTNamelst = BindLTNamelst();
            ic.Itemlst = BindDesignation();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = BonusM.GetBonusMasterEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.BType = dt.Rows[0]["BONUSTYPE"].ToString();
                    ic.Itemlst = BindDesignation();
                    ic.ADes = dt.Rows[0]["ADESIGNATIONS"].ToString();
                    ic.CType = dt.Rows[0]["CALCULATION"].ToString();
                    ic.BValue = dt.Rows[0]["BONUSVALUE"].ToString();
                    ic.EFrom = dt.Rows[0]["EFFECTIVEFROM"].ToString();
                    ic.Remarks = dt.Rows[0]["REMARKS"].ToString();
                   
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult BonusMaster(BonusMaster Em, string id)
        {
            try
            {
                Em.ID = id;
                string Strout = BonusM.GetInsBonusM(Em);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Em.ID == null)
                    {
                        TempData["notice"] = "BonusMaster  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "BonusMaster  Updated Successfully...!";
                    }
                    return RedirectToAction("BonusMasterList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit BonusMaster";

                    return View(Em);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Em);

        }

        public IActionResult BonusMasterList()
        {
            return View();
        }

        public ActionResult MyListBonusMastergrid(string strStatus)
        {
            List<BonusMasterList> Reg = new List<BonusMasterList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = BonusM.GetAllBonusMaster(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=BonusMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    ViewRow = "<a href=ViewBonusMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }


                Reg.Add(new BonusMasterList
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    bonustype = dtUsers.Rows[i]["BONUSTYPE"].ToString(),
                    applicabledesignations = dtUsers.Rows[i]["DESIGNATION"].ToString(),
                    bonusvalue = dtUsers.Rows[i]["BONUSVALUE"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    
                });
            }

            return Json(new
            {
                Reg
            });

        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = BonusM.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("BonusMasterList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("BonusMasterList");
            }

        }
        public List<SelectListItem> BindDesignation()
        {
            try
            {
                DataTable dtDesg = BonusM.GetDesignation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESIGNATION"].ToString(), Value = dtDesg.Rows[i]["DDDETAILID"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ViewBonusMaster(string id)
        {
            BonusMaster Emp = new BonusMaster();

            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select ID,BONUSTYPE,DDDETAIL.DESIGNATION,BONUSVALUE,to_char(EFFECTIVEFROM,'dd-MON-yyyy')EFFECTIVEFROM,CALCULATION from BONUSMASTER LEFT OUTER JOIN DDDETAIL ON DDDETAIL.DDDETAILID = BONUSMASTER.ADESIGNATIONS WHERE ID='" + id + "'");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                Emp.ID = dt.Rows[0]["ID"].ToString();
                Emp.BType = dt.Rows[0]["BONUSTYPE"].ToString();
                Emp.ADes = dt.Rows[0]["DESIGNATION"].ToString();
                Emp.BValue = dt.Rows[0]["BONUSVALUE"].ToString();
                Emp.EFrom = dt.Rows[0]["EFFECTIVEFROM"].ToString();
                Emp.CType = dt.Rows[0]["CALCULATION"].ToString();
                Emp.ID = id;
            }
            return View(Emp);

        }
    }
}
