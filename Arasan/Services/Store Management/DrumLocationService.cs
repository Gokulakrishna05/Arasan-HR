using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
 
using Arasan.Interface ;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class DrumLocationService : IDrumLocation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DrumLocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<DrumLocation> GetAllDrumLocation()
        {
            List<DrumLocation> daList = new List<DrumLocation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "select DRUM_NO,ITEMMASTER.ITEMID,LOCDETAILS.LOCID from drum_stock LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=drum_stock.ITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=drum_stock.LOCID where BALANCE_QTY > 0 AND DRUM_NO IS NOT NULL";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumLocation dl = new DrumLocation
                        {
                            Drum = rdr["DRUM_NO"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            




                        };
                        daList.Add(dl);
                    }
                }
            }
            return daList;
        }
    }
}
