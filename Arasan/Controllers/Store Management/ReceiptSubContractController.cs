using Arasan.Interface;
using Arasan.Interface.Qualitycontrol;
 
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;


namespace Arasan.Controllers
{
    public class ReceiptSubContractController : Controller
    {

        IReceiptSubContract Receipt;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ReceiptSubContractController(IReceiptSubContract _Receipt, IConfiguration _configuratio)
        {
            Receipt = _Receipt;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReceiptSubContract(string id)
        {
            ReceiptSubContract ca = new ReceiptSubContract();
            ca.Branch = Request.Cookies["BranchId"];
            ca.DClst = BindDC();
            ca.Loclst = BindLoc();
            ca.Suplst = BindSupplier("");
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.enterd = Request.Cookies["UserId"];
            DataTable dtv = datatrans.GetSequence("rsubc");
            if (dtv.Rows.Count > 0)
            {
                ca.DocNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
           
            List<ReceiptDeliverItem> TData = new List<ReceiptDeliverItem>();
            ReceiptDeliverItem tda = new ReceiptDeliverItem();
            List<ReceiptRecivItem> TData1 = new List<ReceiptRecivItem>();
            ReceiptRecivItem tda1 = new ReceiptRecivItem();
            List<DrumItem> TData2 = new List<DrumItem>();
            DrumItem tda2 = new DrumItem();
            for (int i = 0; i < 1; i++)
            {
                tda = new ReceiptDeliverItem();
                 
                tda.Itemlst = BindItemlst();
                 
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            for (int i = 0; i < 1; i++)
            {
                tda1 = new ReceiptRecivItem();
               
                tda1.Itemlst = BindItemlst();
               
                tda1.Isvalid = "Y";
                TData1.Add(tda1);
            }
            ca.Reclst = TData1;
            ca.Delilst = TData;
            ca.drumlst = TData2;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ReceiptSubContract(ReceiptSubContract Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = "";// Receipt.ReceiptSubContractCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ReceiptSubContract Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ReceiptSubContract Updated Successfully...!";
                    }
                    return RedirectToAction("ListReceiptSubContract");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ReceiptSubContract";
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
        public List<SelectListItem> BindSupplier(string id)
        {
            try
            {
                DataTable dtDesg = Receipt.GetSupplier(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
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
        public List<SelectListItem> BindDC()
        {
            try
            {
                DataTable dtDesg = Receipt.GetDC();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["SUBCONTDCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetDCDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string add1 = "";
                string add2 = "";
                string city = "";
                dt = Receipt.GetDCDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    add1 = dt.Rows[0]["ADD1"].ToString();
                    add2 = dt.Rows[0]["ADD2"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                   
                }

                var result = new { add1 = add1, add2 = add2, city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetPartyJSON(string itemid)
        {
            ReceiptSubContract model = new ReceiptSubContract();
            model.Suplst = BindSupplier(itemid);
            return Json(BindSupplier(itemid));

        }
        public ActionResult GetRecivedDetail(string ItemId)
        {
            ReceiptSubContract ca = new ReceiptSubContract();
            List<ReceiptRecivItem> TData = new List<ReceiptRecivItem>();
            ReceiptRecivItem tda = new ReceiptRecivItem();

            DataTable dt2 = new DataTable();

             

            dt2 = Receipt.GetRecvItemDetails(ItemId);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ReceiptRecivItem();
                


                  
                    tda.item = dt2.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dt2.Rows[i]["RITEM"].ToString();
                
                    tda.unit = dt2.Rows[i]["RUNIT"].ToString();
                    //tda.unitid = dt2.Rows[i]["UNITID"].ToString();
                    tda.qty = dt2.Rows[i]["ERQTY"].ToString();
                  
                    tda.rate = dt2.Rows[i]["ERATE"].ToString();
                    tda.amount = dt2.Rows[i]["EAMOUNT"].ToString();
                     

                    tda.Isvalid = "Y";
                    tda.id = ItemId;
                    TData.Add(tda);


                }
            }

            ca.Reclst = TData;
            return Json(ca.Reclst);



        }
        public ActionResult GetDeliverItemDetail (string ItemId)
        {
            ReceiptSubContract ca = new ReceiptSubContract();
            List<ReceiptDeliverItem> TData = new List<ReceiptDeliverItem>();
            ReceiptDeliverItem tda = new ReceiptDeliverItem();

            DataTable dt2 = new DataTable();
 

            dt2 = Receipt.GetDelivItemDetails(ItemId);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ReceiptDeliverItem();
                    


                   
                    tda.item = dt2.Rows[i]["ITEMID"].ToString();
                    tda.itemid = dt2.Rows[i]["item"].ToString();
                    
                    tda.unit = dt2.Rows[i]["UNIT"].ToString();
                    //tda.unitid = dt2.Rows[i]["UNITID"].ToString();
                    tda.qty = dt2.Rows[i]["QTY"].ToString();
               
                    tda.rate = dt2.Rows[i]["RATE"].ToString();
                    tda.amount = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.supid = dt2.Rows[i]["SUBCONTDCDETAILID"].ToString();
                   


                    tda.Isvalid = "Y";
                    tda.id = ItemId;
                    TData.Add(tda);


                }
            }

            ca.Delilst = TData;
            return Json(ca.Delilst);



        }
        public ActionResult DrumSelection(string id)
        {
            ReceiptSubContract ca = new ReceiptSubContract();
            List<DrumItem> TData = new List<DrumItem>();
            DrumItem tda = new DrumItem();

            DataTable dt2 = new DataTable();


            dt2 = Receipt.GetDrumItemDetails(id);
            if (dt2.Rows.Count > 0)
            {

                ca.item = dt2.Rows[0]["ITEMID"].ToString();
                ca.itemid = dt2.Rows[0]["item"].ToString();
                ca.qty = dt2.Rows[0]["QTY"].ToString();
                ca.rate = dt2.Rows[0]["RATE"].ToString();
            }

            for (int i = 0; i < 1; i++)
            {
                tda = new DrumItem();
                tda.drumlist = BindDrum();
                tda.ID = id;
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.drumlst = TData;
            return View(ca);



        }
        public ActionResult GetDrumDetails(int ItemId,double rate,int qty)
        {
            ReceiptSubContract ca = new ReceiptSubContract();
            List<DrumItem> TData = new List<DrumItem>();
            DrumItem tda = new DrumItem();

            int sqty = qty / ItemId;
            for (int i = 1; i <= sqty; i++)
            {
                tda = new DrumItem();

                    tda.drumlist = BindDrum();

                tda.qty = ItemId.ToString();
                tda.rate = rate.ToString();
                Double tamt = ItemId * rate;
                tda.amount = tamt.ToString();


                    tda.Isvalid = "Y";
                    
                    TData.Add(tda);


                }
            

            ca.drumlst = TData;
            return Json(ca.drumlst);



        }
        public JsonResult GetDrumSON()
        {
            DrumItem model = new DrumItem();
           
            model.drumlist = BindDrum();
            return Json(BindDrum());

        }
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = Receipt.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListReceiptSubContractGrid(string strStatus)
        {
            List<ReceiptSubContractItem> Reg = new List<ReceiptSubContractItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)Receipt.GetAllReceiptSubContractItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string View = string.Empty;
               

                View = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["RECFSUBBASICID"].ToString() + "><img src='../Images/view_icon.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["RECFSUBBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
               
                
               
                Reg.Add(new ReceiptSubContractItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["RECFSUBBASICID"].ToString()),
                   
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    view = View,
 
                    delrow = DeleteRow,
 


                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ListReceiptSubContract()
        {
            //IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur(status);
            return View();
        }
    }
}
