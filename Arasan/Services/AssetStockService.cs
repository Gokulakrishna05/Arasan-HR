 
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
    public class AssetStockService : IAssetStock
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AssetStockService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER Where SUBGROUPCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllAssetStock(string dtFrom, string dtTo, string Branch, string Location)
        {
            try
            {
                string SvSql = "";
                SvSql = " Select I.ItemID , U.UnitID , Br.BranchID , L.LocID , Sum(x.OQ) OQ , Sum(x.OV) OV , Sum(x.RQ) RQ , Sum(x.RV) RV , Sum(x.IQ) IQ , Sum(x.IV) IV From(Select Sv.ItemID , Sv.LocID , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,-Sv.Qty)) OQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, -Sv.StockValue)) OV , 0 RQ , 0 RV , 0 IQ , 0 IV From AsStockValue Sv Where Sv.DocDate < '" + dtFrom + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID Union All Select Sv.ItemID , Sv.LocID , 0 OQ , 0 OV , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.Qty, 0)) RQ , Sum(Decode(Sv.PlusOrMinus, 'p', Sv.StockValue, 0)) RV   , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.Qty, 0)) IQ , Sum(Decode(Sv.PlusOrMinus, 'm', Sv.StockValue, 0)) IV From AsStockValue Sv Where Sv.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And Sv.Cancel<> 'T' Group By Sv.ItemID , Sv.LocID) x,  ItemMaster I, LocDetails L , UnitMast U, BranchMast Br Where x.ItemID = I.ItemMasterID And x.LocID = L.LocDetailsID And L.BranchID = Br.BranchMastID And U.UnitMastID = I.PriUnit And Br.BranchID = '" + Branch + "' And(L.LocID = '" + Location + "' ) Group By Br.BranchID , L.LocID , I.ItemID , U.UnitID Order By Br.BranchID , L.LocID , I.ItemID , U.UnitID";

                //if (dtFrom == null && Branch == null && Location == null)
                //{
                //    SvSql = "Select I.ItemID , U.UnitID , Br.BranchID , L.LocID , Sum(x.OQ) OQ , Sum(x.OV) OV , Sum(x.RQ) RQ , Sum(x.RV) RV , Sum(x.IQ) IQ , Sum(x.IV) IV From (Select Sv.ItemID , Sv.LocID , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,-Sv.Qty)) OQ , Sum(Decode(Sv.PlusOrMinus,'p',Sv.StockValue,-Sv.StockValue)) OV , 0 RQ , 0 RV , 0 IQ , 0 IV  From AsStockValue Sv Where Sv.DocDate < :SD And Sv.Cancel <> 'T' Group By Sv.ItemID , Sv.LocID Union All Select Sv.ItemID , Sv.LocID , 0 OQ , 0 OV , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,0)) RQ , Sum(Decode(Sv.PlusOrMinus,'p',Sv.StockValue,0)) RV   , Sum(Decode(Sv.PlusOrMinus,'m',Sv.Qty,0)) IQ , Sum(Decode(Sv.PlusOrMinus,'m',Sv.StockValue,0)) IV From AsStockValue Sv Where Sv.DocDate Between :SD And :ED And Sv.Cancel <> 'T' Group By Sv.ItemID , Sv.LocID) x ,  ItemMaster I , LocDetails L , UnitMast U , BranchMast Br Where x.ItemID = I.ItemMasterID And x.LocID = L.LocDetailsID And L.BranchID = Br.BranchMastID And U.UnitMastID = I.PriUnit And Br.BranchID = :BranchID And ( L.LocID = :LocID Or 'ALL' = :LocID ) Group By Br.BranchID , L.LocID , I.ItemID , U.UnitID Order By Br.BranchID , L.LocID , I.ItemID , U.UnitID";

                //}
                //else
                //{
                //    SvSql = "Select I.ItemID , U.UnitID , Br.BranchID , L.LocID , Sum(x.OQ) OQ , Sum(x.OV) OV , Sum(x.RQ) RQ , Sum(x.RV) RV , Sum(x.IQ) IQ , Sum(x.IV) IV From (Select Sv.ItemID , Sv.LocID , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,-Sv.Qty)) OQ , Sum(Decode(Sv.PlusOrMinus,'p',Sv.StockValue,-Sv.StockValue)) OV , 0 RQ , 0 RV , 0 IQ , 0 IV  From AsStockValue Sv  Where Sv.DocDate Between :SD And :ED And Sv.Cancel <> 'T' Group By Sv.ItemID , Sv.LocID) x ,  ItemMaster I , LocDetails L , UnitMast U , BranchMast Br Where x.ItemID = I.ItemMasterID And x.LocID = L.LocDetailsID And L.BranchID = Br.BranchMastID And U.UnitMastID = I.PriUnit And Br.BranchID = :BranchID And ( L.LocID = :LocID Or 'ALL' = :LocID ) Group By Br.BranchID , L.LocID , I.ItemID , U.UnitID Order By Br.BranchID , L.LocID , I.ItemID , U.UnitID";
                //    if (dtFrom != null && dtTo != null)
                //    {
                //        SvSql += " Where Sv.DocDate < :SD And Sv.Cancel <> 'T' Group By Sv.ItemID , Sv.LocID ";
                //    }
                //    SvSql += " Union All Select Sv.ItemID , Sv.LocID , 0 OQ , 0 OV , Sum(Decode(Sv.PlusOrMinus,'p',Sv.Qty,0)) RQ , Sum(Decode(Sv.PlusOrMinus,'p',Sv.StockValue,0)) RV   , Sum(Decode(Sv.PlusOrMinus,'m',Sv.Qty,0)) IQ , Sum(Decode(Sv.PlusOrMinus,'m',Sv.StockValue,0)) IV From AsStockValue Sv ";

                //    if (dtFrom != null && dtTo != null)
                //    {
                //        SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                //    }

                //    if (Branch != null)
                //    {
                //        SvSql += " and Br.BranchID='" + Branch + "'";
                //    }

                //    if (Location != null)
                //    {
                //        SvSql += " and P.LocID='" + Location + "'";
                //    }




                //}
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
        public DataTable GetLocation(string BranchID)
        {
            string SvSql = string.Empty;
            string brid = datatrans.GetDataString("select BRANCHMASTID FROM BRANCHMAST WHERE BRANCHID = '"+ BranchID + "'");
            SvSql = "Select L.LocDetailsID , L.LocID Location From LocDetails L , BranchMast Br Where L.BranchID = Br.BranchMastID And ( L.BranchID = '"+ brid + "'  ) Union All Select 1 ID , 'ALL' From Dual Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
