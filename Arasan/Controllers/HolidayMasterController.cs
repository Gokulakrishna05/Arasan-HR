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
    public class HolidayMasterController : Controller
    {

        IHolidayMaster HolidayM;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public HolidayMasterController(IHolidayMaster _HolidayM, IConfiguration _configuratio)
        {
            HolidayM = _HolidayM;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult HolidayMaster(string id)
        {
            HolidayMaster ic = new HolidayMaster();
            //ic.LTNamelst = BindLTNamelst();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = HolidayM.GetHolidayMasterEdit(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["HOLIDAYID"].ToString();
                    ic.Hname = dt.Rows[0]["HOLIDAYNAME"].ToString();
                    ic.Hdate = dt.Rows[0]["HOLIDAYDATE"].ToString();
                    ic.DWeek = dt.Rows[0]["DAYOFWEEK"].ToString();
                    ic.HType = dt.Rows[0]["HOLIDAYTYPE"].ToString();
                    ic.Rmk = dt.Rows[0]["REMARKS"].ToString();
                    //ic.Cdate = dt.Rows[0]["CREATEDDATE"].ToString();
                    //ic.Cby = dt.Rows[0]["CREATEDBY"].ToString();
                    //ic.Mdate = dt.Rows[0]["MODIFIEDDATE"].ToString();
                    //ic.Mby = dt.Rows[0]["MODIFIEDBY"].ToString();
                   
                }
            }
            return View(ic);
        }

        [HttpPost]
        public ActionResult HolidayMaster(HolidayMaster Em, string id)
        {


            try
            {
                Em.ID = id;
                string Strout = HolidayM.GetHMaster(Em);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Em.ID == null)
                    {
                        TempData["notice"] = "HolidayMaster  Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "HolidayMaster  Updated Successfully...!";
                    }
                    //return RedirectToAction("LeaveTypeMasterlist");
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit HolidayMaster";

                    return View(Em);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Em);

        }

        public IActionResult HolidayMasterList()
        {
            return View();
        }

        public ActionResult MyListHolidayMastergrid(string strStatus)
        {
            List<HolidayMasterList> Reg = new List<HolidayMasterList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = HolidayM.GetAllHolidayMaster(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=HolidayMaster?id=" + dtUsers.Rows[i]["HOLIDAYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";

                    ViewRow = "<a href=ViewHolidayMaster?id=" + dtUsers.Rows[i]["HOLIDAYID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["HOLIDAYID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["HOLIDAYID"].ToString() + "";
                }


                Reg.Add(new HolidayMasterList
                {
                    id = dtUsers.Rows[i]["HOLIDAYID"].ToString(),
                    hname = dtUsers.Rows[i]["HOLIDAYNAME"].ToString(),
                    hdate = dtUsers.Rows[i]["HOLIDAYDATE"].ToString(),
                    dweek = dtUsers.Rows[i]["DAYOFWEEK"].ToString(),

                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    //rrow = Regenerate
                });
            }

            return Json(new
            {
                Reg
            });



        }

        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = HolidayM.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("HolidayMasterList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("HolidayMasterList");
            }

        }

        public IActionResult ViewHolidayMaster(string id)
        {
            HolidayMaster Emp = new HolidayMaster();

            //List<LeaveTypeMasterList> TData = new List<LeaveTypeMasterList>();
            // AttendanceDetails tda = new AttendanceDetails();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select HOLIDAYID,HOLIDAYNAME,to_char(HOLIDAYDATE,'dd-MON-yyyy')HOLIDAYDATE,DAYOFWEEK from HOLIDAYMASTER WHERE HOLIDAYID ='" + id + "'");
            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                Emp.ID = dt.Rows[0]["HOLIDAYID"].ToString();
                Emp.Hname = dt.Rows[0]["HOLIDAYNAME"].ToString();
                Emp.Hdate = dt.Rows[0]["HOLIDAYDATE"].ToString();
                Emp.DWeek = dt.Rows[0]["DAYOFWEEK"].ToString();
                //Emp.ID = id;
            }
            return View(Emp);

        }

    }
}
