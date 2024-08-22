using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;

using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services.Report
{
    public class FinishedGoodsStockDetailsDrumwiseService : IFinishedGoodsStockDetailsDrumwise
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public FinishedGoodsStockDetailsDrumwiseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
       
        public DataTable GetAllFinishedGoodsStockDetailsDrumwise(string dtFrom,string Location)
        {
            try
            {
                string SvSql = "";
                SvSql = "Select  M.Docdate,L.LocID , I.Itemid,M.LotNo BatchNo, M.DrumNo, Sum(S.PlusQty-S.MinusQty) Qty , M.Rate Rate,PackInsFlag , Decode(PACKINSFLAG,0,'Q.C Not Completed','Ready to Despatch') Status,sysdate Asondate, To_date(sysdate, 'DD/MM/YYYY')-To_Date(m.docdate, 'DD/MM/YYYY') Laydays From PLotMast M, PLStockValue S,LOCDETAILS L, ITEMMASTER I, SelectedValues SS Where M.LotNo = S.LotNo And S.LocID = L.LOCDETAILSID And S.ItemID = I.ItemMasterID And S.Cancel = 'F' And M.DrumNo is Not Null And(L.LocID = '" + Location + "' Or 'ALL' = '" + Location + "') And S.DocDate <= '" + dtFrom + "' And I.ItemMasterID = SS.SelectedID And Not Exists(Select DrumNo from DrumMast D Where D.DrumNo = M.DrumNo) Group By M.Docdate,L.LocID , I.itemid,M.PLotMastID, M.DrumNo, M.LotNo, M.Rate,PackInsFlag Having((Sum(S.PlusQty - S.MinusQty) > 0)) Order by  2, 3, 9, 5";


                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILSID,LOCID from LOCDETAILS  WHERE IS_ACTIVE='Y' Union Select 1,'ALL' From Dual Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
