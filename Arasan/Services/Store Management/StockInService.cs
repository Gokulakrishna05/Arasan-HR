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
                    cmd.CommandText = "Select SUM(INVENTORY_ITEM.BALANCE_QTY) as QTY,ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID from INVENTORY_ITEM LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=INVENTORY_ITEM.BRANCH_ID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID GROUP BY ITEMMASTER.ITEMID,UNITMAST.UNITID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        StockIn cmp = new StockIn
                        {

                            //ID = rdr["DPBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Unit = rdr["UNITID"].ToString(),
                            //GoodQty = rdr["DOCDATE"].ToString(),
                            Qty = rdr["QTY"].ToString(),
                           
                            Location = rdr["LOCID"].ToString(),
                            //Currency = rdr["MAINCURR"].ToString(),
                            //Narration = rdr["NARR"].ToString()
                           



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

    }
}
