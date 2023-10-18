using Arasan.Interface;
using Arasan.Models;
using Arasan.Services.Master;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Arasan.Controllers 
{
	public class APProductionentryController : Controller
	{
		IAPProductionEntry IProductionEntry;
		IConfiguration? _configuratio;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private string? _connectionString;
		DataTransactions datatrans;
		public APProductionentryController(IAPProductionEntry _IProductionEntry, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
		{
            this._WebHostEnvironment = WebHostEnvironment;
            IProductionEntry = _IProductionEntry;
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
			datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public ActionResult APProductinentryselection()
        {
            var userId = Request.Cookies["UserId"];
            int cunt = datatrans.GetDataId("select count(*) as cunt from APPRODUCTIONBASIC where IS_CURRENT='Yes' AND ASSIGNENG='"+ userId + "'");
            if (cunt == 0)
            {
                return RedirectToAction("APProductionentry");
            }
            else
            {
                return RedirectToAction("APProductionentryDetail", new { tag = 1 });
            }
        }


		public ActionResult APProductionentry(string id)
		{
			APProductionentry ca = new APProductionentry();
			DataTable dtv = datatrans.GetSequence("APPro");
			ca.Loclst = BindAPWorkCenter();
			ca.Englst = BindEmp();
			ca.Shiftlst = BindShift();
            ca.Branch = Request.Cookies["BranchId"];
            ca.batchcomplete = "N";
			ca.Shift = "A";
			ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
			if (dtv.Rows.Count > 0)
			{
				ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
			}
			ca.Batchlst = BindBatch();
			if (id == null)
			{

			}
			else
			{
				if (!string.IsNullOrEmpty(id))
				{
					ca.APID = id;
					
				}
			}

			//List<BreakDetail> TData3 = new List<BreakDetail>();
			//BreakDetail tda3 = new BreakDetail();
			//List<ProInput> TData = new List<ProInput>();
			//ProInput tda = new ProInput();
			//List<APProInCons> TData1 = new List<APProInCons>();
			//APProInCons tda1 = new APProInCons();
			//List<EmpDetails> TTData2 = new List<EmpDetails>();
			//EmpDetails tda2 = new EmpDetails();
			//for (int i = 0; i < 3; i++)
			//{
			//	tda3 = new BreakDetail();

			//	tda3.Machinelst = BindMachineID();
			//	tda3.Emplst = BindEmp();
			//	tda3.Isvalid = "Y";
			//	TData3.Add(tda3);

			//}
			//for (int i = 0; i < 3; i++)
			//{
			//	tda = new ProInput();

			//	tda.Itemlst = BindItemlst();


			//	tda.Isvalid = "Y";
			//	TData.Add(tda);

			//}
			//for (int i = 0; i < 1; i++)
			//{
			//	tda1 = new APProInCons();
			//	tda1.Itemlst = BindItemlst();
			//	tda1.Isvalid = "Y";
			//	TData1.Add(tda1);
			//}
			//for (int i = 0; i < 3; i++)
			//{
			//	tda2 = new EmpDetails();

			//	tda2.Employeelst = BindEmp();
			//	tda2.Isvalid = "Y";
			//	TTData2.Add(tda2);
			//}

			//ca.BreakLst = TData3;
			//ca.inplst = TData;
			//ca.EmplLst = TTData2;
			//ca.Binconslst = TData1;
			return View(ca);
		}
		public JsonResult GetEmpJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindEmp());
		}
        public JsonResult GetLogJSON()
        {
            LogDetails model = new LogDetails();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public JsonResult GetBreakJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindMachineID());
		}
		public JsonResult GetBreakEmpJSON()
		{
			//EnqItem model = new EnqItem();
			//  model.ItemGrouplst = BindItemGrplst(value);
			return Json(BindEmp());
		}



		[HttpPost]
		public ActionResult APProductionentry(APProductionentry Cy, string id)
		{

			try
			{
				Cy.ID = id;
				string Strout = IProductionEntry.APProductionEntryCRUD(Cy);
				if (string.IsNullOrEmpty(Strout))
				{
					if (Cy.ID == null)
					{
						TempData["notice"] = "APProductionentry Inserted Successfully...!";
					}
					else
					{
						TempData["notice"] = "APProductionentry Updated Successfully...!";
					}
					//return RedirectToAction("APProductionentryDetail", new { id = id });
				}

				else
				{
					ViewBag.PageTitle = "Edit APProductionentry";
					TempData["notice"] = Strout;
					//return View();
				}
				return RedirectToAction("APProductionentryDetail", new { id = Cy.APID ,tag=2});
				// }
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
			return View(Cy);
		}

        [HttpPost]
        public ActionResult ApproveAPProductionentry(APProductionentryDet Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.ApproveAPProEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                     TempData["notice"] = "APProductionentry Approved Successfully...!";
                }

                else
                {
                    ViewBag.PageTitle = "Edit APProductionentry";
                    TempData["notice"] = Strout;
                }
                return RedirectToAction("ListAPProductionentry");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Cy);
        }

        public List<SelectListItem> BindEmp()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetEmp();
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
		public List<SelectListItem> BindBatch()
		{
			try
			{
				DataTable dtDesg = datatrans.GetBatch();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["DOCID"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<SelectListItem> BindAPWorkCenter()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetAPWorkCenter();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public ActionResult GetEmployeeDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();

				string code = "";


				dt = IProductionEntry.GetEmployeeDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					code = dt.Rows[0]["EMPID"].ToString();



				}

				var result = new { code = code };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public IActionResult ApproveAPProductionentry(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            DataTable dt = new DataTable();
            dt = IProductionEntry.GetAPProductionentryName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHYN"].ToString();
                ca.ID = id;
                ca.LOCID= dt.Rows[0]["WCBASICID"].ToString();
                ca.BranchId = Request.Cookies["BranchId"];
            }
            DataTable dt2 = new DataTable();
            List<ProInput> TData = new List<ProInput>();
            ProInput tda = new ProInput();
            dt2 = IProductionEntry.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                   
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Item = dt2.Rows[i]["item"].ToString();
                    tda.lotlist = BindLotNo(tda.Item, ca.LOCID, ca.BranchId);
                    tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.StockAvailable = Convert.ToDouble(dt2.Rows[i]["STOCK"].ToString() == "" ? "0" : dt2.Rows[i]["STOCK"].ToString());
                    tda.APID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }

            }
            DataTable dt3 = new DataTable();
            List<APProInCons> TData1 = new List<APProInCons>();
            APProInCons tda1 = new APProInCons();
            dt3 = IProductionEntry.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                    tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.saveitemId = dt3.Rows[i]["item"].ToString();
                    tda1.consunit = dt3.Rows[i]["UNITID"].ToString();
                    tda1.BinId = dt3.Rows[i]["BIN"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

           
           
            DataTable dt6 = new DataTable();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            dt6 = IProductionEntry.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new ProOutput();
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.saveitemId = dt6.Rows[i]["item"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                    tda4.drumid = dt6.Rows[i]["drum"].ToString();
                    tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    tda4.ExcessQty= Convert.ToDouble(dt6.Rows[i]["EXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["EXQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
                    if (dt7.Rows.Count > 0)
                    {
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                    tda4.APID = id;
                    tda4.outid= dt6.Rows[i]["APPRODOUTDETID"].ToString();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }

            }
           
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.Binconslst = TData1;
            return View(ca);

        }
        public List<SelectListItem> BindLotNo(string item,string loc,string branch)
        {
            string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + loc + "' ");
            try
            {
                DataTable dtDesg = IProductionEntry.GetLotNo(item, locid, branch);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOT_NO"].ToString(), Value = dtDesg.Rows[i]["LOT_NO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ViewAPProductionentry(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            DataTable dt = new DataTable();
            dt = IProductionEntry.GetAPProductionentryName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift = dt.Rows[0]["SHIFT"].ToString();
                ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
                ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHYN"].ToString();
                ca.ID = id;
            }
            DataTable dt2 = new DataTable();
            List<ProInput> TData = new List<ProInput>();
            ProInput tda = new ProInput();
            dt2 = IProductionEntry.GetInputDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                    tda.StockAvailable = Convert.ToDouble(dt2.Rows[i]["STOCK"].ToString() == "" ? "0" : dt2.Rows[i]["STOCK"].ToString());
                    tda.APID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }

            }
            DataTable dt3 = new DataTable();
            List<APProInCons> TData1 = new List<APProInCons>();
            APProInCons tda1 = new APProInCons();
            dt3 = IProductionEntry.GetConsDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                    tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["UNIT"].ToString();
                    tda1.BinId = dt3.Rows[i]["BINID"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

            DataTable dt4 = new DataTable();
            List<EmpDetails> TTData2 = new List<EmpDetails>();
            EmpDetails tda2 = new EmpDetails();
            dt4 = IProductionEntry.GetEmpdetDeatils(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new EmpDetails();
                    tda2.Employeelst = BindEmp();
                    tda2.Employee = dt4.Rows[i]["EMPID"].ToString();

                    tda2.EmpCode = dt4.Rows[i]["EMPCODE"].ToString();
                    tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                    tda2.StartDate = dt4.Rows[i]["STARTDATE"].ToString();
                    tda2.StartTime = dt4.Rows[i]["STARTTIME"].ToString();
                    tda2.EndDate = dt4.Rows[i]["ENDDATE"].ToString();
                    tda2.EndTime = dt4.Rows[i]["ENDTIME"].ToString();
                    tda2.OTHrs = dt4.Rows[i]["OTHOUR"].ToString();

                    tda2.ETOther = dt4.Rows[i]["ETOTHER"].ToString();
                    tda2.Normal = dt4.Rows[i]["NHOUR"].ToString();
                    tda2.NOW = dt4.Rows[i]["NATUREOFWORK"].ToString();
                    tda2.ID = id;
                    tda2.Isvalid = "Y";
                    TTData2.Add(tda2);

                }

            }
            DataTable dt5 = new DataTable();
            List<BreakDet> TData3 = new List<BreakDet>();
            BreakDet tda3 = new BreakDet();
            dt5 = IProductionEntry.GetBreakDeatils(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new BreakDet();
                    tda3.Machinelst = BindMachineID();
                    tda3.MachineId = dt5.Rows[i]["MCODE"].ToString();
                    tda3.Emplst = BindEmp();
                    tda3.MachineDes = dt5.Rows[i]["DESCRIPTION"].ToString();
                    tda3.StartTime = dt5.Rows[i]["FROMTIME"].ToString();
                    tda3.EndTime = dt5.Rows[i]["TOTIME"].ToString();
                    tda3.PB = dt5.Rows[i]["PB"].ToString();
                    tda3.Isvalid = "Y";
                    tda3.Alloted = dt5.Rows[i]["EMPNAME"].ToString();
                    tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                    tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                    tda3.Reason = dt5.Rows[i]["REASON"].ToString();

                    tda3.APID = id;
                    TData3.Add(tda3);
                }

            }
            DataTable dt6 = new DataTable();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            dt6 = IProductionEntry.GetOutputDeatils(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new ProOutput();
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    tda4.drumlst = BindDrum();
                    tda4.statuslst = BindStatus();
                    tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                    tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                    tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                    tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
                    if (dt7.Rows.Count > 0)
                    {
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                    tda4.APID = id;
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);

                }

            }
            DataTable adt7 = new DataTable();
            List<LogDetails> TTData5 = new List<LogDetails>();
            LogDetails tda5 = new LogDetails();
            adt7 = IProductionEntry.GetLogdetailDeatils(id);
            if (adt7.Rows.Count > 0)
            {
                for (int i = 0; i < adt7.Rows.Count; i++)
                {
                    tda5 = new LogDetails();

                    tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                    tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                    tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                    tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                    tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                    tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                    tda5.APID = id;
                    TTData5.Add(tda5);
                    tda5.Isvalid = "Y";
                }

            }
            ca.BreakLst = TData3;
            ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
            ca.Binconslst = TData1;
            ca.LogLst = TTData5;
            return View(ca);

        }
        public List<SelectListItem> BindMachineID()
		{
			try
			{
				DataTable dtDesg = datatrans.GetMachine();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MCODE"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString()});
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<SelectListItem> BindShift()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.ShiftDeatils();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
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
				DataTable dtDesg = IProductionEntry.GetItem();
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

        public List<SelectListItem> BindOutItemlst()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetOutItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString()});
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlstCon()
		{
			try
			{
				DataTable dtDesg = IProductionEntry.GetItemCon();
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString()});
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public List<SelectListItem> BindDrum()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.GetDrum();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString()});
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetshiftDetail(string Shiftid)
		{
			try
			{
				DataTable dt = new DataTable();
				string fromtime = "";
				string totime = "";
				string tothrs = "";
				dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTNO='" + Shiftid + "'");
				if (dt.Rows.Count > 0)
				{

					fromtime = dt.Rows[0]["FROMTIME"].ToString();
					totime = dt.Rows[0]["TOTIME"].ToString();
					tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
				}

				var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
            }
        }
        public ActionResult InsertProInput([FromBody] ProInput[] model)
        {
            try
			{
                foreach (ProInput input in model)
                {
                   
                    string item = input.ItemId;
                    string bin = input.BinId;
                    string batch = input.batchno;
                    string time = input.Time;
                    string id = input.APID;
                    string stock = input.StockAvailable.ToString();
                    string qty = input.IssueQty.ToString();
                    DataTable dt = new DataTable();
                    
                    dt = IProductionEntry.SaveInputDetails(id, item, bin, time, qty, stock, batch);
                }
                    if (model != null)
                    {

                        return Json("Success");
                    }
                    else
                    {
                        return Json("An Error Has occoured");
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertConsInput([FromBody] APProInCons[] model)
        {
            try
            {
                foreach (APProInCons Cons in model)
                {

                    string item = Cons.ItemId;
                    string bin = Cons.BinId;
                    string unit = Cons.consunit;
                    string qty = Cons.consQty.ToString(); 
                    string id = Cons.APID;
                    string stock = Cons.ConsStock.ToString();
                    string usedqty = Cons.Qty.ToString();
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveConsDetails(id, item, bin, unit, usedqty, qty, stock);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProEmp([FromBody] EmpDetails[] model)
        {
            try
            {
                foreach (EmpDetails emp in model)
                {

                    string empname = emp.Employee;
                    string code = emp.EmpCode;
                    string depat = emp.Depart;
                    string sdate = emp.StartDate ;
                    string id = emp.APID;
                    string stime = emp.StartTime ;
                    string edate = emp.EndDate ;
                    string etime = emp.EndTime;
                    string ot = emp.OTHrs;
                    string et = emp.ETOther;
                    string normal = emp.Normal;
                    string now = emp.NOW;
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveEmpDetails(id,empname, code, depat, sdate, stime, edate, etime, ot, et, normal, now);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProLog([FromBody] LogDetails[] model)
        {
            try
            {
                foreach (LogDetails log in model)
                {

                  
                    string sdate = log.StartDate;
                    string id = log.APID;
                    string stime = log.StartTime;
                    string edate = log.EndDate;
                    string etime = log.EndTime;
                    string tot = log.tothrs;
                    string reason = log.Reason;
                     
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveLogDetails(id, sdate, stime, edate, etime, tot, reason);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult InsertProBreak([FromBody] BreakDet[] model)
        {
            try
            {
                foreach (BreakDet det in model)
                {

                    string machine = det.MachineId;
                    string des = det.MachineDes;
                    string dtype = det.DType;
                    string mtype = det.MType;
                    string id = det.APID;
                    string stime = det.StartTime;
                    string pb = det.PB;
                    string etime = det.EndTime;
                    string reason = det.Reason;
                    string all = det.Alloted;
                    
                    DataTable dt = new DataTable();

                    dt = IProductionEntry.SaveBreakDetails(id, machine, des, dtype, mtype, stime, etime, pb, all, reason);
                }

                if (model != null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("An Error Has occoured");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetMachineDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();

				string name = "";
				string type = "";

				dt = IProductionEntry.GetMachineDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					name = dt.Rows[0]["MNAME"].ToString();
					type = dt.Rows[0]["MTYPE"].ToString();
				}
				var result = new { name = name, type = type };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public IActionResult ListAPProductionentry()
		{
			IEnumerable<APProductionentry> cmp = IProductionEntry.GetAllAPProductionentry();
			return View(cmp);
		}

		
		public ActionResult APProductionentryDetail(string id,string tag)
		{
			APProductionentryDet ca = new APProductionentryDet();
			//ca.Complete = "No";
            APProductionentry cy = new APProductionentry();
            List<BreakDet> TData3 = new List<BreakDet>();
            BreakDet tda3 = new BreakDet();
			List<ProInput> TData = new List<ProInput>();
			ProInput tda = new ProInput();
            List<ProOutput> TData4 = new List<ProOutput>();
            ProOutput tda4 = new ProOutput();
            List<APProInCons> TData1 = new List<APProInCons>();
			APProInCons tda1 = new APProInCons();
			List<EmpDetails> TTData2 = new List<EmpDetails>();
			EmpDetails tda2 = new EmpDetails();
            List<LogDetails> TTData5 = new List<LogDetails>();
            LogDetails tda5 = new LogDetails();
            if (tag == "2")
			{
				for (int i = 0; i < 3; i++)
				{
					tda3 = new BreakDet();

					tda3.Machinelst = BindMachineID();
					tda3.Emplst = BindEmp();
					tda3.Isvalid = "Y";
					tda3.APID = id;
					TData3.Add(tda3);

				}
				for (int i = 0; i < 3; i++)
				{
					tda = new ProInput();
					tda.APID = id;
					tda.Itemlst = BindItemlst();

					tda.Isvalid = "Y";
					TData.Add(tda);

				}
				for (int i = 0; i < 1; i++)
				{
					tda4 = new ProOutput();
					tda4.APID = id;
					tda4.Itemlst = BindOutItemlst();
					tda4.drumlst = BindDrum();
                    tda4.statuslst=BindStatus();
					tda4.Isvalid = "Y";
					TData4.Add(tda4);

				}
				for (int i = 0; i < 1; i++)
				{
					tda1 = new APProInCons();
					tda1.Itemlst = BindItemlstCon();
					tda1.Isvalid = "Y";
					tda1.APID = id;
					TData1.Add(tda1);
				}
				for (int i = 0; i < 3; i++)
				{
					tda2 = new EmpDetails();
					tda2.APID = id;
					tda2.Employeelst = BindEmp();
					tda2.Isvalid = "Y";
					TTData2.Add(tda2);
				}
                for (int i = 0; i < 1; i++)
                {
                    tda5 = new LogDetails();
                    tda5.APID = id;
                    string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                    tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                    tda5.StartTime = DateTime.Now.ToString("HH:mm");
                    DateTime dateTime = DateTime.Parse(tda5.StartDate);
                    //TimeSpan t1 = new TimeSpan(24,0,0);


                    int hours = int.Parse(ShiftTime);
                    TimeSpan t2 = new TimeSpan(hours, 0, 0);
                    DateTime resultDateTime = dateTime + t2;
                    tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                    string[] sdateList = tda5.StartDate.Split(" ");
                    string sdate = "";
                    string stime = "";
                    if (sdateList.Length > 0)
                    {
                        sdate = sdateList[0];
                        stime = sdateList[1];
                    }
                    string[] edateList = tda5.EndDate.Split(" - ");
                    string endate = "";
                    string endtime = "";
                    if (sdateList.Length > 0)
                    {
                        endate = edateList[0];
                        endtime = edateList[1];
                    }
                    tda5.StartDate = sdate;
                    tda5.EndDate = endate;

                    tda5.EndTime = endtime;

                    tda5.Isvalid = "Y";
                    TTData5.Add(tda5);

                }
            }
			if (!string.IsNullOrEmpty(id))
			{

			
            DataTable dt = new DataTable();

            dt = IProductionEntry.GetAPProd(id);
            if (dt.Rows.Count > 0)
            {
                ca.Location = dt.Rows[0]["WCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Eng = dt.Rows[0]["EMPNAME"].ToString();
                ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                ViewBag.shift= dt.Rows[0]["SHIFT"].ToString();
                    ca.SchQty = dt.Rows[0]["SCHQTY"].ToString();
               ca.LOCID= dt.Rows[0]["ILOCATION"].ToString();
                    ca.BranchId= dt.Rows[0]["BRANCHID"].ToString();
                    //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                    ca.ProdQty = dt.Rows[0]["PRODQTY"].ToString();
                //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCH"].ToString();
                ca.batchcomplete = dt.Rows[0]["BATCHYN"].ToString();
                    ca.APID = id;
                }
            DataTable dt2 = new DataTable();
                DataTable dtstk = new DataTable();
            dt2 = IProductionEntry.GetInput(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ProInput();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Time = dt2.Rows[i]["CHARGINGTIME"].ToString();
                        tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.batchno = dt2.Rows[i]["BATCHNO"].ToString();
                    tda.IssueQty = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString() == "" ? "0" : dt2.Rows[i]["QTY"].ToString());
                        dtstk = IProductionEntry.Getstkqty(tda.ItemId, ca.LOCID, ca.BranchId);
                        if (dtstk.Rows.Count > 0)
                        {
                            tda.StockAvailable = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                        }
                        tda.APID = id;
					 tda.Isvalid = "Y";
                    TData.Add(tda);
                    
                }

            }
            DataTable dt3 = new DataTable();
            dt3 = IProductionEntry.GetCons(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new APProInCons();
                    tda1.Itemlst = BindItemlstCon();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.consunit = dt3.Rows[i]["UNITID"].ToString();
                    tda1.BinId = dt3.Rows[i]["BINID"].ToString();
                    tda1.Qty = Convert.ToDouble(dt3.Rows[i]["QTY"].ToString() == "" ? "0" : dt3.Rows[i]["QTY"].ToString());
                    tda1.consQty = Convert.ToDouble(dt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : dt3.Rows[i]["CONSQTY"].ToString());
                    tda1.ConsStock = Convert.ToDouble(dt3.Rows[i]["STOCK"].ToString() == "" ? "0" : dt3.Rows[i]["STOCK"].ToString());

                    tda1.APID = id;
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }

            DataTable dt4 = new DataTable();
            dt4 = IProductionEntry.GetEmpdet(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new EmpDetails();
                    tda2.Employeelst = BindEmp();
                    tda2.Employee = dt4.Rows[i]["EMPID"].ToString();
                    
                    tda2.EmpCode = dt4.Rows[i]["EMPCODE"].ToString();
                    tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                    tda2.StartDate = dt4.Rows[i]["STARTDATE"].ToString();
                    tda2.StartTime = dt4.Rows[i]["STARTTIME"].ToString();
                    tda2.EndDate = dt4.Rows[i]["ENDDATE"].ToString();
                    tda2.EndTime = dt4.Rows[i]["ENDTIME"].ToString();
                    tda2.OTHrs = dt4.Rows[i]["OTHOUR"].ToString();

                    tda2.ETOther = dt4.Rows[i]["ETOTHER"].ToString();
                    tda2.Normal = dt4.Rows[i]["NHOUR"].ToString();
                    tda2.NOW = dt4.Rows[i]["NATUREOFWORK"].ToString();
                    tda2.ID = id;
                        tda2.Isvalid = "Y";
                        TTData2.Add(tda2);
                    
                }

            }
            DataTable dt5 = new DataTable();
            dt5 = IProductionEntry.GetBreak(id);
            if (dt5.Rows.Count > 0)
            {
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new BreakDet();
                    tda3.Machinelst = BindMachineID();
                    tda3.MachineId = dt5.Rows[i]["MACHCODE"].ToString();
                    tda3.Emplst = BindEmp();
                    tda3.MachineDes = dt5.Rows[i]["DESCRIPTION"].ToString();
                    tda3.StartTime = dt5.Rows[i]["FROMTIME"].ToString();
                    tda3.EndTime = dt5.Rows[i]["TOTIME"].ToString();
                    tda3.PB = dt5.Rows[i]["PB"].ToString();
                    tda3.Isvalid = "Y";
                    tda3.Alloted = dt5.Rows[i]["ALLOTTEDTO"].ToString();
                    tda3.DType = dt5.Rows[i]["DTYPE"].ToString();
                    tda3.MType = dt5.Rows[i]["MTYPE"].ToString();
                    tda3.Reason = dt5.Rows[i]["REASON"].ToString();
                    
                    tda3.APID = id;
                    TData3.Add(tda3);
                }

            }
            DataTable dt6 = new DataTable();

            dt6 = IProductionEntry.GetOutput(id);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new ProOutput();
                    tda4.Itemlst = BindOutItemlst();
                    tda4.ItemId = dt6.Rows[i]["ITEMID"].ToString();
                    tda4.BinId = dt6.Rows[i]["BINID"].ToString();
                    tda4.drumlst = BindDrum();
                        tda4.statuslst = BindStatus();
                        tda4.StID = dt6.Rows[i]["STATUS"].ToString();
                        tda4.ExcessQty = Convert.ToDouble(dt6.Rows[i]["EXQTY"].ToString() == "" ? "0" : dt6.Rows[i]["EXQTY"].ToString());
                        tda4.drumno = dt6.Rows[i]["DRUMNO"].ToString();
                        tda4.FromTime = dt6.Rows[i]["FROMTIME"].ToString();
                        tda4.ToTime = dt6.Rows[i]["TOTIME"].ToString();
                        tda4.OutputQty = Convert.ToDouble(dt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : dt6.Rows[i]["OUTQTY"].ToString());
                    DataTable dt7 = new DataTable();
                    dt7 = IProductionEntry.GetResult(id);
					if (dt7.Rows.Count > 0)
					{
                        tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                        tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                    }
                        tda4.APID = id;
                        tda4.Isvalid = "Y";
                        TData4.Add(tda4);
                   
                }

            }
                DataTable adt7 = new DataTable();

                adt7 = IProductionEntry.GetLogdetail(id);
                if (adt7.Rows.Count > 0)
                {
                    for (int i = 0; i < dt6.Rows.Count; i++)
                    {
                        tda5 = new LogDetails();

                        tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                        tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();

                        tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                        tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                        tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                        tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();

                        tda5.APID = id;
                        TTData5.Add(tda5);
                        tda5.Isvalid = "Y";
                    }

                }
            }
            if (tag=="1")
			{
              
                DataTable ap = datatrans.GetData("select APPRODUCTIONBASICID,DOCID,DOCDATE,SHIFT from APPRODUCTIONBASIC WHERE IS_CURRENT='Yes'");
				if (ap.Rows.Count > 0)
				{
					string apID = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where IS_CURRENT='Yes' ");

                    DataTable adt = new DataTable();
                    DataTable dt6 = new DataTable();
                    adt = IProductionEntry.GetAPProd(apID);
                    if (adt.Rows.Count > 0)
                    {
                        ca.Location = adt.Rows[0]["WCID"].ToString();
                        ca.Docdate = adt.Rows[0]["DOCDATE"].ToString();
                        ca.DocId = adt.Rows[0]["DOCID"].ToString();
                        ca.Eng = adt.Rows[0]["EMPNAME"].ToString();
                        ca.Shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.SchQty = adt.Rows[0]["SCHQTY"].ToString();
                        ViewBag.shift = adt.Rows[0]["SHIFT"].ToString();
                        ca.LOCID = adt.Rows[0]["ILOCATION"].ToString();
                        ca.BranchId = adt.Rows[0]["BRANCHID"].ToString();
                        //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                        ca.ProdQty = adt.Rows[0]["PRODQTY"].ToString();
                        //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                        ca.BatchNo = adt.Rows[0]["BATCH"].ToString();
                        ca.batchcomplete = adt.Rows[0]["BATCHYN"].ToString();
                        ca.APID= apID;
                    }
                  
                    DataTable adt2 = new DataTable();
                    DataTable dtstk = new DataTable();
                    adt2 = IProductionEntry.GetInput(apID);
                    if (adt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt2.Rows.Count; i++)
                        {
                            tda = new ProInput();
                            tda.Itemlst = BindItemlst();
                            tda.ItemId = adt2.Rows[i]["ITEMID"].ToString();
                            tda.BinId = adt2.Rows[i]["BINID"].ToString();
                            tda.Time = adt2.Rows[i]["CHARGINGTIME"].ToString();
                            tda.batchno = adt2.Rows[i]["BATCHNO"].ToString();
                            tda.IssueQty = Convert.ToDouble(adt2.Rows[i]["QTY"].ToString() == "" ? "0" : adt2.Rows[i]["QTY"].ToString());
                            dtstk = IProductionEntry.Getstkqty(tda.ItemId, ca.LOCID, ca.BranchId);
                            if(dtstk.Rows.Count > 0)
                            {
                                tda.StockAvailable = Convert.ToDouble(dtstk.Rows[0]["QTY"].ToString() == "" ? "0" : dtstk.Rows[0]["QTY"].ToString());
                            }
                            //tda.StockAvailable = Convert.ToDouble(adt2.Rows[i]["STOCK"].ToString() == "" ? "0" : adt2.Rows[i]["STOCK"].ToString());
                            tda.APID = apID;
                            TData.Add(tda);
                            tda.Isvalid = "Y";
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new ProInput();
                            tda.APID = apID;
                            tda.Itemlst = BindItemlst();

                            tda.Isvalid = "Y";
                            TData.Add(tda);

                        }
                    }
                    DataTable adt3 = new DataTable();
                    adt3 = IProductionEntry.GetCons(apID);
                    if (adt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt3.Rows.Count; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon();
                            tda1.ItemId = adt3.Rows[i]["ITEMID"].ToString();
                            tda1.consunit = adt3.Rows[i]["UNITID"].ToString();
                            tda1.BinId = adt3.Rows[i]["BINID"].ToString();
                            tda1.Qty = Convert.ToDouble(adt3.Rows[i]["QTY"].ToString() == "" ? "0" : adt3.Rows[i]["QTY"].ToString());
                            tda1.consQty = Convert.ToDouble(adt3.Rows[i]["CONSQTY"].ToString() == "" ? "0" : adt3.Rows[i]["CONSQTY"].ToString());
                            tda1.ConsStock = Convert.ToDouble(adt3.Rows[i]["STOCK"].ToString() == "" ? "0" : adt3.Rows[i]["STOCK"].ToString());

                            tda1.APID = apID;
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new APProInCons();
                            tda1.Itemlst = BindItemlstCon();
                            tda1.Isvalid = "Y";
                            tda1.APID = apID;
                            TData1.Add(tda1);
                        }
                    }

                    DataTable adt4 = new DataTable();
                    adt4 = IProductionEntry.GetEmpdet(apID);
                    if (adt4.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt4.Rows.Count; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.Employeelst = BindEmp();
                            tda2.Employee = adt4.Rows[i]["EMPID"].ToString();

                            tda2.EmpCode = adt4.Rows[i]["EMPCODE"].ToString();
                            tda2.Depart = adt4.Rows[i]["DEPARTMENT"].ToString();
                            tda2.StartDate = adt4.Rows[i]["STARTDATE"].ToString();
                            tda2.StartTime = adt4.Rows[i]["STARTTIME"].ToString();
                            tda2.EndDate = adt4.Rows[i]["ENDDATE"].ToString();
                            tda2.EndTime = adt4.Rows[i]["ENDTIME"].ToString();
                            tda2.OTHrs = adt4.Rows[i]["OTHOUR"].ToString();

                            tda2.ETOther = adt4.Rows[i]["ETOTHER"].ToString();
                            tda2.Normal = adt4.Rows[i]["NHOUR"].ToString();
                            tda2.NOW = adt4.Rows[i]["NATUREOFWORK"].ToString();
                            tda2.APID = apID;
                            TTData2.Add(tda2);
                            tda2.Isvalid = "Y";
                        }

                    }
					else
					{
                       
                        for (int i = 0; i < 1; i++)
                        {
                            tda2 = new EmpDetails();
                            tda2.APID = apID;
                            tda2.Employeelst = BindEmp();
                            tda2.Isvalid = "Y";
                            TTData2.Add(tda2);
                        }
                    }
                    DataTable adt5 = new DataTable();
                    adt5 = IProductionEntry.GetBreak(apID);
                    if (adt5.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt5.Rows.Count; i++)
                        {
                            tda3 = new BreakDet();
                            tda3.Machinelst = BindMachineID();
                            tda3.MachineId = adt5.Rows[i]["MACHCODE"].ToString();
                            tda3.Emplst = BindEmp();
                            tda3.MachineDes = adt5.Rows[i]["DESCRIPTION"].ToString();
                            tda3.StartTime = adt5.Rows[i]["FROMTIME"].ToString();
                            tda3.EndTime = adt5.Rows[i]["TOTIME"].ToString();
                            tda3.PB = adt5.Rows[i]["PB"].ToString();
                            tda3.Isvalid = "Y";
                            tda3.Alloted = adt5.Rows[i]["ALLOTTEDTO"].ToString();
                            tda3.DType = adt5.Rows[i]["DTYPE"].ToString();
                            tda3.MType = adt5.Rows[i]["MTYPE"].ToString();
                            tda3.Reason = adt5.Rows[i]["REASON"].ToString();

                            tda3.APID = apID;
                            TData3.Add(tda3);
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda3 = new BreakDet();

                            tda3.Machinelst = BindMachineID();
                            tda3.Emplst = BindEmp();
                            tda3.Isvalid = "Y";
                            tda3.APID = apID;
                            TData3.Add(tda3);

                        }
                    }
                    DataTable adt6 = new DataTable();

                    adt6 = IProductionEntry.GetOutput(apID);
                    if (adt6.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt6.Rows.Count; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.Itemlst = BindOutItemlst();
                            tda4.ItemId = adt6.Rows[i]["ITEMID"].ToString();
                            tda4.saveitemId= adt6.Rows[i]["ITEMNAME"].ToString();
                            tda4.BinId = adt6.Rows[i]["BINID"].ToString();
                            tda4.drumlst = BindDrum();
                            tda4.statuslst=BindStatus();
                            tda4.StID = adt6.Rows[i]["STATUS"].ToString();
                            tda4.ExcessQty= Convert.ToDouble(adt6.Rows[i]["EXQTY"].ToString() == "" ? "0" : adt6.Rows[i]["EXQTY"].ToString());
                            tda4.drumno = adt6.Rows[i]["DRUMNO"].ToString();
                            tda4.FromTime = adt6.Rows[i]["FROMTIME"].ToString();
                            tda4.ToTime = adt6.Rows[i]["TOTIME"].ToString();
                            tda4.OutputQty = Convert.ToDouble(adt6.Rows[i]["OUTQTY"].ToString() == "" ? "0" : adt6.Rows[i]["OUTQTY"].ToString());
                            tda4.Result = adt6.Rows[i]["TESTRESULT"].ToString();
                            tda4.Status = adt6.Rows[i]["MOVETOQC"].ToString();
                           DataTable dt7 = new DataTable();
                            dt7 = IProductionEntry.GetResult(id);
                            if (dt7.Rows.Count > 0)
                            {
                                tda4.Result = dt7.Rows[i]["TESTRESULT"].ToString();
                                tda4.Status = dt7.Rows[i]["MOVETOQC"].ToString();
                            }
                            tda4.APID = adt6.Rows[i]["APPRODOUTDETID"].ToString();
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);
                           
                        }

                    }
					else
					{
                        for (int i = 0; i < 1; i++)
                        {
                            tda4 = new ProOutput();
                            tda4.APID = apID;
                            tda4.Itemlst = BindOutItemlst();
                            tda4.drumlst = BindDrum();
                            tda4.statuslst = BindStatus();
                            tda4.StID = "COMPLETED";
                            tda4.Result = "";
                            tda4.Isvalid = "Y";
                            TData4.Add(tda4);

                        }
                    }
                    DataTable adt7 = new DataTable();

                    adt7 = IProductionEntry.GetLogdetail(apID);
                    if (adt7.Rows.Count > 0)
                    {
                        for (int i = 0; i < adt7.Rows.Count; i++)
                        {
                            tda5 = new LogDetails();
                          
                            tda5.StartDate = adt7.Rows[i]["STARTDATE"].ToString();
                            tda5.StartTime = adt7.Rows[i]["STARTTIME"].ToString();
                           
                            tda5.EndDate = adt7.Rows[i]["ENDDATE"].ToString();

                            tda5.Reason = adt7.Rows[i]["REASON"].ToString();



                            tda5.EndTime = adt7.Rows[i]["ENDTIME"].ToString();
                                tda5.tothrs = adt7.Rows[i]["TOTALHRS"].ToString();
                         
                            tda5.APID = apID;
                            TTData5.Add(tda5);
                            tda5.Isvalid = "Y";
                        }

                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda5 = new LogDetails();
                            tda5.APID = apID;
                            string ShiftTime = datatrans.GetDataString("Select SHIFTHRS from SHIFTMAST where shiftno='" + ca.Shift + "' ");
                            tda5.StartDate = DateTime.Now.ToString("dd-MMM-yyyy  HH:mm:ss");
                            tda5.StartTime = DateTime.Now.ToString("HH:mm");
                            DateTime dateTime = DateTime.Parse(tda5.StartDate);
                            //TimeSpan t1 = new TimeSpan(24,0,0);
                             
                           
                            int hours = int.Parse(ShiftTime);
                            TimeSpan t2 = new TimeSpan(hours,0,0);
                            DateTime resultDateTime = dateTime + t2;
                            tda5.EndDate = resultDateTime.ToString("dd-MMM-yyyy - HH:mm");

                            string[] sdateList = tda5.StartDate.Split(" ");
                            string sdate = "";
                            string stime = "";
                            if (sdateList.Length > 0)
                            {
                                sdate = sdateList[0];
                                stime = sdateList[1];
                            }
                            string[] edateList = tda5.EndDate.Split(" - ");
                            string endate = "";
                            string endtime = "";
                            if (sdateList.Length > 0)
                            {
                                endate = edateList[0];
                                endtime = edateList[1];
                            }
                            tda5.StartDate = sdate;
                            tda5.EndDate = endate;
                            
                            tda5.EndTime = endtime;
                             
                           tda5.Isvalid = "Y";
                            TTData5.Add(tda5);
                          
                        }
                    }
                }
				else
				{
                    return RedirectToAction("APProductionentry");
                }
            }
            ca.BreakLst = TData3;
			ca.inplst = TData;
            ca.outlst = TData4;
            ca.EmplLst = TTData2;
			ca.Binconslst = TData1;
            ca.LogLst = TTData5;

            return View(ca);
		}
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "COMPLETED", Value = "COMPLETED" });
                lstdesg.Add(new SelectListItem() { Text = "PENDING", Value = "PENDING" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult APProductionentryDetail(APProductionentryDet Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = IProductionEntry.APProductionEntryDetCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "APProductionentryDetail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "APProductionentryDetail Updated Successfully...!";
                    }
                    return RedirectToAction("APProductionentryDetail", new { id = Cy.inplst[0].APID });
                }

                else
                {
                    ViewBag.PageTitle = "Edit APProductionentryDetail";
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

        public ActionResult APProdApprove(string id)
        {
            APProductionentryDet ca = new APProductionentryDet();
            ca.ID = id;
            DataTable dt = datatrans.GetData("Select SHIFTNO from SHIFTMAST WHERE SHIFTNO IN ('A','B','C') and SHIFTNO not IN  (Select Shift from  APPRODUCTIONBASIC where DOCID=(select DOCID from APPRODUCTIONBASIC where APPRODUCTIONBASICID='" + id + "'))");
            List<string> list = new List<string>();
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]["SHIFTNO"].ToString());
            }
            list.Add("Complete");
            ca.ShiftNames = list;
            return View(ca);
        }
        [HttpPost]
        public ActionResult APProdApprove(APProductionentryDet Cy, string id)
        {
			
				try
				{
                //Cy.ID = id;
                string Strout = IProductionEntry.APProEntryCRUD(Cy);
					if (string.IsNullOrEmpty(Strout))
					{
						return RedirectToAction("APProductionentryDetail", new { idasd = Cy.APID });
					}

					else
					{
						ViewBag.PageTitle = "Edit APProductionentryDetail";
						TempData["notice"] = Strout;
						//return View();
					}

					// }
				}
				catch (Exception ex)
				{
					throw ex;
				}
			
			
                return RedirectToAction("ListAPProductionentry");
            
            //return View(Cy);
        }
        public ActionResult GetItemDetail(string ItemId, string branch,string loc)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
                string stk = "";
                dt = IProductionEntry.GetItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();

				}
                dt1 = IProductionEntry.Getstkqty(ItemId, loc, branch);
                if (dt1.Rows.Count > 0)
                {
                    stk = dt1.Rows[0]["QTY"].ToString();
                }
                if (stk == "")
                {
                    stk = "0";
                }
                var result = new { bin = bin, binid= binid, stk = stk };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public ActionResult GetStockQty(string Lot, string branch, string loc,string item)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string locid = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + loc + "' ");

                string stk = "";
                dt = IProductionEntry.GetStkDetails(Lot, branch, locid, item);

                if (dt.Rows.Count > 0)
                {

                    stk = dt.Rows[0]["BALANCE_QTY"].ToString();
                   

                }
               
                var result = new { stk = stk };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst());
        }
        public JsonResult GetconsItemJSON()
        {
            return Json(BindItemlstCon());
        }
        public JsonResult GetOutItemJSON()
        {
            //EnqItem model = new EnqItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindOutItemlst());
        }
        public JsonResult GetDrumJSON()
        {
            return Json(BindDrum());
        }
        public ActionResult SaveOutDetail(string id,string ItemId,string drum,string time,string qty,string totime,string exqty,string stat)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = IProductionEntry.SaveOutDetails(id,ItemId, drum, time, qty, totime,exqty,stat);

                 

                var result ="";
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetQCResult(string id, string ItemId, string drum)
        {
            try
            {
                var res = "No";
                var resdetail = "";
                int cunt = datatrans.GetDataId("select count(APPRODOUTDETID) as cunt  from APPRODOUTDET where TESTRESULT is not null and APPRODOUTDETID='" + id + "'");
                if (cunt > 0)
                {
                    res = "Yes";
                   resdetail= datatrans.GetDataString("select TESTRESULT  from APPRODOUTDET where TESTRESULT is not null and APPRODOUTDETID='" + id + "'");
                }
                var result = new { res = res, resdetail = resdetail };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetOutItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string bin = "";
                string binid = "";
                dt = IProductionEntry.GetOutItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    bin = dt.Rows[0]["BINID"].ToString();
                    binid = dt.Rows[0]["bin"].ToString();

                }

                var result = new { bin = bin, binid = binid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetConItemDetail(string ItemId)
		{
			try
			{
				DataTable dt = new DataTable();
				DataTable dt1 = new DataTable();

				string bin = "";
				string binid = "";
				string unit = "";
				string unitid = "";

				dt = IProductionEntry.GetConItemDetails(ItemId);

				if (dt.Rows.Count > 0)
				{

					bin = dt.Rows[0]["BINID"].ToString();
					binid = dt.Rows[0]["bin"].ToString();
					unit = dt.Rows[0]["UNITID"].ToString();
					unitid = dt.Rows[0]["unit"].ToString();

				}

				var result = new { bin = bin, binid = binid, unit= unit , unitid = unitid };
				return Json(result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public async Task<IActionResult> Print(string id)

        {

            string mimtype = "";
            int extension = 1;
            DataTable ap = datatrans.GetData("Select SHIFT from APPRODUCTIONBASIC where DOCID='" + id + "' ");
            string a = ap.Rows[0]["SHIFT"].ToString();
            string b = ap.Rows[1]["SHIFT"].ToString();
            string c = ap.Rows[2]["SHIFT"].ToString();

            string aid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + a + "' and DOCID='" + id + "' ");
            string bid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + b + "'  and DOCID='" + id + "' ");
            string cid = datatrans.GetDataString("Select APPRODUCTIONBASICID from APPRODUCTIONBASIC where SHIFT='" + c + "'  and DOCID='" + id + "' ");
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Production.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");

            var APitem = await IProductionEntry.GetAPItem(aid);

            var APitems = await IProductionEntry.GetAPItems(bid);
            var APitemsc = await IProductionEntry.GetAPItemsc(cid);
            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("APProduction", APitem);
            localReport.AddDataSource("APOut", APitems);
            localReport.AddDataSource("APCShift", APitemsc);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }
    }
}
