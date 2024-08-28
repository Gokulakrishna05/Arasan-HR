
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
    public class RVDPowderStockInPasteService : IRVDPowderStockInPaste
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RVDPowderStockInPasteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllRVDPowderStockInPaste(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                SvSql = "Select x.DocDate , Sum(x.OpQty) OQ , Sum(x.RQty) RQ , Sum(x.IQty) IQ\r\nFrom \r\n(\r\n\r\nSelect '"+dtFrom+"' DocDate , Sum(Qty) OpQty , 0 RQty , 0 IQty\r\nFrom \r\n(\r\nSelect TL.LocationType , Ls.LotNo , I.ItemID , Ls.DrumNo , Sum(Ls.PlusQty)-Sum(Ls.MinusQty) Qty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.DrumNo Is Not Null\r\nAnd Ls.DocDate < '"+dtFrom+"'\r\nGroup By TL.LocationType , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHaving Sum(Ls.PlusQty)-Sum(Ls.MinusQty)  > 0\r\n)\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RecQty , 0 IssQty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I , LocDetails FL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.StockTransType In ('DRUM ISSUE','CURING RECHARGE')\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd ( FL.LocationType = 'RVD' Or FL.LocID = 'RVD SHED' ) \r\nGroup By Ls.DocDate , TL.LocationType , FL.LocationType\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RecQty , Sum(Ls.MinusQty) IssQty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.StockTransType In ('BPROD INPUT')\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nGroup By Ls.DocDate , TL.LocationType\r\n\r\n) x\r\nGroup By x.DocDate\r\nOrder By x.DocDate";

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


    }
}
