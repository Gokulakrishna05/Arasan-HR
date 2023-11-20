using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
 
using Arasan.Models;
using Arasan.Services.Master;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Arasan.Controllers
{
    public class PackDrumAllocationController : Controller
    {
        IPackDrumAllocation Pack;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public PackDrumAllocationController(IPackDrumAllocation _Pack, IConfiguration _configuratio)
        {
            Pack = _Pack;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PackDrumAllocation()
        {
            PackDrumAllocation pa = new PackDrumAllocation();
            pa.Brlst = BindBranch();
            pa.Branch = Request.Cookies["BranchId"];
            pa.Enter = Request.Cookies["UserId"];
            pa.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            pa.Loclst = BindLoc();
            pa.Emplst = BindEmp();
            DataTable dtv = datatrans.GetSequence("PDA");
            if (dtv.Rows.Count > 0)
            {
                pa.Docid = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<DrumCreate> TData = new List<DrumCreate>();
            DrumCreate tda = new DrumCreate();
            for (int i = 0; i < 3; i++)
            {
                tda = new DrumCreate();
               
             

                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            pa.drumlst = TData;
            return View(pa);
        }
        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = Pack.GetLoc();
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
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
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
        public ActionResult GetPrefixDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string start = "";
                string end = "";
                string prefix = "";
                string tot = "";
                 
                dt = Pack.GetDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    end = dt.Rows[0]["LASTNO"].ToString();
                    prefix = dt.Rows[0]["SPREFIX"].ToString();
                    start = dt.Rows[0]["STARTNO"].ToString();
                    tot = dt.Rows[0]["TOTDRUMS"].ToString();
                    
                }
             
                int lanumber = Convert.ToInt32(end);
                int total = Convert.ToInt32(tot);
                
                int last = lanumber + total;
                int stnumber = last + 1;
                var result = new { last = last, prefix = prefix, stnumber = stnumber };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult PackDrumAllocation(PackDrumAllocation Cy, string id)
        {

            try
            {
                Cy.ID = id;

                string Strout = Pack.PackDrumCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "  PackDrumAllocation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "  PackDrumAllocation Updated Successfully...!";
                    }
                    return RedirectToAction("PackDrumAllocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit  PackDrumAllocation";
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
        public ActionResult GetDrumDetails(int id,int st,string pre)
        {
            PackDrumAllocation model = new PackDrumAllocation();
            DataTable dtt = new DataTable();
            List<DrumCreate> Data = new List<DrumCreate>();
            DrumCreate tda = new DrumCreate();
            
            for (int i = 1; i <= id; i++)
                {
                    tda = new DrumCreate();


                int s = st;
                int legcode = Convert.ToInt32(s);
                string code = GetNumberwithPrefix(legcode, 6);
                //int prefix = Convert.ToInt32(pre);
                tda.totaldrum = code;
                string drum = pre +"" + code;
                tda.packdrum = drum.ToString();
                legcode++;
                st = legcode;
                tda.packyn = "N";


                    Data.Add(tda);
                }
           
            model.drumlst = Data;
            return Json(model.drumlst);

        }
        public static string GetNumberwithPrefix(int Ledgercode, int totalchar)
        {
            string tempnumber = Ledgercode.ToString();
            while (tempnumber.Length < 6)
                tempnumber = "0" + tempnumber;
            return tempnumber;
        }


        public IActionResult ListPackDrum()
        {
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<PackList> Reg = new List<PackList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Pack.GetAllPackDerum(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;

                View = "<a href=ViewPackDrum?id=" + dtUsers.Rows[i]["PDABASICID"].ToString() + "><img src='../Images/view_icon.png' alt='View' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PDABASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new PackList
                {
                    id = dtUsers.Rows[i]["PDABASICID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    start = dtUsers.Rows[i]["STARTNO"].ToString(),
                    last = dtUsers.Rows[i]["LASTNO"].ToString(),
                  
                    location = dtUsers.Rows[i]["LOCID"].ToString(),
                    totdrum = dtUsers.Rows[i]["TOTDRUMS"].ToString(),
                    prefix = dtUsers.Rows[i]["SPREFIX"].ToString(),
                    
                    viewrow = View,
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
            string flag = Pack.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPackDrum");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPackDrum");
            }

        }
        public IActionResult ViewPackDrum(string id)
        {
            PackDrumAllocation ca = new PackDrumAllocation();
            DataTable dt = Pack.GetPackDrum(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();


                ca.Docid = dt.Rows[0]["DOCID"].ToString();
                
                ca.Start = dt.Rows[0]["STARTNO"].ToString();
                ca.End = dt.Rows[0]["LASTNO"].ToString();
                ca.Pri = dt.Rows[0]["SPREFIX"].ToString();
                ca.Totdrum = dt.Rows[0]["TOTDRUMS"].ToString();
                ca.Enter = dt.Rows[0]["ENTEREDBY"].ToString();
                 
                List<DrumCreate> TData = new List<DrumCreate>();
                DrumCreate tda = new DrumCreate();
                DataTable dtDrum = Pack.EditDrumDetail(id);
                for (int i = 0; i < dtDrum.Rows.Count; i++)
                {
                    tda = new DrumCreate();
                    //tda.DrumNolst = BindDrumNo(ca.DrumLoc);
                    tda.packdrum = dtDrum.Rows[i]["DRUMNO"].ToString();
                    //tda.Batchlst = BindBatch(tda.DrumNo);



                    tda.packyn = dtDrum.Rows[i]["TSTATUS"].ToString();
                   


                    TData.Add(tda);
                }


                ca.drumlst = TData;
            }
            return View(ca);
        }
    }
}
