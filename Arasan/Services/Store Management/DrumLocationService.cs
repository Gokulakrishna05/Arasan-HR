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
                    cmd.CommandText = "select DRUMNO,ITEMMASTER.ITEMID,LOCDETAILS.LOCID from LSTOCKVALUE LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=LSTOCKVALUE.ITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=LSTOCKVALUE.LOCID ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DrumLocation dl = new DrumLocation
                        {
                            Drum = rdr["DRUMNO"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Location = rdr["LOCID"].ToString(),
                            




                        };
                        daList.Add(dl);
                    }
                }
            }
            return daList;
        }

        public DataTable GetAllListDrumItem()
        {
            string SvSql = string.Empty;
            SvSql = "select LSTOCKVALUEID,DRUMNO,ITEMMASTER.ITEMID,LOCDETAILS.LOCID from LSTOCKVALUE LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=LSTOCKVALUE.ITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=LSTOCKVALUE.LOCID ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
