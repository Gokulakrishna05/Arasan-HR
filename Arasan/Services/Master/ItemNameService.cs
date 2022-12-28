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
        public ItemNameService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ItemName> GetAllItemName()
        {
            List<ItemName> staList = new List<ItemName>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRI,ITEMMASTERID from ITEMMASTER";
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
                            Selling = rdr["SELLINGPRI"].ToString(),
                           

                        };
                        staList.Add(sta);
                    }
                }
            }
            return staList;
        }
        public ItemName GetSupplierDetailById(string eid)
        {
            ItemName ItemName = new ItemName();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRI,ITEMMASTERID from ITEMMASTER where ITEMMASTERID=" + eid + "";
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
                            Selling = rdr["SELLINGPRI"].ToString(),
                           
                        };
                        ItemName = sta;
                    }
                }
            }
            return ItemName;
        }
        public string ItemNameCRUD(ItemName ss)
        {
            string msg = " ";
            try
            {
                string StatementType = string.Empty;
                //string svSQL = "";

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
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }
                      //  foreach (DirItem cp in cy.DirLst)
                        //{
                            
                            
                                using (OracleConnection objConns = new OracleConnection(_connectionString))
                                {
                                    OracleCommand objCmds = new OracleCommand("BINMASTERID", objConns);
                                    if (ss.ID == null)
                                    {
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        StatementType = "Update";
                                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                                    }
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("BINMASTERID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("BINID", OracleDbType.NVarchar2).Value = ss.BinID;
                                    objCmds.Parameters.Add("BINYN", OracleDbType.NVarchar2).Value = ss.BinYN;
                                  //  objCmds.Parameters.Add("ITEMMASTERID", OracleDbType.NVarchar2).Value = ss.ItemMas;
                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            
                      //  }
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
            SvSql = "Select IGROUP,ISUBGROUP,SUBCATEGORY,ITEMCODE,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MAXSTOCKLVL,MINSTOCKLVL,CONVERAT,UOM,HSN,SELLINGPRI,BINNO,BINYN,ITEMMASTERID  from ITEMMASTER where ITEMMASTERID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBinDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select BINMASTER.BINID,BINMASTER.BINYN,BINMASTER.ITEMMASTERID,BINMASTERID  from BINMASTER where BINMASTER.BINMASTERID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemGroup()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMGROUPID,GROUPCODE from ITEMGROUP where APPROVALSTATUS='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable BindBinID()
        {
            string SvSql = string.Empty;
            SvSql = "Select BINBASICID,BINID from BINBASIC";
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
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYRCODE.PARTY from PARTYMAST LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYRCODE.PARTY IS NOT NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<ItemName> GetAllSupplier(string id)
        {
            List<ItemName> cmpList = new List<ItemName>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SUPPLIERID,SUPPLIERPARTNO,SPURPRICE,DELDAYS,REMARKS,SUPPLIERPARTNOID from SUPPLIERPARTNO";
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
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
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

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("SUPPLIERPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "SUPPLIERPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    }

                    objCmd.Parameters.Add("SUPPLIERID", OracleDbType.NVarchar2).Value = cy.SupName;
                    objCmd.Parameters.Add("SUPPLIERPARTNO", OracleDbType.NVarchar2).Value = cy.SupPartNo;
                    objCmd.Parameters.Add("SPURPRICE", OracleDbType.NVarchar2).Value = cy.Price;
                    objCmd.Parameters.Add("DELDAYS", OracleDbType.NVarchar2).Value = cy.Dy;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
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

        DataTable IItemNameService.GetAllSupplier(string id)
        {
            throw new NotImplementedException();
        }
    }

}