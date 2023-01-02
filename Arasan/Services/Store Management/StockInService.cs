using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class StockInService : IStockIn
    {
        private readonly string _connectionString;
        public StockInService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<StockIn> GetAllStock()
        {
            List<StockIn> cmpList = new List<StockIn>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID GROUP BY ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,INVENTORY_ITEM.ITEM_ID ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StockIn cmp = new StockIn
                        {

                            Branch = rdr["BRANCHID"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            Qty = rdr["QTY"].ToString(),
                            ItemID= rdr["ITEM_ID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                         };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

    }
}
