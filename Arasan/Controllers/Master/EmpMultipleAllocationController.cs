using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;
using System;

namespace Arasan.Controllers.Master
{
    public class EmpMultipleAllocationController : Controller
    {
        IEmpMultipleAllocationService EmpMultipleAllocationService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public EmpMultipleAllocationController(IEmpMultipleAllocationService _EmpMultipleAllocationService, IConfiguration _configuratio)
        {
            EmpMultipleAllocationService = _EmpMultipleAllocationService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult EmpMultipleAllocation(string piid, string tag)
        {
            EmpMultipleAllocation ca = new EmpMultipleAllocation();
            
           

            //ca.Location = Request.Cookies["LocationId"];
            ca.EDate = DateTime.Now.ToString("dd-MMM-yyyy");

            if (piid == null)
            {
                ca.Loclst = BindMlocation("");
                ca.Emolst = BindEmp("insert");
            }
            else
            {
                ca.Emolst = BindEmp("update");
                DataTable dt = datatrans.GetData("select * from EMPALLOCATION WHERE IS_ACTIVE='Y' AND EMPALLOCATIONID='" + piid + "'");
                if(dt.Rows.Count > 0)
                {
                    ca.Emp = dt.Rows[0]["EMPID"].ToString();
                }
                ca.Loclst = BindMlocation(piid); 

                //dt = DebitNoteBillService.GetDebitNoteBillDetail(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //    ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                //    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                //}

            }
           
            return View(ca);
        }
        [HttpPost]
        public ActionResult EmpMultipleAllocation(EmpMultipleAllocation Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = EmpMultipleAllocationService.EmpMultipleAllocationCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "EmpMultipleAllocation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "EmpMultipleAllocation Updated Successfully...!";
                    }
                    return RedirectToAction("ListEmpMultipleAllocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit EmpMultipleAllocation";
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
        public ActionResult ReassignEmpMultipleAllocation(EmpReasign Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = EmpMultipleAllocationService.ReassignEmpMultipleAllocation(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = "EmpMultipleAllocation Inserted Successfully...!";
                    return RedirectToAction("ListEmpMultipleAllocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit EmpMultipleAllocation";
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
        public ActionResult MyEmpMultipleAllocationgrid()
        {
            List<EmpBindList> Reg = new List<EmpBindList>();
            DataTable dtUsers = new DataTable();

            dtUsers = EmpMultipleAllocationService.GetEmpAllocation();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string ViewPage = string.Empty;
                string EditRow = string.Empty;
                string ReAssign = string.Empty;
                
                EditRow = "<a href=EmpMultipleAllocation?piid=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                ViewPage = "<a href=ViewEmpMultipleAllocation?piid=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                ReAssign = "<a href=ReassignEmpMultipleAllocation?piid=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + "><img src='../Images/D2.png' alt='Waiting for approval' /></a>";
                DeleteRow = "<a href=EmpMultipleAllocation?tag=Del&piid=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                

                Reg.Add(new EmpBindList
                {
                    piid = Convert.ToInt64(dtUsers.Rows[i]["EMPALLOCATIONID"].ToString()),
                    
                    emp = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    edate = dtUsers.Rows[i]["EMPDATE"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,
                    view = ViewPage,
                    reassign = ReAssign,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ListEmpMultipleItemgrid(string PRID)
        {
            List<EmpMultipleItemBindList> EnqChkItem = new List<EmpMultipleItemBindList>();
            DataTable dtEnq = new DataTable();
            dtEnq = EmpMultipleAllocationService.GetEmpMultipleItem(PRID);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new EmpMultipleItemBindList
                {
                    piid = Convert.ToInt64(dtEnq.Rows[i]["EMPALLOCATIONID"].ToString()),
                    location = dtEnq.Rows[i]["LOCID"].ToString(),
                   
                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }
        public IActionResult ViewEmpMultipleAllocation(string id)
        {
            EmpView ca = new EmpView();
            DataTable dt = new DataTable();
            dt = EmpMultipleAllocationService.GetEmpMultipleAllocationServiceName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Emp = dt.Rows[0]["EMPNAME"].ToString();
                ca.EDate = dt.Rows[0]["EMPDATE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.ID = id;
            }
            return View(ca);
           
        }
        public IActionResult ReassignEmpMultipleAllocation(string piid)
        {
            EmpReasign ca = new EmpReasign();
            ca.Emolst = BindEmp("update");
            ca.Loclst = BindMlocation(piid);
            DataTable dt1 = new DataTable();
            dt1 = EmpMultipleAllocationService.GetEmpMultipleAllocationReassign(piid);
            if (dt1.Rows.Count > 0)
            {
                ca.EDate = dt1.Rows[0]["EMPDATE"].ToString();
             
            }
            return View(ca);

        }
        public IActionResult ListEmpMultipleAllocation()
        {
            //IEnumerable<EmpMultipleAllocation> sta = EmpMultipleAllocationService.GetAllEmpMultipleAllocation();
            return View();
        }
        public List<SelectListItem> BindEmp(string action)
        {
            try
            {
                DataTable dtDesg = EmpMultipleAllocationService.GetEmp(action);
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
        public List<SelectListItem> BindMlocation(string id)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dtCity = new DataTable();
                dtCity = datatrans.GetPyroLocation();
                if (dtCity.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        bool sel = false;
                        long Region_id = EmpMultipleAllocationService.GetMregion(dtCity.Rows[i]["LOCDETAILSID"].ToString(), id);
                        if (Region_id == 0)
                        {
                            sel = false;
                        }
                        else
                        {
                            sel = true;
                        }
                        items.Add(new SelectListItem
                        {
                            Text = dtCity.Rows[i]["LOCID"].ToString(),
                            Value = dtCity.Rows[i]["LOCDETAILSID"].ToString(),
                            Selected = sel
                        });
                    }
                }
                return items;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

