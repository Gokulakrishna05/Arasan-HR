using Arasan.Interface;
using Arasan.Models;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;

namespace Arasan.Services
{
    public class ImportPOService :IIPO
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ImportPOService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
       
        
        

        public DataTable GetPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,POBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,POBASICID,POBASIC.STATUS,PURQUOTBASIC.DOCID as Quotno,to_char(PURQUOTBASIC.DOCDATE,'dd-MON-yyyy') Quotedate,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,POBASIC.FREIGHT,POBASIC.GROSS,POBASIC.NET from POBASIC LEFT OUTER JOIN PURQUOTBASIC ON PURQUOTBASIC.PURQUOTBASICID=POBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=POBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on POBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=POBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND POBASIC.POBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOrderID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,IPOBASIC.DOCID,to_char(IPOBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,IPOBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,IPOBASICID,IPOBASIC.STATUS,IPURQUOTBASIC.DOCID as Quotno,IPOBASIC.GROSS,IPOBASIC.NET,IPOBASIC.RNDOFF from IPOBASIC LEFT OUTER JOIN IPURQUOTBASIC ON IPURQUOTBASIC.IPURQUOTBASICID=IPOBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=IPOBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPOBASIC.IPOBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPObySuppID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "select IPOBASIC.IPOBASICID,IPOBASIC.DOCID from IPOBASIC where IPOBASIC.PARTYID='" + name + "' AND IPOBASIC.STATUS IS NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable EditPObyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,IPOBASIC.DOCID,to_char(IPOBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,IPOBASIC.MAINCURRENCY,IPOBASIC.PARTYID,IPOBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,IPOBASIC.REFNO,to_char(IPOBASIC.REFDT,'dd-MON-yyyy') REFDT,IPOBASICID,IPOBASIC.STATUS,IPURQUOTBASIC.DOCID as Quotno,IPOBASIC.GROSS,IPOBASIC.NET,IPOBASIC.RNDOFF from IPOBASIC LEFT OUTER JOIN IPURQUOTBASIC ON IPURQUOTBASIC.IPURQUOTBASICID=IPOBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=IPOBASIC.MAINCURRENCY  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND IPOBASIC.IPOBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItembyID(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPODETAIL.ITEMID as Itemi,IPODETAIL.QTY,IPODETAIL.IPODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID,IPODETAIL.RATE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE,IPODETAIL.AMOUNT from IPODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where IPODETAIL.IPOBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItem(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPODETAIL.ITEMID as Itemi,IPODETAIL.QTY,IPODETAIL.IPODETAILID,ITEMMASTER.ITEMID,UNITMAST.UNITID,IPODETAIL.RATE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE,IPODETAIL.AMOUNT from IPODETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=IPODETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT  where IPODETAIL.IPOBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPOItemDetails(string name)
        {
            string SvSql = string.Empty;
            SvSql = "Select IPODETAIL.ITEMID,IPODETAIL.QTY,IPODETAIL.IPODETAILID,IPODETAIL.PUNIT,PURTYPE,IPODETAIL.RATE,to_char(DUEDATE,'dd-MON-yyyy') DUEDATE,IPODETAIL.AMOUNT,IPODETAIL.CF from IPODETAIL   where IPODETAIL.IPOBASICID='" + name + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public string POtoGRN(string POID)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'GR-F' AND ACTIVESEQUENCE = 'T'");
                string PONo = string.Format("{0}{1}", "GR-F", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='GR-F' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into GRNBLBASIC (PARTYID,BRANCHID,POBASICID,EXRATE,MAINCURRENCY,DOCID,DOCDATE,PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET,IS_ACTIVE,AMTINWORDS) (Select PARTYID,BRANCHID,'" + POID + "',EXRATE,MAINCURRENCY,'" + PONo + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "',PACKING_CHRAGES,OTHER_CHARGES,OTHER_DEDUCTION,ROUND_OFF_PLUS,ROUND_OFF_MINUS,FREIGHT,GROSS,NET ,'Y',AMTINWORDS from POBASIC where POBASICID='" + POID + "')";
                    OracleCommand objCmd = new OracleCommand(svSQL, objConn);
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }

                string quotid = datatrans.GetDataString("Select GRNBLBASICID from GRNBLBASIC Where POBASICID=" + POID + "");
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    string Sql = "Insert into GRNBLDETAIL (GRNBLBASICID,ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTP,CGST,SGSTP,SGST,IGSTP,IGST,TOTAMT,DISCPER,DISC,PURTYPE) (Select '" + quotid + "',ITEMID,RATE,QTY,UNIT,AMOUNT,CF,CGSTP,CGST,SGSTP,SGST,IGSTP,IGST,TOTAMT,DISCPER,DISCAMT,PURTYPE FROM PODETAIL WHERE POBASICID=" + POID + ")";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (OracleConnection objConnE = new OracleConnection(_connectionString))
                {
                    string Sql = "UPDATE POBASIC SET STATUS='GRN Generated' where POBASICID='" + POID + "'";
                    OracleCommand objCmds = new OracleCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

       

        public string PurOrderCRUD(ImportPO cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("IPOPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.POID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.POID;

                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.BranchId;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.SuppId;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.POID;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.POdate;
                    objCmd.Parameters.Add("REFNO", OracleDbType.NVarchar2).Value = cy.RefNo;
                    if (cy.RefDate == null)
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    }

                    objCmd.Parameters.Add("GROSS", OracleDbType.NVarchar2).Value = cy.Gross;
                    objCmd.Parameters.Add("NET", OracleDbType.NVarchar2).Value = cy.Net;
                    objCmd.Parameters.Add("FREIGHT", OracleDbType.NVarchar2).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("OTHER_CHARGES", OracleDbType.NVarchar2).Value = cy.Othercharges;
                    objCmd.Parameters.Add("RNDOFF", OracleDbType.NVarchar2).Value = cy.Round;
                    
                    objCmd.Parameters.Add("EXRATE", OracleDbType.NVarchar2).Value = cy.ExRate;
                    objCmd.Parameters.Add("TRANSID", OracleDbType.NVarchar2).Value ="impo";
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.assignid;
                    objCmd.Parameters.Add("QUOTNO", OracleDbType.NVarchar2).Value = cy.QuoteNo;
                    objCmd.Parameters.Add("MAINCURRENCY", OracleDbType.NVarchar2).Value = cy.Cur;
                   
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        foreach (IPOItem cp in cy.PoItem)
                        {
                            if (cp.Isvalid == "Y" && cp.saveItemId != null)
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    if (StatementType == "Update")
                                    {
                                        Sql = "Update IPODETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.Conversionfactor + "',AMOUNT='" + cp.Amount + "',DUEDATE='" + cp.Duedate + "' where IPOBASICID='" + cy.POID + "'  AND ITEMID='" + cp.saveItemId + "' ";
                                    }
                                    else
                                    {
                                        svSQL = "Insert into IPODETAIL (IPOBASICID,ITEMID,QTY,PUNIT,RATE,AMOUNT,DUEDATE,CF,UNIT,PURTYPE) VALUES ('" + Pid + "','" + cp.ItemId + "','" + cp.Quantity + "','" + cp.Unit + "','" + cp.rate + "','" + cp.Amount + "','" + cp.Duedate + "','" + cp.Conversionfactor + "','" + cp.Unit + "','" + cp.Purtype + "')";

                                    }
                                    OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                                    objConnT.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConnT.Close();
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE IPOBASIC SET IS_ACTIVE ='N' WHERE IPOBASICID='" + id + "'";
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
        public DataTable GetPOItemrep(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PARTYMAST.PARTYID, POBASIC.GROSS,POBASIC.NET,POBASIC.POBASICID,POBASIC.DOCID,to_char(POBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,POBASIC.PAYTERMS,POBASIC.DESP,POBASIC.DELTERMS,POBASIC.WARRTERMS,POBASIC.AMTINWORDS,ITEMMASTER.ITEMID,PODETAIL.RATE,PODETAIL.AMOUNT, PODETAIL.QTY, PODETAIL.PUNIT,PODETAIL.SGST, PODETAIL.CGST, PODETAIL.IGST, PODETAIL.TOTAMT,PARTYMAST.PARTYID AS EXPR1, PARTYMAST.ADD1, PARTYMAST.ADD2, PARTYMAST.ADD3, PARTYMAST.CITY, PARTYMAST.PINCODE,PARTYMAST.STATE,PARTYMAST.CSTNO, PARTYMAST.MOBILE   FROM  POBASIC INNER JOIN PARTYMAST ON POBASIC.PARTYID = PARTYMAST.PARTYMASTID, PODETAIL  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PODETAIL.ITEMID where PODETAIL.POBASICID='" + id + "' and POBASIC.POBASICID ='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        
        public DataTable GetHsn(string id)
        {
            string SvSql = string.Empty;
            //  996519 -frieght
            SvSql = "select HSN,ITEMMASTERID from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetgstDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID from HSNROW WHERE HSNCODEID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GethsnDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select HSNCODEID from HSNCODE WHERE HSNCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        

        public DataTable GetAllPoItems(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select BRANCHMAST.BRANCHID,IPOBASIC.DOCID,to_char(IPOBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,IPOBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,IPOBASICID ,IPURQUOTBASIC.DOCID as Quotno,IPOBASIC.IS_ACTIVE from IPOBASIC LEFT OUTER JOIN IPURQUOTBASIC ON IPURQUOTBASIC.IPURQUOTBASICID=IPOBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=IPOBASIC.MAINCURRENCY AND IPOBASIC.IS_ACTIVE='Y'  ORDER BY IPOBASIC.IPOBASICID DESC";
            }
            else
            {
                SvSql = "Select BRANCHMAST.BRANCHID,IPOBASIC.DOCID,to_char(IPOBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,IPOBASIC.EXRATE,CURRENCY.MAINCURR,PARTYMAST.PARTYNAME,IPOBASICID ,IPURQUOTBASIC.DOCID as Quotno,IPOBASIC.IS_ACTIVE from IPOBASIC LEFT OUTER JOIN IPURQUOTBASIC ON IPURQUOTBASIC.IPURQUOTBASICID=IPOBASIC.QUOTNO LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=IPOBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on IPOBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=IPOBASIC.MAINCURRENCY AND IPOBASIC.IS_ACTIVE='N'  ORDER BY IPOBASIC.IPOBASICID DESC";
            }
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

        public string CRUDPOAttachement(List<IFormFile> files,ImportPO cy)
        {
            string msg = string.Empty;
            string svSQL = string.Empty;
            using (OracleConnection objConn = new OracleConnection(_connectionString))
            {
                objConn.Open();
                if (files != null && files.Count > 0)
                {
                    int r = 1;
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Get the file name and combine it with the target folder path
                            String strLongFilePath1 = file.FileName;
                            String sFileType1 = "";
                            sFileType1 = System.IO.Path.GetExtension(file.FileName);
                            sFileType1 = sFileType1.ToLower();

                            String strFleName = strLongFilePath1.Replace(sFileType1, "") + String.Format("{0:ddMMMyyyy-hhmmsstt}", DateTime.Now) + sFileType1;
                            var fileName = Path.Combine("wwwroot/uploads", strFleName);
                            var fileName1 = Path.Combine("uploads", strFleName);
                            var name = file.FileName;
                            // Save the file to the target folder

                            using (var fileStream = new FileStream(fileName, FileMode.Create))
                            {
                                file.CopyTo(fileStream);


                                 
                                svSQL = "Insert into IPODOCDETAIL (IPOBASICID,IPODOCDETAILROW,DOCNAME,PATH) VALUES ('" + cy.ID + "','" + r + "','"+ name +"','" + fileName1 + "')";
                                OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
                                objCmdss.ExecuteNonQuery();

                                r++;
                            }
                        }
                        
                    }
                    objConn.Close();
                }


                msg = "Uploaded";

            }
            return msg;
        }
        public DataTable GetAllAttachment(long id)
        {
            string SvSql = string.Empty;
           
                SvSql = "Select IPOBASICID,IPODOCDETAILROW,DOCNAME,PATH,IPODOCDETAILID FROM IPODOCDETAIL WHERE IPOBASICID='" + id+"' ";
            
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
