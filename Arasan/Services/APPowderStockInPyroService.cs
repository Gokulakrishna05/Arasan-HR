
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
    public class APPowderStockInPyroService : IAPPowderStockInPyro
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public APPowderStockInPyroService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
       
        public DataTable GetAllAPPowderStockInPyro(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                //"SELECT z.Dt , SUM(z.OpQty) Op , SUM(z.RecQty) ApRec , SUM(z.IssQty) ApIss\r\nFROM \r\n(\r\nSELECT :SD Dt , SUM(Qty) OpQty, 0 RecQty, 0 IssQty\r\nFROM \r\n(\r\nSELECT SUM(Ls.PlusQty-Ls.MinusQty) Qty\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'BALL MILL'\r\nAND I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG')\r\nAND Ls.DocDate < :SD\r\nGROUP BY TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHAVING SUM(Ls.PlusQty-Ls.MinusQty)  > 0\r\n)\r\nUNION ALL";
                //) z GROUP BY z.Dt ORDER BY  z.Dt
                SvSql = "SELECT Ls.DocDate, 0 OpQty, SUM(Ls.PlusQty) RecQty, 0 IssQty FROM LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I WHERE Ls.StockTransType = 'DRUM ISSUE' AND Ls.ItemID = I.ItemMasterID AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType IN ('AP MILL','SIEVE & BLEND')  AND Ls.LocID = TL.LocDetailsID AND TL.LocationType = 'BALL MILL' AND (I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG') OR I.ITEMID='ATOMIS.ALU.GRADE A-150 MICRON') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY Ls.DocDate , FL.LocationType , TL.LocationType UNION ALL SELECT Ls.DocDate, 0 OpQty, 0 RecQty, SUM(Ls.MinusQty) IssQty FROM LStockValue Ls , LocDetails TL , ItemMaster I WHERE Ls.StockTransType in  ('PROD INPUT','Stock Reconcilation') AND Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND TL.LocationType = 'BALL MILL' AND (I.SNCategory IN ('AP POWDER - SFG','AP POWDER - FG') OR I.ITEMID='ATOMIS.ALU.GRADE A-150 MICRON') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' GROUP BY Ls.DocDate , TL.LocationType";


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
