using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PurchaseEnquiry
    {
        public PurchaseEnquiry()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.EnqLst = new List<EnqItem>();
            this.EnqRecList = new List<SelectListItem>();
            this.EnqassignList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string ID { get; set; }
        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;

        public string Currency { get; set; }

        public List<SelectListItem> Curlst;

        public List<EnqItem> EnqLst;

        public List<SelectListItem> EnqRecList;

        public List<SelectListItem> EnqassignList;

        public string EnqRecid { get; set; }

        public string Enqassignid { get; set; }

        public double Gross { get; set; }
        public double Net { get; set; }

        public string EnqNo { get; set; }
        public string PartyrefNo { get; set; }
        public string ExchangeRate { get; set; }
        public string EnqDate { get; set; }

    }

    public class EnqItem
    {
        public string ItemId { get;set;}

        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public string Desc { get; set; }
        public string Unit { get; set; }
        public string Conversionfactor { get; set; }
        public double Quantity { get; set; }
        public string unitprim { get; set; }
        public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public string Isvalid { get; set; }

    }
    public class EnquiryList
    {
       
        public DataTable GetEnquiry()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("ID", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Branch", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("SuppName", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("EnqNo", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("EnqDate", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Currency", typeof(String));
            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();
            dr = dt.NewRow();
            dr["ID"] = "1";
            dr["Branch"] = "(TAAI)SVK - FACTORY";
            dr["SuppName"] = "Supplier 1";
            dr["EnqNo"] = "PENQ100001";
            dr["EnqDate"] = "25-Aug-2022";
            dr["Currency"] = "Rs";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "2";
            dr["Branch"] = "(TAAI)SVK - FACTORY";
            dr["SuppName"] = "Supplier 2";
            dr["EnqNo"] = "PENQ100002";
            dr["EnqDate"] = "26-Aug-2022";
            dr["Currency"] = "Rs";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "3";
            dr["Branch"] = "(TAAI)SVK - FACTORY";
            dr["SuppName"] = "Supplier 1";
            dr["EnqNo"] = "PENQ100003";
            dr["EnqDate"] = "26-Aug-2022";
            dr["Currency"] = "Rs";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "4";
            dr["Branch"] = "(TAAI)SVK - FACTORY";
            dr["SuppName"] = "Supplier 3";
            dr["EnqNo"] = "PENQ100004";
            dr["EnqDate"] = "26-Aug-2022";
            dr["Currency"] = "Rs";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "4";
            dr["Branch"] = "(TAAI)SVK - FACTORY";
            dr["SuppName"] = "Supplier 2";
            dr["EnqNo"] = "PENQ100005";
            dr["EnqDate"] = "26-Aug-2022";
            dr["Currency"] = "Rs";
            dt.Rows.Add(dr);

            return dt;
        }
        public DataTable GetEnquiryItem(string ENQID)
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("OrderID", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("PRID", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("ProName", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Unit", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Quantity", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Rate", typeof(String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Amount", typeof(String));
            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();

            if(ENQID== "1") { 

            dr = dt.NewRow();
            dr["OrderID"] = "10248";
            dr["PRID"] = "1";
            dr["ProName"] = "Empty Old Drums Renaatus";
            dr["Unit"] = "No's";
            dr["Quantity"] = "100";
            dr["Rate"] = "22.3800";
            dr["Amount"] = "2238";
             dt.Rows.Add(dr);
            }
            if (ENQID == "2")
            {
                dr = dt.NewRow();
                dr["OrderID"] = "10249";
                dr["PRID"] = "2";
                dr["ProName"] = "Empty Old Drums Renaatus";
                dr["Unit"] = "No's";
                dr["Quantity"] = "300";
                dr["Rate"] = "32.3800";
                dr["Amount"] = "9840";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["OrderID"] = "10247";
                dr["PRID"] = "2";
                dr["ProName"] = "+100 Fine";
                dr["Unit"] = "Bag";
                dr["Quantity"] = "5";
                dr["Rate"] = "500";
                dr["Amount"] = "2500";
                dt.Rows.Add(dr);

                
            }
                return dt;
        }
       
    }
    public class EnquiryItemBindList
    {
        public int OrderID { get; set; }
        public int PRID { get; set; }
        public string ProName { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }

    }
    public class EnquiryBindList
    {
        public int PRID { get; set; }
        public string SuppName { get; set; }
        public string SendMail { get; set; }
        public string EditRow { get; set; }
        public string DelRow { get; set; }
        public string FollowUp { get; set; }

        public string EnqNo { get; set; }
        public string EnqDate { get; set; }
        public string Currency { get; set; }

        public string Branch { get; set; }

    }
}
