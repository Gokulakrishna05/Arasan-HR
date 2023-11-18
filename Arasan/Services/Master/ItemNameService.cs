using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class ItemNameService : IItemNameService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ItemNameService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IEnumerable<ItemName> GetAllItemName()
        {
            List<ItemName> staList = new List<ItemName>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    //cmd.CommandText = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE,ITEMACC,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,QCT,QCCOMPFLAG,LATPURPRICE,TARIFFHEADING,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMMASTERID from ITEMMASTER ORDER BY ITEMMASTER.ITEMMASTERID ASC";
                    cmd.CommandText = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE,ITEMACC,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,QCT,QCCOMPFLAG,LATPURPRICE,TARIFFHEADING,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,CURINGDAY,ITEMMASTERID from ITEMMASTER ORDER BY ITEMMASTER.ITEMMASTERID DESC ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemName sta = new ItemName
                        {
                            ID = rdr["ITEMMASTERID"].ToString(),
                            ItemG = rdr["IGROUP"].ToString(),
                            ItemSub = rdr["ISUBGROUP"].ToString(),
                            SubCat = rdr["SUBCATEGORY"].ToString(),
                            ItemCode = rdr["ITEMCODE"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            ItemDes = rdr["ITEMDESC"].ToString(),
                            Reorderqu = rdr["REORDERQTY"].ToString(),
                            Reorderlvl = rdr["REORDERLVL"].ToString(),
                            Maxlvl = rdr["MAXSTOCKLVL"].ToString(),
                            Minlvl = rdr["MINSTOCKLVL"].ToString(),
                            Con = rdr["CONVERAT"].ToString(),
                            Uom = rdr["UOM"].ToString(),
                            Hcode = rdr["HSN"].ToString(),
                            Selling = rdr["SELLINGPRICE"].ToString(),
                            StackAccount = rdr["ITEMACC"].ToString(),
                            Expiry = rdr["EXPYN"].ToString(),
                            ValuationMethod = rdr["VALMETHOD"].ToString(),
                            Serial = rdr["SERIALYN"].ToString(),
                            Batch = rdr["BSTATEMENTYN"].ToString(),
                            QCTemplate = rdr["QCT"].ToString(),
                            QCRequired = rdr["QCCOMPFLAG"].ToString(),
                            Latest = rdr["LATPURPRICE"].ToString(),
                            SubHeading = rdr["TARIFFHEADING"].ToString(),
                            Rejection = rdr["REJRAWMATPER"].ToString(),
                            Percentage = rdr["RAWMATPER"].ToString(),
                            PercentageAdd = rdr["ADD1PER"].ToString(),
                            Additive = rdr["ADD1"].ToString(),
                            RawMaterial = rdr["RAWMATCAT"].ToString(),
                            Curing = rdr["CURINGDAY"].ToString()


                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        public DataTable GetAllItems()
        {
            string SvSql = string.Empty;
            //SvSql = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE,ITEMMASTERID from ITEMMASTER";
            SvSql = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE,ITEMACC,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,QCT,QCCOMPFLAG,LATPURPRICE,TARIFFHEADING,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMMASTERID from ITEMMASTER ORDER BY ITEMMASTER.ITEMMASTERID DESC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public ItemName GetSupplierDetailById(string eid)
        //{
        //    ItemName ItemName = new ItemName();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRI,ITEMMASTERID from ITEMMASTER where ITEMMASTERID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemName sta = new ItemName
        //                {
        //                    ID = rdr["ITEMMASTERID"].ToString(),
        //                    ItemG = rdr["IGROUP"].ToString(),
        //                    ItemSub = rdr["ISUBGROUP"].ToString(),
        //                    SubCat = rdr["SUBCATEGORY"].ToString(),
        //                    ItemCode = rdr["ITEMCODE"].ToString(),
        //                    Item = rdr["ITEMID"].ToString(),
        //                    ItemDes = rdr["ITEMDESC"].ToString(),
        //                    Reorderqu = rdr["REORDERQTY"].ToString(),
        //                    Reorderlvl = rdr["REORDERLVL"].ToString(),
        //                    Maxlvl = rdr["MAXSTOCKLVL"].ToString(),
        //                    Minlvl = rdr["MINSTOCKLVL"].ToString(),
        //                    Con = rdr["CONVERAT"].ToString(),
        //                    Uom = rdr["UOM"].ToString(),
        //                    Hcode = rdr["HSN"].ToString(),
        //                    Selling = rdr["SELLINGPRI"].ToString(),

        //                };
        //                ItemName = sta;
        //            }
        //        }
        //    }
        //    return ItemName;
        //}
        public string ItemNameCRUD(ItemName ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (ss.ID == null)
                {
                    svSQL = " SELECT Count(ITEMID) as cnt FROM ITEMMASTER WHERE ITEMID =LTRIM(RTRIM('" + ss.Item + "')) and ITEMCODE =LTRIM(RTRIM('" + ss.ItemCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Item Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ITEMMASTERPROC", objConn);
                    /*objCmd.Connection = objConn; 
                    objCmd.CommandText = "ITEMMASTERPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID; 
                    }

                    objCmd.Parameters.Add("IGROUP", OracleDbType.NVarchar2).Value = ss.ItemG;
                    objCmd.Parameters.Add("ISUBGROUP", OracleDbType.NVarchar2).Value = ss.ItemSub;
                    objCmd.Parameters.Add("SUBCATEGORY", OracleDbType.NVarchar2).Value = ss.SubCat;
                    objCmd.Parameters.Add("ITEMCODE", OracleDbType.NVarchar2).Value = ss.ItemCode;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = ss.Item;
                    objCmd.Parameters.Add("ITEMDESC", OracleDbType.NVarchar2).Value = ss.ItemDes;
                    objCmd.Parameters.Add("REORDERQTY", OracleDbType.NVarchar2).Value = ss.Reorderqu;
                    objCmd.Parameters.Add("REORDERLVL", OracleDbType.NVarchar2).Value = ss.Reorderlvl;
                    objCmd.Parameters.Add("MAXSTOCKLVL", OracleDbType.NVarchar2).Value = ss.Maxlvl;
                    objCmd.Parameters.Add("MINSTOCKLVL", OracleDbType.NVarchar2).Value = ss.Minlvl;
                    objCmd.Parameters.Add("CONVERAT", OracleDbType.NVarchar2).Value = ss.Con;
                    objCmd.Parameters.Add("UOM", OracleDbType.NVarchar2).Value = ss.Uom;
                    objCmd.Parameters.Add("HSN", OracleDbType.NVarchar2).Value = ss.Hcode;
                    objCmd.Parameters.Add("SELLINGPRI", OracleDbType.NVarchar2).Value = ss.Selling;
                    objCmd.Parameters.Add("ITEMACC ", OracleDbType.NVarchar2).Value = ss.StackAccount;
                    objCmd.Parameters.Add("EXPYN", OracleDbType.NVarchar2).Value = ss.Expiry;
                    objCmd.Parameters.Add("VALMETHOD", OracleDbType.NVarchar2).Value = ss.ValuationMethod;
                    objCmd.Parameters.Add("SERIALYN", OracleDbType.NVarchar2).Value = ss.Serial;
                    objCmd.Parameters.Add("BSTATEMENTYN", OracleDbType.NVarchar2).Value = ss.Batch;
                    objCmd.Parameters.Add("QCT", OracleDbType.NVarchar2).Value = ss.QCTemplate;
                    objCmd.Parameters.Add("QCCOMPFLAG", OracleDbType.NVarchar2).Value = ss.QCRequired;
                    objCmd.Parameters.Add("LATPURPRICE", OracleDbType.NVarchar2).Value = ss.Latest;
                    objCmd.Parameters.Add("TARIFFHEADING", OracleDbType.NVarchar2).Value = ss.SubHeading;
                    objCmd.Parameters.Add("REJRAWMATPER", OracleDbType.NVarchar2).Value = ss.Rejection;
                    objCmd.Parameters.Add("RAWMATPER", OracleDbType.NVarchar2).Value = ss.Percentage;
                    objCmd.Parameters.Add("ADD1PER ", OracleDbType.NVarchar2).Value = ss.PercentageAdd;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = ss.Additive;
                    objCmd.Parameters.Add("RAWMATCAT", OracleDbType.NVarchar2).Value = ss.RawMaterial;
                    objCmd.Parameters.Add("LEDGERNAME", OracleDbType.NVarchar2).Value = ss.Ledger;
                    objCmd.Parameters.Add("IQCTEMP", OracleDbType.NVarchar2).Value = ss.QCTemp;
                    objCmd.Parameters.Add("FGQCTEMP", OracleDbType.NVarchar2).Value = ss.FQCTemp;
                    objCmd.Parameters.Add("CURINGDAY", OracleDbType.NVarchar2).Value = ss.Curing;

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                        //  foreach (DirItem cp in cy.DirLst)
                        //{
                        string latestbin = datatrans.GetDataString("Select BINID from BINMASTER where ITEMID='" + Pid + "' AND ISUPDATED='Y'");
                        if (latestbin != ss.BinID)
                        {
                            bool resultsds = datatrans.UpdateStatus("UPDATE BINMASTER SET ISUPDATED='N' Where ITEMID='" + Pid + "'");
                            using (OracleConnection objConns = new OracleConnection(_connectionString))
                            {
                                OracleCommand objCmds = new OracleCommand("BINMASTEPROC", objConns);
                                StatementType = "Insert";
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = ss.BinID;
                                objCmds.Parameters.Add("BINYN", OracleDbType.NVarchar2).Value = ss.BinYN;
                                objCmds.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objConns.Open();
                                objCmds.ExecuteNonQuery();
                                objConns.Close();
                            }
                        }
                        bool result = datatrans.UpdateStatus("DELETE SUPPLIERPARTNO  Where ITEMMASTERID='" + Pid + "' ");
                        if (ss.Suplst != null)
                        {
                            foreach (SupItem cp in ss.Suplst) 
                            {
                                using (OracleConnection objConnI = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmdI = new OracleCommand("SUPPLIERPROC", objConnI);

                                    objCmdI.CommandType = CommandType.StoredProcedure;
                                    StatementType = "Insert";
                                    objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    objCmdI.Parameters.Add("SUPPLIERID", OracleDbType.NVarchar2).Value = cp.SupName;
                                    objCmdI.Parameters.Add("SUPPLIERPARTNO", OracleDbType.NVarchar2).Value = cp.SupplierPart;
                                    objCmdI.Parameters.Add("SPURPRICE", OracleDbType.NVarchar2).Value = cp.PurchasePrice;
                                    objCmdI.Parameters.Add("DELDAYS", OracleDbType.NVarchar2).Value = cp.Delivery;
                                    objCmdI.Parameters.Add("ITEMMASTERID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    try
                                    {
                                        objConnI.Open();
                                        objCmdI.ExecuteNonQuery();
                                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                                    }

                                    objConnI.Close();
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
        public DataTable GetItemNameDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRICE SELLINGPRI,ITEMACC,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,QCT,QCCOMPFLAG,LATPURPRICE,TARIFFHEADING,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,LEDGERNAME,ITEMMASTERID ,CURINGDAY from ITEMMASTER where ITEMMASTERID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBinDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select BINMASTER.BINID,BINMASTER.BINYN,BINMASTERID  from BINMASTER where BINMASTER.ITEMID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemGroup()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMGROUPID,GROUPCODE from ITEMGROUP where STATUS='ACTIVE'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable BindBinID()
        {
            string SvSql = string.Empty;
            SvSql = "Select BINID from BINBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetItem()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select ITEMMASTERID,ITEMID from ITEMMASTER";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LEDGERID,LEDNAME FROM accledger where IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMCATEGORYID,CATEGORY from ITEMCATEGORY where APPROVALSTATUS='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMSUBGROUPID,SGCODE from ITEMSUBGROUP order by ITEMSUBGROUPID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetHSNcode()
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNCODEID,HSNCODE from HSNCODE order by HSNCODEID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCTemp()
        {
            string SvSql = string.Empty;
            SvSql = "Select TEMPLATEID,TESTTBASICID from TESTTBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME  PARTY from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSupplierName(string subid)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUPPLIERID,SUPPLIERPARTNO,SPURPRICE,DELDAYS,SUPPLIERPARTNOID from SUPPLIERPARTNO WHERE SUPPLIERPARTNOID='" + subid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public IEnumerable<ItemName> GetAllSupplier(string id)
        //{
        //    List<ItemName> cmpList = new List<ItemName>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select SUPPLIERID,SUPPLIERPARTNO,SPURPRICE,DELDAYS,SUPPLIERPARTNOID from SUPPLIERPARTNO";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemName cmp = new ItemName
        //                {
        //                    ID = rdr["SUPPLIERPARTNOID"].ToString(),
        //                    SupName = rdr["SUPPLIERID"].ToString(),
        //                    SupPartNo = rdr["SUPPLIERPARTNO"].ToString(),
        //                    Price = rdr["SPURPRICE"].ToString(),
        //                    Dy = rdr["DELDAYS"].ToString(),

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        public DataTable GetAllSupplier(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select SUPPLIERID,SUPPLIERPARTNO,SPURPRICE,DELDAYS,SUPPLIERPARTNOID from SUPPLIERPARTNO where ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public ItemName GetSupplierById(string eid)
        {
            ItemName ItemName = new ItemName();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SUPPLIERID,SUPPLIERPARTNO,SPURPRICE,DELDAYS,REMARKS,SUPPLIERPARTNOID  from SUPPLIERPARTNO where SUPPLIERPARTNOID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemName cmp = new ItemName
                        {
                            ID = rdr["SUPPLIERPARTNOID"].ToString(),
                            SupName = rdr["SUPPLIERID"].ToString(),
                            SupPartNo = rdr["SUPPLIERPARTNO"].ToString(),
                            Price = rdr["SPURPRICE"].ToString(),
                            Dy = rdr["DELDAYS"].ToString(),


                        };

                        ItemName = cmp;
                    }
                }
            }
            return ItemName;
        }
        public string SupplierCRUD(ItemName cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
    }
}