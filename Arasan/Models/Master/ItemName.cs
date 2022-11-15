

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
           
        }
        public List<SelectListItem> IgLst;
        public string ItemGroup { get; set; }
        public string ItemCategory { get; set; }

        public List<SelectListItem> Iclst;
        public string ItemSubGroup { get; set; }

        public List<SelectListItem> Isglst;
        public List<SupItem> Suplst { get; set; }

        public string HSNcode { get; set; }

        public List<SelectListItem> Hsn;

       
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

    }
    public class SupItem
    {

        public List<SelectListItem> Suplst { get; set; }
        public string SupplierPart { get; set; }
        public string PurchasePrice { get; set; }
        public string Preforder { get; set; }
        public string Delivery { get; set; }
        public string Isvalid { get; set; }

    }
}
