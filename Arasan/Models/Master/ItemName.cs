using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemName
    {
        public ItemName()
        {
            this.IgLst = new List<SelectListItem>();
            this.Iclst = new List<SelectListItem>();
            this.Isglst = new List<SelectListItem>();
            this.Hsn =  new List<SelectListItem>();
            this.Bin = new List<SelectListItem>();

        }
        public List<SelectListItem> IgLst;
        public string ItemGroup { get; set; }
        public string ItemCategory { get; set; }

        public List<SelectListItem> Iclst;
        public string ItemSubGroup { get; set; }

        public List<SelectListItem> Isglst;
        public List<SupItem> Suplst;

        public string HSNcode { get; set; }

        public List<SelectListItem> Hsn;
        public string BinId { get; set; }

        public List<SelectListItem> Bin { get; set; }

        public List<BinItem> Binlst;

        public string ID { get; set; }
        public string ItemG { get; set; }
        public string ItemSub { get; set; }
        public string SubCat { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public string ItemDes { get; set; }
        public string Reorderqu { get; set; }
        public string Reorderlvl { get; set; }
        public string Maxlvl { get; set; }
        public string Minlvl { get; set; }
        public string Con { get; set; }
        public string Uom { get; set; }
        public string Hcode { get; set; }
        public string Selling { get; set; }
        public string Stock { get; set; }
        public string Control { get; set; }
        public string SupName { get; set; }
        public string SupPartNo { get; set; }
        public string Price { get; set; }
        public string Dy { get; set; }
        public string BinID { get; set; }
        public string BinYN { get; set; }
        //public string ItemMas { get; set; }
        public string EditRow { get; set; }
        public string DelRow { get; set; }
        public List<ItemName> pflst { get; set; }

        public string StackAccount { get; set; }
        public string Expiry { get; set; }
        public string ValuationMethod { get; set; }
        public string Serial { get; set; }
        public string Batch { get; set; }
        public string QCTemplate { get; set; }
        public string QCRequired { get; set; }
        public string Latest { get; set; }
        public string SubHeading { get; set; }
        public string Rejection { get; set; }
        public string Percentage { get; set; }
        public string PercentageAdd { get; set; }
        public string Additive { get; set; }
        public string RawMaterial { get; set; }


    }

    public class SupItem
    {

        public List<SelectListItem> Suplst { get; set; }
        public string SupName { get; set; }
        public string SupplierPart { get; set; }
        public string PurchasePrice { get; set; }
        public string Preforder { get; set; }
        public string Delivery { get; set; }
        public string Isvalid { get; set; }

        public string ID { get; set; }
        //public string BinID { get; set; }
        //public string BinYN { get; set; }
       // public string ItemMas { get; set; }


    }
    public class BinItem
    {
        public string Isvalid { get; set; }
        public string BinID { get; set; }
        public string BinYN { get; set; }
    }

    public class ItemList
    {
        public string id  { get;set;}
        public string itemgroup { get; set; }
        public string itemsubgroup { get; set; }
        public string itemname { get; set; }
        public string bin { get; set; }
        public string cf { get; set; }
        public string uom { get; set; }
        public string hsncode { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string itemcode { get; set; }    

    }
    
}
