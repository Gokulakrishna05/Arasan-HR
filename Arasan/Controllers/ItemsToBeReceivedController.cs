using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;
using Arasan.Interface;
using Elasticsearch.Net;
using Arasan.Services.Report;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers
{
    public class ItemsToBeReceivedController : Controller
    {
        IItemsToBeReceived ItemsToBeReceivedService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public ItemsToBeReceivedController(IItemsToBeReceived _ItemsToBeReceivedService, IConfiguration _configuratio)
        {
            ItemsToBeReceivedService = _ItemsToBeReceivedService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemsToBeReceived(string strfrom)
        {
            try
            {
                ItemsToBeReceivedModel objR = new ItemsToBeReceivedModel();
 
                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetBranchJSON(string ItemId)
        //{

        //    return Json(BindMaster(ItemId));

        //}


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom)
        {
            List<ItemsToBeReceivedModelItems> Reg = new List<ItemsToBeReceivedModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)ItemsToBeReceivedService.GetAllItemsToBeReceived(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new ItemsToBeReceivedModelItems
                {

                    docno = dtUsers.Rows[i]["DOCNO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    partyid = dtUsers.Rows[i]["PARTYID"].ToString(),
                    locid = dtUsers.Rows[i]["LOCID"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    dqty = dtUsers.Rows[i]["DQTY"].ToString(),
                    rqty = dtUsers.Rows[i]["RQTY"].ToString(),
                    pendqty = dtUsers.Rows[i]["PENDQTY"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    expretdt = dtUsers.Rows[i]["EXPRETDT"].ToString(),
                    days = dtUsers.Rows[i]["DAYS"].ToString(),

                });
            }
            return Json(new
            {
                Reg
            });

        }
        




        public IActionResult ExportToExcel(string dtFrom)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " Select Db.DocID DcNo , Db.DocDate DcDt, P.PartyID , L.LocID , Dd.ItemID , Dd.UNIT , Sum(Dd.Qty) DQty , Sum(Dd.RecdQty) RQty , Sum(Dd.Qty - Dd.RecdQty ) PendQty,E.EMPNAME,DD.EXPRETDT,round((Sysdate-DD.EXPRETDT)+1,0) Days From RDelBasic Db , RDelDetail Dd, LocDetails L,Partymast P, Empmast E , SelectedValues S Where Db.RDelBasicID = Dd.RDelBasicID And L.LocDetailsID = Db.FromLocID And Db.DocDate <= '" + dtFrom + "' And E.EMPMASTID = Db.AppBY And P.PARTYMASTID = Db.PARTYID And L.LocDetailsID = S.SelectedID And Db.Docid Not Like 'Con%' And Db.Docid Not Like 'Ndc%' Group By Db.PartyID , L.LocID , Db.DocDate , Db.DocID , Dd.ItemID , dd.Unit, P.PartyID,E.EMPNAME,DD.EXPRETDT Having Sum(Dd.Qty - Dd.RecdQty) > 0 and(Sysdate - DD.EXPRETDT) > 0 Order By 12 desc,Db.PartyID , L.LocID , Db.DocDate , Db.DocID , Dd.ItemID , dd.Unit";

            //}

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "ItemsToBeReceivedDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=ItemsToBeReceivedDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "ItemsToBeReceivedDetails.xlsx");
                    }
                }

            }

        }
    }
}
