using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services.Sales
{
    public class DepotInvoiceService : IDepotInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DepotInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IEnumerable<DepotInvoice> GetAllDepotInvoice()
        {
            List<DepotInvoice> cmpList = new List<DepotInvoice>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID,INVTYPE,DOCID,VTYPE,DEPINVBASICID from DEPINVBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=DEPINVBASIC.BRANCHID WHERE DEPINVBASIC.CANCEL='T'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DepotInvoice cmp = new DepotInvoice
                        {
                            ID = rdr["DEPINVBASICID"].ToString(),
                            Branch = rdr["BRANCHID"].ToString(),
                            InvoType = rdr["INVTYPE"].ToString(),
                            InvNo = rdr["DOCID"].ToString(),
                            Vocher = rdr["VTYPE"].ToString(),
                         
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public DataTable GetDepotInvoiceDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,INVTYPE,DOCID,to_char(DEPINVBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REFNO,to_char(DEPINVBASIC.REFDATE,'dd-MON-yyyy')REFDATE,MAINCURRENCY,EXRATE,PARTYID,VTYPE,CUSTPO,TYPE,ORDERSAMPLE,SALVAL,RECDBY,DESPBY,INSPBY,DOCTHORUGH,PACKING,RNDOFF,GROSS,NET,AMTWORDS,SERNO,NARRATION FROM DEPINVBASIC Where DEPINVBASICID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DEPINVDETAIL.QTY,DEPINVDETAIL.DEPINVDETAILID,DEPINVDETAIL.ITEMID,UNITMAST.UNITID,CF,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT  from DEPINVDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=DEPINVDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=DEPINVDETAIL.UNIT  where DEPINVDETAIL.DEPINVBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string DirectPurCRUD(DepotInvoice cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
               
                //string party = datatrans.GetDataString("Select ID from PARTYRCODE where PARTY='" + cy.Customer + "' ");
                //string partyid = datatrans.GetDataString("Select PARTYMASTID from PARTYMAST where PARTYNAME='" + party + "' ");
                using (OracleConnection objConn = new OracleConnection(_connectionString))

                {
                    objConn.Open();
                        
                    if (cy.ID == null)
                    {
                        svSQL = "Insert into DEPINVBASIC (BRANCHID,INVTYPE,DOCID,DOCDATE,REFNO,REFDATE,MAINCURRENCY,EXRATE,PARTYID,VTYPE,CUSTPO,TYPE,ORDERSAMPLE,SALVAL,RECDBY,DESPBY,INSPBY,DOCTHORUGH,PACKING,RNDOFF,GROSS,NET,AMTWORDS,NARRATION,CANCEL) VALUES ('" + cy.Branch + "','" + cy.InvoType + "','" + cy.InvNo + "','" + cy.RefNo + "','" + cy.InvDate + "','" + cy.RefDate + "','" + cy.Currency + "','" + cy.ExRate + "','" + cy.Party + "','" + cy.Vocher + "','" + cy.Customer + "','" + cy.Type + "','" + cy.Ordsam + "','" + cy.Sales + "','" + cy.RecBy + "','" + cy.Dis + "','" + cy.Inspect + "','" + cy.Doc + "','" + cy.Packing + "','" + cy.Round + "','" + cy.Gross + "','" + cy.Net + "','" + cy.AinWords + "','" + cy.Narration + "','T')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = "Update DEPINVBASIC SET  BRANCHID= '" + cy.Branch + "',INVTYPE= '" + cy.InvoType + "',DOCID='" + cy.InvNo + "',DOCDATE='" + cy.InvDate + "',REFNO='" + cy.RefNo + "',REFDATE='" + cy.RefDate + "',MAINCURRENCY='" + cy.Currency + "',EXRATE='" + cy.ExRate + "',PARTYID='" + cy.Party + "',VTYPE='" + cy.Vocher + "',CUSTPO='" + cy.Customer + "',TYPE='" + cy.Type + "',ORDERSAMPLE='" + cy.Ordsam + "',SALVAL='" + cy.Sales + "',RECDBY='" + cy.RecBy + "',DESPBY='" + cy.Dis + "',INSPBY='" + cy.Inspect + "',DOCTHORUGH='" + cy.Doc + "',PACKING='" + cy.Packing + "',RNDOFF='" + cy.Round + "',GROSS='" + cy.Gross + "',NET='" + cy.Net + "',AMTWORDS='" + cy.AinWords + "',NARRATION='" + cy.Narration + "' ";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }

                    try
                    {
                        //objConn.Open();
                        //Object Pid = objCmds.Parameters["OUTID"].Value;
                        string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        if (cy.Depotlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DepotInvoiceItem cp in cy.Depotlst)
                                {

                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DEPINVDETAIL (DEPINVBASICID,ITEMID,UNIT,CF,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + UnitId + "','" + cp.ConFac + "','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "','" + cp.DiscAmount + "','" + cp.IntroDiscount + "','" + cp.CashDiscount + "','" + cp.TradeDiscount + "','" + cp.AddDiscount + "','" + cp.SpecDiscount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.TotalAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DEPINVDETAIL WHERE DEPINVBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DepotInvoiceItem cp in cy.Depotlst)
                                {

                                    string UnitId = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.Unit + "' ");

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DEPINVDETAIL (DEPINVBASICID,ITEMID,UNIT,CF,QTY,RATE,AMOUNT,DISCOUNT,IDISC,CDISC,TDISC,ADISC,SDISC,FREIGHT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAMT) VALUES ('" + Pid + "','" + cp.ItemId + "','" + UnitId + "','" + cp.ConFac + "','" + cp.Quantity + "','" + cp.rate + "','" + cp.Amount + "','" + cp.DiscAmount + "','" + cp.IntroDiscount + "','" + cp.CashDiscount + "','" + cp.TradeDiscount + "','" + cp.AddDiscount + "','" + cp.SpecDiscount + "','" + cp.FrigCharge + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.TotalAmount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }



                        }

                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public string StatusChange(string tag, int id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DEPINVBASIC SET CANCEL ='F' WHERE DEPINVBASICID='" + id + "'";
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
