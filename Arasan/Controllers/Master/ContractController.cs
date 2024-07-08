using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class ContractController : Controller
    {
        IContract ContractService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ContractController(IContract _ContractService, IConfiguration _configuratio)
        {
            ContractService = _ContractService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult Contract(string id)
        {
            Contract ca = new Contract();
            ca.conlst = BindConty();
            ca.kglst = BindKg();
            DataTable dt = new DataTable();
            //double total = 0;
            dt = ContractService.GetEditContract(id);
            if (dt.Rows.Count > 0)
            {
                ca.conlst = BindConty();
                ca.Contype = dt.Rows[0]["CONTTYPE"].ToString();
                ca.Salpd = dt.Rows[0]["SALPD"].ToString();
                ca.kglst = BindKg();
                ca.Dkg = dt.Rows[0]["DAYORKGS"].ToString();
                ca.ID = id;

            }
            return View(ca);
        }
        [HttpPost]  
        public ActionResult Contract(Contract Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ContractService.ContractCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Contract Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Contract Updated Successfully...!";
                    }
                    return RedirectToAction("ListContract");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Contract";
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
        public List<SelectListItem> BindConty()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PYRO POLISHING", Value = "PYRO POLISHING" });
                lstdesg.Add(new SelectListItem() { Text = "DG PYRO MIXING", Value = "DG PYRO MIXING" });
                lstdesg.Add(new SelectListItem() { Text = "PYRO PACKING", Value = "PYRO PACKING" });
                lstdesg.Add(new SelectListItem() { Text = "DG PIGMENT MIXING", Value = "DG PIGMENT MIXING" });
                lstdesg.Add(new SelectListItem() { Text = "PIGMENTS POLISHING", Value = "PIGMENTS POLISHING" });
                lstdesg.Add(new SelectListItem() { Text = "Remelting Powder", Value = "Remelting Powder" });
                lstdesg.Add(new SelectListItem() { Text = "Remelting Dross", Value = "Remelting Dross" });
                lstdesg.Add(new SelectListItem() { Text = "Blending", Value = "Blending" });
                lstdesg.Add(new SelectListItem() { Text = "Production", Value = "Production" });
                lstdesg.Add(new SelectListItem() { Text = "Packing", Value = "Packing" });
                lstdesg.Add(new SelectListItem() { Text = "Maintenance", Value = "Maintenance" });
                lstdesg.Add(new SelectListItem() { Text = "PASTE MIXING", Value = "PASTE MIXING" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindKg()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PER KGS", Value = "PER KGS" });
                lstdesg.Add(new SelectListItem() { Text = "PER DAY", Value = "PER DAY" });
                


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListContract()
        {
            return View();
        }
        public ActionResult MyListContractgrid(string strStatus)
        {
            List<ContractList> Reg = new List<ContractList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "ACTIVE" : strStatus;
            dtUsers = ContractService.GetAllContracts(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                //if (dtUsers.Rows[i]["STATUS"].ToString() == "ACTIVE")
                //{

                    EditRow = "<a href=Contract?id=" + dtUsers.Rows[i]["CONTRMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CONTRMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                //}
                //else
                //{
                //    EditRow = "";
                //    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["CONTRMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                //}

                Reg.Add(new ContractList
                {
                    contype = dtUsers.Rows[i]["CONTTYPE"].ToString(),
                    salpd = dtUsers.Rows[i]["SALPD"].ToString(),
                    dkg = dtUsers.Rows[i]["DAYORKGS"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ContractService.StatusDelete(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListContract");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListContract");
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
