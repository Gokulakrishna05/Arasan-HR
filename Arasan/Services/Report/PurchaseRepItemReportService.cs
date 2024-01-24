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

namespace Arasan.Services.Report
{
    public class PurchaseRepItemReportService : IPurchaseRepItemReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseRepItemReportService(IConfiguration _configuratio)
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
        public DataTable GetAllPurchaseItemReport(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            
            try
            {
                string SvSql = "";
                if (dtFrom == null && Branch == null && Customer == null && Item == null)
                {
                    SvSql = "Select Br.BranchID , P.PartyID , Db.DocID ,to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno) Refno,DD.Costrate From DPBasic Db , DPDetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U  Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno),DD.Costrate From grnBLbasic Db , grnBLdetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U Where Db.grnBLbasicID = Dd.grnBLbasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit, Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,Db.Refno,DD.Costrate From igrnbasic Db, igrndetail Dd , ItemMaster I, PartyMast P , BranchMast Br, UnitMast U Where Db.igrnbasicID = Dd.igrnbasicID And Db.PartyID = P.PartyMastID And Dd.ItemmasterID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";

                }
                else
                {
                    SvSql = "Select Br.BranchID , P.PartyID , Db.DocID ,to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno) Refno,DD.Costrate From DPBasic Db , DPDetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U  Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";
                    if (dtFrom != null && dtTo != null)
                    {
                        SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                    }

                    if (Branch != null)
                    {
                        SvSql += " and Br.BranchID='" + Branch + "'";
                    }

                    if (Customer != null)
                    {
                        SvSql += " and P.PartyID='" + Customer + "'";
                    }

                    if (Item != null)
                    {
                        SvSql += " and I.ItemID='" + Item + "'";
                    }

                    SvSql += "Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno),DD.Costrate From grnBLbasic Db , grnBLdetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U Where Db.grnBLbasicID = Dd.grnBLbasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";
                    if (dtFrom != null && dtTo != null)
                    {
                        SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                    }

                    if (Branch != null)
                    {
                        SvSql += " and Br.BranchID='" + Branch + "'";
                    }

                    if (Customer != null)
                    {
                        SvSql += " and P.PartyID='" + Customer + "'";
                    }

                    if (Item != null)
                    {
                        SvSql += " and I.ItemID='" + Item + "'";
                    }

                    SvSql += "Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit, Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,Db.Refno,DD.Costrate From igrnbasic Db, igrndetail Dd , ItemMaster I, PartyMast P , BranchMast Br, UnitMast U Where Db.igrnbasicID = Dd.igrnbasicID And Db.PartyID = P.PartyMastID And Dd.ItemmasterID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";
                    if (dtFrom != null && dtTo != null)
                    {
                        SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                    }

                    if (Branch != null)
                    {
                        SvSql += " and Br.BranchID='" + Branch + "'";
                    }

                    if (Customer != null)
                    {
                        SvSql += " and P.PartyID='" + Customer + "'";
                    }

                    if (Item != null)
                    {
                        SvSql += " and I.ItemID='" + Item + "'";
                    }
                }
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
