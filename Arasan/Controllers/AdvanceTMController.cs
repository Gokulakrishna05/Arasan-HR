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
    public class AdvanceTMController : Controller
    {
        IAdvanceTM Advance;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public AdvanceTMController(IAdvanceTM _Advance, IConfiguration _configuratio)
        {
            Advance = _Advance;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AdvanceTM(String id)
        {
            AdvanceTM ic = new AdvanceTM();
         
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = Advance.GetAdvanceTMEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.AType = dt.Rows[0]["ATYPE"].ToString();
                    ic.MALmt = dt.Rows[0]["MXLIMIT"].ToString();
                    ic.ERules = dt.Rows[0]["EGTYRULES"].ToString();
                    ic.RPType = dt.Rows[0]["RETYPE"].ToString();
                    ic.NOIns = dt.Rows[0]["NOFINS"].ToString();
                    ic.Dedn = dt.Rows[0]["DEON"].ToString();
                    ic.Rmarks = dt.Rows[0]["REMARKS"].ToString();
                  

                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult AdvanceTM(AdvanceTM Em, string id)
        {
            try
            {
                Em.ID = id;
                string Strout = Advance.GetAdvanceT(Em);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Em.ID == null)
                    {
                        TempData["notice"] = "AdvanceTM  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AdvanceTM  Updated Successfully...!";
                    }
                    return RedirectToAction("AdvanceTMList");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit AdvanceTM";

                    return View(Em);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Em);

        }


        public IActionResult AdvanceTMList()
        {
            return View();
        }

        public ActionResult MyListAdvanceTMgrid(string strStatus)
        {
            List<AdvanceTMList> Reg = new List<AdvanceTMList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Advance.GetAllAdvanceTM(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=AdvanceTM?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    ViewRow = "<a href=ViewAdvanceTM?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }


                Reg.Add(new AdvanceTMList
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    atype = dtUsers.Rows[i]["ATYPE"].ToString(),
                    maxlmt = dtUsers.Rows[i]["MXLIMIT"].ToString(),
                    egrules = dtUsers.Rows[i]["EGTYRULES"].ToString(),
                    rtype = dtUsers.Rows[i]["RETYPE"].ToString(),
                   

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
            string flag = Advance.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("AdvanceTMList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("AdvanceTMList");
            }

        }


        public IActionResult ViewAdvanceTM(string id)
        {
            AdvanceTM Emp = new AdvanceTM();

            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select ID,ATYPE,MXLIMIT,EGTYRULES,RETYPE,NOFINS,DEON,REMARKS  from ADVTYPEMASTER WHERE ID='" + id + "'  ");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                Emp.ID = dt.Rows[0]["ID"].ToString();
                Emp.AType = dt.Rows[0]["ATYPE"].ToString();
                Emp.MALmt = dt.Rows[0]["MXLIMIT"].ToString();
                Emp.ERules = dt.Rows[0]["EGTYRULES"].ToString();
                Emp.RPType = dt.Rows[0]["RETYPE"].ToString();
                Emp.NOIns = dt.Rows[0]["NOFINS"].ToString();
                Emp.Dedn = dt.Rows[0]["DEON"].ToString();
                Emp.Rmarks = dt.Rows[0]["REMARKS"].ToString();
                Emp.ID = id;
            }
            return View(Emp);

        }
    }
}
