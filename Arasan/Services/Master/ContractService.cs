using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Master
{
    public class ContractService : IContract
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ContractService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<Contract> GetAllContract(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "ACTIVE";
            }

            List<Contract> crList = new List<Contract>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CONTTYPE FROM CONTRMAST ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Contract br = new Contract
                        {
                            Contype = rdr["CONTTYPE"].ToString(),
                            Salpd = rdr["SALPD"].ToString(),
                            Dkg = rdr["DAYORKGS"].ToString(),
                           
                        };
                        crList.Add(br);
                    }
                }
            }
            return crList;
        }
        public string ContractCRUD(Contract cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {


                    objConn.Open();


                    if (cy.ID == null)
                    {

                        svSQL = "Insert into CONTRMAST (CONTTYPE,SALPD,DAYORKGS) VALUES ('" + cy.Contype + "','" + cy.Salpd + "','" + cy.Dkg + "')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = " UPDATE CONTRMAST SET CONTYPE ='" + cy.Contype + "',SALPD='" + cy.Salpd + "',DAYORKGS='" + cy.Dkg + "' Where CONTRMASTID = '" + cy.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }



                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetAllContracts(string strStatus)
        {
            string SvSql = string.Empty;
            //if (strStatus == "ACTIVE" || strStatus == null)
            //{
                SvSql = "Select CONTRMASTID,CONTTYPE,SALPD,DAYORKGS FROM CONTRMAST WHERE IS_ACTIVE='Y'";
            //}
            //else
            //{
            //    SvSql = "Select CONTTYPE,SALPD,DAYORKGS FROM CONTRMAST";

            //}
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditContract(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CONTRMASTID,CONTTYPE,SALPD,DAYORKGS from  CONTRMAST  where CONTRMASTID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusDelete(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE CONTRMAST SET IS_ACTIVE ='N' WHERE CONTRMASTID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }

    }
}
