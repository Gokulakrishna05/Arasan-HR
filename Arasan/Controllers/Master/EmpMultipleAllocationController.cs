using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;




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
        public IActionResult EmpMultipleAllocation(string id, string tag)
        {
            EmpMultipleAllocation ca = new EmpMultipleAllocation();
            ca.Emolst = BindEmp();
            ca.Loclst = BindMlocation();

            //ca.Location = Request.Cookies["LocationId"];
            ca.EDate = DateTime.Now.ToString("dd-MMM-yyyy");

            if (id == null)
            {
               

            }
            else
            {


                //DataTable dt = new DataTable();
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
        public ActionResult MyEmpMultipleAllocationgrid()
        {
            List<EmpBindList> Reg = new List<EmpBindList>();
            DataTable dtUsers = new DataTable();

            dtUsers = EmpMultipleAllocationService.GetEmpAllocation();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=EmpMultipleAllocation?id=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteEmpMultipleAllocation?tag=Del&id=" + dtUsers.Rows[i]["EMPALLOCATIONID"].ToString() + " onclick='return confirm(" + "\"Are you sure you want to Disable this record...?\"" + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new EmpBindList
                {
                    piid = Convert.ToInt64(dtUsers.Rows[i]["EMPALLOCATIONID"].ToString()),
                    emp = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    edate = dtUsers.Rows[i]["EMPDATE"].ToString(),
                    EditRow = EditRow,
                    DelRow = DeleteRow,

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
        public IActionResult ListEmpMultipleAllocation()
        {
            //IEnumerable<EmpMultipleAllocation> sta = EmpMultipleAllocationService.GetAllEmpMultipleAllocation();
            return View();
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = EmpMultipleAllocationService.GetEmp();
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
        public List<SelectListItem> BindMlocation()
        {
            try
            {
                DataTable dtDesg = EmpMultipleAllocationService.GetMlocation();
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
    }
}
