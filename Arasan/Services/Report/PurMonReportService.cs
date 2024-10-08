﻿using Arasan.Interface.Report;
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

namespace Arasan.Services
{
    public class PurMonReportService : IPurMonReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurMonReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllReport(string SFINYR) 
        {
            string SvSql = string.Empty;

            SvSql = " Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr, Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep , Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec, Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan , Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From DPBasic Db , DPDetail Dd , PartyMast P , ItemMaster I , UnitMast U , finyrsplit f Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And  Dd.Unit = U.UnitMastID And f.sfinyr = '" + SFINYR + "'  And Db.DocDate Between f.SFinyrst And f.SFinyred Group By  P.PartyID , I.ItemID , U.UnitID Union Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr, Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep, Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec , Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan, Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb  , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From grnBLbasic Db , grnBLdetail Dd , PartyMast P , ItemMaster I , UnitMast U , finyrsplit f Where Db.grnBLbasicID = Dd.grnBLbasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And U.UnitMastID = I.PriUnit And f.sfinyr = '" + SFINYR + "'  And Db.DocDate Between f.SFinyrst And f.SFinyred Group By   P.PartyID , I.ItemID , U.UnitID Union Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May, Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep , Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec , Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan , Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From igrnbasic Db , igrndetail Dd , PartyMast P , ItemMaster I , UnitMast U   , finyrsplit f Where Db.igrnbasicID = Dd.igrnbasicID And Db.PartyID = P.PartyMastID And Dd.ItemmasterID = I.ItemMasterID And U.UnitMastID = I.PriUnit And f.sfinyr = '" + SFINYR + "' And Db.DocDate Between f.SFinyrst And f.SFinyred Group By  P.PartyID , I.ItemID , U.UnitID Order By 2 , 1";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetFinyr()
        {
            string SvSql = string.Empty;

            SvSql = " select SFINYR,FINYRSPLITID from FINYRSPLIT ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}
