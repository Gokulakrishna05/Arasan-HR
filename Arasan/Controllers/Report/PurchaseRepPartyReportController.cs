using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Controllers.Report
{
    public class PurchaseRepPartyReportController : Controller
    {
        IPurchaseRepPartyReportService PurchaseRepPartyReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurchaseRepPartyReportController(IPurchaseRepPartyReportService _PurchaseRepPartyReportService, IConfiguration _configuratio)
        {
            PurchaseRepPartyReportService = _PurchaseRepPartyReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseRepPartyReport(string strfrom, string strTo)
        {
            try
            {
                PurchaseRepPartyReport objR = new PurchaseRepPartyReport();
                objR.Brlst = BindBranch();
                objR.Suplst = BindSupplier();
                objR.ItemGrouplst = BindItemGrplst();
                objR.Itemlst = BindItemlst("");
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListPurchaseRepPartyReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = PurchaseRepPartyReportService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
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
                DataTable dtDesg = datatrans.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYNAME"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            PurchaseRepPartyReport model = new PurchaseRepPartyReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListPurchaseRepPartyReportGrid(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            List<PurchaseRepPartyReportItems> Reg = new List<PurchaseRepPartyReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurchaseRepPartyReportService.GetAllPurchasePartyReport(dtFrom, dtTo, Branch, Customer, Item);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new PurchaseRepPartyReportItems
                {
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),
                    sgst = dtUsers.Rows[i]["SGST"].ToString(),
                    cgst = dtUsers.Rows[i]["CGST"].ToString(),
                    igst = dtUsers.Rows[i]["IGST"].ToString(),
                    rem = dtUsers.Rows[i]["REM"].ToString(),
                    ind = dtUsers.Rows[i]["INDDEPT"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),





                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ExportToExcel(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            DataTransactions _datatransactions;

            DataTable dtNew = new DataTable();

            string SvSql = "Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,SGST,CGST,IGST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID ,P.PartyID , B.DocID , B.DocDate , to_char(B.RefNo) RefNo , B.RefDt ,  I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount,D.SGST,D.CGST,D.IGST, Pd.Narration Rem,l.locid inddept,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM DpBasic B , PartyMast P , BranchMast Br , DpDetail D , ItemMaster I , UnitMast U , PindDetail Pd,locdetails l WHERE B.PartyID = P.PartyMastID AND B.DpBasicID = D.DpBasicID AND B.BranchID = Br.BranchMastID AND D.ItemID = I.ItemMasterID AND D.Unit = U.UnitMastID AND Pd.PindDetailID = D.PindDetailID AND pd.DEPARTMENT = l.LOCDETAILSID)";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and Docdate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and BranchID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Customer != null)
            {
                SvSql += " and PartyID='" + Customer + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Item != null)
            {
                SvSql += " and ItemID='" + Item + "'";
            }
            else
            {
                SvSql += "";
            }
            SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,SGST,CGST,IGST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID ,P.PartyID , B.DocID , B.DocDate , to_char(B.RefNo) RefNo , B.RefDt ,  I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount,D.SGST,D.CGST,D.IGST , Pd.Narration Rem,l.locid inddept,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM grnBLBasic B , PartyMast P , BranchMast Br , grnBLDetail D , ItemMaster I , UnitMast U , PoDetail Pod , PindDetail Pd,locdetails l WHERE B.PartyID = P.PartyMastID AND B.grnBLBasicID = D.grnBLBasicID AND B.BranchID = Br.BranchMastID AND D.ItemID = I.ItemMasterID AND D.Unit = U.UnitMastID AND D.PoDetailID = Pod.PoDetailID AND Pd.PindDetailID = Pod.PindDetailID AND pd.DEPARTMENT = l.LOCDETAILSID) ";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and BranchID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Customer != null)
            {
                SvSql += " and PartyID='" + Customer + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Item != null)
            {
                SvSql += " and ItemID='" + Item + "'";
            }
            else
            {
                SvSql += "";
            }
            SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,0 GST,0,0,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID , P.PartyID , B.DocID , B.DocDate , B.RefNo , B.RefDate refdt , I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount, Pd.Narration Rem,l.locid inddept ,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM IGrnBasic B,IGrndetail D,PartyMast P , BranchMast Br,ItemMaster I, UnitMast U , IPodetail Pod,IPinddetail Pd,IPindbasic Pb,IPobasic PoB,locdetails l WHERE B.PartyID = P.PartyMastID AND PD.itemid = Pod.Itemid AND Pb.IPindbasicid = Pd.IPindbasicid AND Pod.Indentno = Pb.Docid AND B.igrnBasicID = D.IgrnBasicID AND B.BranchID = Br.BranchMastID AND D.ItemmasterID = I.Itemmasterid AND D.Unit = U.UnitMastID AND Pob.Ipobasicid = Pod.Ipobasicid AND D.Refnod = Pob.Ipobasicid AND pd.DEPARTMENT = l.LOCDETAILSID)";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and BranchID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Customer != null)
            {
                SvSql += " and PartyID='" + Customer + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Item != null)
            {
                SvSql += " and ItemID='" + Item + "'";
            }
            else
            {
                SvSql += "";
            }
            SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,0,0,0 GST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT B.Branchid,P.Partyid,S.Docid,S.Docdate ,S.Refno,S.Refdate refdt,I.Itemid,SD.MUnit unit,SD.MrQty Qty,SD.BRate Rate,(SD.MrQty*SD.BRate) amount ,S.Narration Rem,'CONVERSION' Inddept ,0 net,lead(S.DOCID,1) Over(Order by P.partyid,S.docdate,S.docid) Ndoc FROM SubMRBasic S,Branchmast B,Partymast P,subactmrdet SD,Itemmaster I WHERE B.Branchmastid = S.Branch AND P.Partymastid = S.Partyid AND S.SubMRBasicid = SD.SubMRBasicid AND SD.Mitemid = I.Itemmasterid)";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and BranchID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Customer != null)
            {
                SvSql += " and PartyID='" + Customer + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Item != null)
            {
                SvSql += " and ItemID='" + Item + "'";
            }
            else
            {
                SvSql += "";
            }

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew, "PurchaseRepPartyWise");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PurchaseRepPartyWise.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PurchaseRepPartyWise.xlsx");
                    }
                }

                //OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                //DataTable dtReport = new DataTable();
                //adapter.Fill(dtReport);
                //return dtReport;}
            }

        }
        private IActionResult File(object excelData, string v)
        {
            throw new NotImplementedException();
        }
    }
}
