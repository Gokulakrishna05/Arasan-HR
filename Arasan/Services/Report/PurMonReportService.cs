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

        public DataTable GetAllReport(string Branch, string Sdate, string Edate)
        {
            string SvSql = string.Empty;
            //SvSql = "Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep, Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec, Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan, Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb, Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From DPBasic Db , DPDetail Dd , PartyMast P , ItemMaster I , UnitMast U Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And U.UnitMastID = I.PriUnit Group By P.PartyID , I.ItemID , U.UnitID  Where DPBasic.BRANCHID='" + Branch + "' AND RECDCBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}