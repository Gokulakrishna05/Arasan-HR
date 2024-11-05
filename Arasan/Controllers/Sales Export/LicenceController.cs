using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Arasan.Controllers.Sales_Export
{
    public class LicenceController : Controller
    {
        ILicence Licence;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public LicenceController(ILicence _LicenceController, IConfiguration _configuratio)
        {
            Licence = _LicenceController;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Licence_Entry(string id)
        {
            Licence ca = new Licence();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];

            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.LicenceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("vchsl");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<ImportDeatils> TData = new List<ImportDeatils>();
            ImportDeatils tda = new ImportDeatils();

            List<ExportDeatils> TData1 = new List<ExportDeatils>();
            ExportDeatils tda1 = new ExportDeatils();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ImportDeatils();

                    
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new ExportDeatils();
                    
                    tda1.ExportItemlst = BindExportItemlst();
                    tda1.Suplst = BindSupplier();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                //double total = 0;
                dt = Licence.GetEditLicence(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.LicenceNo = dt.Rows[0]["LICENCENO"].ToString();
                    ca.ID = id;
                    ca.LicenceDate = dt.Rows[0]["LICENCEDATE"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();
                   
                    
                }
                DataTable dt2 = new DataTable();

                dt2 = Licence.GetLicenceImport(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ImportDeatils();
                        tda.InvNo = dt2.Rows[0]["IBILLNO"].ToString();
                        tda.InvDate = dt2.Rows[0]["IBILLDT"].ToString();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["IITEMDESC"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.Qty = dt2.Rows[i]["IQTY"].ToString();
                        tda.Amount = dt2.Rows[i]["IAMOUNT"].ToString();
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();

                dt3 = Licence.GetLicenceExport(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ExportDeatils();
                        tda1.ExpNo = dt3.Rows[0]["EINVNO"].ToString();
                        tda1.ExpDate = dt3.Rows[0]["EINVDT"].ToString();

                        tda1.Suplst = BindSupplier();
                        tda1.Customer = dt3.Rows[i]["EPARTYID"].ToString();

                        tda1.ExportItemlst = BindExportItemlst();
                        tda1.ItemId = dt3.Rows[i]["EITEMDESC"].ToString();
                        
                        tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                        tda1.Qty = dt3.Rows[i]["EQTY"].ToString();
                        tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                        TData1.Add(tda1);
                    }
                }
            }
            ca.ImportLst = TData;
            ca.ExportLst = TData1;
            return View(ca);
        }
        [HttpPost]
        public ActionResult Licence_Entry(Licence Cy, string id)
        {

            try
            {
                Cy.ID = id;
                Cy.Branch = Request.Cookies["BranchId"];
                string Strout = Licence.LicenceCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Licence Closing Entry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Licence Closing Entry Updated Successfully...!";
                    }
                    return RedirectToAction("ListLicence");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Licence_Entry";
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = Licence.GetItem();
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
        public List<SelectListItem> BindExportItemlst()
        {
            try
            {
                DataTable dtDesg = Licence.GetExportItem();
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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = Licence.GetSupplier();
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
        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string price = "";
                dt = Licence.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetExportDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string price = "";
                dt = Licence.GetExportDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            ImportDeatils model = new ImportDeatils();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());
        }
        public JsonResult GetExportItemJSON()
        {
            ExportDeatils model = new ExportDeatils();
            model.ExportItemlst = BindExportItemlst();
            return Json(BindExportItemlst());
        }
        public JsonResult GetSupplierJSON()
        {
            ExportDeatils model = new ExportDeatils();
            model.Suplst = BindSupplier();
            return Json(BindSupplier());
        }
        public IActionResult ListLicence()
        {
            return View();
        }
        public ActionResult MyListLicenceGrid(string strStatus)
        {
            List<ListLicenceItems> Reg = new List<ListLicenceItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)Licence.GetAllListLicenceItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["LICCLBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                    EditRow = "<a href=Licence_Entry?id=" + dtUsers.Rows[i]["LICCLBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewLicence?id=" + dtUsers.Rows[i]["LICCLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["LICCLBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["LICCLBASICID"].ToString() + "";
                }
                Reg.Add(new ListLicenceItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["LICCLBASICID"].ToString()),
                    docno = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    licno = dtUsers.Rows[i]["LICENCENO"].ToString(),
                    sendmail = SendMail,
                    view = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewLicence(string id)
        {
            Licence ca = new Licence();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = Licence.GetEditLicence(id);
            if (dt.Rows.Count > 0)
            {
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.LicenceNo = dt.Rows[0]["LICENCENO"].ToString();
                ca.ID = id;
                ca.LicenceDate = dt.Rows[0]["LICENCEDATE"].ToString();
                ca.Narration = dt.Rows[0]["NARRATION"].ToString();

                List<ImportDeatils> TData = new List<ImportDeatils>();
                ImportDeatils tda = new ImportDeatils();

                DataTable dt2 = new DataTable();
                dt2 = Licence.GetLicenceImportView(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ImportDeatils();
                        tda.InvNo = dt2.Rows[0]["IBILLNO"].ToString();
                        tda.InvDate = dt2.Rows[0]["IBILLDT"].ToString();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.Qty = dt2.Rows[i]["IQTY"].ToString();
                        tda.Amount = dt2.Rows[i]["IAMOUNT"].ToString();
                        TData.Add(tda);
                    }
                }

                List<ExportDeatils> TData1 = new List<ExportDeatils>();
                ExportDeatils tda1 = new ExportDeatils();

                DataTable dt3 = new DataTable();

                dt3 = Licence.GetLicenceExportView(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ExportDeatils();
                        tda1.ExpNo = dt3.Rows[0]["EINVNO"].ToString();
                        tda1.ExpDate = dt3.Rows[0]["EINVDT"].ToString();

                        tda1.Suplst = BindSupplier();
                        tda1.Customer = dt3.Rows[i]["PARTYID"].ToString();

                        tda1.ExportItemlst = BindExportItemlst();
                        tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();

                        tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                        tda1.Qty = dt3.Rows[i]["EQTY"].ToString();
                        tda1.Amount = dt3.Rows[i]["EAMOUNT"].ToString();
                        TData1.Add(tda1);
                    }
                }
                ca.ImportLst = TData;
                ca.ExportLst = TData1;
            }
            
            return View(ca);
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = Licence.StatusChange(tag, id);
            }
            else
            {
                flag = Licence.ActStatusChange(tag, id);
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLicence");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLicence");
            }
        }
    }
}
