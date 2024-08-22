using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services
{
    public class ItemsToBeReceivedService : IItemsToBeReceived
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ItemsToBeReceivedService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllItemsToBeReceived(string dtFrom)
        {
            try
            {
                string SvSql = "";
                SvSql = " Select Db.DocID DcNo , Db.DocDate DcDt, P.PartyID , L.LocID , Dd.ItemID , Dd.UNIT , Sum(Dd.Qty) DQty , Sum(Dd.RecdQty) RQty , Sum(Dd.Qty - Dd.RecdQty ) PendQty,E.EMPNAME,DD.EXPRETDT,round((Sysdate-DD.EXPRETDT)+1,0) Days From RDelBasic Db , RDelDetail Dd, LocDetails L,Partymast P, Empmast E , SelectedValues S Where Db.RDelBasicID = Dd.RDelBasicID And L.LocDetailsID = Db.FromLocID And Db.DocDate <= '" + dtFrom + "' And E.EMPMASTID = Db.AppBY And P.PARTYMASTID = Db.PARTYID And L.LocDetailsID = S.SelectedID And Db.Docid Not Like 'Con%' And Db.Docid Not Like 'Ndc%' Group By Db.PartyID , L.LocID , Db.DocDate , Db.DocID , Dd.ItemID , dd.Unit, P.PartyID,E.EMPNAME,DD.EXPRETDT Having Sum(Dd.Qty - Dd.RecdQty) > 0 and(Sysdate - DD.EXPRETDT) > 0 Order By 12 desc,Db.PartyID , L.LocID , Db.DocDate , Db.DocID , Dd.ItemID , dd.Unit";

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
